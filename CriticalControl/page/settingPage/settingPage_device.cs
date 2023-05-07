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
using wLib.UI;

namespace CriticalControl.page
{
    public partial class settingPage_device : Form
    {
        // 로그
        LOG_T log = LOG_T.Instance;

        // 설정 파일
        TABLE_CONF table_conf = new TABLE_CONF();
        CRI_CONF cr_conf = new CRI_CONF();

        public settingPage_device()
        {
            InitializeComponent();
        }

        private void settingPage_device_Load(object sender, EventArgs e)
        {
            // ComboBox 리스트 생성
            #region ComboBox 리스트 생성
            List<ComboBoxItem> cri_minute_list = new List<ComboBoxItem>
            {
                new ComboBoxItem() { Text = "1분", Tag = "1" },
                new ComboBoxItem() { Text = "2분", Tag = "2" },
                new ComboBoxItem() { Text = "3분", Tag = "3" },
                new ComboBoxItem() { Text = "4분", Tag = "4" },
                new ComboBoxItem() { Text = "5분", Tag = "5" },
                new ComboBoxItem() { Text = "6분", Tag = "6" },
                new ComboBoxItem() { Text = "7분", Tag = "7" },
                new ComboBoxItem() { Text = "8분", Tag = "8" },
                new ComboBoxItem() { Text = "9분", Tag = "9" },
                new ComboBoxItem() { Text = "10분", Tag = "10" }
            };

            List<ComboBoxItem> data_minute_list = new List<ComboBoxItem>
            {
                new ComboBoxItem() { Text = "0분", Tag = "1" },
                new ComboBoxItem() { Text = "1분", Tag = "2" },
                new ComboBoxItem() { Text = "2분", Tag = "3" },
                new ComboBoxItem() { Text = "3분", Tag = "4" },
                new ComboBoxItem() { Text = "4분", Tag = "5" },
                new ComboBoxItem() { Text = "5분", Tag = "6" },
                new ComboBoxItem() { Text = "6분", Tag = "7" },
                new ComboBoxItem() { Text = "7분", Tag = "8" },
                new ComboBoxItem() { Text = "8분", Tag = "9" },
                new ComboBoxItem() { Text = "9분", Tag = "10" }
            };

            critermCb.DisplayMember = "Text";
            critermCb.ValueMember = "Tag";
            critermCb.DataSource = cri_minute_list;
            critermCb.SelectedIndex = -1;

            datatermCb.DisplayMember = "Text";
            datatermCb.ValueMember = "Tag";
            datatermCb.DataSource = data_minute_list;
            datatermCb.SelectedIndex = -1;
            #endregion
        }

        private void settingPage_device_Shown(object sender, EventArgs e)
        {
            ReadConfig();
        }

        private void settingPage_device_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void settingPage_device_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void ReadConfig()
        {
            tabControl.Controls.Clear();

            // TABLE_CONF
            table_conf.ReadConfig();
            {
                wbCheck.Checked = table_conf.wb_used;

                tabControl.TabPages.Add(tablePage);
            }

            // CRI_CONF
            cr_conf.ReadConfig();
            {
                ndmsCheck.Checked = cr_conf.ndms_used;

                critermCb.SelectedIndex = cr_conf.criticalDataTerm - 1;
                datatermCb.SelectedIndex = cr_conf.dataInsertTerm - 1;
            }

            EnableConfig(false);
        }

        private void SaveConfig()
        {
            table_conf.ReadConfig();
            {
                table_conf.wb_used = wbCheck.Checked;
                table_conf.jh_used = jhCheck.Checked;
            }
            table_conf.SaveConfig();

            cr_conf.ReadConfig();
            {
                cr_conf.ndms_used = ndmsCheck.Checked;

                cr_conf.criticalDataTerm = critermCb.SelectedIndex + 1;
                cr_conf.dataInsertTerm = datatermCb.SelectedIndex + 1;
            }
            cr_conf.SaveConfig() ;
        }

        private void EnableConfig(bool flag)
        {
            // TABLE_CONF
            wbCheck.Enabled = flag;
            jhCheck.Enabled = flag;

            // CONF
            ndmsCheck.Enabled = flag;
            critermCb.Enabled = flag;
            datatermCb.Enabled = flag;

            if (flag == true)
            {
                lockButton.Tag = null;
            }
            else
            {
                lockButton.Tag = new object();
            }
        }

        private void On_Click_Reset(object sender, EventArgs e)
        {
            ReadConfig();

            if (lockButton.Tag != null)
            {
                lockButton.Text = "설정";
            }
        }

        private void On_Click_Lock(object sender, EventArgs e)
        {
            Button button = sender as Button;

            if (button.Tag == null)
            {
                button.Text = "설정";
                SaveConfig();
                ReadConfig();
                return;
            }
            else
            {
                button.Text = "저장";
            }

            EnableConfig(true);
        }
    }
}
