using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wLib;
using wLib.DB;

namespace CriticalControl
{
    public class SendNdmsEquip
    {
        DB_CONF db_conf = new DB_CONF("NDMS");

        public NDMS_EQUIP_VO vo = new NDMS_EQUIP_VO();

        public bool movement;

        public SendNdmsEquip(NDMS_EQUIP_VO vo, bool movement)
        {
            this.movement = movement;
            this.vo = vo;

            Insert();
        }

        public void Insert() 
        {
            if (vo == null)
            {
                return;
            }

            try
            {
                switch(vo.gb_obsv)
                {
                    case "18":
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
            catch { }
        }

        public void Display()
        {
        }

        public void Gate()
        {
            using (MYSQL_T mysql = new MYSQL_T(db_conf))
            {
                NDMS_INTRCP_DAO dao = new NDMS_INTRCP_DAO(mysql);
                NDMS_INTRCP_VO gate_vo = new NDMS_INTRCP_VO()
                {
                    FlCode = vo.Dscode,
                    Cd_dist_intrcp = vo.Cd_dist_obsv,
                    Nm_dist_intrcp = vo.Nm_dist_obsv,
                    Gb_intrcp = vo.Cd_dist_obsv.Substring(1, 1) == "0" ? "1" : "2",
                    mod_intrcp = "2",
                    Comm_sttus = vo.LastStatus.ToLower() == "ok" ? "1" : "0",
                    intrcp_sttus = movement ? "2" : "1",
                    Use_YN = vo.Use_YN
                };

                dao.Insert(gate_vo);
            }
        }
    }
}
