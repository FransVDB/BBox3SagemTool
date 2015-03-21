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
            this.backgroundWorkerBbox = new System.ComponentModel.BackgroundWorker();
            this.panelDebug = new System.Windows.Forms.Panel();
            this.label14 = new System.Windows.Forms.Label();
            this.textBoxDebug = new System.Windows.Forms.TextBox();
            this.buttonDebug = new System.Windows.Forms.Button();
            this.textBoxDebugResult = new System.Windows.Forms.TextBox();
            this.buttonClipboard = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelVDSLProfile = new System.Windows.Forms.Label();
            this.labelVDSLProfileLabel = new System.Windows.Forms.Label();
            this.labelProximusProfile = new System.Windows.Forms.Label();
            this.labelProximusProfileLabel = new System.Windows.Forms.Label();
            this.labelVectoring = new System.Windows.Forms.Label();
            this.labelRepair = new System.Windows.Forms.Label();
            this.labelVectoringLabel = new System.Windows.Forms.Label();
            this.labelRepairLabel = new System.Windows.Forms.Label();
            this.labelDLMLabel = new System.Windows.Forms.Label();
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
            this.labelDownstreamNoiseMargin = new System.Windows.Forms.Label();
            this.labelDownstreamAttenuation = new System.Windows.Forms.Label();
            this.labelUpstreamAttenuation = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.labelDSLStandard = new System.Windows.Forms.Label();
            this.labelLinkUptime = new System.Windows.Forms.Label();
            this.labelDeviceUptime = new System.Windows.Forms.Label();
            this.labelGUIVersion = new System.Windows.Forms.Label();
            this.labelHardwareVersion = new System.Windows.Forms.Label();
            this.labelSoftwareVersion = new System.Windows.Forms.Label();
            this.panelLogin = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxBboxUrl = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxUsername = new System.Windows.Forms.TextBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.panelInfo = new System.Windows.Forms.Panel();
            this.labelDistance = new System.Windows.Forms.Label();
            this.panelDebug.SuspendLayout();
            this.panelLogin.SuspendLayout();
            this.panelInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // backgroundWorkerBbox
            // 
            this.backgroundWorkerBbox.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerBbox_DoWork);
            this.backgroundWorkerBbox.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerBbox_ProgressChanged);
            this.backgroundWorkerBbox.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerBbox_RunWorkerCompleted);
            // 
            // panelDebug
            // 
            this.panelDebug.Controls.Add(this.label14);
            this.panelDebug.Controls.Add(this.textBoxDebug);
            this.panelDebug.Controls.Add(this.buttonDebug);
            this.panelDebug.Controls.Add(this.textBoxDebugResult);
            this.panelDebug.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDebug.Location = new System.Drawing.Point(0, 0);
            this.panelDebug.Name = "panelDebug";
            this.panelDebug.Size = new System.Drawing.Size(704, 331);
            this.panelDebug.TabIndex = 43;
            this.panelDebug.Visible = false;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 8);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(38, 13);
            this.label14.TabIndex = 0;
            this.label14.Text = "Xpath:";
            // 
            // textBoxDebug
            // 
            this.textBoxDebug.Location = new System.Drawing.Point(47, 5);
            this.textBoxDebug.Name = "textBoxDebug";
            this.textBoxDebug.Size = new System.Drawing.Size(569, 20);
            this.textBoxDebug.TabIndex = 1;
            this.textBoxDebug.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxDebug_KeyDown);
            // 
            // buttonDebug
            // 
            this.buttonDebug.Location = new System.Drawing.Point(625, 3);
            this.buttonDebug.Name = "buttonDebug";
            this.buttonDebug.Size = new System.Drawing.Size(75, 23);
            this.buttonDebug.TabIndex = 2;
            this.buttonDebug.Text = "Send";
            this.buttonDebug.UseVisualStyleBackColor = true;
            this.buttonDebug.Click += new System.EventHandler(this.buttonDebug_Click);
            // 
            // textBoxDebugResult
            // 
            this.textBoxDebugResult.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBoxDebugResult.Location = new System.Drawing.Point(0, 32);
            this.textBoxDebugResult.Multiline = true;
            this.textBoxDebugResult.Name = "textBoxDebugResult";
            this.textBoxDebugResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxDebugResult.Size = new System.Drawing.Size(704, 299);
            this.textBoxDebugResult.TabIndex = 3;
            // 
            // buttonClipboard
            // 
            this.buttonClipboard.Location = new System.Drawing.Point(12, 282);
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
            this.buttonCancel.Location = new System.Drawing.Point(111, 282);
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
            this.labelVDSLProfile.Location = new System.Drawing.Point(364, 76);
            this.labelVDSLProfile.Name = "labelVDSLProfile";
            this.labelVDSLProfile.Size = new System.Drawing.Size(51, 13);
            this.labelVDSLProfile.TabIndex = 40;
            this.labelVDSLProfile.Text = "unknown";
            // 
            // labelVDSLProfileLabel
            // 
            this.labelVDSLProfileLabel.AutoSize = true;
            this.labelVDSLProfileLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(138)))));
            this.labelVDSLProfileLabel.Location = new System.Drawing.Point(262, 76);
            this.labelVDSLProfileLabel.Name = "labelVDSLProfileLabel";
            this.labelVDSLProfileLabel.Size = new System.Drawing.Size(72, 13);
            this.labelVDSLProfileLabel.TabIndex = 39;
            this.labelVDSLProfileLabel.Text = "VDSL2 profile";
            // 
            // labelProximusProfile
            // 
            this.labelProximusProfile.AutoSize = true;
            this.labelProximusProfile.Location = new System.Drawing.Point(364, 132);
            this.labelProximusProfile.Name = "labelProximusProfile";
            this.labelProximusProfile.Size = new System.Drawing.Size(51, 13);
            this.labelProximusProfile.TabIndex = 38;
            this.labelProximusProfile.Text = "unknown";
            // 
            // labelProximusProfileLabel
            // 
            this.labelProximusProfileLabel.AutoSize = true;
            this.labelProximusProfileLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(138)))));
            this.labelProximusProfileLabel.Location = new System.Drawing.Point(262, 132);
            this.labelProximusProfileLabel.Name = "labelProximusProfileLabel";
            this.labelProximusProfileLabel.Size = new System.Drawing.Size(80, 13);
            this.labelProximusProfileLabel.TabIndex = 37;
            this.labelProximusProfileLabel.Text = "Proximus profile";
            // 
            // labelVectoring
            // 
            this.labelVectoring.AutoSize = true;
            this.labelVectoring.Location = new System.Drawing.Point(364, 108);
            this.labelVectoring.Name = "labelVectoring";
            this.labelVectoring.Size = new System.Drawing.Size(51, 13);
            this.labelVectoring.TabIndex = 36;
            this.labelVectoring.Text = "unknown";
            // 
            // labelRepair
            // 
            this.labelRepair.AutoSize = true;
            this.labelRepair.Location = new System.Drawing.Point(364, 188);
            this.labelRepair.Name = "labelRepair";
            this.labelRepair.Size = new System.Drawing.Size(51, 13);
            this.labelRepair.TabIndex = 35;
            this.labelRepair.Text = "unknown";
            // 
            // labelVectoringLabel
            // 
            this.labelVectoringLabel.AutoSize = true;
            this.labelVectoringLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(138)))));
            this.labelVectoringLabel.Location = new System.Drawing.Point(262, 108);
            this.labelVectoringLabel.Name = "labelVectoringLabel";
            this.labelVectoringLabel.Size = new System.Drawing.Size(52, 13);
            this.labelVectoringLabel.TabIndex = 34;
            this.labelVectoringLabel.Text = "Vectoring";
            // 
            // labelRepairLabel
            // 
            this.labelRepairLabel.AutoSize = true;
            this.labelRepairLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(138)))));
            this.labelRepairLabel.Location = new System.Drawing.Point(262, 188);
            this.labelRepairLabel.Name = "labelRepairLabel";
            this.labelRepairLabel.Size = new System.Drawing.Size(38, 13);
            this.labelRepairLabel.TabIndex = 33;
            this.labelRepairLabel.Text = "Repair";
            // 
            // labelDLMLabel
            // 
            this.labelDLMLabel.AutoSize = true;
            this.labelDLMLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(138)))));
            this.labelDLMLabel.Location = new System.Drawing.Point(262, 164);
            this.labelDLMLabel.Name = "labelDLMLabel";
            this.labelDLMLabel.Size = new System.Drawing.Size(30, 13);
            this.labelDLMLabel.TabIndex = 32;
            this.labelDLMLabel.Text = "DLM";
            // 
            // labelDLM
            // 
            this.labelDLM.AutoSize = true;
            this.labelDLM.Location = new System.Drawing.Point(364, 162);
            this.labelDLM.Name = "labelDLM";
            this.labelDLM.Size = new System.Drawing.Size(51, 13);
            this.labelDLM.TabIndex = 31;
            this.labelDLM.Text = "unknown";
            // 
            // labelUpstreamCurrentBitRate
            // 
            this.labelUpstreamCurrentBitRate.AutoSize = true;
            this.labelUpstreamCurrentBitRate.Location = new System.Drawing.Point(160, 76);
            this.labelUpstreamCurrentBitRate.Name = "labelUpstreamCurrentBitRate";
            this.labelUpstreamCurrentBitRate.Size = new System.Drawing.Size(22, 13);
            this.labelUpstreamCurrentBitRate.TabIndex = 30;
            this.labelUpstreamCurrentBitRate.Text = "0.0";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(138)))));
            this.label10.Location = new System.Drawing.Point(9, 76);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(123, 13);
            this.label10.TabIndex = 29;
            this.label10.Text = "Upstream current bit rate";
            // 
            // labelDownstreamCurrentBitRate
            // 
            this.labelDownstreamCurrentBitRate.AutoSize = true;
            this.labelDownstreamCurrentBitRate.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelDownstreamCurrentBitRate.Location = new System.Drawing.Point(160, 52);
            this.labelDownstreamCurrentBitRate.Name = "labelDownstreamCurrentBitRate";
            this.labelDownstreamCurrentBitRate.Size = new System.Drawing.Size(22, 13);
            this.labelDownstreamCurrentBitRate.TabIndex = 28;
            this.labelDownstreamCurrentBitRate.Text = "0.0";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(138)))));
            this.label12.Location = new System.Drawing.Point(9, 51);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(137, 13);
            this.label12.TabIndex = 27;
            this.label12.Text = "Downstream current bit rate";
            // 
            // labelUpstreamMaxBitRate
            // 
            this.labelUpstreamMaxBitRate.AutoSize = true;
            this.labelUpstreamMaxBitRate.Location = new System.Drawing.Point(160, 132);
            this.labelUpstreamMaxBitRate.Name = "labelUpstreamMaxBitRate";
            this.labelUpstreamMaxBitRate.Size = new System.Drawing.Size(22, 13);
            this.labelUpstreamMaxBitRate.TabIndex = 26;
            this.labelUpstreamMaxBitRate.Text = "0.0";
            // 
            // label01
            // 
            this.label01.AutoSize = true;
            this.label01.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(138)))));
            this.label01.Location = new System.Drawing.Point(9, 132);
            this.label01.Name = "label01";
            this.label01.Size = new System.Drawing.Size(109, 13);
            this.label01.TabIndex = 25;
            this.label01.Text = "Upstream max bit rate";
            // 
            // labelDownstreamMaxBitRate
            // 
            this.labelDownstreamMaxBitRate.AutoSize = true;
            this.labelDownstreamMaxBitRate.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelDownstreamMaxBitRate.Location = new System.Drawing.Point(160, 108);
            this.labelDownstreamMaxBitRate.Name = "labelDownstreamMaxBitRate";
            this.labelDownstreamMaxBitRate.Size = new System.Drawing.Size(22, 13);
            this.labelDownstreamMaxBitRate.TabIndex = 24;
            this.labelDownstreamMaxBitRate.Text = "0.0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(138)))));
            this.label7.Location = new System.Drawing.Point(9, 108);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(123, 13);
            this.label7.TabIndex = 23;
            this.label7.Text = "Downstream max bit rate";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(138)))));
            this.label4.Location = new System.Drawing.Point(9, 164);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(122, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Downstream attenuation";
            // 
            // labelUpstreamNoiseMargin
            // 
            this.labelUpstreamNoiseMargin.AutoSize = true;
            this.labelUpstreamNoiseMargin.Location = new System.Drawing.Point(160, 241);
            this.labelUpstreamNoiseMargin.Name = "labelUpstreamNoiseMargin";
            this.labelUpstreamNoiseMargin.Size = new System.Drawing.Size(22, 13);
            this.labelUpstreamNoiseMargin.TabIndex = 22;
            this.labelUpstreamNoiseMargin.Text = "0.0";
            // 
            // labelDownstreamNoiseMargin
            // 
            this.labelDownstreamNoiseMargin.AutoSize = true;
            this.labelDownstreamNoiseMargin.Location = new System.Drawing.Point(160, 217);
            this.labelDownstreamNoiseMargin.Name = "labelDownstreamNoiseMargin";
            this.labelDownstreamNoiseMargin.Size = new System.Drawing.Size(22, 13);
            this.labelDownstreamNoiseMargin.TabIndex = 21;
            this.labelDownstreamNoiseMargin.Text = "0.0";
            // 
            // labelDownstreamAttenuation
            // 
            this.labelDownstreamAttenuation.AutoSize = true;
            this.labelDownstreamAttenuation.Location = new System.Drawing.Point(160, 164);
            this.labelDownstreamAttenuation.Name = "labelDownstreamAttenuation";
            this.labelDownstreamAttenuation.Size = new System.Drawing.Size(22, 13);
            this.labelDownstreamAttenuation.TabIndex = 17;
            this.labelDownstreamAttenuation.Text = "0.0";
            // 
            // labelUpstreamAttenuation
            // 
            this.labelUpstreamAttenuation.AutoSize = true;
            this.labelUpstreamAttenuation.Location = new System.Drawing.Point(160, 188);
            this.labelUpstreamAttenuation.Name = "labelUpstreamAttenuation";
            this.labelUpstreamAttenuation.Size = new System.Drawing.Size(22, 13);
            this.labelUpstreamAttenuation.TabIndex = 18;
            this.labelUpstreamAttenuation.Text = "0.0";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(185)))), ((int)(((byte)(255)))));
            this.label16.Location = new System.Drawing.Point(9, 16);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(39, 20);
            this.label16.TabIndex = 46;
            this.label16.Text = "Line";
            // 
            // labelDSLStandard
            // 
            this.labelDSLStandard.AutoSize = true;
            this.labelDSLStandard.Location = new System.Drawing.Point(364, 51);
            this.labelDSLStandard.Name = "labelDSLStandard";
            this.labelDSLStandard.Size = new System.Drawing.Size(51, 13);
            this.labelDSLStandard.TabIndex = 49;
            this.labelDSLStandard.Text = "unknown";
            // 
            // labelLinkUptime
            // 
            this.labelLinkUptime.AutoSize = true;
            this.labelLinkUptime.Location = new System.Drawing.Point(589, 76);
            this.labelLinkUptime.Name = "labelLinkUptime";
            this.labelLinkUptime.Size = new System.Drawing.Size(51, 13);
            this.labelLinkUptime.TabIndex = 60;
            this.labelLinkUptime.Text = "unknown";
            // 
            // labelDeviceUptime
            // 
            this.labelDeviceUptime.AutoSize = true;
            this.labelDeviceUptime.Location = new System.Drawing.Point(589, 51);
            this.labelDeviceUptime.Name = "labelDeviceUptime";
            this.labelDeviceUptime.Size = new System.Drawing.Size(51, 13);
            this.labelDeviceUptime.TabIndex = 58;
            this.labelDeviceUptime.Text = "unknown";
            // 
            // labelGUIVersion
            // 
            this.labelGUIVersion.AutoSize = true;
            this.labelGUIVersion.Location = new System.Drawing.Point(589, 164);
            this.labelGUIVersion.Name = "labelGUIVersion";
            this.labelGUIVersion.Size = new System.Drawing.Size(51, 13);
            this.labelGUIVersion.TabIndex = 56;
            this.labelGUIVersion.Text = "unknown";
            // 
            // labelHardwareVersion
            // 
            this.labelHardwareVersion.AutoSize = true;
            this.labelHardwareVersion.Location = new System.Drawing.Point(589, 108);
            this.labelHardwareVersion.Name = "labelHardwareVersion";
            this.labelHardwareVersion.Size = new System.Drawing.Size(51, 13);
            this.labelHardwareVersion.TabIndex = 55;
            this.labelHardwareVersion.Text = "unknown";
            // 
            // labelSoftwareVersion
            // 
            this.labelSoftwareVersion.AutoSize = true;
            this.labelSoftwareVersion.Location = new System.Drawing.Point(589, 132);
            this.labelSoftwareVersion.Name = "labelSoftwareVersion";
            this.labelSoftwareVersion.Size = new System.Drawing.Size(51, 13);
            this.labelSoftwareVersion.TabIndex = 54;
            this.labelSoftwareVersion.Text = "unknown";
            // 
            // panelLogin
            // 
            this.panelLogin.BackColor = System.Drawing.Color.Transparent;
            this.panelLogin.Controls.Add(this.label1);
            this.panelLogin.Controls.Add(this.textBoxBboxUrl);
            this.panelLogin.Controls.Add(this.label3);
            this.panelLogin.Controls.Add(this.textBoxUsername);
            this.panelLogin.Controls.Add(this.textBoxPassword);
            this.panelLogin.Controls.Add(this.buttonConnect);
            this.panelLogin.Controls.Add(this.label2);
            this.panelLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLogin.Location = new System.Drawing.Point(0, 0);
            this.panelLogin.Name = "panelLogin";
            this.panelLogin.Size = new System.Drawing.Size(704, 331);
            this.panelLogin.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(298, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "B-Box3 IP address";
            // 
            // textBoxBboxUrl
            // 
            this.textBoxBboxUrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxBboxUrl.Location = new System.Drawing.Point(301, 84);
            this.textBoxBboxUrl.Name = "textBoxBboxUrl";
            this.textBoxBboxUrl.Size = new System.Drawing.Size(100, 21);
            this.textBoxBboxUrl.TabIndex = 1;
            this.textBoxBboxUrl.Text = "192.168.1.1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(319, 175);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Password";
            // 
            // textBoxUsername
            // 
            this.textBoxUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxUsername.Location = new System.Drawing.Point(301, 140);
            this.textBoxUsername.Name = "textBoxUsername";
            this.textBoxUsername.Size = new System.Drawing.Size(100, 21);
            this.textBoxUsername.TabIndex = 4;
            this.textBoxUsername.Text = "User";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPassword.Location = new System.Drawing.Point(301, 193);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(100, 21);
            this.textBoxPassword.TabIndex = 6;
            // 
            // buttonConnect
            // 
            this.buttonConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonConnect.Location = new System.Drawing.Point(301, 237);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(100, 26);
            this.buttonConnect.TabIndex = 0;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(318, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Username";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(185)))), ((int)(((byte)(255)))));
            this.label6.Location = new System.Drawing.Point(474, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 20);
            this.label6.TabIndex = 50;
            this.label6.Text = "Device";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(138)))));
            this.label11.Location = new System.Drawing.Point(475, 108);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(90, 13);
            this.label11.TabIndex = 51;
            this.label11.Text = "Hardware version";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(138)))));
            this.label13.Location = new System.Drawing.Point(475, 164);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(63, 13);
            this.label13.TabIndex = 52;
            this.label13.Text = "GUI version";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(138)))));
            this.label15.Location = new System.Drawing.Point(475, 132);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(86, 13);
            this.label15.TabIndex = 53;
            this.label15.Text = "Software version";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(138)))));
            this.label29.Location = new System.Drawing.Point(475, 52);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(75, 13);
            this.label29.TabIndex = 57;
            this.label29.Text = "Device uptime";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(138)))));
            this.label31.Location = new System.Drawing.Point(475, 76);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(61, 13);
            this.label31.TabIndex = 59;
            this.label31.Text = "Line uptime";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(138)))));
            this.label34.Location = new System.Drawing.Point(262, 51);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(72, 13);
            this.label34.TabIndex = 48;
            this.label34.Text = "DSL standard";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label35.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(185)))), ((int)(((byte)(255)))));
            this.label35.Location = new System.Drawing.Point(261, 16);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(53, 20);
            this.label35.TabIndex = 47;
            this.label35.Text = "Profile";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(138)))));
            this.label37.Location = new System.Drawing.Point(262, 217);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(49, 13);
            this.label37.TabIndex = 44;
            this.label37.Text = "Distance";
            this.label37.Visible = false;
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(138)))));
            this.label39.Location = new System.Drawing.Point(9, 217);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(128, 13);
            this.label39.TabIndex = 19;
            this.label39.Text = "Downstream noise margin";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(138)))));
            this.label41.Location = new System.Drawing.Point(9, 241);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(114, 13);
            this.label41.TabIndex = 20;
            this.label41.Text = "Upstream noise margin";
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(138)))));
            this.label44.Location = new System.Drawing.Point(9, 188);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(108, 13);
            this.label44.TabIndex = 16;
            this.label44.Text = "Upstream attenuation";
            // 
            // panelInfo
            // 
            this.panelInfo.BackColor = System.Drawing.Color.Transparent;
            this.panelInfo.Controls.Add(this.buttonClipboard);
            this.panelInfo.Controls.Add(this.label6);
            this.panelInfo.Controls.Add(this.buttonCancel);
            this.panelInfo.Controls.Add(this.label11);
            this.panelInfo.Controls.Add(this.label13);
            this.panelInfo.Controls.Add(this.labelSoftwareVersion);
            this.panelInfo.Controls.Add(this.labelVDSLProfile);
            this.panelInfo.Controls.Add(this.label15);
            this.panelInfo.Controls.Add(this.labelHardwareVersion);
            this.panelInfo.Controls.Add(this.labelVDSLProfileLabel);
            this.panelInfo.Controls.Add(this.labelGUIVersion);
            this.panelInfo.Controls.Add(this.labelProximusProfile);
            this.panelInfo.Controls.Add(this.labelProximusProfileLabel);
            this.panelInfo.Controls.Add(this.labelDeviceUptime);
            this.panelInfo.Controls.Add(this.labelVectoring);
            this.panelInfo.Controls.Add(this.label29);
            this.panelInfo.Controls.Add(this.labelRepair);
            this.panelInfo.Controls.Add(this.labelLinkUptime);
            this.panelInfo.Controls.Add(this.labelVectoringLabel);
            this.panelInfo.Controls.Add(this.label31);
            this.panelInfo.Controls.Add(this.labelDSLStandard);
            this.panelInfo.Controls.Add(this.labelRepairLabel);
            this.panelInfo.Controls.Add(this.labelDLMLabel);
            this.panelInfo.Controls.Add(this.labelDLM);
            this.panelInfo.Controls.Add(this.label34);
            this.panelInfo.Controls.Add(this.label16);
            this.panelInfo.Controls.Add(this.labelUpstreamCurrentBitRate);
            this.panelInfo.Controls.Add(this.label35);
            this.panelInfo.Controls.Add(this.label10);
            this.panelInfo.Controls.Add(this.labelDistance);
            this.panelInfo.Controls.Add(this.label37);
            this.panelInfo.Controls.Add(this.labelDownstreamCurrentBitRate);
            this.panelInfo.Controls.Add(this.label12);
            this.panelInfo.Controls.Add(this.labelUpstreamAttenuation);
            this.panelInfo.Controls.Add(this.label39);
            this.panelInfo.Controls.Add(this.labelUpstreamMaxBitRate);
            this.panelInfo.Controls.Add(this.label01);
            this.panelInfo.Controls.Add(this.labelDownstreamAttenuation);
            this.panelInfo.Controls.Add(this.label41);
            this.panelInfo.Controls.Add(this.labelDownstreamMaxBitRate);
            this.panelInfo.Controls.Add(this.labelDownstreamNoiseMargin);
            this.panelInfo.Controls.Add(this.label7);
            this.panelInfo.Controls.Add(this.label4);
            this.panelInfo.Controls.Add(this.labelUpstreamNoiseMargin);
            this.panelInfo.Controls.Add(this.label44);
            this.panelInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelInfo.Location = new System.Drawing.Point(0, 0);
            this.panelInfo.Name = "panelInfo";
            this.panelInfo.Size = new System.Drawing.Size(704, 331);
            this.panelInfo.TabIndex = 61;
            this.panelInfo.Visible = false;
            // 
            // labelDistance
            // 
            this.labelDistance.AutoSize = true;
            this.labelDistance.Location = new System.Drawing.Point(364, 217);
            this.labelDistance.Name = "labelDistance";
            this.labelDistance.Size = new System.Drawing.Size(51, 13);
            this.labelDistance.TabIndex = 45;
            this.labelDistance.Text = "unknown";
            this.labelDistance.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(704, 331);
            this.Controls.Add(this.panelInfo);
            this.Controls.Add(this.panelLogin);
            this.Controls.Add(this.panelDebug);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(720, 370);
            this.MinimumSize = new System.Drawing.Size(720, 370);
            this.Name = "Form1";
            this.Text = "B-Box 3 Sagem Tool";
            this.panelDebug.ResumeLayout(false);
            this.panelDebug.PerformLayout();
            this.panelLogin.ResumeLayout(false);
            this.panelLogin.PerformLayout();
            this.panelInfo.ResumeLayout(false);
            this.panelInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorkerBbox;
        private System.Windows.Forms.Panel panelDebug;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBoxDebugResult;
        private System.Windows.Forms.TextBox textBoxDebug;
        private System.Windows.Forms.Button buttonDebug;
        private System.Windows.Forms.Button buttonClipboard;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelVDSLProfile;
        private System.Windows.Forms.Label labelVDSLProfileLabel;
        private System.Windows.Forms.Label labelProximusProfile;
        private System.Windows.Forms.Label labelProximusProfileLabel;
        private System.Windows.Forms.Label labelVectoring;
        private System.Windows.Forms.Label labelRepair;
        private System.Windows.Forms.Label labelVectoringLabel;
        private System.Windows.Forms.Label labelRepairLabel;
        private System.Windows.Forms.Label labelDLMLabel;
        private System.Windows.Forms.Label labelDLM;
        private System.Windows.Forms.Label labelUpstreamCurrentBitRate;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label labelDownstreamCurrentBitRate;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label labelUpstreamMaxBitRate;
        private System.Windows.Forms.Label label01;
        private System.Windows.Forms.Label labelDownstreamMaxBitRate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelUpstreamNoiseMargin;
        private System.Windows.Forms.Label labelDownstreamNoiseMargin;
        private System.Windows.Forms.Label labelDownstreamAttenuation;
        private System.Windows.Forms.Label labelUpstreamAttenuation;
        private System.Windows.Forms.Panel panelLogin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxBboxUrl;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxUsername;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label labelDSLStandard;
        private System.Windows.Forms.Label labelGUIVersion;
        private System.Windows.Forms.Label labelHardwareVersion;
        private System.Windows.Forms.Label labelSoftwareVersion;
        private System.Windows.Forms.Label labelDeviceUptime;
        private System.Windows.Forms.Label labelLinkUptime;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.Panel panelInfo;
        private System.Windows.Forms.Label labelDistance;
    }
}

