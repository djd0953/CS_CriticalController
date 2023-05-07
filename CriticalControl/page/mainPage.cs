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
using wLib.UI;

namespace CriticalControl.page
{
    public partial class mainPage : Form
    {
        // 로그
        LOG_T log = LOG_T.Instance;

        // 타이머
        DateTime process_time = new DateTime();

        //설정 파일
        APP_CONF app_conf = new APP_CONF(Properties.Resources.APP_NAME);
        RUN_CONF run_conf = new RUN_CONF(Properties.Resources.APP_NAME);

        public mainPage()
        {
            InitializeComponent();
        }

        private void mainPage_Load(object sender, EventArgs e)
        {
            // RunInterval 리스트 생성
            #region RunInterval 리스트 생성
            List<ComboBoxItem> interval_list = new List<ComboBoxItem>
            {
                new ComboBoxItem() { Text = "1초", Tag = "1" },
                new ComboBoxItem() { Text = "3초", Tag = "3" },
                new ComboBoxItem() { Text = "5초", Tag = "5" },
                new ComboBoxItem() { Text = "10초", Tag = "10" },
                new ComboBoxItem() { Text = "30초", Tag = "30" },
                new ComboBoxItem() { Text = "1분", Tag = "60" },
                new ComboBoxItem() { Text = "5분", Tag = "300" },
                new ComboBoxItem() { Text = "10분", Tag = "600" },
                new ComboBoxItem() { Text = "30분", Tag = "1800" },
                new ComboBoxItem() { Text = "1시간", Tag = "3600" }
            };

            runInterval.DisplayMember = "Text";
            runInterval.ValueMember = "Tag";
            runInterval.DataSource = interval_list;
            runInterval.SelectedIndex = -1;
            runInterval.SelectedValueChanged += On_SelectedValueChanged;
            #endregion

            // dataBinding
            dataBindingSource.DataSource = area_list;
        }

        private void mainPage_Shown(object sender, EventArgs e)
        {
            // 설정파일 읽기
            ReadConfig();

            // 타이머 시작
            process_timer.Start();
        }

        private void mainPage_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void mainPage_FormClosed(object sender, FormClosedEventArgs e)
        {
            run_conf.ReadConfig();
            {
                // 실행 모드
                runMode_auto.Checked = (run_conf.run_mode == true) ? true : false;
                runMode_manual.Checked = !(run_conf.run_mode == true) ? true : false;

                // 실행 주기
                foreach (ComboBoxItem item in runInterval.Items)
                {
                    if (run_conf.run_interval == Convert.ToInt32(item.Tag))
                    {
                        runInterval.SelectedItem = item;
                        break;
                    }
                }

                bottomPanel.Visible = (run_conf.run_visible == true) ? true : false;
            }
        }

        void ReadConfig()
        {
            run_conf.ReadConfig();
            {
                // 실행 모드
                runMode_auto.Checked = (run_conf.run_mode == true) ? true : false;
                runMode_manual.Checked = !(run_conf.run_mode == true) ? true : false;

                // 실행 주기
                foreach (ComboBoxItem item in runInterval.Items)
                {
                    if (run_conf.run_interval == Convert.ToInt32(item.Tag))
                    {
                        runInterval.SelectedItem = item;
                        break;
                    }
                }

                bottomPanel.Visible = (run_conf.run_visible == true) ? true : false;
            }
        }

        private void On_Process_Timer(object sender, EventArgs e)
        {
            if (runMode_auto.Checked == false)
                return;

            process_time = process_time.AddSeconds(1);
            if (runInterval.Tag != null)
            {
                int total_seconds = (int)(process_time - new DateTime()).TotalSeconds;
                int interval = Convert.ToInt32(runInterval.Tag);

                int minute = (interval - (total_seconds % interval)) / 60;
                int second = (interval - (total_seconds % interval)) % 60;

                runButton.Text = string.Format("자 동 실 행 ({0}{1:D02} 초)", (minute > 0) ? $"{minute:D02} 분" : "", second);
                progressBar.Value = progressBar.Maximum - (total_seconds % interval);

                // 실행 이벤트 발생
                if (progressBar.Value == progressBar.Maximum)
                {
                    app_conf.ReadConfig();
                    if (app_conf.window.Visible)
                        runButton.PerformClick();
                    else On_Click_Run(runButton, null);
                }
            }
        }

        private async void On_Click_Run(object sender, EventArgs e)
        {
            Button button = sender as Button;

            process_timer.Stop();
            button.Text = "실 행 중";
            button.Enabled = false;
            {
                await Process();
            }
            button.Enabled = true;
            button.Text = "실  행";
            process_timer.Start();
        }

        private void On_Checked_RunMode(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;

            // 실행 모드 변경
            if (radioButton == runMode_auto)
            {
                // 자동 모드
                runInterval.Enabled = true;
                runPanel.BackColor = Color.Transparent;
                progressBar.Value = progressBar.Minimum;
                process_time = new DateTime();
                process_timer.Start();
            }
            else if (radioButton == runMode_manual)
            {
                // 수동 모드
                runInterval.Enabled = false;
                runPanel.BackColor = Color.Red;
                progressBar.Value = progressBar.Minimum;
                runButton.Text = "수 동 실 행";
                process_timer.Stop();
            }

            // RUN_CONF SAVE
            run_conf.ReadConfig();
            {
                run_conf.run_mode = runMode_auto.Checked;
            }
            run_conf.SaveConfig();
        }

        private void On_SelectedValueChanged(object sender, EventArgs e)
        {
            ComboBox combobox = sender as ComboBox;

            if (combobox.SelectedValue == null)
                return;

            // 실행 주기 변경
            combobox.Tag = Convert.ToInt32(combobox.SelectedValue);
            process_time = new DateTime();
            progressBar.Maximum = Convert.ToInt32(combobox.Tag);

            // RUN_CONF SAVE
            run_conf.ReadConfig();
            {
                run_conf.run_interval = Convert.ToInt32(combobox.Tag);
            }
            run_conf.SaveConfig();

            On_Process_Timer(null, null);
        }

        ////////////////////////////////////////////////////////////////////////////////
        // 구동 로직과는 상관 없는 부분
        ////////////////////////////////////////////////////////////////////////////////
        private void DataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            
            DataGridView dataGridView = sender as DataGridView;

            if (dataGridView.RowCount == 0)
                return;

            if (e.ListChangedType != ListChangedType.Reset)
                return;

            try
            {
                // 보여줄 데이터 형식?
            }
            catch (Exception ex)
            {
                log.Warning($"{GetType()}: {ex.Message}");
            }
        }

        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView dataGridView = sender as DataGridView;

            try
            {
                dataGridView.ClearSelection();
            }
            catch { }
        }

        private void DataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
