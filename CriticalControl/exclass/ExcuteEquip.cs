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

        public WB_ISUMENT_VO ment_vo = new WB_ISUMENT_VO();
        public WB_EQUIP_VO vo = new WB_EQUIP_VO();

        public int level;
        public bool doBroadCast;
        public bool doGateClose;

        public ExcuteEquip(WB_EQUIP_VO vo, int level, bool broadcastBool = true, bool gateCloseBool = true)
        {
            //TODO: vo.Data에 해당 장비를 동작시킬지 True or False를 String으로 받아 실행 or (방송, 차단기) 동작 여부를 변수로 받아 실행
            this.level = level;
            this.vo = vo;
            this.doBroadCast = broadcastBool;
            this.doGateClose = gateCloseBool;

            Alert();
        }

        public void Alert()
        {
            try
            {
                using (MYSQL_T mysql = new MYSQL_T(db_local_conf)) 
                { 
                    WB_ISUMENT_DAO ment_dao = new WB_ISUMENT_DAO(mysql);
                    ment_dao.Create();

                    ment_vo = ment_dao.Select("MentCode = 1").FirstOrDefault();
                    if (ment_vo == null) 
                    {
                        ment_dao.Insert("INSERT INTO wb_isument (MentCode) VALUES (1)");
                        ment_vo = ment_dao.Select("MentCode = 1").FirstOrDefault();
                    }
                }

                switch(vo.gb_obsv)
                {
                    case "18":
                        BroadCast();
                        break;

                    case "19":
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
            if (level < 1 || doBroadCast == false)
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
            }
        }

        public void Gate()
        {
            using (MYSQL_T mysql = new MYSQL_T(db_local_conf))
            {
                WB_GATECONTROL_DAO gate_dao = new WB_GATECONTROL_DAO(mysql);
                WB_GATECONTROL_VO gate_vo = new WB_GATECONTROL_VO()
                {
                    Cd_dist_obsv = vo.Cd_dist_obsv,
                    Gate = level > 0 && doGateClose ? "close" : "open",
                    GStatus = "start",
                    RegDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
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
        public int level;

        public SendSMS(WB_SMSUSER_VO vo, int level)
        {
            this.level = level;
            this.vo = vo;

            SMS();
        }

        public void SMS()
        {
            if (level < 1)
            {
                return;
            }

            using (MYSQL_T mysql = new MYSQL_T(db_local_conf))
            {
                WB_SENDMESSAGE_DAO smssend_dao = new WB_SENDMESSAGE_DAO(mysql);
                WB_SENDMESSAGE_VO smssend_vo = new WB_SENDMESSAGE_VO()
                {
                    PhoneNum = vo.Phone,
                    SendMessage = ment_vo.SMSMent[level] ?? $"경보 {level}단계 문자 발송 테스트 중 입니다",
                    SendStatus = "start",
                    RegDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };

                smssend_dao.Insert(smssend_vo);
            }
        }
    }
}
