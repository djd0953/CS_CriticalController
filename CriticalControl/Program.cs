using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using wLib;
using wLib.DB;

namespace CriticalControl
{
    internal static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Console.WriteLine("Content-type: text/html\r\n");
            string parm = Environment.GetEnvironmentVariable("QUERY_STRING");
            StringBuilder sb = new StringBuilder();

            if (parm != null && parm.Length > 0)
            {
                
                sb.Append("{\r\n");

                try
                {
                    Dictionary<string, string> paramToDic = parm.Split('&').Select(x => x.Split('=')).ToDictionary(x => x[0].Trim(), x => x.LastOrDefault()?.Trim());
                    int lv = -1;
                    string smsGCode = null;

                    try
                    {
                        foreach (var dic in paramToDic)
                        {
                            if (dic.Key == "level")
                            {
                                if (int.TryParse(dic.Value, out lv) == false) throw new Exception("Param Error");
                            }

                            if (dic.Key == "sms")
                            {
                                smsGCode = dic.Value;
                            }
                        }

                        if (lv < 0) throw new Exception("Param Error");
                    }
                    catch(Exception ex) 
                    {
                        sb.Append("\"resultCode\":400,\r\n");
                        sb.Append($"\"resultMessage\":\"{ex}\"\r\n");
                        throw;
                    }

                    try
                    {
                        DB_CONF db_local_conf = new DB_CONF("LOCAL");
                        using (MYSQL_T mysql = new MYSQL_T(db_local_conf))
                        {
                            WB_EQUIP_DAO equip_dao = new WB_EQUIP_DAO(mysql);
                            IEnumerable<WB_EQUIP_VO> equip_list = equip_dao.Select();

                            foreach(WB_EQUIP_VO vo in equip_list)
                            {
                                new ExcuteEquip(vo, lv);
                            }

                            if (smsGCode != null)
                            {
                                WB_SMSUSER_DAO sms_dao = new WB_SMSUSER_DAO(mysql);
                                IEnumerable<WB_SMSUSER_VO> sms_list = sms_dao.Select($"GCode IN ({smsGCode})");

                                foreach (WB_SMSUSER_VO vo in sms_list)
                                {
                                    new SendSMS(vo, lv);
                                }
                            }
                        }

                        sb.Append("\"resultCode\":200,\r\n");
                        sb.Append($"\"resultMessage\":\"{lv}\"\r\n");
                    }
                    catch(Exception ex)
                    {
                        sb.Append("\"resultCode\":400,\r\n");
                        sb.Append($"\"resultMessage\":\"{ex}\"\r\n");
                        throw;
                    }
                }
                finally
                {
                    sb.Append("}");
                    string rtv = sb.ToString();
                    Console.WriteLine(rtv);
                }
            }
            else
            {
                _ = WLIB_DLL.Instance;

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new mainForm());
            }
        }
    }
}
