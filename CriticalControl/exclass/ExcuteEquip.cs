using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wLib;
using wLib.DB;

namespace CriticalControl
{
    public class ExcuteEquip
    {
        DB_CONF db_local_conf = new DB_CONF("LOCAL");
        DB_CONF db_ndms_conf = new DB_CONF("NDMS");

        public WB_ISUMENT_VO ment_vo = new WB_ISUMENT_VO();
        public WB_EQUIP_VO vo = new WB_EQUIP_VO();

        // 레벨
        public int level;

        // 방송, 차단기 동작 여부
        public bool broadCasting;
        public bool gateControl;
        public bool gateClose;

        // NDMS 설정
        public string DsCode;
        public bool sendNdms = false;

        public ExcuteEquip()
        {
            doBroadCasting(true);
            doGateControl(true, true);
        }

        /// <summary>
        /// 단계별 작동할 장비 세팅
        /// </summary>
        /// <param name="vo">작동할 장비 VO</param>
        /// <param name="level">해당 Level</param>
        public void setEquip(WB_EQUIP_VO vo, int level)
        {
            this.level = level;
            this.vo = vo;
        }

        /// <summary>
        /// 방송 실행 여부
        /// </summary>
        /// <param name="broadCast">true=작동</param>
        public void doBroadCasting(bool broadCast)
        {
            this.broadCasting = broadCast;
        }

        /// <summary>
        /// 차단기 작동 여부
        /// </summary>
        /// <param name="gateControl">true=작동</param>
        /// <param name="gateClose">true=Close</param>
        public void doGateControl(bool gateControl, bool gateClose = true)
        {
            this.gateControl = gateControl;
            this.gateClose = gateClose;
        }

        /// <summary>
        /// NDMS 연동 여부
        /// </summary>
        /// <param name="DsCode">DsCode(필수)</param>
        public void useNdms(string DsCode)
        {
            this.DsCode = DsCode;
            sendNdms = true;
        }

        public void Start()
        {
            try
            {
                using (MYSQL_T mysql = new MYSQL_T(db_local_conf)) 
                { 
                    WB_ISUMENT_DAO ment_dao = new WB_ISUMENT_DAO(mysql);
                    ment_vo = ment_dao.Select();
                }

                switch(vo.gb_obsv)
                {
                    case "17":
                        BroadCast();
                        break;

                    case "18":
                        Display();
                        break;

                    case "20":
                        Gate();
                        break;

                    default:
                        return;

                }
            }
            catch { throw; }
        }

        public void BroadCast()
        {
            if (level < 1 || broadCasting == false)
            {
                return;
            }

            using (MYSQL_T mysql = new MYSQL_T(db_local_conf))
            {
                WB_BRDSEND_DAO brdsend_dao = new WB_BRDSEND_DAO(mysql);
                WB_BRDSEND_VO brdsend_vo = new WB_BRDSEND_VO()
                {
                    Cd_dist_obsv = vo.Cd_dist_obsv,
                    Rcmd = "B010",
                    Parm1 = "00000000",
                    Parm2 = "1",
                    Parm3 = ment_vo.BrdMent[level] ?? $"경보 {level}단계 예경보 발령 테스트 중 입니다",
                    BStatus = "start",
                    RegDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };

                brdsend_dao.Insert(brdsend_vo);
            }
        }

        public void Display()
        {
            List<string> Msg_boardList = new List<string>();
            using (MYSQL_T mysql = new MYSQL_T(db_local_conf))
            {
                WB_DISSEND_DAO dissend_dao = new WB_DISSEND_DAO(mysql);
                WB_DISSEND_VO dissend_vo = new WB_DISSEND_VO()
                {
                    Cd_dist_obsv = vo.Cd_dist_obsv,
                    Rcmd = level > 0 ? "D090" : "D060",
                    Parm1 = level > 0 ? $"level{level}" : "00",
                    BStatus = "start",
                    RegDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };
                dissend_dao.Insert(dissend_vo);

                if (level > 0)
                {
                    string[] DisCodeList = ment_vo.DisMent[level].Split(',');
                    WB_DISPLAY_DAO dis_dao = new WB_DISPLAY_DAO(mysql);
                    foreach (var DisCode in DisCodeList)
                    {
                        WB_DISPLAY_VO dis_vo = dis_dao.Select($"DisCode = '{DisCode}'").FirstOrDefault();
                        string board_message = System.Text.RegularExpressions.Regex.Replace(dis_vo.HtmlData, @"<(.|\n)*?>", string.Empty);
                        board_message = board_message.Replace("&nbsp;", string.Empty);

                        Msg_boardList.Add(board_message);
                    }
                }
            }

            if (sendNdms == false)
            {
                return;
            }

            using (MYSQL_T mysql = new MYSQL_T(db_ndms_conf))
            {
                NDMS_BOARD_DAO dao = new NDMS_BOARD_DAO(mysql);
                foreach (string Msg_board in Msg_boardList)
                {
                    NDMS_BOARD_VO display_vo = new NDMS_BOARD_VO()
                    {
                        FlCode = DsCode,
                        Cd_dist_board = vo.Cd_dist_obsv,
                        Nm_dist_board = vo.Nm_dist_obsv,
                        Comm_sttus = vo.LastStatus.ToLower() == "ok" ? "1" : "0",
                        Msg_board = Msg_board,
                        Lat = vo.Lat,
                        Lon = vo.Lon,
                        Use_YN = vo.Use_YN
                    };
                    dao.Insert(display_vo);
                }
            }
        }

        public void Gate()
        {
            if (gateControl == false)
            {
                return;
            }

            using (MYSQL_T mysql = new MYSQL_T(db_local_conf))
            {
                WB_GATECONTROL_DAO gate_dao = new WB_GATECONTROL_DAO(mysql);
                WB_GATECONTROL_VO gate_vo = new WB_GATECONTROL_VO()
                {
                    Cd_dist_obsv = vo.Cd_dist_obsv,
                    Gate = level > 0 && gateClose ? "close" : "open",
                    GStatus = "start",
                    RegDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };

                gate_dao.Insert(gate_vo);
            }

            if (sendNdms == false) 
            {
                return;
            }

            using (MYSQL_T mysql = new MYSQL_T(db_ndms_conf))
            {
                NDMS_INTRCP_DAO gate_dao = new NDMS_INTRCP_DAO(mysql);
                NDMS_INTRCP_VO gate_vo = new NDMS_INTRCP_VO()
                {
                    FlCode = DsCode,
                    Cd_dist_intrcp = vo.Cd_dist_obsv,
                    Nm_dist_intrcp = vo.Nm_dist_obsv,
                    Gb_intrcp = vo.Cd_dist_obsv.Substring(1, 1) == "0" ? "1" : "2",
                    mod_intrcp = "2",
                    Comm_sttus = vo.LastStatus.ToLower() == "ok" ? "1" : "0",
                    intrcp_sttus = level > 0 && gateClose ? "2" : "1",
                    Lat = vo.Lat,
                    Lon = vo.Lon,
                    Use_YN = vo.Use_YN
                };
                gate_dao.Insert(gate_vo);
            }
        }
    }

    public class SendSMS
    {
        DB_CONF db_local_conf = new DB_CONF("LOCAL");

        public WB_ISUMENT_VO ment_vo = new WB_ISUMENT_VO();
        public WB_SMSUSER_VO vo = new WB_SMSUSER_VO();
        public string phone;
        public int level;

        public SendSMS(string phone, int level)
        {
            this.level = level;
            this.phone = phone;

            Start();
        }

        public void Start()
        {
            if (level < 1)
            {
                return;
            }

            using (MYSQL_T mysql = new MYSQL_T(db_local_conf))
            {
                WB_ISUMENT_DAO ment_dao = new WB_ISUMENT_DAO(mysql);
                ment_vo = ment_dao.Select();

                WB_SENDMESSAGE_DAO smssend_dao = new WB_SENDMESSAGE_DAO(mysql);
                WB_SENDMESSAGE_VO smssend_vo = new WB_SENDMESSAGE_VO()
                {
                    PhoneNum = phone,
                    SendMessage = ment_vo.SMSMent[level] ?? $"경보 {level}단계 문자 발송 테스트 중 입니다",
                    SendStatus = "start",
                    RegDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };

                smssend_dao.Insert(smssend_vo);
            }
        }
    }
}
