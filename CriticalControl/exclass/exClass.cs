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
        // 임계치 확인을 위한 Group, Alert, UI List
        List<WB_ISUALERTGROUP_VO> group_list = new List<WB_ISUALERTGROUP_VO> ();
        List<WB_ISUALERT_VO> alert_list = new List<WB_ISUALERT_VO> ();
        List<WB_ISUALERTVIEW_VO> area_list = new List<WB_ISUALERTVIEW_VO> ();

        // 경보발령시 작동을 위한 Equip/SMS List
        List<WB_EQUIP_VO> equip_list = new List<WB_EQUIP_VO> ();
        List<WB_SMSUSER_VO> smsuser_list = new List<WB_SMSUSER_VO> ();
        List<WB_PARKCARNOW_VO> car_list = new List<WB_PARKCARNOW_VO> ();
        string FloodSMSMent = null;

        // 설정 파일
        // DB
        DB_CONF db_local_conf = new DB_CONF("LOCAL");
        DB_CONF db_ndms_conf = new DB_CONF("NDMS");

        // TABLE
        TABLE_CONF table_conf = new TABLE_CONF();

        // CONF
        CRI_CONF cr_conf = new CRI_CONF();

        public DateTime currentTime;
        public int minute;
        public int minuteIdx;

        private async Task Process()
        {
            try
            {
                GetConfig();

                await CreateTable();

                await GetGroupList();

                await GetData();

                await GetAlertStatus();

                await SetEquip();

                await SendAlertInfoToNDMS();
            }
            catch { }
        }

        private void GetConfig()
        {
            // DB
            db_local_conf.ReadConfig();

            // TABLE
            table_conf.ReadConfig();

            // CONF
            cr_conf.ReadConfig();
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
                            new WB_ISUALERTGROUP_DAO(mysql).Create();
                            new WB_ISUALERT_DAO(mysql).Create();
                            new WB_ISUMENT_DAO(mysql).Create();
                            new WB_ISULIST_DAO(mysql).Create();
                            
                            new WB_SMSUSER_DAO(mysql).Create();
                            new WB_PARKSMSMENT_DAO(mysql).Create();
                            new WB_PARKCARNOW_DAO(mysql).Create();
                            new WB_PARKSMSLIST_DAO(mysql).Create();
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
                    { // 시각 동기화
                        currentTime = DateTime.Now.AddMinutes(cr_conf.dataInsertTerm * -1);
                        minute = currentTime.Minute;
                        minuteIdx = minute % 10;
                    }

                    using (MYSQL_T mysql = new MYSQL_T(db_local_conf))
                    {
                        List<WB_ISUALERTGROUP_VO> new_group_list = new List<WB_ISUALERTGROUP_VO>();
                        List<WB_ISUALERT_VO> new_alert_list = new List<WB_ISUALERT_VO>();
                        bool isFloodSMSSend = false;

                        // 경보그룹
                        if (table_conf.wb_used)
                        {
                            WB_ISUALERTGROUP_DAO group_dao = new WB_ISUALERTGROUP_DAO(mysql);
                            WB_ISUALERT_DAO alert_dao = new WB_ISUALERT_DAO(mysql);

                            WB_EQUIP_DAO equip_dao = new WB_EQUIP_DAO(mysql);

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
                                    vo.IsuCode = group_list_vo.IsuCode;
                                    vo.NowLevel = group_list_vo.NowLevel;
                                    vo.AlertDate = group_list_vo.AlertDate;
                                    group_list_vo.SetData(vo);
                                }

                                foreach (string floodsms in vo.FloodSMSAuto)
                                {
                                    if (floodsms != null && floodsms.ToLower() == "on")
                                    {
                                        isFloodSMSSend = true;
                                    }
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
                                    vo.Data = alert_list_vo.Data;
                                    vo.SecondData = alert_list_vo.SecondData;
                                    vo.Dplace_stand = alert_list_vo.Dplace_stand;

                                    for (int i = 1; i <= 4; i++)
                                    {
                                        switch (vo.EquType.ToLower())
                                        {
                                            case "rain":
                                            case "water":
                                            case "soil":
                                            case "snow":
                                            case "tilt":
                                                if (double.TryParse(vo.Std[i], out vo.firstAlert[i]) == false)
                                                {
                                                    continue;
                                                }
                                                break;

                                            case "dplace":
                                                string[] dplace_array = vo.Std[i].Split('/');

                                                if (double.TryParse(dplace_array[0], out vo.firstAlert[i]) == false) { }
                                                if (double.TryParse(dplace_array[1], out vo.secondAlert[i]) == false) { }

                                                break;

                                            case "flood":
                                                vo.firstAlert[i] = Convert.ToDouble(i);

                                                double alertVal = 0;
                                                double rb = 1;
                                                switch (i)
                                                {
                                                    case 1:
                                                        alertVal = 5;
                                                        break;

                                                    case 2:
                                                        alertVal = 13;
                                                        break;

                                                    case 3:
                                                        alertVal = 21;
                                                        break;

                                                    case 4:
                                                        alertVal = 0;
                                                        break;
                                                }

                                                if (table_conf.rb_used == true)
                                                {
                                                    WB_EQUIP_VO equip_vo = equip_dao.Select($"CD_DIST_OBSV = '{vo.Cd_dist_obsv}'").FirstOrDefault();
                                                    if (equip_vo != null)
                                                    {
                                                        if (double.TryParse(equip_vo.RainBit, out rb) == false) 
                                                        {
                                                            rb = 1;
                                                        }
                                                        else
                                                        {
                                                            rb *= 10;
                                                        }
                                                    }
                                                }

                                                vo.secondAlert[i] = alertVal * rb;
                                                break;
                                        }
                                    }

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

                            // Equip, SMS User, Car List 갱신
                            equip_list = equip_list = equip_dao.Select("USE_YN IN ('1')").ToList();

                            WB_SMSUSER_DAO smsuser_dao = new WB_SMSUSER_DAO(mysql);
                            smsuser_list = smsuser_dao.Select().ToList();

                            if (isFloodSMSSend)
                            {
                                string today = currentTime.ToString("yyyy-MM-dd HH:mm:ss");
                                string yesterday = currentTime.AddDays(-1).ToString("yyyy-MM-dd") + " 00:00:00";

                                WB_PARKCARNOW_DAO car_dao = new WB_PARKCARNOW_DAO(mysql);
                                car_list = car_dao.Select($"GateDate BETWEEN '{yesterday}' AND '{today}'").ToList();

                                // FloodSMSMent 갱신
                                WB_PARKSMSMENT_DAO carMent_dao = new WB_PARKSMSMENT_DAO(mysql);
                                WB_PARKSMSMENT_VO carMent_vo = carMent_dao.Select("SMSMentCode = 1").FirstOrDefault();

                                if (carMent_vo == null) 
                                {
                                    carMent_dao.Insert("INSERT INTO wb_parksmsment (SMSMentCode, Title, Content) VALUES (1, '침수위험알림', '둔치주차장 침수위험발생, 차량 이동주차 바랍니다.')");
                                    FloodSMSMent = "둔치주차장 침수위험발생, 차량 이동주차 바랍니다.";
                                }
                                else
                                {
                                    FloodSMSMent = carMent_vo.Content ?? "둔치주차장 침수위험발생, 차량 이동주차 바랍니다.";
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
                                    if (area_list.Exists(x => x.GCode == vo.GCode && x.AltCode == AltCode))
                                    { 
                                        // 해당 경보그룹의 장비임계치가 View 리스트에 있는 경우 데이터 갱신
                                        WB_ISUALERTVIEW_VO area_list_vo = area_list.Find(x => x.AltCode == AltCode);
                                        {
                                            area_list_vo.GName = vo.GName;
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
                                            AltDate = vo.AlertDate,

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
                                if (group_list.Exists(x => x.GCode == area_list_vo.GCode && x.AltCodeList.Contains(area_list_vo.AltCode) == false))
                                { 
                                    // 경보그룹은 있으나 AltCode에서 빠진 List 삭제
                                    area_list.RemoveAt(i);
                                }
                            }
                        }
                    }

                    area_list.Sort();
                    dataGridView_Refresh();
                }
                catch { }
            });
        }

        private async Task GetData()
        {
            if (db_local_conf.used == false)
            {
                return;
            }

            await Task.Run(() =>
            {
                using (MYSQL_T mysql = new MYSQL_T(db_local_conf))
                {
                    // wb_{type}dis 에서 현재 데이터 가져오기
                    foreach (var vo in alert_list)
                    {
                        WB_DATA_DTO dto = new WB_DATA_DTO()
                        {
                            Cd_dist_obsv = vo.Cd_dist_obsv,
                            Datatime = currentTime
                        };

                        try
                        {
                            string table = $"wb_{vo.EquType.ToLower()}dis";
                            string sql = $"SHOW TABLES LIKE '{table}'";

                            try
                            {
                                if (mysql.ExecuteScalar(sql) == null)
                                {
                                    throw new Exception($"{table} Table이 없어 1Min Table에서 Data 가공");
                                }

                                switch (vo.EquType.ToLower())
                                {
                                    case "rain":
                                        WB_DATA_RAIN_DAO rain_dao = new WB_DATA_RAIN_DAO(mysql);
                                        dto.Type = WB_DATA_TYPE.RAIN;

                                        if (double.TryParse(rain_dao.SELECT_dis(dto, vo.RainTime), out vo.Data[minuteIdx]) == false)
                                        {
                                            throw new Exception($"{table} Table의 값을 확인하세요.");
                                        }
                                        break;

                                    case "water":
                                        WB_DATA_WATER_DAO water_dao = new WB_DATA_WATER_DAO(mysql);
                                        dto.Type = WB_DATA_TYPE.WATER;

                                        if (double.TryParse(water_dao.SELECT_dis(dto), out vo.Data[minuteIdx]) == false)
                                        {
                                            throw new Exception($"{table} Table의 값을 확인하세요.");
                                        }
                                        break;

                                    case "dplace": // NowData = {누적}/{속도} (10/1)
                                        WB_DATA_DPLACE_DAO dplace_dao = new WB_DATA_DPLACE_DAO(mysql);
                                        dto.Type = WB_DATA_TYPE.DPLACE;

                                        WB_EQUIP_DAO equip_dao = new WB_EQUIP_DAO(mysql);
                                        WB_EQUIP_VO equip_vo = equip_dao.Select($"CD_DIST_OBSV = '{vo.Cd_dist_obsv}'").FirstOrDefault();

                                        if (int.TryParse(equip_vo.SubObCount, out int subObCount) == false)
                                        {
                                            log.Info($"wb_equip 장비번호 {vo.Cd_dist_obsv}의 SubOBCount값을 확인하세요.");
                                            subObCount = 1;
                                        }

                                        double changeVal = 0;
                                        double speedVal = 0;
                                        for (int sub_obsv = 1; sub_obsv <= subObCount; sub_obsv++)
                                        {
                                            dto.Sub_obsv = sub_obsv.ToString();
                                            string dplace_dataset = dplace_dao.SELECT_dis(dto);
                                            if (string.IsNullOrEmpty(dplace_dataset))
                                            {
                                                throw new Exception($"{table} Table의 값을 확인하세요.");
                                            }

                                            string[] dplace_data = dplace_dataset.Split('/');
                                            int chk = 0;
                                            if (double.TryParse(dplace_data[0], out vo.Data[minuteIdx]) == false) { chk++; } 
                                            if (double.TryParse(dplace_data[1], out vo.SecondData[minuteIdx]) == false) { chk++; }

                                            if (chk < 2)
                                            {
                                                if (changeVal < vo.Data[minuteIdx]) changeVal = vo.Data[minuteIdx];
                                                if (speedVal < vo.SecondData[minuteIdx]) speedVal = vo.SecondData[minuteIdx];
                                            }
                                        }

                                        vo.Data[minuteIdx] = changeVal;
                                        vo.SecondData[minuteIdx] = speedVal;
                                        break;

                                    case "soil":
                                        WB_DATA_SOIL_DAO soil_dao = new WB_DATA_SOIL_DAO(mysql);
                                        dto.Type = WB_DATA_TYPE.SOIL;

                                        if (double.TryParse(soil_dao.SELECT_dis(dto), out vo.Data[minuteIdx]) == false)
                                        {
                                            throw new Exception($"{table} Table의 값을 확인하세요.");
                                        }
                                        break;

                                    case "snow":
                                        WB_DATA_SNOW_DAO snow_dao = new WB_DATA_SNOW_DAO(mysql);
                                        dto.Type = WB_DATA_TYPE.SNOW;

                                        if (double.TryParse(snow_dao.SELECT_dis(dto), out vo.Data[minuteIdx]) == false)
                                        {
                                            throw new Exception($"{table} Table의 값을 확인하세요.");
                                        }
                                        break;

                                    case "tilt":
                                        WB_DATA_TILT_DAO tilt_dao = new WB_DATA_TILT_DAO(mysql);
                                        dto.Type = WB_DATA_TYPE.TILT;

                                        if (double.TryParse(tilt_dao.SELECT_dis(dto), out vo.Data[minuteIdx]) == false)
                                        {
                                            throw new Exception($"{table} Table의 값을 확인하세요.");
                                        }
                                        break;

                                    case "flood": // NowData = {침수}/{침수위} (000/0.05)
                                        WB_DATA_FLOOD_DAO flood_dao = new WB_DATA_FLOOD_DAO(mysql);
                                        dto.Type = WB_DATA_TYPE.FLOOD;

                                        string flood_dataset = flood_dao.SELECT_dis(dto);
                                        if (string.IsNullOrEmpty(flood_dataset))
                                        {
                                            throw new Exception($"{table} Table의 값을 확인하세요.");
                                        }

                                        string[] flood_data = flood_dataset.Split('/');
                                        if (!string.IsNullOrEmpty(flood_data[0]) && double.TryParse(flood_data[1], out vo.SecondData[minuteIdx]) == false)
                                        {
                                            throw new Exception($"{table} Table의 값을 확인하세요.");
                                        }

                                        if (flood_data[0] != null)
                                        {
                                            double lv = 3;
                                            foreach (char s in flood_data[0])
                                            {
                                                if (s == '1')
                                                {
                                                    vo.Data[minuteIdx] = lv;
                                                    break;
                                                }

                                                lv--;
                                            }
                                        }
                                        break;
                                }
                            }
                            catch (Exception e)
                            {
                                //log.Info(e.Message);

                                // wb_{data}dis Table이 없을 경우 wb_{data}_1min에서 데이터를 가공하여 가져옴 (DataState Program의 리뉴얼이 없으면 사용)
                                table = $"wb_{vo.EquType.ToLower()}1min_{currentTime.ToString("yyyy")}";

                                // TEMP 변수
                                string[] data;

                                switch (vo.EquType.ToLower())
                                {
                                    case "water":
                                    case "soil":
                                    case "snow":
                                    case "tilt":
                                        sql = $"SELECT MRMin FROM {table} WHERE cd_dist_obsv = '{vo.Cd_dist_obsv}' AND RegDate = '{currentTime.ToString("yyyyMMddHH")}'";
                                        string res = mysql.ExecuteScalar<string>(sql);
                                        if (res == string.Empty || res == null)
                                        {
                                            // 없을 경우 생성
                                            data = new string[60];
                                        }
                                        else
                                        {
                                            // 있을 경우 업데이트
                                            data = res.Split('/');
                                            if (data.Length != 60)
                                            {
                                                // 기존 데이터를 신뢰할 수 없으므로 지우고 새로 생성
                                                data = new string[60];
                                            }
                                        }

                                        if (double.TryParse(data[minute], out vo.Data[minuteIdx]) == false)
                                        {
                                            throw new Exception($"{table} Table의 값을 확인하세요.");
                                        }
                                        break;

                                    case "rain":
                                        if (int.TryParse(vo.RainTime, out int rainTime) == false)
                                        {
                                            throw new Exception($"wb_alert 장비번호 {vo.Cd_dist_obsv}의 RainTime값을 확인하세요.");
                                        }

                                        // RainTime 시간만큼의 강우량을 더한 값 (이동 {RainTime}시간 강우량)
                                        WB_DATA_RAIN_DAO rain_dao = new WB_DATA_RAIN_DAO(mysql);
                                        dto.Type = WB_DATA_TYPE.RAIN;
                                        List<double> data_sum = new List<double>();
                                        for (int i = rainTime; i >= 0; i--)
                                        {
                                            dto.Datatime = currentTime.AddHours(i * -1);
                                            data = rain_dao.SELECT_1min(dto);

                                            foreach (string minuteData in data)
                                            {
                                                if (double.TryParse(minuteData, out double md))
                                                {
                                                    data_sum.Add(md);
                                                }
                                                else
                                                {
                                                    data_sum.Add(0); // 강우는 0이 데이터로 사용되지 않아서 0으로 하드코딩
                                                }
                                            }
                                        }

                                        vo.Data[minuteIdx] = data_sum.GetRange(currentTime.Minute + 1, rainTime * 60).Sum();
                                        break;

                                    case "dplace":
                                        WB_EQUIP_DAO equip_dao = new WB_EQUIP_DAO(mysql);
                                        WB_EQUIP_VO equip_vo = equip_dao.Select($"CD_DIST_OBSV = '{vo.Cd_dist_obsv}'").FirstOrDefault();

                                        if (int.TryParse(equip_vo.SubObCount, out int subObCount) == false)
                                        {
                                            log.Info($"wb_equip 장비번호 {vo.Cd_dist_obsv}의 SubOBCount값을 확인하세요.");
                                            subObCount = 1;
                                        }

                                        // 누적 변위는 초기 세팅값(dplace_stand)이 필요하기에 최초 Insert된 값으로 지정
                                        if (vo.Dplace_stand == null)
                                        {
                                            // 65536: 나올 수 없는 값 (65535 == FF FF)
                                            vo.Dplace_stand = Enumerable.Repeat<double>(65536, subObCount + 1).ToArray();
                                            for (int sub_obsv = 1; sub_obsv <= subObCount; sub_obsv++)
                                            {
                                                if (vo.Dplace_stand[sub_obsv] != 65536) break;

                                                sql = $"SELECT MRMin FROM {table} WHERE CD_DIST_OBSV = '{vo.Cd_dist_obsv}' AND SUB_OBSV = {sub_obsv} ORDER BY RegDate ASC LIMIT 100";
                                                DataTable resDT = mysql.ExecuteReader(sql);

                                                foreach (DataRow row in resDT.Rows)
                                                {
                                                    if (vo.Dplace_stand[sub_obsv] != 65536)
                                                        break;

                                                    data = row["MRMin"].ToString().Split('/');
                                                    foreach (string md in data)
                                                    {
                                                        if (double.TryParse(md, out double d) == false) continue;

                                                        // 50000 이상 값 || 0 은 오류값일 확률이 높아 배제
                                                        if (d != 0 || d < 50000)
                                                        {
                                                            vo.Dplace_stand[sub_obsv] = d;
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        // 현재 변위값 구하기
                                        WB_DATA_DPLACE_DAO dplace_dao = new WB_DATA_DPLACE_DAO(mysql);
                                        dto.Type = WB_DATA_TYPE.DPLACE;

                                        // 누적 변위
                                        double sumData = 0;
                                        dto.Datatime = currentTime;
                                        for (int sub_obsv = 1; sub_obsv <= subObCount; sub_obsv++)
                                        {
                                            if (vo.Dplace_stand[sub_obsv] == 65536)
                                                continue;

                                            dto.Sub_obsv = sub_obsv.ToString();
                                            data = dplace_dao.SELECT_1min(dto);
                                            if (double.TryParse(data[minute], out double nowData) == false)
                                            {
                                                log.Info($"{table} Table의 값을 확인하세요.(Sub_obsv: {sub_obsv})");
                                                continue;
                                            }

                                            if (sumData < Abs(nowData - vo.Dplace_stand[sub_obsv])) sumData = Abs(nowData - vo.Dplace_stand[sub_obsv]);
                                        }

                                        // 속도 변위
                                        double speedData = 0;

                                        for (int sub_obsv = 1; sub_obsv <= subObCount; sub_obsv++)
                                        {
                                            dto.Sub_obsv = sub_obsv.ToString();

                                            dto.Datatime = currentTime.AddHours(-1);
                                            data = dplace_dao.SELECT_1min(dto);
                                            if (double.TryParse(data[minute], out double beforeData) == false)
                                            {
                                                log.Info($"{table} Table의 값을 확인하세요.(Sub_obsv: {sub_obsv})");
                                                continue;
                                            }

                                            dto.Datatime = currentTime;
                                            data = dplace_dao.SELECT_1min(dto);
                                            if (double.TryParse(data[minute], out double nowData) == false)
                                            {
                                                log.Info($"{table} Table의 값을 확인하세요.(Sub_obsv: {sub_obsv})");
                                                continue;
                                            }

                                            if (speedData < Abs(beforeData - nowData)) speedData = Abs(beforeData - nowData);
                                        }

                                        vo.Data[minuteIdx] = sumData;
                                        vo.SecondData[minuteIdx] = speedData;
                                        break;

                                    case "flood":
                                        WB_DATA_FLOOD_DAO flood_dao = new WB_DATA_FLOOD_DAO(mysql);
                                        dto.Type = WB_DATA_TYPE.FLOOD;
                                        data = flood_dao.SELECT_1min(dto);

                                        if (data[minute] != null)
                                        {
                                            double lv = 3;
                                            foreach (char s in data[minute])
                                            {
                                                if (s == '1')
                                                {
                                                    vo.Data[minuteIdx] = lv;
                                                    break;
                                                }

                                                lv--;
                                            }
                                        }

                                        WB_DATA_WATER_DAO water_dao = new WB_DATA_WATER_DAO(mysql);
                                        dto.Type = WB_DATA_TYPE.WATER;
                                        data = water_dao.SELECT_1min(dto);

                                        if (double.TryParse(data[minute], out vo.SecondData[minuteIdx]) == false)
                                        {
                                            new Exception($"{table} Table의 값을 확인하세요.");
                                        }
                                        break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Info(ex.Message);
                            continue;
                        }
                    }
                }
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
                        foreach (var vo in alert_list)
                        {
                            int NowLevel = vo.NowType;

                            // 1단계 임계치보다 값이 낮다면 for문에 들어가지 않고 종료
                            bool isKeep = true;
                            bool isSecondKeep = true;
                            switch (vo.EquType.ToLower())
                            {
                                case "rain":
                                case "water":
                                case "soil":
                                case "snow":
                                case "tilt":
                                    for (int j = 0; j < cr_conf.criticalDataTerm; j++)
                                    {
                                        if (vo.Data[currentTime.AddMinutes(j * -1).Minute % 10] >= vo.firstAlert[1]) isKeep = false;
                                    }
                                    break;

                                case "dplace":
                                case "flood":
                                    for (int j = 0; j < cr_conf.criticalDataTerm; j++)
                                    {
                                        if (vo.Data[currentTime.AddMinutes(j * -1).Minute % 10] >= vo.firstAlert[1]) isKeep = false;
                                        if (vo.SecondData[currentTime.AddMinutes(j * -1).Minute % 10] >= vo.secondAlert[1]) isSecondKeep = false;
                                    }
                                    break;
                            }

                            if (isKeep && isSecondKeep)
                            {
                                vo.NowType = 0;
                            }
                            else
                            {
                                // wb_isualert에서 L1Std ~ L4Std 값과 현재 데이터 비교
                                for (int i = 1; i <= 4; i++)
                                {
                                    isKeep = true;
                                    isSecondKeep = true;
                                    switch (vo.EquType.ToLower())
                                    {
                                        case "rain":
                                        case "water":
                                        case "soil":
                                        case "snow":
                                        case "tilt":
                                            // term(분)만큼 해당 데이터가 발령 조건을 계속 만족하는지 확인
                                            for (int j = 0; j < cr_conf.criticalDataTerm; j++)
                                            {
                                                if (vo.Data[currentTime.AddMinutes(j * -1).Minute % 10] < vo.firstAlert[i]) isKeep = false;
                                            }

                                            // term(분)만큼 해당 데이터가 발령 조건을 만족한다면 vo에 현재 레벨 적용
                                            if (isKeep) vo.NowType = i;
                                            break;

                                        case "dplace":
                                        case "flood":
                                            if (vo.EquType.ToLower() == "flood" && i == 4) break;

                                            for (int j = 0; j < cr_conf.criticalDataTerm; j++)
                                            {
                                                if (vo.Data[currentTime.AddMinutes(j * -1).Minute % 10] < vo.firstAlert[i]) isKeep = false;
                                                if (vo.SecondData[currentTime.AddMinutes(j * -1).Minute % 10] < vo.secondAlert[i]) isSecondKeep = false;
                                            }

                                            // term(분)만큼 해당 데이터가 발령 조건을 만족한다면 vo에 현재 레벨 적용
                                            if (isKeep || isSecondKeep) vo.NowType = i;
                                            break;
                                    }
                                }
                            }

                            // 현재 Level과 과거 Level 비교 if(현재, 과거 Lv이 같고 0(Normal)이 아닌경우 ChkCount++)
                            if (NowLevel == vo.NowType && vo.NowType != 0)
                            {
                                vo.ChkCount++;
                            }
                            else
                            {
                                vo.ChkCount = 0;
                            }

                            // DB wb_alert 테이블 업데이트 (현재, 과거 lv이 0인경우 업데이트 안함)
                            if (NowLevel != 0 || vo.NowType != 0)
                            {
                                string where = $"CD_DIST_OBSV = {vo.Cd_dist_obsv}";
                                if (vo.EquType.ToLower() == "rain") where += $" AND RainTime = '{vo.RainTime}'";

                                WB_ISUALERT_DAO alt_dao = new WB_ISUALERT_DAO(mysql);
                                alt_dao.Update($"NowType = {vo.NowType}", where);
                                alt_dao.Update($"ChkCount = {vo.ChkCount}", where);
                            }
                        }

                        // UI 갱신
                        foreach (var vo in alert_list)
                        {
                            if (area_list.Exists(x => x.AltCode == vo.AltCode)) 
                            {
                                WB_ISUALERTVIEW_VO view_vo = area_list.Find(x => x.AltCode == vo.AltCode);
                                {

                                    switch (vo.EquType.ToLower())
                                    {
                                        case "rain":
                                        case "water":
                                        case "soil":
                                        case "snow":
                                        case "tilt":
                                            view_vo.NowCritical = vo.NowType > 0 ? vo.Std[vo.NowType] : "-";
                                            view_vo.NextCritical = vo.NowType < 4 ? vo.Std[vo.NowType + 1] : "-";
                                            view_vo.NowData = vo.Data[minuteIdx].ToString() ?? "-";
                                            break;

                                        case "dplace":
                                            view_vo.NowCritical = vo.NowType > 0 ? vo.Std[vo.NowType] : "-";
                                            view_vo.NextCritical = vo.NowType < 4 ? vo.Std[vo.NowType + 1] : "-";
                                            view_vo.NowData = vo.Data[minuteIdx].ToString() != null && vo.SecondData[minuteIdx].ToString() != null ? $"{vo.Data[minuteIdx]}/{vo.SecondData[minuteIdx]}" : "-";
                                            break;

                                        case "flood":
                                            switch (vo.Data[minuteIdx])
                                            {
                                                case 0:
                                                    view_vo.NowCritical = "-";
                                                    view_vo.NextCritical = "001/5Cm";
                                                    view_vo.NowData = "000";
                                                    break;

                                                case 1:
                                                    view_vo.NowCritical = "001/5Cm";
                                                    view_vo.NextCritical = "011/13Cm";
                                                    view_vo.NowData = "001";
                                                    break;

                                                case 2:
                                                    view_vo.NowCritical = "011/13Cm";
                                                    view_vo.NextCritical = "111/21Cm";
                                                    view_vo.NowData = "011";
                                                    break;

                                                case 3:
                                                    view_vo.NowCritical = "111/21Cm";
                                                    view_vo.NextCritical = "-";
                                                    view_vo.NowData = "111";
                                                    break;
                                            }

                                            view_vo.NowData += vo.SecondData[minuteIdx].ToString() != null ? $"/{vo.SecondData[minuteIdx]}" : "/-";
                                            break;
                                    }
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
                    // 침수지역 주차중인 차량 문자 발송은 한번만 보낼 수 있게끔 조건 변수 설정
                    //TODO: 차후 어느 지역에 주차중인지 확인하여 그 지역에 관련된 차량에게만 문자가 갈 수 있게끔 변경 필요
                    bool isFloodSMSSend = false;

                    using (MYSQL_T mysql = new MYSQL_T(db_local_conf))
                    {
                        // 경보 그룹별 Alert에 대한 Level 비교 및 Equip 발령 진행
                        foreach (var vo in group_list)
                        {
                            // Group별 Alert List의 Level 중 가장 높은 Level을 현재 상황으로 판단
                            int level = 0;
                            string occur = null;
                            string retreatNum = null;
                            foreach (string AltCode in vo.AltCodeList)
                            {
                                if (alert_list.Exists(x => x.AltCode == AltCode))
                                {
                                    WB_ISUALERT_VO alert_list_vo = alert_list.Find(x => x.AltCode == AltCode);

                                    if (alert_list_vo.NowType > level)
                                    {
                                        // 최대 Level
                                        level = alert_list_vo.NowType;

                                        // 최대 Level의 계측구분
                                        occur = alert_list_vo.EquType;
                                        //if (occur.ToLower() == "rain") occur += $"{alert_list_vo.RainTime}";

                                        // NDMS 계측기 확인
                                        retreatNum = alert_list_vo.Cd_dist_obsv;
                                    }
                                }
                            }

                            // List에 등록하기 위해 DAO 변수 선언
                            WB_ISULIST_DAO list_dao = new WB_ISULIST_DAO(mysql);

                            // 평시 상황 || 프로그램이 껐다 켜졌을 때 상태 유지
                            if ((level == vo.NowLevel && level == 0) || (level == vo.NowLevel && vo.IsuCode == null))
                            {
                                continue;
                            }
                            else if (level == vo.NowLevel || level > vo.NowLevel) // Now Level이 기존 Level보다 높은 경우 (Level이 0일 경우는 없으니 가정하지 않음)
                            {
                                WB_ISULIST_VO list_vo = new WB_ISULIST_VO();
                                if (level == vo.NowLevel) // "m-start"로 사용자의 발령대기중인 경보 발령 됐는지 확인
                                {
                                    list_vo = list_dao.Select($"IsuCode = {vo.IsuCode}").FirstOrDefault();
                                    if (list_vo == null || list_vo.IStatus.ToLower() != "start")
                                    {
                                        return;
                                    }
                                }
                                else // level > vo.NowLevel (현재레벨 > 기존레벨)
                                {
                                    // 경보 상향 발령이 필요하므로 기존 발령중인 경보 List End처리
                                    if (vo.IsuCode != null && vo.NowLevel != 0)
                                    {
                                        list_dao.Update("IsuEndAuto = 'retreat'", $"IsuCode = {vo.IsuCode}");
                                        list_dao.Update("IsuEndDate = NOW()", $"IsuCode = {vo.IsuCode}");
                                        list_dao.Update("IStatus = 'end'", $"IsuCode = {vo.IsuCode}");
                                    }

                                    list_vo = new WB_ISULIST_VO()
                                    {
                                        GCode = vo.GCode,
                                        IsuKind = $"level{level}",
                                        IsuSrtAuto = "auto",
                                        IsuSrtDate = currentTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                        Occur = occur,
                                        Equip = vo.Equip[level],
                                        SMS = vo.SMS[level],
                                        IStatus = "start",
                                        Send = "Y",
                                        HAOK = "E"
                                    };

                                    // wb_isulist에 Insert (등록)
                                    list_dao.Insert(list_vo);

                                    // Temp Column IsuCode에 wb_isulist의 IsuCode를 변수에 등록
                                    string query = "SELECT LAST_INSERT_ID() AS idx";
                                    vo.IsuCode = mysql.ExecuteScalar(query).ToString();
                                    vo.retreat = retreatNum;
                                }

                                log.Info($"[{vo.GName}]경보 {level}단계 경보 발령");

                                // Level별 등록된 SMS User에게 문자 전송
                                if (vo.SMSList[level] != null)  // SMS List에 문자 전송
                                {
                                    foreach (string GCode in vo.SMSList[level])
                                    {
                                        var user = smsuser_list.Find(x => x.GCode == GCode);
                                        if (user != null) // wb_alertgroup sms에는 있지만 wb_smsuser에서 삭제된 경우
                                        {
                                            new SendSMS(user.Phone, level);
                                            log.Info($"{user.Phone} 문자 발송!");
                                        }
                                    }

                                    log.Info($"[{vo.GName}]경보 {level}단계 SMS 발송");
                                }

                                // Level별 등록된 장비에 경보 발령!
                                if (vo.Auto[level].ToLower().Equals("on") || level == vo.NowLevel) // Auto 1 ~ 4가 "on"일 경우 Equip List 작동
                                {
                                    if (vo.EquipList != null)
                                    {
                                        foreach (string Cd_dist_obsv in vo.EquipList[level])
                                        {
                                            WB_EQUIP_VO equip_vo = equip_list.Find(x => x.Cd_dist_obsv == Cd_dist_obsv);
                                            if (equip_vo != null) // wb_alertgroup Equip에는 있지만 wb_equip에 등록이 안돼있거나, USE_YN이 0인 경우
                                            {
                                                new ExcuteEquip(equip_vo, level);
                                            }
                                        }

                                        list_dao.Update("IStatus = 'ing'", $"IsuCode = {vo.IsuCode}");
                                        log.Info($"[{vo.GName}]경보 {level}단계 등록장비 작동");
                                    }
                                }
                                else if (vo.Auto[level].ToLower().Equals("off")) // Auto 1 ~ 4가 "off"일 경우 담당자에게 문자만 발송
                                {
                                    if (vo.AdmSMSList != null)
                                    {
                                        foreach (string phone in vo.AdmSMSList)
                                        {
                                            new SendSMS(phone, level);
                                        }

                                        list_dao.Update("IStatus = 'm-start'", $"IsuCode = {vo.IsuCode}");
                                        log.Info($"[{vo.GName}]경보 {level}단계 담당자에게 SMS 발송 (발령 대기)");
                                    }
                                }

                                // 주차중인 차량이동 안내문자 발송여부
                                if (vo.FloodSMSAuto[level].ToLower().Equals("on"))
                                {
                                    isFloodSMSSend = true;
                                }
                            }
                            else if (level < vo.NowLevel) // Now Level이 기존 Level보다 낮은 경우 Now Level의 장비에 대해 작동 해제 명령 (Level이 4일 경우는 없으니 가정하지 않음)
                            {
                                log.Info($"[{vo.GName}]경보 {(level != 0 ? $"{level}단계" : "평시")}로 경보 하향");

                                // 경보 하향 발령이 필요하므로 기존 발령중인 경보 List End처리
                                if (vo.IsuCode != null)
                                {
                                    list_dao.Update($"IsuEndAuto = '{(level != 0 ? "advance" : "end")}'", $"IsuCode = {vo.IsuCode}");
                                    list_dao.Update("IsuEndDate = NOW()", $"IsuCode = {vo.IsuCode}");
                                    list_dao.Update("IStatus = 'end'", $"IsuCode = {vo.IsuCode}");
                                }

                                if (level == 0) // Level이 0일 경우 전체 장비 발령 중 작동에 대해 해제 명령
                                {
                                    foreach (string Cd_dist_obsv in vo.EquipList[vo.NowLevel])
                                    {
                                        WB_EQUIP_VO equip_vo = equip_list.Find(x => x.Cd_dist_obsv == Cd_dist_obsv);
                                        if (equip_vo != null) // wb_alertgroup Equip에는 있지만 wb_equip에 등록이 안돼있거나, USE_YN이 0인 경우
                                        {
                                            new ExcuteEquip(equip_vo, level);
                                        }
                                    }

                                    log.Info($"[{vo.GName}]경보 등록장비 평시모드로 변경완료");
                                }
                                else // Level이 1 ~ 3일 경우 : 방송 = 작동 안함, 전광판 = 작동, 차단기 = Level에 Now Level에서 Close명령을 받은 차단기가 있을 경우 Open 명령 그 외 작동 안함
                                {
                                    // wb_isulist에 Insert (등록)
                                    WB_ISULIST_VO list_vo = new WB_ISULIST_VO()
                                    {
                                        GCode = vo.GCode,
                                        IsuKind = $"level{level}",
                                        IsuSrtAuto = "auto",
                                        IsuSrtDate = currentTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                        Occur = occur,
                                        Equip = vo.Equip[level],
                                        SMS = vo.SMS[level],
                                        IStatus = "start",
                                        Send = "Y",
                                        HAOK = "E"
                                    };
                                    list_dao.Insert(list_vo);

                                    // Temp Column IsuCode에 wb_isulist의 IsuCode를 변수에 등록
                                    string query = "SELECT LAST_INSERT_ID() AS idx";
                                    vo.IsuCode = mysql.ExecuteScalar(query).ToString();

                                    foreach (string Cd_dist_obsv in vo.EquipList[vo.NowLevel])
                                    {
                                        WB_EQUIP_VO equip_vo = equip_list.Find(x => x.Cd_dist_obsv == Cd_dist_obsv);

                                        if (equip_vo != null) // wb_alertgroup Equip에는 있지만 wb_equip에 등록이 안돼있거나, USE_YN이 0인 경우
                                        {
                                            ExcuteEquip exec = new ExcuteEquip(equip_vo, level, false);
                                            switch (equip_vo.gb_obsv)
                                            {
                                                case "18":
                                                    break;

                                                case "19":
                                                    exec.Alert();
                                                    break;

                                                case "20":
                                                    // 기존 상황에 Close 명령을 받은 차단기가 있고 현재 상황에 차단기 작동 여부가 해제돼있을 경우 차단기 Open
                                                    if (vo.EquipList[level].Exists(x => x != Cd_dist_obsv))
                                                    {
                                                        exec.doGateControl(true, false);
                                                        exec.Alert();
                                                    }
                                                    break;
                                            }
                                        }
                                    }

                                    list_dao.Update("IStatus = 'ing'", $"IsuCode = {vo.IsuCode}");
                                    log.Info($"[{vo.GName}]경보 등록장비 하향 발령 (전광판 문구 수정)");
                                }
                            }

                            if (level != 0) vo.AlertDate = currentTime.ToString("yyyy-MM-dd HH:mm:ss");
                            vo.NowLevel = level;
                        }

                        // 침수위험 알림문자가 필요한 경우
                        if (isFloodSMSSend)
                        {
                            WB_PARKSMSLIST_DAO send_dao = new WB_PARKSMSLIST_DAO(mysql);

                            foreach (WB_PARKCARNOW_VO vo in car_list)
                            {
                                WB_PARKSMSLIST_VO send_vo = new WB_PARKSMSLIST_VO()
                                {
                                    CarNum = vo.CarNum,
                                    SMSContent = FloodSMSMent,
                                    RegDate = currentTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                    SendStatus = "T10",
                                    SendType = "autosend"
                                };

                                send_dao.Insert(send_vo);
                            }
                        }

                        // NDMS 정보 전송
                        if (cr_conf.ndms_used == true)
                        {
                            using (MYSQL_T ndms_sql = new MYSQL_T(db_ndms_conf))
                            {
                                NDMS_EQUIP_DAO equip_dao = new NDMS_EQUIP_DAO(ndms_sql);
                                NDMS_ALMORD_DAO ndms_dao = new NDMS_ALMORD_DAO(ndms_sql);

                                foreach (var vo in group_list)
                                {
                                    if (string.IsNullOrEmpty(vo.retreat))
                                    {
                                        NDMS_EQUIP_VO equip_vo = equip_dao.Select($"CD_DIST_OBSV = '{vo.retreat}'").FirstOrDefault();
                                        if (equip_vo != null && equip_vo.Dscode.Length == 10)
                                        {

                                            NDMS_ALMORD_VO ndms_vo = new NDMS_ALMORD_VO()
                                            {
                                                Dscode = equip_vo.Dscode,
                                                Cd_dist_obsv = equip_vo.Cd_dist_obsv,
                                                AlmCode = $"0{vo.NowLevel}",
                                                Almde = DateTime.Now.ToString("yyyyMMddHHmmss"),
                                                Almgb = "1",
                                                Almnote = null,
                                                table_name = equip_vo.gb_obsv != "21" ? "TCM_COU_DNGR_ALMORD" : "TCM_FLUD_ALMORD",
                                                table_comment = equip_vo.gb_obsv != "21" ? "위험경보발령 정보" : "침수경보발령 정보"
                                            };

                                            ndms_dao.EndBeforeAlmord(ndms_vo);
                                            ndms_dao.Insert(ndms_vo);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // UI 업데이트
                    foreach (WB_ISUALERTGROUP_VO group_vo in group_list)
                    {
                        foreach (string AltCode in group_vo.AltCodeList)
                        {
                            WB_ISUALERTVIEW_VO view_vo = area_list.Find(x => x.GCode == group_vo.GCode && x.AltCode == AltCode);
                            {
                                if (group_vo.NowLevel == 0) view_vo.AltDate = "-";
                                else if (group_vo.NowLevel != view_vo.NowLevel) view_vo.AltDate = group_vo.AlertDate;

                                view_vo.NowLevel = group_vo.NowLevel;
                            }
                        }
                    }

                    dataGridView_Refresh();
                }
                catch { }
            });
        }

        private double Abs(double val)
        {
            return Math.Abs(val);
        }

        private async Task SendAlertInfoToNDMS()
        {
            if (cr_conf.ndms_used == false)
            {
                return;
            }

            await Task.Run(() =>
            {
                using (MYSQL_T mysql = new MYSQL_T(db_local_conf))
                {
                    foreach (var vo in alert_list)
                    {
                        WB_EQUIP_DAO equip_dao = new WB_EQUIP_DAO(mysql);
                        WB_EQUIP_VO equip_vo = equip_dao.Select($"CD_DIST_OBSV = '{vo.Cd_dist_obsv}'").FirstOrDefault();

                        if (equip_vo == null && equip_vo.DsCode == null && equip_vo.DsCode.Length != 10)
                        {
                            continue;
                        }

                        string table_name;
                        switch (equip_vo.DsCode.Substring(5,1))
                        {
                            case "0":
                            case "1":
                            case "2":
                                table_name = "TCM_COU_DD_THOLD";
                                break;

                            case "3":
                                table_name = "TCM_COU_DR_THOLD";
                                break;

                            case "4":
                                table_name = "TCM_COU_SS_THOLD";
                                break;

                            case "5":
                            case "6":
                            case "7":
                                table_name = "TCM_COU_FC_THOLD";
                                break;

                            case "z":
                            default:
                                continue;
                        }

                        string obsr_gb;
                        switch (equip_vo.gb_obsv)
                        {
                            case "01":
                                if (vo.RainTime != "1") continue;
                                obsr_gb = "RF";
                                break;

                            case "02":
                                obsr_gb = "WL";
                                break;

                            case "03":
                                obsr_gb = "DP";
                                break;

                            case "04":
                                obsr_gb = "MC";
                                break;

                            case "06":
                                obsr_gb = "SF";
                                break;

                            case "08":
                                obsr_gb = "SP";
                                break;

                            case "21":
                                obsr_gb = "WL";
                                break;

                            default:
                                continue;
                        }

                        using (MYSQL_T ndms_sql = new MYSQL_T()) 
                        {
                            NDMS_THOLD_DAO ndms_dao = new NDMS_THOLD_DAO(ndms_sql);
                            for (int i = 1; i <= 4; i++)
                            {
                                string thold_value;
                                switch (equip_vo.gb_obsv)
                                {
                                    case "01":
                                    case "02":
                                    case "04":
                                    case "06":
                                    case "08":
                                        thold_value = vo.Std[i];
                                        break;

                                    case "03":
                                        thold_value = vo.firstAlert[i].ToString();
                                        break;

                                    case "21":
                                        thold_value = null;
                                        break;

                                    default:
                                        continue;
                                }

                                NDMS_THOLD_VO ndms_vo = new NDMS_THOLD_VO()
                                {
                                    Table_Name = table_name,

                                    Dscode = equip_vo.DsCode,
                                    Cd_dist_obsv = vo.Cd_dist_obsv,
                                    AlmCode = $"0{i}",
                                    Gb_obsv = equip_vo.gb_obsv,
                                    Obsr_gb = obsr_gb,
                                    Thold_value = thold_value,
                                    Use_YN = vo.Use[i].ToLower() == "on" ? "Y" : "N"
                                };

                                ndms_dao.Insert(ndms_vo);
                            }
                        }
                    }
                }
            });
        }

        private void dataGridView_Refresh()
        {
            try
            {
                if (dataGridView.InvokeRequired)
                {
                    dataGridView.Invoke(new Action(() =>
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
