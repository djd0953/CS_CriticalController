namespace CriticalControl.page
{
    partial class settingPage_device
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tablePage = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.resetButton = new System.Windows.Forms.Button();
            this.lockButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ndmsCheck = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.jhCheck = new System.Windows.Forms.CheckBox();
            this.wbCheck = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.critermCb = new System.Windows.Forms.ComboBox();
            this.datatermCb = new System.Windows.Forms.ComboBox();
            this.tabControl.SuspendLayout();
            this.tablePage.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tablePage);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(780, 400);
            this.tabControl.TabIndex = 0;
            // 
            // tablePage
            // 
            this.tablePage.Controls.Add(this.tableLayoutPanel1);
            this.tablePage.Location = new System.Drawing.Point(4, 22);
            this.tablePage.Name = "tablePage";
            this.tablePage.Padding = new System.Windows.Forms.Padding(3);
            this.tablePage.Size = new System.Drawing.Size(772, 374);
            this.tablePage.TabIndex = 0;
            this.tablePage.Text = "장비 설정";
            this.tablePage.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(766, 368);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.resetButton);
            this.panel3.Controls.Add(this.lockButton);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 341);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(760, 24);
            this.panel3.TabIndex = 11;
            // 
            // resetButton
            // 
            this.resetButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.resetButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.resetButton.Location = new System.Drawing.Point(610, 0);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(75, 24);
            this.resetButton.TabIndex = 1;
            this.resetButton.Text = "초기화";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.On_Click_Reset);
            // 
            // lockButton
            // 
            this.lockButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.lockButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lockButton.Location = new System.Drawing.Point(685, 0);
            this.lockButton.Name = "lockButton";
            this.lockButton.Size = new System.Drawing.Size(75, 24);
            this.lockButton.TabIndex = 0;
            this.lockButton.Text = "설정";
            this.lockButton.UseVisualStyleBackColor = true;
            this.lockButton.Click += new System.EventHandler(this.On_Click_Lock);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(760, 332);
            this.panel1.TabIndex = 12;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.datatermCb);
            this.groupBox2.Controls.Add(this.critermCb);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.ndmsCheck);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 100);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(760, 100);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "데이터 관리";
            // 
            // ndmsCheck
            // 
            this.ndmsCheck.AutoSize = true;
            this.ndmsCheck.Location = new System.Drawing.Point(6, 21);
            this.ndmsCheck.Name = "ndmsCheck";
            this.ndmsCheck.Size = new System.Drawing.Size(121, 17);
            this.ndmsCheck.TabIndex = 6;
            this.ndmsCheck.Text = "NDMS 데이터 전송";
            this.ndmsCheck.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.jhCheck);
            this.groupBox1.Controls.Add(this.wbCheck);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(760, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "DB 테이블 구조";
            // 
            // jhCheck
            // 
            this.jhCheck.AutoSize = true;
            this.jhCheck.Location = new System.Drawing.Point(6, 44);
            this.jhCheck.Name = "jhCheck";
            this.jhCheck.Size = new System.Drawing.Size(101, 17);
            this.jhCheck.TabIndex = 5;
            this.jhCheck.Text = "JH 테이블 구조";
            this.jhCheck.UseVisualStyleBackColor = true;
            // 
            // wbCheck
            // 
            this.wbCheck.AutoSize = true;
            this.wbCheck.Location = new System.Drawing.Point(6, 21);
            this.wbCheck.Name = "wbCheck";
            this.wbCheck.Size = new System.Drawing.Size(105, 17);
            this.wbCheck.TabIndex = 4;
            this.wbCheck.Text = "WB 테이블 구조";
            this.wbCheck.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "임계치 데이터 Term";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "데이터 Insert Term";
            // 
            // critermCb
            // 
            this.critermCb.DisplayMember = "Text";
            this.critermCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.critermCb.FormattingEnabled = true;
            this.critermCb.Location = new System.Drawing.Point(116, 38);
            this.critermCb.Name = "critermCb";
            this.critermCb.Size = new System.Drawing.Size(55, 21);
            this.critermCb.TabIndex = 9;
            this.critermCb.ValueMember = "Tag";
            // 
            // datatermCb
            // 
            this.datatermCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.datatermCb.FormattingEnabled = true;
            this.datatermCb.Location = new System.Drawing.Point(116, 63);
            this.datatermCb.Name = "datatermCb";
            this.datatermCb.Size = new System.Drawing.Size(55, 21);
            this.datatermCb.TabIndex = 10;
            // 
            // settingPage_device
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 400);
            this.Controls.Add(this.tabControl);
            this.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "settingPage_device";
            this.Text = "settingPage_cast";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.settingPage_device_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.settingPage_device_FormClosed);
            this.Load += new System.EventHandler(this.settingPage_device_Load);
            this.Shown += new System.EventHandler(this.settingPage_device_Shown);
            this.tabControl.ResumeLayout(false);
            this.tablePage.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tablePage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.Button lockButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox ndmsCheck;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox jhCheck;
        private System.Windows.Forms.CheckBox wbCheck;
        private System.Windows.Forms.ComboBox datatermCb;
        private System.Windows.Forms.ComboBox critermCb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}