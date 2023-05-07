using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using wLib;
using wLib.DB;

namespace CriticalControl.page
{
    public partial class mainPage : Form
    {
        List<WB_ISUALERTGROUP_VO> group_list = new List<WB_ISUALERTGROUP_VO> ();
        List<WB_ISUALERT_VO> alert_list = new List<WB_ISUALERT_VO> ();
        List<WB_ISUALERTVIEW_VO> area_list = new List<WB_ISUALERTVIEW_VO> ();

        // 설정 파일
        // DB
        DB_CONF db_local_conf = new DB_CONF("LOCAL");

        // TABLE
        TABLE_CONF table_conf = new TABLE_CONF();

        // TEMP
        //TODO: 5분동안 해당 Lv 유지 할 경우 경보발령, 차후 .ini에 기록해서 Setting Page에서 관리 할 예정
        public int term = 5;

        private async Task Process()
        {
            GetConfig();

            await CreateTable();

            await GetGroupList();

            await GetAlertStatus();

            // await SetEquip();
        }

        private void GetConfig()
        {
            // DB
            db_local_conf.ReadConfig();

            // TABLE
            table_conf.ReadConfig();
        }

        private async Task CreateTable()
        {
            if (db_local_conf.used == false)
            {
                return;
            }

            await Task.Run(() =>
            {
                try
                {
                    using (MYSQL_T mysql = new MYSQL_T(db_local_conf))
                    {
                        if (table_conf.wb_used == true)
                        {
                            new WB_ISSUESTATUS_DAO(mysql).Create();
                            new WB_ISUALERTGROUP_DAO(mysql).Create();
                            new WB_ISUALERT_DAO(mysql).Create();
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Warning($"{GetType()}::{ex.Message}");
                }
            });
        }

        private async Task GetGroupList()
        {
            if (db_local_conf.used == false)
            {
                return;
            }

            await Task.Run(() =>
            {
                try
                {
                    using (MYSQL_T mysql = new MYSQL_T(db_local_conf))
                    {
                        List<WB_ISUALERTGROUP_VO> new_group_list = new List<WB_ISUALERTGROUP_VO>();
                        List<WB_ISUALERT_VO> new_alert_list = new List<WB_ISUALERT_VO>();

                        // 경보그룹
                        if (table_conf.wb_used)
                        {
                            WB_ISUALERTGROUP_DAO group_dao = new WB_ISUALERTGROUP_DAO(mysql);
                            WB_ISUALERT_DAO alert_dao = new WB_ISUALERT_DAO(mysql);

                            new_group_list = group_dao.Select("AltUse = 'Y'").ToList();
                            foreach (WB_ISUALERTGROUP_VO vo in new_group_list)
                            {
                                // List에 데이터 추가
                                if (group_list.Exists(x => x.GCode == vo.GCode) == false)
                                {
                                    group_list.Add(vo);
                                }

                                // 데이터 갱신
                                var group_list_vo = group_list.Find(x => x.GCode == vo.GCode);
                                {
                                    vo.NowLevel = group_list_vo.NowLevel;
                                    vo.AlertDate = group_list_vo.AlertDate;
                                    group_list_vo.SetData(vo);
                                }
                            }

                            // 데이터 삭제
                            for (int i = group_list.Count - 1; i >= 0; i--)
                            {
                                var group_list_vo = group_list[i];

                                if (new_group_list.Exists(x => x.GCode == group_list_vo.GCode) == false)
                                {
                                    group_list.RemoveAt(i);
                                }
                            }

                            // 장비임계치
                            new_alert_list = alert_dao.Select().ToList();
                            foreach (WB_ISUALERT_VO vo in new_alert_list)
                            {
                                // List에 데이터 추가
                                if (alert_list.Exists(x => x.AltCode == vo.AltCode) == false)
                                {
                                    alert_list.Add(vo);
                                }

                                // 데이터 갱신
                                var alert_list_vo = alert_list.Find(x => x.AltCode == vo.AltCode);
                                {
                                    alert_list_vo.SetData(vo);
                                }
                            }

                            // 데이터 삭제
                            for (int i = alert_list.Count - 1; i >= 0; i--)
                            {
                                var alert_list_vo = alert_list[i];

                                if (new_alert_list.Exists(x => x.AltCode == alert_list_vo.AltCode) == false)
                                {
                                    alert_list.RemoveAt(i);
                                }
                            }
                        }
                    }

                    { //VIEW 처리
                        foreach (var vo in group_list) 
                        {
                            foreach (string AltCode in vo.AltCodeList) 
                            {
                                if (alert_list.Exists(x => x.AltCode == AltCode))
                                {
                                    var alert_list_vo = alert_list.Find(x => x.AltCode == AltCode);
                                    if (area_list.Exists(x => x.GCode == vo.GCode) && area_list.Exists(x => x.AltCode == AltCode))
                                    { 
                                        // 해당 경보그룹의 장비임계치가 View 리스트에 있는 경우 데이터 갱신
                                        WB_ISUALERTVIEW_VO area_list_vo = area_list.Find(x => x.AltCode == AltCode);
                                        {
                                            area_list_vo.NowLevel = vo.NowLevel;
                                            area_list_vo.AltDate = vo.AltDate;

                                            area_list_vo.AltName = alert_list_vo.EquType.ToLower() == "rain" ? $"{alert_list_vo.EquType}({alert_list_vo.RainTime})" : alert_list_vo.EquType;
                                        }
                                    }
                                    else
                                    { 
                                        // 이 외 경보그룹이 없거나, 경보그룹은 있지만 장비임계치가 없는 경우 View 리스트에 추가
                                        WB_ISUALERTVIEW_VO view_vo = new WB_ISUALERTVIEW_VO()
                                        {
                                            GCode = vo.GCode,
                                            GName = vo.GName,
                                            NowLevel = vo.NowLevel,
                                            AltDate = vo.AltDate,

                                            AltCode = alert_list_vo.AltCode,
                                            AltName = alert_list_vo.EquType.ToLower() == "rain" ? $"{alert_list_vo.EquType}({alert_list_vo.RainTime})" : alert_list_vo.EquType
                                        };

                                        area_list.Add(view_vo);
                                    }
                                }
                            }
                        }

                        // View 리스트에 있는데 DB에 삭제된 정보 View 리스트에서도 데이터 삭제
                        for (int i = area_list.Count - 1; i >= 0; i--) 
                        {
                            WB_ISUALERTVIEW_VO area_list_vo = area_list[i];

                            if (group_list.Exists(x => x.GCode == area_list_vo.GCode) == false)
                            { 
                                // 경보그룹 자체가 삭제된 List 삭제
                                area_list.RemoveAt(i);
                            }
                            else
                            {
                                WB_ISUALERTGROUP_VO group_list_vo = group_list.Find(x => x.GCode == area_list_vo.GCode);
                                if (group_list.Exists(x => x.GCode == area_list_vo.GCode) && group_list_vo.AltCodeList.Contains(area_list_vo.AltCode) == false)
                                { 
                                    // 경보그룹은 있으나 AltCode에서 빠진 List 삭제
                                    area_list.RemoveAt(i);
                                }
                            }
                        }
                    }

                    area_list = area_list.OrderBy(x => int.Parse(x.GCode)).ToList();
                    dataGridView_Refresh();
                }
                catch { }
            });
        }

        private async Task GetAlertStatus()
        {
            if (db_local_conf.used == false)
            {
                return;
            }

            await Task.Run(() =>
            {
                try
                {
                    using (MYSQL_T mysql = new MYSQL_T(db_local_conf))
                    {
                        WB_ISUALERT_DAO dao = new WB_ISUALERT_DAO(mysql);
                        alert_list = dao.Select().ToList();

                        foreach (var vo in alert_list)
                        {
                            // wb_{type}dis 에서 현재 데이터 가져오기
                            int minute = DateTime.Now.Minute;
                            string[] data;

                            WB_DATA_DTO dto = new WB_DATA_DTO()
                            {
                                Cd_dist_obsv = vo.Cd_dist_obsv,
                                Datatime = DateTime.Now
                            };

                            switch (vo.EquType.ToLower())
                            {
                                case "rain":
                                    WB_DATA_RAIN_DAO rain_dao = new WB_DATA_RAIN_DAO(mysql);
                                    dto.Type = WB_DATA_TYPE.RAIN;

                                    vo.NowData = rain_dao.SELECT_dis(dto, vo.RainTime);
                                    break;

                                case "water":
                                    WB_DATA_WATER_DAO water_dao = new WB_DATA_WATER_DAO(mysql);
                                    dto.Type = WB_DATA_TYPE.WATER;

                                    vo.NowData = water_dao.SELECT_dis(dto);
                                    break;

                                case "dplace": // NowData = {누적}/{속도} (10/1)
                                    WB_DATA_DPLACE_DAO dplace_dao = new WB_DATA_DPLACE_DAO(mysql);
                                    dto.Type = WB_DATA_TYPE.DPLACE;

                                    vo.NowData = dplace_dao.SELECT_dis(dto);
                                    break;

                                case "soil":
                                    WB_DATA_SOIL_DAO soil_dao = new WB_DATA_SOIL_DAO(mysql);
                                    dto.Type = WB_DATA_TYPE.SOIL;

                                    vo.NowData = soil_dao.SELECT_dis(dto);

                                    /* wb_soildis TABLE이 없을 경우 사용
                                    data = soil_dao.SELECT_1min(dto);
                                    vo.NowData = data[minute];
                                    // */
                                    break;

                                case "snow":
                                    WB_DATA_SNOW_DAO snow_dao = new WB_DATA_SNOW_DAO(mysql);
                                    dto.Type = WB_DATA_TYPE.SNOW;

                                    vo.NowData = snow_dao.SELECT_dis(dto);
                                    break;

                                case "tilt":
                                    WB_DATA_TILT_DAO tilt_dao = new WB_DATA_TILT_DAO(mysql);
                                    dto.Type = WB_DATA_TYPE.TILT;

                                    vo.NowData = tilt_dao.SELECT_dis(dto);

                                    /* wb_tiltdis TABLE이 없을 경우 사용
                                    data = tilt_dao.SELECT_1min(dto);
                                    vo.NowData = data[minute];
                                    // */
                                    break;

                                case "flood": // NowData = {침수}/{침수위} (000/0.05)
                                    WB_DATA_FLOOD_DAO flood_dao = new WB_DATA_FLOOD_DAO(mysql);
                                    dto.Type = WB_DATA_TYPE.FLOOD;

                                    vo.NowData = flood_dao.SELECT_dis(dto);

                                    /* wb_flooddis TABLE이 없을 경우 사용
                                    WB_DATA_WATER_DAO floodwater_dao = new WB_DATA_WATER_DAO(mysql);
                                    data = flood_dao.SELECT_1min(dto);
                                    string nowFloodData = data[minute];

                                    data = floodwater_dao.SELECT_1min(dto);
                                    string nowFloodWaterData = data[minute];

                                    vo.NowData = $"{nowFloodData}/{nowFloodWaterData}";
                                    // */
                                    break;
                            }

                            // wb_isualert에서 L1Std ~ L4Std 값과 현재 데이터 비교
                            // TODO: (작동시간이 1분을 안넘을 경우 ? 발령 대기 시간 = ChkCount * 1분 : 발령 대기 시간 = ChkCount * 작동시간) 발령 대기 시간 >= term || 발령 대기 시간 <= term 의 경우만 Level이 변경 예정
                            // TODO: Ex) 현재 Normal상태이고 현재값이 1단계와 2단계를 번갈아가면서 들어올경우 ChkCount가 계속 초기화해서 발령이 안될 수 있기에 예외처리 필요
                            for (int i = 1; i <= 4; i++)
                            {
                                if (vo.Use[i].ToLower() == "on" && vo.NowData != null)
                                {
                                    switch (vo.EquType.ToLower())
                                    {
                                        case "rain":
                                        case "water":
                                        case "soil":
                                        case "snow":
                                        case "tilt":
                                            if (double.TryParse(vo.Std[i], out double std) == false)
                                            {
                                                throw new Exception($"L{i}Std 값이 잘못되었습니다.");
                                            }

                                            if (double.TryParse(vo.NowData, out double minuteData) == false)
                                            {
                                                throw new Exception($"wb_{vo.EquType}dis Table에 값을 확인해주세요.");
                                            }

                                            if (minuteData >= std) vo.NowType = i;
                                            break;

                                        case "dplace":
                                            string[] totalStd = vo.Std[i].Split('/');
                                            string[] totalData = vo.NowData.Split('/');

                                            if (double.TryParse(totalStd[0], out double sum_std) == false || double.TryParse(totalStd[1], out double speed_std) == false)
                                            {
                                                throw new Exception($"L{i}Std 값이 잘못되었습니다.");
                                            }

                                            if (double.TryParse(totalData[0], out double sum_data) == false || double.TryParse(totalData[1], out double speed_data) == false)
                                            {
                                                throw new Exception($"wb_{vo.EquType}dis Table에 값을 확인해주세요.");
                                            }

                                            if (sum_data >= sum_std || speed_data >= speed_std) vo.NowType = i;
                                            break;

                                        case "flood": 
                                            //TODO: 침수의 경우 Cm를 하드코딩하기에 5가 들어가야할지 0.05가 들어가야할지 Conf로 빼서 관리할지도 생각해야함
                                            double waterStd;
                                            if (i == 1) waterStd = 5;
                                            else if (i == 2) waterStd = 13;
                                            else if (i == 3) waterStd = 21;
                                            else break;

                                            //TODO: 전체적인 시퀀스 수정 필요. 현재 침수의 경우 wb_isualert에서 L1Std ~ L3Std가 (string)1 ~ 3으로 고정
                                            string[] floodData = vo.NowData.Split('/');

                                            string flood_data = floodData[0];
                                            if (double.TryParse(floodData[1], out double flood_water_data) == false)
                                            {
                                                throw new Exception($"wb_{vo.EquType}dis Table에 값을 확인해주세요.");
                                            }

                                            if (vo.Std[i] == i.ToString() && waterStd > 0) 
                                            {
                                                if (flood_water_data >= waterStd && flood_data[i] == '1') vo.NowType = i;
                                            }
                                            break;
                                    }
                                }
                            }

                            // 현재 Level과 과거 Level 비교 if(현재, 과거 Lv이 같고 0(Normal)이 아닌경우 ChkCount++)
                            if (vo.NowType == vo.NowType)
                            {
                                if (vo.NowType == 0) vo.ChkCount = 0;
                                else vo.ChkCount += 1;
                            }
                            else
                            {
                                vo.ChkCount = 0;
                            }

                            //TODO: wb_isualert Table UPDATE 로직 추가
                            string sql = $"UPDATE wb_isualert SET NowType = {vo.NowType}, ChkCount = {vo.ChkCount} WHERE CD_DIST_OBSV = '{vo.Cd_dist_obsv}'";
                            if (dto.Type == WB_DATA_TYPE.RAIN) sql += $" AND RainTime = '{vo.RainTime}'";
                            log.Info(sql);
                        }

                        // UI 갱신
                        foreach (var vo in alert_list)
                        {
                            if (area_list.Exists(x => x.AltCode == vo.AltCode)) 
                            {
                                WB_ISUALERTVIEW_VO view_vo = area_list.Find(x => x.AltCode == vo.AltCode);
                                {
                                    view_vo.NowCritical = vo.NowType > 0 ? vo.Std[vo.NowType] : "-";
                                    view_vo.NextCritical = vo.NowType < 4 ? vo.Std[vo.NowType + 1] : "-";
                                    view_vo.NowData = vo.NowData ?? "-";
                                }
                            }
                        }

                        dataGridView_Refresh();
                    }
                }
                catch { }
            });
        }

        private async Task SetEquip()
        {
            if (db_local_conf.used == false)
            {
                return;
            }

            await Task.Run(() =>
            {
                try
                {
                    using (MYSQL_T mysql = new MYSQL_T(db_local_conf))
                    {
                        foreach (var vo in group_list)
                        {
                            int level = 0;
                            foreach(string AltCode in vo.AltCodeList)
                            {
                                if (alert_list.Exists(x => x.AltCode == AltCode))
                                {
                                    WB_ISUALERT_VO alert_list_vo = alert_list.Find(x => x.AltCode == AltCode);
                                    
                                    if (alert_list_vo.NowType > level)
                                    {
                                        level = alert_list_vo.NowType;
                                    }
                                }
                            }

                            if (level > vo.NowLevel)
                            {
                                
                            }
                        }
                    }
                }
                catch { }
            });
        }

        private void dataGridView_Refresh()
        {
            try
            {
                if (groupDataGridView.InvokeRequired)
                {
                    groupDataGridView.Invoke(new Action(() =>
                    {
                        dataBindingSource.ResetBindings(false);
                    }));
                }
                else
                {
                    dataBindingSource.ResetBindings(false);
                }
            }
            catch { }
        }
    }
}
