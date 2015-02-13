namespace BBox3Tool
{
    partial class Form1
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
            this.buttonConnect = new System.Windows.Forms.Button();
            this.textBoxBboxUrl = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxUsername = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.backgroundWorkerBbox = new System.ComponentModel.BackgroundWorker();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageLogin = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabPageLineInfo = new System.Windows.Forms.TabPage();
            this.buttonClipboard = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelVDSLProfile = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.labelProximusProfile = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.labelVectoring = new System.Windows.Forms.Label();
            this.labelRepair = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.labelDLM = new System.Windows.Forms.Label();
            this.labelUpstreamCurrentBitRate = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.labelDownstreamCurrentBitRate = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.labelUpstreamMaxBitRate = new System.Windows.Forms.Label();
            this.label01 = new System.Windows.Forms.Label();
            this.labelDownstreamMaxBitRate = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelUpstreamNoiseMargin = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.labelDownstreamNoiseMargin = new System.Windows.Forms.Label();
            this.labelDownstreamAttenuation = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.labelUpstreamAttenuation = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tabControlMain.SuspendLayout();
            this.tabPageLogin.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPageLineInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonConnect
            // 
            this.buttonConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonConnect.Location = new System.Drawing.Point(36, 178);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(100, 26);
            this.buttonConnect.TabIndex = 0;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // textBoxBboxUrl
            // 
            this.textBoxBboxUrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxBboxUrl.Location = new System.Drawing.Point(36, 25);
            this.textBoxBboxUrl.Name = "textBoxBboxUrl";
            this.textBoxBboxUrl.Size = new System.Drawing.Size(100, 21);
            this.textBoxBboxUrl.TabIndex = 1;
            this.textBoxBboxUrl.Text = "192.168.1.1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(35, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Bbox3 IP address";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(53, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Username";
            // 
            // textBoxUsername
            // 
            this.textBoxUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxUsername.Location = new System.Drawing.Point(36, 81);
            this.textBoxUsername.Name = "textBoxUsername";
            this.textBoxUsername.Size = new System.Drawing.Size(100, 21);
            this.textBoxUsername.TabIndex = 4;
            this.textBoxUsername.Text = "User";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(54, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Password";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPassword.Location = new System.Drawing.Point(36, 134);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(100, 21);
            this.textBoxPassword.TabIndex = 6;
            // 
            // backgroundWorkerBbox
            // 
            this.backgroundWorkerBbox.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerBbox_DoWork);
            this.backgroundWorkerBbox.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerBbox_ProgressChanged);
            this.backgroundWorkerBbox.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerBbox_RunWorkerCompleted);
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageLogin);
            this.tabControlMain.Controls.Add(this.tabPageLineInfo);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(434, 261);
            this.tabControlMain.TabIndex = 15;
            // 
            // tabPageLogin
            // 
            this.tabPageLogin.BackColor = System.Drawing.Color.Transparent;
            this.tabPageLogin.Controls.Add(this.panel1);
            this.tabPageLogin.Location = new System.Drawing.Point(4, 22);
            this.tabPageLogin.Name = "tabPageLogin";
            this.tabPageLogin.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLogin.Size = new System.Drawing.Size(426, 235);
            this.tabPageLogin.TabIndex = 0;
            this.tabPageLogin.Text = "Login";
            this.tabPageLogin.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.textBoxBboxUrl);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.textBoxUsername);
            this.panel1.Controls.Add(this.textBoxPassword);
            this.panel1.Controls.Add(this.buttonConnect);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(125, 9);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(175, 217);
            this.panel1.TabIndex = 7;
            // 
            // tabPageLineInfo
            // 
            this.tabPageLineInfo.BackColor = System.Drawing.Color.Transparent;
            this.tabPageLineInfo.Controls.Add(this.buttonClipboard);
            this.tabPageLineInfo.Controls.Add(this.buttonCancel);
            this.tabPageLineInfo.Controls.Add(this.labelVDSLProfile);
            this.tabPageLineInfo.Controls.Add(this.label17);
            this.tabPageLineInfo.Controls.Add(this.labelProximusProfile);
            this.tabPageLineInfo.Controls.Add(this.label15);
            this.tabPageLineInfo.Controls.Add(this.labelVectoring);
            this.tabPageLineInfo.Controls.Add(this.labelRepair);
            this.tabPageLineInfo.Controls.Add(this.label13);
            this.tabPageLineInfo.Controls.Add(this.label11);
            this.tabPageLineInfo.Controls.Add(this.label6);
            this.tabPageLineInfo.Controls.Add(this.labelDLM);
            this.tabPageLineInfo.Controls.Add(this.labelUpstreamCurrentBitRate);
            this.tabPageLineInfo.Controls.Add(this.label10);
            this.tabPageLineInfo.Controls.Add(this.labelDownstreamCurrentBitRate);
            this.tabPageLineInfo.Controls.Add(this.label12);
            this.tabPageLineInfo.Controls.Add(this.labelUpstreamMaxBitRate);
            this.tabPageLineInfo.Controls.Add(this.label01);
            this.tabPageLineInfo.Controls.Add(this.labelDownstreamMaxBitRate);
            this.tabPageLineInfo.Controls.Add(this.label7);
            this.tabPageLineInfo.Controls.Add(this.label4);
            this.tabPageLineInfo.Controls.Add(this.labelUpstreamNoiseMargin);
            this.tabPageLineInfo.Controls.Add(this.label5);
            this.tabPageLineInfo.Controls.Add(this.labelDownstreamNoiseMargin);
            this.tabPageLineInfo.Controls.Add(this.labelDownstreamAttenuation);
            this.tabPageLineInfo.Controls.Add(this.label8);
            this.tabPageLineInfo.Controls.Add(this.labelUpstreamAttenuation);
            this.tabPageLineInfo.Controls.Add(this.label9);
            this.tabPageLineInfo.Location = new System.Drawing.Point(4, 22);
            this.tabPageLineInfo.Name = "tabPageLineInfo";
            this.tabPageLineInfo.Size = new System.Drawing.Size(426, 235);
            this.tabPageLineInfo.TabIndex = 2;
            this.tabPageLineInfo.Text = "Line Info";
            this.tabPageLineInfo.UseVisualStyleBackColor = true;
            // 
            // buttonClipboard
            // 
            this.buttonClipboard.Location = new System.Drawing.Point(268, 166);
            this.buttonClipboard.Name = "buttonClipboard";
            this.buttonClipboard.Size = new System.Drawing.Size(83, 23);
            this.buttonClipboard.TabIndex = 42;
            this.buttonClipboard.Text = "To Clipboard";
            this.buttonClipboard.UseVisualStyleBackColor = true;
            this.buttonClipboard.Click += new System.EventHandler(this.buttonClipboard_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Enabled = false;
            this.buttonCancel.Location = new System.Drawing.Point(268, 195);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(83, 23);
            this.buttonCancel.TabIndex = 41;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // labelVDSLProfile
            // 
            this.labelVDSLProfile.AutoSize = true;
            this.labelVDSLProfile.Location = new System.Drawing.Point(350, 35);
            this.labelVDSLProfile.Name = "labelVDSLProfile";
            this.labelVDSLProfile.Size = new System.Drawing.Size(51, 13);
            this.labelVDSLProfile.TabIndex = 40;
            this.labelVDSLProfile.Text = "unknown";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(268, 35);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(75, 13);
            this.label17.TabIndex = 39;
            this.label17.Text = "VDSL2 profile:";
            // 
            // labelProximusProfile
            // 
            this.labelProximusProfile.AutoSize = true;
            this.labelProximusProfile.Location = new System.Drawing.Point(350, 10);
            this.labelProximusProfile.Name = "labelProximusProfile";
            this.labelProximusProfile.Size = new System.Drawing.Size(51, 13);
            this.labelProximusProfile.TabIndex = 38;
            this.labelProximusProfile.Text = "unknown";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(268, 10);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(83, 13);
            this.label15.TabIndex = 37;
            this.label15.Text = "Proximus profile:";
            // 
            // labelVectoring
            // 
            this.labelVectoring.AutoSize = true;
            this.labelVectoring.Location = new System.Drawing.Point(350, 123);
            this.labelVectoring.Name = "labelVectoring";
            this.labelVectoring.Size = new System.Drawing.Size(51, 13);
            this.labelVectoring.TabIndex = 36;
            this.labelVectoring.Text = "unknown";
            // 
            // labelRepair
            // 
            this.labelRepair.AutoSize = true;
            this.labelRepair.Location = new System.Drawing.Point(350, 91);
            this.labelRepair.Name = "labelRepair";
            this.labelRepair.Size = new System.Drawing.Size(51, 13);
            this.labelRepair.TabIndex = 35;
            this.labelRepair.Text = "unknown";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(268, 123);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(55, 13);
            this.label13.TabIndex = 34;
            this.label13.Text = "Vectoring:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(268, 91);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(41, 13);
            this.label11.TabIndex = 33;
            this.label11.Text = "Repair:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(268, 67);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 13);
            this.label6.TabIndex = 32;
            this.label6.Text = "DLM:";
            // 
            // labelDLM
            // 
            this.labelDLM.AutoSize = true;
            this.labelDLM.Location = new System.Drawing.Point(350, 67);
            this.labelDLM.Name = "labelDLM";
            this.labelDLM.Size = new System.Drawing.Size(51, 13);
            this.labelDLM.TabIndex = 31;
            this.labelDLM.Text = "unknown";
            // 
            // labelUpstreamCurrentBitRate
            // 
            this.labelUpstreamCurrentBitRate.AutoSize = true;
            this.labelUpstreamCurrentBitRate.Location = new System.Drawing.Point(159, 35);
            this.labelUpstreamCurrentBitRate.Name = "labelUpstreamCurrentBitRate";
            this.labelUpstreamCurrentBitRate.Size = new System.Drawing.Size(22, 13);
            this.labelUpstreamCurrentBitRate.TabIndex = 30;
            this.labelUpstreamCurrentBitRate.Text = "0.0";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(8, 35);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(126, 13);
            this.label10.TabIndex = 29;
            this.label10.Text = "Upstream current bit rate:";
            // 
            // labelDownstreamCurrentBitRate
            // 
            this.labelDownstreamCurrentBitRate.AutoSize = true;
            this.labelDownstreamCurrentBitRate.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelDownstreamCurrentBitRate.Location = new System.Drawing.Point(159, 11);
            this.labelDownstreamCurrentBitRate.Name = "labelDownstreamCurrentBitRate";
            this.labelDownstreamCurrentBitRate.Size = new System.Drawing.Size(22, 13);
            this.labelDownstreamCurrentBitRate.TabIndex = 28;
            this.labelDownstreamCurrentBitRate.Text = "0.0";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label12.Location = new System.Drawing.Point(8, 10);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(140, 13);
            this.label12.TabIndex = 27;
            this.label12.Text = "Downstream current bit rate:";
            // 
            // labelUpstreamMaxBitRate
            // 
            this.labelUpstreamMaxBitRate.AutoSize = true;
            this.labelUpstreamMaxBitRate.Location = new System.Drawing.Point(159, 91);
            this.labelUpstreamMaxBitRate.Name = "labelUpstreamMaxBitRate";
            this.labelUpstreamMaxBitRate.Size = new System.Drawing.Size(22, 13);
            this.labelUpstreamMaxBitRate.TabIndex = 26;
            this.labelUpstreamMaxBitRate.Text = "0.0";
            // 
            // label01
            // 
            this.label01.AutoSize = true;
            this.label01.Location = new System.Drawing.Point(8, 91);
            this.label01.Name = "label01";
            this.label01.Size = new System.Drawing.Size(112, 13);
            this.label01.TabIndex = 25;
            this.label01.Text = "Upstream max bit rate:";
            // 
            // labelDownstreamMaxBitRate
            // 
            this.labelDownstreamMaxBitRate.AutoSize = true;
            this.labelDownstreamMaxBitRate.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelDownstreamMaxBitRate.Location = new System.Drawing.Point(159, 67);
            this.labelDownstreamMaxBitRate.Name = "labelDownstreamMaxBitRate";
            this.labelDownstreamMaxBitRate.Size = new System.Drawing.Size(22, 13);
            this.labelDownstreamMaxBitRate.TabIndex = 24;
            this.labelDownstreamMaxBitRate.Text = "0.0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label7.Location = new System.Drawing.Point(8, 67);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(126, 13);
            this.label7.TabIndex = 23;
            this.label7.Text = "Downstream max bit rate:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Downstream attenuation:";
            // 
            // labelUpstreamNoiseMargin
            // 
            this.labelUpstreamNoiseMargin.AutoSize = true;
            this.labelUpstreamNoiseMargin.Location = new System.Drawing.Point(159, 200);
            this.labelUpstreamNoiseMargin.Name = "labelUpstreamNoiseMargin";
            this.labelUpstreamNoiseMargin.Size = new System.Drawing.Size(22, 13);
            this.labelUpstreamNoiseMargin.TabIndex = 22;
            this.labelUpstreamNoiseMargin.Text = "0.0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 147);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(111, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Upstream attenuation:";
            // 
            // labelDownstreamNoiseMargin
            // 
            this.labelDownstreamNoiseMargin.AutoSize = true;
            this.labelDownstreamNoiseMargin.Location = new System.Drawing.Point(159, 176);
            this.labelDownstreamNoiseMargin.Name = "labelDownstreamNoiseMargin";
            this.labelDownstreamNoiseMargin.Size = new System.Drawing.Size(22, 13);
            this.labelDownstreamNoiseMargin.TabIndex = 21;
            this.labelDownstreamNoiseMargin.Text = "0.0";
            // 
            // labelDownstreamAttenuation
            // 
            this.labelDownstreamAttenuation.AutoSize = true;
            this.labelDownstreamAttenuation.Location = new System.Drawing.Point(159, 123);
            this.labelDownstreamAttenuation.Name = "labelDownstreamAttenuation";
            this.labelDownstreamAttenuation.Size = new System.Drawing.Size(22, 13);
            this.labelDownstreamAttenuation.TabIndex = 17;
            this.labelDownstreamAttenuation.Text = "0.0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 200);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(117, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Upstream noise margin:";
            // 
            // labelUpstreamAttenuation
            // 
            this.labelUpstreamAttenuation.AutoSize = true;
            this.labelUpstreamAttenuation.Location = new System.Drawing.Point(159, 147);
            this.labelUpstreamAttenuation.Name = "labelUpstreamAttenuation";
            this.labelUpstreamAttenuation.Size = new System.Drawing.Size(22, 13);
            this.labelUpstreamAttenuation.TabIndex = 18;
            this.labelUpstreamAttenuation.Text = "0.0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 176);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(131, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "Downstream noise margin:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 261);
            this.Controls.Add(this.tabControlMain);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(450, 300);
            this.MinimumSize = new System.Drawing.Size(450, 300);
            this.Name = "Form1";
            this.Text = "Bbox 3 Sagem Tool v0.4";
            this.tabControlMain.ResumeLayout(false);
            this.tabPageLogin.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPageLineInfo.ResumeLayout(false);
            this.tabPageLineInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.TextBox textBoxBboxUrl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxUsername;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.ComponentModel.BackgroundWorker backgroundWorkerBbox;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageLogin;
        private System.Windows.Forms.TabPage tabPageLineInfo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelUpstreamNoiseMargin;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelDownstreamNoiseMargin;
        private System.Windows.Forms.Label labelDownstreamAttenuation;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label labelUpstreamAttenuation;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label labelUpstreamMaxBitRate;
        private System.Windows.Forms.Label label01;
        private System.Windows.Forms.Label labelDownstreamMaxBitRate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelUpstreamCurrentBitRate;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label labelDownstreamCurrentBitRate;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label labelVectoring;
        private System.Windows.Forms.Label labelRepair;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelDLM;
        private System.Windows.Forms.Label labelVDSLProfile;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label labelProximusProfile;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonClipboard;
    }
}

