namespace CriticalControl
{
    partial class mainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.bottomLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.configButton = new System.Windows.Forms.Button();
            this.clockPanel = new System.Windows.Forms.Label();
            this.logPanel = new wLib.UI.logPanel();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.mainButton = new System.Windows.Forms.ToolStripMenuItem();
            this.eventButton = new System.Windows.Forms.ToolStripMenuItem();
            this.settingButton = new System.Windows.Forms.ToolStripMenuItem();
            this.settingButton_system = new System.Windows.Forms.ToolStripMenuItem();
            this.settingButton_db = new System.Windows.Forms.ToolStripMenuItem();
            this.settingButton_device = new System.Windows.Forms.ToolStripMenuItem();
            this.testButton = new System.Windows.Forms.ToolStripMenuItem();
            this.helpButton = new System.Windows.Forms.ToolStripMenuItem();
            this.titlePanel = new wLib.UI.TitlePanel();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.refresh_timer = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.bottomLayoutPanel.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 380F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.bottomLayoutPanel, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.logPanel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.menuStrip, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.titlePanel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.mainPanel, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(764, 461);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // bottomLayoutPanel
            // 
            this.bottomLayoutPanel.ColumnCount = 4;
            this.tableLayoutPanel1.SetColumnSpan(this.bottomLayoutPanel, 2);
            this.bottomLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.bottomLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.bottomLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.bottomLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.bottomLayoutPanel.Controls.Add(this.configButton, 2, 0);
            this.bottomLayoutPanel.Controls.Add(this.clockPanel, 0, 0);
            this.bottomLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bottomLayoutPanel.Location = new System.Drawing.Point(0, 431);
            this.bottomLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.bottomLayoutPanel.Name = "bottomLayoutPanel";
            this.bottomLayoutPanel.RowCount = 1;
            this.bottomLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.bottomLayoutPanel.Size = new System.Drawing.Size(764, 30);
            this.bottomLayoutPanel.TabIndex = 17;
            // 
            // configButton
            // 
            this.configButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.configButton.Location = new System.Drawing.Point(303, 3);
            this.configButton.Name = "configButton";
            this.configButton.Size = new System.Drawing.Size(74, 24);
            this.configButton.TabIndex = 1;
            this.configButton.TabStop = false;
            this.configButton.Text = "Config ▷";
            this.configButton.UseVisualStyleBackColor = true;
            this.configButton.Click += new System.EventHandler(this.On_Click_Config);
            // 
            // clockPanel
            // 
            this.clockPanel.AutoSize = true;
            this.clockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clockPanel.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.clockPanel.Location = new System.Drawing.Point(3, 0);
            this.clockPanel.Name = "clockPanel";
            this.clockPanel.Size = new System.Drawing.Size(114, 30);
            this.clockPanel.TabIndex = 2;
            this.clockPanel.Text = "0000-00-00 00:00:00";
            this.clockPanel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // logPanel
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.logPanel, 2);
            this.logPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logPanel.Location = new System.Drawing.Point(3, 396);
            this.logPanel.Name = "logPanel";
            this.logPanel.Size = new System.Drawing.Size(758, 32);
            this.logPanel.TabIndex = 14;
            // 
            // menuStrip
            // 
            this.menuStrip.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("menuStrip.BackgroundImage")));
            this.menuStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuStrip.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainButton,
            this.eventButton,
            this.settingButton,
            this.testButton,
            this.helpButton});
            this.menuStrip.Location = new System.Drawing.Point(380, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(0);
            this.menuStrip.Size = new System.Drawing.Size(384, 40);
            this.menuStrip.TabIndex = 13;
            this.menuStrip.Text = "menuStrip";
            // 
            // mainButton
            // 
            this.mainButton.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.mainButton.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.mainButton.Image = ((System.Drawing.Image)(resources.GetObject("mainButton.Image")));
            this.mainButton.Name = "mainButton";
            this.mainButton.Size = new System.Drawing.Size(83, 40);
            this.mainButton.Text = "메인화면";
            this.mainButton.Click += new System.EventHandler(this.On_Click_Page);
            // 
            // eventButton
            // 
            this.eventButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.eventButton.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.eventButton.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.eventButton.Image = ((System.Drawing.Image)(resources.GetObject("eventButton.Image")));
            this.eventButton.Name = "eventButton";
            this.eventButton.Size = new System.Drawing.Size(83, 40);
            this.eventButton.Text = "전송이력";
            this.eventButton.Click += new System.EventHandler(this.On_Click_Page);
            // 
            // settingButton
            // 
            this.settingButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingButton_system,
            this.settingButton_db,
            this.settingButton_device});
            this.settingButton.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.settingButton.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.settingButton.Image = ((System.Drawing.Image)(resources.GetObject("settingButton.Image")));
            this.settingButton.Name = "settingButton";
            this.settingButton.Size = new System.Drawing.Size(59, 40);
            this.settingButton.Text = "설정";
            this.settingButton.Click += new System.EventHandler(this.On_Click_Page);
            // 
            // settingButton_system
            // 
            this.settingButton_system.Name = "settingButton_system";
            this.settingButton_system.Size = new System.Drawing.Size(180, 22);
            this.settingButton_system.Text = "SYSTEM";
            this.settingButton_system.Click += new System.EventHandler(this.On_Click_Page);
            // 
            // settingButton_db
            // 
            this.settingButton_db.Name = "settingButton_db";
            this.settingButton_db.Size = new System.Drawing.Size(180, 22);
            this.settingButton_db.Text = "DB";
            this.settingButton_db.Click += new System.EventHandler(this.On_Click_Page);
            // 
            // settingButton_device
            // 
            this.settingButton_device.Name = "settingButton_device";
            this.settingButton_device.Size = new System.Drawing.Size(180, 22);
            this.settingButton_device.Text = "DEVICE";
            this.settingButton_device.Click += new System.EventHandler(this.On_Click_Page);
            // 
            // testButton
            // 
            this.testButton.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.testButton.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.testButton.Image = ((System.Drawing.Image)(resources.GetObject("testButton.Image")));
            this.testButton.Name = "testButton";
            this.testButton.Size = new System.Drawing.Size(71, 40);
            this.testButton.Text = "테스트";
            this.testButton.Click += new System.EventHandler(this.On_Click_Page);
            // 
            // helpButton
            // 
            this.helpButton.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.helpButton.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.helpButton.Image = ((System.Drawing.Image)(resources.GetObject("helpButton.Image")));
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(71, 40);
            this.helpButton.Text = "도움말";
            this.helpButton.Click += new System.EventHandler(this.On_Click_Page);
            // 
            // titlePanel
            // 
            this.titlePanel.AreaCode = "255";
            this.titlePanel.AreaName = "ㅇㅇ시";
            this.titlePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titlePanel.Location = new System.Drawing.Point(0, 0);
            this.titlePanel.Margin = new System.Windows.Forms.Padding(0);
            this.titlePanel.Name = "titlePanel";
            this.titlePanel.ProgramName = "ㅇㅇ프로그램";
            this.titlePanel.Size = new System.Drawing.Size(380, 40);
            this.titlePanel.SystemName = "(ㅇㅇㅇ시스템)";
            this.titlePanel.TabIndex = 12;
            // 
            // mainPanel
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.mainPanel, 2);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 40);
            this.mainPanel.Margin = new System.Windows.Forms.Padding(0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(764, 353);
            this.mainPanel.TabIndex = 16;
            // 
            // refresh_timer
            // 
            this.refresh_timer.Enabled = true;
            this.refresh_timer.Interval = 500;
            this.refresh_timer.Tick += new System.EventHandler(this.On_Refresh_timer);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(764, 461);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 500);
            this.Name = "mainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "mainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.mainForm_FormClosed);
            this.Load += new System.EventHandler(this.mainForm_Load);
            this.Shown += new System.EventHandler(this.mainForm_Shown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.bottomLayoutPanel.ResumeLayout(false);
            this.bottomLayoutPanel.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private wLib.UI.logPanel logPanel;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem mainButton;
        private System.Windows.Forms.ToolStripMenuItem eventButton;
        private System.Windows.Forms.ToolStripMenuItem settingButton;
        private System.Windows.Forms.ToolStripMenuItem settingButton_system;
        private System.Windows.Forms.ToolStripMenuItem settingButton_db;
        private System.Windows.Forms.ToolStripMenuItem settingButton_device;
        private System.Windows.Forms.ToolStripMenuItem testButton;
        private System.Windows.Forms.ToolStripMenuItem helpButton;
        private wLib.UI.TitlePanel titlePanel;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.TableLayoutPanel bottomLayoutPanel;
        private System.Windows.Forms.Button configButton;
        private System.Windows.Forms.Label clockPanel;
        private System.Windows.Forms.Timer refresh_timer;
    }
}