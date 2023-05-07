namespace CriticalControl.page
{
    partial class mainPage
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.TopPanel = new System.Windows.Forms.TableLayoutPanel();
            this.runButton = new System.Windows.Forms.Button();
            this.runMode_manual = new System.Windows.Forms.RadioButton();
            this.runInterval = new System.Windows.Forms.ComboBox();
            this.runMode_auto = new System.Windows.Forms.RadioButton();
            this.runPanel = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.gCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AltName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nowCriticalDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nextCriticalDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nowDataDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.altDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nowLevelDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.process_timer = new System.Windows.Forms.Timer(this.components);
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.groupBox1.SuspendLayout();
            this.TopPanel.SuspendLayout();
            this.runPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataBindingSource)).BeginInit();
            this.bottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.TopPanel);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(5, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(0);
            this.groupBox1.Size = new System.Drawing.Size(390, 40);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "동작설정";
            // 
            // TopPanel
            // 
            this.TopPanel.ColumnCount = 5;
            this.TopPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.TopPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.TopPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.TopPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.TopPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.TopPanel.Controls.Add(this.runButton, 4, 0);
            this.TopPanel.Controls.Add(this.runMode_manual, 3, 0);
            this.TopPanel.Controls.Add(this.runInterval, 1, 0);
            this.TopPanel.Controls.Add(this.runMode_auto, 0, 0);
            this.TopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TopPanel.Location = new System.Drawing.Point(0, 15);
            this.TopPanel.Name = "TopPanel";
            this.TopPanel.RowCount = 1;
            this.TopPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TopPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.TopPanel.Size = new System.Drawing.Size(390, 25);
            this.TopPanel.TabIndex = 0;
            // 
            // runButton
            // 
            this.runButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runButton.Location = new System.Drawing.Point(273, 3);
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(114, 19);
            this.runButton.TabIndex = 6;
            this.runButton.Text = "실 행";
            this.runButton.UseVisualStyleBackColor = true;
            this.runButton.Click += new System.EventHandler(this.On_Click_Run);
            // 
            // runMode_manual
            // 
            this.runMode_manual.AutoSize = true;
            this.runMode_manual.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runMode_manual.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.runMode_manual.Location = new System.Drawing.Point(192, 3);
            this.runMode_manual.Name = "runMode_manual";
            this.runMode_manual.Size = new System.Drawing.Size(75, 19);
            this.runMode_manual.TabIndex = 5;
            this.runMode_manual.TabStop = true;
            this.runMode_manual.Text = "수동";
            this.runMode_manual.UseVisualStyleBackColor = true;
            this.runMode_manual.CheckedChanged += new System.EventHandler(this.On_Checked_RunMode);
            // 
            // runInterval
            // 
            this.runInterval.DisplayMember = "Text";
            this.runInterval.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.runInterval.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.runInterval.FormattingEnabled = true;
            this.runInterval.Location = new System.Drawing.Point(84, 3);
            this.runInterval.Name = "runInterval";
            this.runInterval.Size = new System.Drawing.Size(75, 21);
            this.runInterval.TabIndex = 4;
            this.runInterval.ValueMember = "Tag";
            // 
            // runMode_auto
            // 
            this.runMode_auto.AutoSize = true;
            this.runMode_auto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runMode_auto.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.runMode_auto.Location = new System.Drawing.Point(3, 3);
            this.runMode_auto.Name = "runMode_auto";
            this.runMode_auto.Size = new System.Drawing.Size(75, 19);
            this.runMode_auto.TabIndex = 2;
            this.runMode_auto.TabStop = true;
            this.runMode_auto.Text = "자동";
            this.runMode_auto.UseVisualStyleBackColor = true;
            this.runMode_auto.CheckedChanged += new System.EventHandler(this.On_Checked_RunMode);
            // 
            // runPanel
            // 
            this.runPanel.ColumnCount = 1;
            this.runPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.runPanel.Controls.Add(this.dataGridView, 0, 0);
            this.runPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runPanel.Location = new System.Drawing.Point(5, 45);
            this.runPanel.Margin = new System.Windows.Forms.Padding(0);
            this.runPanel.Name = "runPanel";
            this.runPanel.RowCount = 1;
            this.runPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.runPanel.Size = new System.Drawing.Size(390, 350);
            this.runPanel.TabIndex = 1;
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AllowUserToOrderColumns = true;
            this.dataGridView.AllowUserToResizeRows = false;
            this.dataGridView.AutoGenerateColumns = false;
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("맑은 고딕", 8F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.gCodeDataGridViewTextBoxColumn,
            this.gNameDataGridViewTextBoxColumn,
            this.AltName,
            this.nowCriticalDataGridViewTextBoxColumn,
            this.nextCriticalDataGridViewTextBoxColumn,
            this.nowDataDataGridViewTextBoxColumn,
            this.altDateDataGridViewTextBoxColumn,
            this.nowLevelDataGridViewTextBoxColumn});
            this.dataGridView.DataSource = this.dataBindingSource;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("맑은 고딕", 8F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView.Location = new System.Drawing.Point(3, 3);
            this.dataGridView.MultiSelect = false;
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.RowTemplate.Height = 23;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.Size = new System.Drawing.Size(384, 344);
            this.dataGridView.TabIndex = 0;
            this.dataGridView.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.DataGridView_DataBindingComplete);
            this.dataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DataGridView_DataError);
            this.dataGridView.SelectionChanged += new System.EventHandler(this.DataGridView_SelectionChanged);
            // 
            // gCodeDataGridViewTextBoxColumn
            // 
            this.gCodeDataGridViewTextBoxColumn.DataPropertyName = "GCode";
            this.gCodeDataGridViewTextBoxColumn.FillWeight = 78.47359F;
            this.gCodeDataGridViewTextBoxColumn.HeaderText = "번호";
            this.gCodeDataGridViewTextBoxColumn.Name = "gCodeDataGridViewTextBoxColumn";
            this.gCodeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // gNameDataGridViewTextBoxColumn
            // 
            this.gNameDataGridViewTextBoxColumn.DataPropertyName = "GName";
            this.gNameDataGridViewTextBoxColumn.FillWeight = 157.757F;
            this.gNameDataGridViewTextBoxColumn.HeaderText = "경보명";
            this.gNameDataGridViewTextBoxColumn.Name = "gNameDataGridViewTextBoxColumn";
            this.gNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // AltName
            // 
            this.AltName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.AltName.DataPropertyName = "AltName";
            this.AltName.FillWeight = 154.0517F;
            this.AltName.HeaderText = "장비명";
            this.AltName.Name = "AltName";
            this.AltName.ReadOnly = true;
            // 
            // nowCriticalDataGridViewTextBoxColumn
            // 
            this.nowCriticalDataGridViewTextBoxColumn.DataPropertyName = "NowCritical";
            this.nowCriticalDataGridViewTextBoxColumn.FillWeight = 64.37556F;
            this.nowCriticalDataGridViewTextBoxColumn.HeaderText = "현재임계치";
            this.nowCriticalDataGridViewTextBoxColumn.Name = "nowCriticalDataGridViewTextBoxColumn";
            this.nowCriticalDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // nextCriticalDataGridViewTextBoxColumn
            // 
            this.nextCriticalDataGridViewTextBoxColumn.DataPropertyName = "NextCritical";
            this.nextCriticalDataGridViewTextBoxColumn.FillWeight = 63.55005F;
            this.nextCriticalDataGridViewTextBoxColumn.HeaderText = "다음임계치";
            this.nextCriticalDataGridViewTextBoxColumn.Name = "nextCriticalDataGridViewTextBoxColumn";
            this.nextCriticalDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // nowDataDataGridViewTextBoxColumn
            // 
            this.nowDataDataGridViewTextBoxColumn.DataPropertyName = "NowData";
            this.nowDataDataGridViewTextBoxColumn.FillWeight = 53.8695F;
            this.nowDataDataGridViewTextBoxColumn.HeaderText = "데이터";
            this.nowDataDataGridViewTextBoxColumn.Name = "nowDataDataGridViewTextBoxColumn";
            this.nowDataDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // altDateDataGridViewTextBoxColumn
            // 
            this.altDateDataGridViewTextBoxColumn.DataPropertyName = "AltDate";
            this.altDateDataGridViewTextBoxColumn.FillWeight = 146.7042F;
            this.altDateDataGridViewTextBoxColumn.HeaderText = "발령시간";
            this.altDateDataGridViewTextBoxColumn.Name = "altDateDataGridViewTextBoxColumn";
            this.altDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // nowLevelDataGridViewTextBoxColumn
            // 
            this.nowLevelDataGridViewTextBoxColumn.DataPropertyName = "NowLevel";
            this.nowLevelDataGridViewTextBoxColumn.FillWeight = 81.21826F;
            this.nowLevelDataGridViewTextBoxColumn.HeaderText = "상태";
            this.nowLevelDataGridViewTextBoxColumn.Name = "nowLevelDataGridViewTextBoxColumn";
            this.nowLevelDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // dataBindingSource
            // 
            this.dataBindingSource.DataSource = typeof(wLib.DB.WB_ISUALERTVIEW_VO);
            // 
            // process_timer
            // 
            this.process_timer.Interval = 1000;
            this.process_timer.Tick += new System.EventHandler(this.On_Process_Timer);
            // 
            // bottomPanel
            // 
            this.bottomPanel.Controls.Add(this.progressBar);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Location = new System.Drawing.Point(5, 385);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Padding = new System.Windows.Forms.Padding(3);
            this.bottomPanel.Size = new System.Drawing.Size(390, 10);
            this.bottomPanel.TabIndex = 2;
            // 
            // progressBar
            // 
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar.Location = new System.Drawing.Point(3, 3);
            this.progressBar.Maximum = 60;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(384, 4);
            this.progressBar.TabIndex = 0;
            // 
            // mainPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(400, 400);
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this.runPanel);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "mainPage";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Text = "mainPage";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainPage_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.mainPage_FormClosed);
            this.Load += new System.EventHandler(this.mainPage_Load);
            this.Shown += new System.EventHandler(this.mainPage_Shown);
            this.groupBox1.ResumeLayout(false);
            this.TopPanel.ResumeLayout(false);
            this.TopPanel.PerformLayout();
            this.runPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataBindingSource)).EndInit();
            this.bottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel TopPanel;
        private System.Windows.Forms.Button runButton;
        private System.Windows.Forms.RadioButton runMode_manual;
        private System.Windows.Forms.ComboBox runInterval;
        private System.Windows.Forms.RadioButton runMode_auto;
        private System.Windows.Forms.TableLayoutPanel runPanel;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Timer process_timer;
        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.BindingSource dataBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn gCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn gNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn AltName;
        private System.Windows.Forms.DataGridViewTextBoxColumn nowCriticalDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nextCriticalDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nowDataDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn altDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nowLevelDataGridViewTextBoxColumn;
    }
}