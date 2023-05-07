using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using wLib;

namespace CriticalControl
{
    public partial class mainForm : Form
    {
        // 로그 UI
        LOG_T log = LOG_T.Instance;

        // 설정
        APP_CONF app_conf = new APP_CONF(Properties.Resources.APP_NAME, "임계치자동경보");

        // 페이지
        page.mainPage main_page = new page.mainPage();

        public mainForm()
        {
            InitializeComponent();
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            /* 제목 표시줄 */
            Text = app_conf.process.ProcessName + " V" + app_conf.process.Version;

            /* 창 위치 및 크기 */
            MaximumSize = new Size(app_conf.window.Width, app_conf.window.Height * 4);
            Top = app_conf.window.Top;
            Left = app_conf.window.Left;
            Width = app_conf.window.Width;
            Height = app_conf.window.Height;
            WindowState = (FormWindowState)app_conf.window.State;

            /* 종복 실행 검사 */
            // TODO: 잠시 실행 할 수도 있어서 pocress 이름 변경 또는 Timer, 변수 등으로 예외처리가 한번 더 들어가야 함
            if (app_conf.process.IsAlone == false)
            {
                System.Timers.Timer exit_timer = new System.Timers.Timer(1000 * 30);
                {
                    exit_timer.Elapsed += delegate (object obj, System.Timers.ElapsedEventArgs args)
                    {
                        Environment.Exit(0);
                    };
                    exit_timer.Start();
                }

                MessageBox.Show("프로그램이 이미 실행중입니다.", app_conf.process.ProcessName);
                Environment.Exit(0);
            }

            /* 로그 설정 */
            log.Init(Properties.Resources.APP_NAME);
            log.ConnectTextBox(logPanel.LogText);

            /* START UP */
            log.Info("");
            log.WriteLine($"========================================");
            log.WriteLine($"| 프로그램을 시작합니다.({Text})");
            log.WriteLine($"========================================");
        }

        private void mainForm_Shown(object sender, EventArgs e)
        {
            // 설정파일 저장
            ReadConfig();

            // 페이지 종료
            On_Click_Page(mainButton, null);

            // 타이머 종료
            refresh_timer.Start();
        }

        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 설정파일 저장
            SaveConfig();

            // 페이지 종료
            LoadPage(main_page).Close();

            // 타이머 종료
            refresh_timer.Stop();
        }

        private void mainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Thread.Sleep(300);
            log.Info("");
            log.WriteLine($"========================================");
            log.WriteLine($"| 프로그램을 종료합니다.({Text})");
            log.WriteLine($"========================================");
            log.WriteLine();
            log.WriteLine();
            log.WriteLine();
            log.WriteLine();
            log.WriteLine();
            log.WriteLine();
            //Environment.Exit(0);
        }

        ////////////////////////////////////////////////////////////////////////////////
        // 설정 파일
        ////////////////////////////////////////////////////////////////////////////////
        void ReadConfig()
        {
            app_conf.ReadConfig();
            {
                // TOP 타이틀
                titlePanel.ProgramName = app_conf.program_name;
                titlePanel.SystemName = app_conf.system_name;
                titlePanel.AreaCode = app_conf.area_code;

                // Visible
                this.Visible = app_conf.window.Visible;
            }
        }

        void SaveConfig()
        {
            // 창 위치 및 크기 저장
            app_conf.ReadConfig();
            {
                app_conf.window.Top = (WindowState == FormWindowState.Normal) ? Bounds.Top : RestoreBounds.Top;
                app_conf.window.Left = (WindowState == FormWindowState.Normal) ? Bounds.Left : RestoreBounds.Left;
                //app_conf.window.Width = (WindowState == FormWindowState.Normal) ? Bounds.Width: RestoreBounds.Width;
                //app_conf.window.Height = (WindowState == FormWindowState.Normal) ? Bounds.Height : RestoreBounds.Height;
                app_conf.window.State = (WindowState == FormWindowState.Minimized) ? (int)FormWindowState.Normal : (int)WindowState;
            }
            app_conf.SaveConfig();
        }

        ////////////////////////////////////////////////////////////////////////////////
        // 메뉴 버튼
        ////////////////////////////////////////////////////////////////////////////////
        private void On_Click_Page(object sender, EventArgs e)
        {
            ToolStripItem button = sender as ToolStripItem;

            // 메인 화면
            if (button == mainButton)
            {
                LoadPage(main_page);
                mainButton.BackColor = Color.LightSteelBlue;
            }
            else mainButton.BackColor = Color.Transparent;

            // 이벤트
            if (button == eventButton)
            {
                //LoadPage(new page.eventPage());
                //eventButton.BackColor = Color.LightSteelBlue;
            }
            else eventButton.BackColor = Color.Transparent;

            // 설정 메뉴
            if (button == settingButton_system)
            {
                // 시스템 페이지
                LoadPage(new wLib.UI.SystemPage(Properties.Resources.APP_NAME));
                settingButton.BackColor = Color.LightSteelBlue;
            }
            else if (button == settingButton_db)
            {
                // DB 페이지
                LoadPage(new wLib.UI.DatabasePage());
                settingButton.BackColor = Color.LightSteelBlue;
            }
            else if (button == settingButton_device)
            {
                // 장치 페이지
                LoadPage(new page.settingPage_device());
                settingButton.BackColor = Color.LightSteelBlue;
            }
            else settingButton.BackColor = Color.Transparent;

            // 테스트 메뉴
            if (button == testButton)
            {
                //LoadPage(new page.testPage());
                //testButton.BackColor = Color.LightSteelBlue;
            }
            else testButton.BackColor = Color.Transparent;

            // 도움말 메뉴
            if (button == helpButton)
            {
                LoadPage(new wLib.UI.HelpPage(Properties.Resources.APP_NAME));
                helpButton.BackColor = Color.LightSteelBlue;
            }
            else helpButton.BackColor = Color.Transparent;
        }

        ////////////////////////////////////////////////////////////////////////////////
        // 페이지
        ////////////////////////////////////////////////////////////////////////////////
        private Form LoadPage(Form page)
        {
            page.TopLevel = false;
            page.FormBorderStyle = FormBorderStyle.None;
            page.Dock = DockStyle.Fill;
            {
                mainPanel.Controls.Clear();
                {
                    // 메인창 유지
                    if (mainPanel.Tag != null && mainPanel.Tag != main_page)
                    {
                        (mainPanel.Tag as Form).Close();
                    }
                }
                mainPanel.Controls.Add(page);
                mainPanel.Tag = page;
            }

            page.Show();

            return page;
        }

        ////////////////////////////////////////////////////////////////////////////////
        // 시간 갱신 타이머
        ////////////////////////////////////////////////////////////////////////////////
        private void On_Refresh_timer(object sender, EventArgs e)
        {
            DateTime nowTime = DateTime.Now;

            clockPanel.Text = nowTime.ToString("yyyy-MM-dd HH:mm:ss");

            // 설정파일 리로드
            if (app_conf.ReadConfig())
            {
                ReadConfig();
            }
        }

        ////////////////////////////////////////////////////////////////////////////////
        // 구동 로직과는 상관 없는 부분
        ////////////////////////////////////////////////////////////////////////////////
        #region 설정창 Expender
        private void On_Click_Config(object sender, EventArgs e)
        {
            Button button = sender as Button;

            if (button.Tag == null)
            {
                // 인증 결과
                // OK : 인증성공
                // Yes: 인증성공(테스트 계정)
                // None: 인증실패
                // Cancel: 인증취소
                DialogResult result = new wLib.UI.LoginForm().ShowDialog(this);
                if (result == DialogResult.OK || result == DialogResult.Yes)
                {
                    this.MaximumSize = new Size(app_conf.window.Width * 2, app_conf.window.Height * 4);
                    this.Width = app_conf.window.Width * 2;
                    button.Text = "Config ◁";
                    button.Tag = new object();

                    // 테스트 인증
                    testButton.Visible = (result == DialogResult.Yes);
                    if (result == DialogResult.Yes)
                    {
                        /* 창 크기 설정 */
                        this.MaximumSize = new Size(0, 0);
                        this.Width = 800;
                        this.Height = 600;

                        // 페이지 이동(테스트 페이지)
                        On_Click_Page(testButton, null);
                    }

                    log.Info(LOG_TYPE.FILE, "관리자 인증에 성공하였습니다.");
                }
                else if (result == DialogResult.None)
                {
                    string temp = log.Info(LOG_TYPE.FILE, "관리자 인증에 실패하였습니다.");
                    MessageBox.Show(temp);
                }
            }
            else
            {
                /* 창 크기 설정 */
                this.MaximumSize = new Size(app_conf.window.Width, app_conf.window.Height * 4);
                this.Width = app_conf.window.Width;
                this.Height = app_conf.window.Height;
                button.Text = "Config ▷";
                button.Tag = null;

                // 페이지 이동(메인 페이지)
                On_Click_Page(mainButton, null);
            }
        }
        #endregion
    }
}
