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
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.panelDebug = new System.Windows.Forms.Panel();
            this.label14 = new System.Windows.Forms.Label();
            this.textBoxDebug = new System.Windows.Forms.TextBox();
            this.buttonDebug = new System.Windows.Forms.Button();
            this.textBoxDebugResult = new System.Windows.Forms.TextBox();
            this.buttonClipboard = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelVDSLProfile = new System.Windows.Forms.Label();
            this.vdslProfileLabel = new System.Windows.Forms.Label();
            this.labelProximusProfile = new System.Windows.Forms.Label();
            this.proximusProfileLabel = new System.Windows.Forms.Label();
            this.labelVectoring = new System.Windows.Forms.Label();
            this.labelRepair = new System.Windows.Forms.Label();
            this.vectoringLabel = new System.Windows.Forms.Label();
            this.repairLabel = new System.Windows.Forms.Label();
            this.dlmLabel = new System.Windows.Forms.Label();
            this.labelDLM = new System.Windows.Forms.Label();
            this.labelUpstreamCurrentBitRate = new System.Windows.Forms.Label();
            this.upstreamCurrentBitRateLabel = new System.Windows.Forms.Label();
            this.labelDownstreamCurrentBitRate = new System.Windows.Forms.Label();
            this.downstreamCurrentBitRateLabel = new System.Windows.Forms.Label();
            this.labelUpstreamMaxBitRate = new System.Windows.Forms.Label();
            this.upstreamMaxBitRateLabel = new System.Windows.Forms.Label();
            this.labelDownstreamMaxBitRate = new System.Windows.Forms.Label();
            this.downstreamMaxBitRateLabel = new System.Windows.Forms.Label();
            this.downstreamAttenuationLabel = new System.Windows.Forms.Label();
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
            this.label7 = new System.Windows.Forms.Label();
            this.fritzboxButton = new System.Windows.Forms.Button();
            this.bbox3button = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.bbox2button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxIpAddress = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxUsername = new System.Windows.Forms.TextBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.hwVersionLabel = new System.Windows.Forms.Label();
            this.guiVersionLabel = new System.Windows.Forms.Label();
            this.softwareVersionLabel = new System.Windows.Forms.Label();
            this.deviceUptimeLabel = new System.Windows.Forms.Label();
            this.lineUptimeLabel = new System.Windows.Forms.Label();
            this.dslStandardLabel = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.distanceLabel = new System.Windows.Forms.Label();
            this.downstreamNoiseMarginLabel = new System.Windows.Forms.Label();
            this.upstreamNoiseMarginLabel = new System.Windows.Forms.Label();
            this.upstreamAttenuationLabel = new System.Windows.Forms.Label();
            this.panelInfo = new System.Windows.Forms.Panel();
            this.labelDistance = new System.Windows.Forms.Label();
            this.panelDebug.SuspendLayout();
            this.panelLogin.SuspendLayout();
            this.panelInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerBbox_DoWork);
            this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerBbox_ProgressChanged);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerBbox_RunWorkerCompleted);
            // 
            // panelDebug
            // 
            this.panelDebug.Controls.Add(this.label14);
            this.panelDebug.Controls.Add(this.textBoxDebug);
            this.panelDebug.Controls.Add(this.buttonDebug);
            this.panelDebug.Controls.Add(this.textBoxDebugResult);
            this.panelDebug.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDebug.Location = new System.Drawing.Point(0, 0);
            this.panelDebug.Margin = new System.Windows.Forms.Padding(6);
            this.panelDebug.Name = "panelDebug";
            this.panelDebug.Size = new System.Drawing.Size(1388, 575);
            this.panelDebug.TabIndex = 43;
            this.panelDebug.Visible = false;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(12, 15);
            this.label14.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(74, 25);
            this.label14.TabIndex = 0;
            this.label14.Text = "Xpath:";
            // 
            // textBoxDebug
            // 
            this.textBoxDebug.Location = new System.Drawing.Point(94, 10);
            this.textBoxDebug.Margin = new System.Windows.Forms.Padding(6);
            this.textBoxDebug.Name = "textBoxDebug";
            this.textBoxDebug.Size = new System.Drawing.Size(1134, 31);
            this.textBoxDebug.TabIndex = 1;
            this.textBoxDebug.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxDebug_KeyDown);
            // 
            // buttonDebug
            // 
            this.buttonDebug.Location = new System.Drawing.Point(1250, 6);
            this.buttonDebug.Margin = new System.Windows.Forms.Padding(6);
            this.buttonDebug.Name = "buttonDebug";
            this.buttonDebug.Size = new System.Drawing.Size(150, 44);
            this.buttonDebug.TabIndex = 2;
            this.buttonDebug.Text = "Send";
            this.buttonDebug.UseVisualStyleBackColor = true;
            this.buttonDebug.Click += new System.EventHandler(this.buttonDebug_Click);
            // 
            // textBoxDebugResult
            // 
            this.textBoxDebugResult.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBoxDebugResult.Location = new System.Drawing.Point(0, 4);
            this.textBoxDebugResult.Margin = new System.Windows.Forms.Padding(6);
            this.textBoxDebugResult.Multiline = true;
            this.textBoxDebugResult.Name = "textBoxDebugResult";
            this.textBoxDebugResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxDebugResult.Size = new System.Drawing.Size(1388, 571);
            this.textBoxDebugResult.TabIndex = 3;
            // 
            // buttonClipboard
            // 
            this.buttonClipboard.Location = new System.Drawing.Point(24, 542);
            this.buttonClipboard.Margin = new System.Windows.Forms.Padding(6);
            this.buttonClipboard.Name = "buttonClipboard";
            this.buttonClipboard.Size = new System.Drawing.Size(166, 44);
            this.buttonClipboard.TabIndex = 42;
            this.buttonClipboard.Text = "To Clipboard";
            this.buttonClipboard.UseVisualStyleBackColor = true;
            this.buttonClipboard.Click += new System.EventHandler(this.buttonClipboard_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Enabled = false;
            this.buttonCancel.Location = new System.Drawing.Point(222, 542);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(6);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(166, 44);
            this.buttonCancel.TabIndex = 41;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // labelVDSLProfile
            // 
            this.labelVDSLProfile.AutoSize = true;
            this.labelVDSLProfile.Location = new System.Drawing.Point(728, 146);
            this.labelVDSLProfile.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelVDSLProfile.Name = "labelVDSLProfile";
            this.labelVDSLProfile.Size = new System.Drawing.Size(98, 25);
            this.labelVDSLProfile.TabIndex = 40;
            this.labelVDSLProfile.Text = "unknown";
            // 
            // vdslProfileLabel
            // 
            this.vdslProfileLabel.AutoSize = true;
            this.vdslProfileLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(138)))));
            this.vdslProfileLabel.Location = new System.Drawing.Point(524, 146);
            this.vdslProfileLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.vdslProfileLabel.Name = "vdslProfileLabel";
            this.vdslProfileLabel.Size = new System.Drawing.Size(144, 25);
            this.vdslProfileLabel.TabIndex = 39;
            this.vdslProfileLabel.Text = "VDSL2 profile";
            // 
            // labelProximusProfile
            // 
            this.labelProximusProfile.AutoSize = true;
            this.labelProximusProfile.Location = new System.Drawing.Point(728, 254);
            this.labelProximusProfile.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelProximusProfile.Name = "labelProximusProfile";
            this.labelProximusProfile.Size = new System.Drawing.Size(98, 25);
            this.labelProximusProfile.TabIndex = 38;
            this.labelProximusProfile.Text = "unknown";
            // 
            // proximusProfileLabel
            // 
            this.proximusProfileLabel.AutoSize = true;
            this.proximusProfileLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(138)))));
            this.proximusProfileLabel.Location = new System.Drawing.Point(524, 254);
            this.proximusProfileLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.proximusProfileLabel.Name = "proximusProfileLabel";
            this.proximusProfileLabel.Size = new System.Drawing.Size(166, 25);
            this.proximusProfileLabel.TabIndex = 37;
            this.proximusProfileLabel.Text = "Proximus profile";
            // 
            // labelVectoring
            // 
            this.labelVectoring.AutoSize = true;
            this.labelVectoring.Location = new System.Drawing.Point(728, 208);
            this.labelVectoring.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelVectoring.Name = "labelVectoring";
            this.labelVectoring.Size = new System.Drawing.Size(98, 25);
            this.labelVectoring.TabIndex = 36;
            this.labelVectoring.Text = "unknown";
            // 
            // labelRepair
            // 
            this.labelRepair.AutoSize = true;
            this.labelRepair.Location = new System.Drawing.Point(728, 362);
            this.labelRepair.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelRepair.Name = "labelRepair";
            this.labelRepair.Size = new System.Drawing.Size(98, 25);
            this.labelRepair.TabIndex = 35;
            this.labelRepair.Text = "unknown";
            // 
            // vectoringLabel
            // 
            this.vectoringLabel.AutoSize = true;
            this.vectoringLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(138)))));
            this.vectoringLabel.Location = new System.Drawing.Point(524, 208);
            this.vectoringLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.vectoringLabel.Name = "vectoringLabel";
            this.vectoringLabel.Size = new System.Drawing.Size(103, 25);
            this.vectoringLabel.TabIndex = 34;
            this.vectoringLabel.Text = "Vectoring";
            // 
            // repairLabel
            // 
            this.repairLabel.AutoSize = true;
            this.repairLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(138)))));
            this.repairLabel.Location = new System.Drawing.Point(524, 362);
            this.repairLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.repairLabel.Name = "repairLabel";
            this.repairLabel.Size = new System.Drawing.Size(75, 25);
            this.repairLabel.TabIndex = 33;
            this.repairLabel.Text = "Repair";
            // 
            // dlmLabel
            // 
            this.dlmLabel.AutoSize = true;
            this.dlmLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(138)))));
            this.dlmLabel.Location = new System.Drawing.Point(524, 315);
            this.dlmLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.dlmLabel.Name = "dlmLabel";
            this.dlmLabel.Size = new System.Drawing.Size(57, 25);
            this.dlmLabel.TabIndex = 32;
            this.dlmLabel.Text = "DLM";
            // 
            // labelDLM
            // 
            this.labelDLM.AutoSize = true;
            this.labelDLM.Location = new System.Drawing.Point(728, 312);
            this.labelDLM.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelDLM.Name = "labelDLM";
            this.labelDLM.Size = new System.Drawing.Size(98, 25);
            this.labelDLM.TabIndex = 31;
            this.labelDLM.Text = "unknown";
            // 
            // labelUpstreamCurrentBitRate
            // 
            this.labelUpstreamCurrentBitRate.AutoSize = true;
            this.labelUpstreamCurrentBitRate.Location = new System.Drawing.Point(320, 146);
            this.labelUpstreamCurrentBitRate.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelUpstreamCurrentBitRate.Name = "labelUpstreamCurrentBitRate";
            this.labelUpstreamCurrentBitRate.Size = new System.Drawing.Size(42, 25);
            this.labelUpstreamCurrentBitRate.TabIndex = 30;
            this.labelUpstreamCurrentBitRate.Text = "0.0";
            // 
            // upstreamCurrentBitRateLabel
            // 
            this.upstreamCurrentBitRateLabel.AutoSize = true;
            this.upstreamCurrentBitRateLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(138)))));
            this.upstreamCurrentBitRateLabel.Location = new System.Drawing.Point(18, 146);
            this.upstreamCurrentBitRateLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.upstreamCurrentBitRateLabel.Name = "upstreamCurrentBitRateLabel";
            this.upstreamCurrentBitRateLabel.Size = new System.Drawing.Size(249, 25);
            this.upstreamCurrentBitRateLabel.TabIndex = 29;
            this.upstreamCurrentBitRateLabel.Text = "Upstream current bit rate";
            // 
            // labelDownstreamCurrentBitRate
            // 
            this.labelDownstreamCurrentBitRate.AutoSize = true;
            this.labelDownstreamCurrentBitRate.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelDownstreamCurrentBitRate.Location = new System.Drawing.Point(320, 100);
            this.labelDownstreamCurrentBitRate.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelDownstreamCurrentBitRate.Name = "labelDownstreamCurrentBitRate";
            this.labelDownstreamCurrentBitRate.Size = new System.Drawing.Size(42, 25);
            this.labelDownstreamCurrentBitRate.TabIndex = 28;
            this.labelDownstreamCurrentBitRate.Text = "0.0";
            // 
            // downstreamCurrentBitRateLabel
            // 
            this.downstreamCurrentBitRateLabel.AutoSize = true;
            this.downstreamCurrentBitRateLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(138)))));
            this.downstreamCurrentBitRateLabel.Location = new System.Drawing.Point(18, 98);
            this.downstreamCurrentBitRateLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.downstreamCurrentBitRateLabel.Name = "downstreamCurrentBitRateLabel";
            this.downstreamCurrentBitRateLabel.Size = new System.Drawing.Size(276, 25);
            this.downstreamCurrentBitRateLabel.TabIndex = 27;
            this.downstreamCurrentBitRateLabel.Text = "Downstream current bit rate";
            // 
            // labelUpstreamMaxBitRate
            // 
            this.labelUpstreamMaxBitRate.AutoSize = true;
            this.labelUpstreamMaxBitRate.Location = new System.Drawing.Point(320, 254);
            this.labelUpstreamMaxBitRate.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelUpstreamMaxBitRate.Name = "labelUpstreamMaxBitRate";
            this.labelUpstreamMaxBitRate.Size = new System.Drawing.Size(42, 25);
            this.labelUpstreamMaxBitRate.TabIndex = 26;
            this.labelUpstreamMaxBitRate.Text = "0.0";
            // 
            // upstreamMaxBitRateLabel
            // 
            this.upstreamMaxBitRateLabel.AutoSize = true;
            this.upstreamMaxBitRateLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(138)))));
            this.upstreamMaxBitRateLabel.Location = new System.Drawing.Point(18, 254);
            this.upstreamMaxBitRateLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.upstreamMaxBitRateLabel.Name = "upstreamMaxBitRateLabel";
            this.upstreamMaxBitRateLabel.Size = new System.Drawing.Size(222, 25);
            this.upstreamMaxBitRateLabel.TabIndex = 25;
            this.upstreamMaxBitRateLabel.Text = "Upstream max bit rate";
            // 
            // labelDownstreamMaxBitRate
            // 
            this.labelDownstreamMaxBitRate.AutoSize = true;
            this.labelDownstreamMaxBitRate.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelDownstreamMaxBitRate.Location = new System.Drawing.Point(320, 208);
            this.labelDownstreamMaxBitRate.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelDownstreamMaxBitRate.Name = "labelDownstreamMaxBitRate";
            this.labelDownstreamMaxBitRate.Size = new System.Drawing.Size(42, 25);
            this.labelDownstreamMaxBitRate.TabIndex = 24;
            this.labelDownstreamMaxBitRate.Text = "0.0";
            // 
            // downstreamMaxBitRateLabel
            // 
            this.downstreamMaxBitRateLabel.AutoSize = true;
            this.downstreamMaxBitRateLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(138)))));
            this.downstreamMaxBitRateLabel.Location = new System.Drawing.Point(18, 208);
            this.downstreamMaxBitRateLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.downstreamMaxBitRateLabel.Name = "downstreamMaxBitRateLabel";
            this.downstreamMaxBitRateLabel.Size = new System.Drawing.Size(249, 25);
            this.downstreamMaxBitRateLabel.TabIndex = 23;
            this.downstreamMaxBitRateLabel.Text = "Downstream max bit rate";
            // 
            // downstreamAttenuationLabel
            // 
            this.downstreamAttenuationLabel.AutoSize = true;
            this.downstreamAttenuationLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(138)))));
            this.downstreamAttenuationLabel.Location = new System.Drawing.Point(18, 315);
            this.downstreamAttenuationLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.downstreamAttenuationLabel.Name = "downstreamAttenuationLabel";
            this.downstreamAttenuationLabel.Size = new System.Drawing.Size(244, 25);
            this.downstreamAttenuationLabel.TabIndex = 15;
            this.downstreamAttenuationLabel.Text = "Downstream attenuation";
            // 
            // labelUpstreamNoiseMargin
            // 
            this.labelUpstreamNoiseMargin.AutoSize = true;
            this.labelUpstreamNoiseMargin.Location = new System.Drawing.Point(320, 463);
            this.labelUpstreamNoiseMargin.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelUpstreamNoiseMargin.Name = "labelUpstreamNoiseMargin";
            this.labelUpstreamNoiseMargin.Size = new System.Drawing.Size(42, 25);
            this.labelUpstreamNoiseMargin.TabIndex = 22;
            this.labelUpstreamNoiseMargin.Text = "0.0";
            // 
            // labelDownstreamNoiseMargin
            // 
            this.labelDownstreamNoiseMargin.AutoSize = true;
            this.labelDownstreamNoiseMargin.Location = new System.Drawing.Point(320, 417);
            this.labelDownstreamNoiseMargin.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelDownstreamNoiseMargin.Name = "labelDownstreamNoiseMargin";
            this.labelDownstreamNoiseMargin.Size = new System.Drawing.Size(42, 25);
            this.labelDownstreamNoiseMargin.TabIndex = 21;
            this.labelDownstreamNoiseMargin.Text = "0.0";
            // 
            // labelDownstreamAttenuation
            // 
            this.labelDownstreamAttenuation.AutoSize = true;
            this.labelDownstreamAttenuation.Location = new System.Drawing.Point(320, 315);
            this.labelDownstreamAttenuation.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelDownstreamAttenuation.Name = "labelDownstreamAttenuation";
            this.labelDownstreamAttenuation.Size = new System.Drawing.Size(42, 25);
            this.labelDownstreamAttenuation.TabIndex = 17;
            this.labelDownstreamAttenuation.Text = "0.0";
            // 
            // labelUpstreamAttenuation
            // 
            this.labelUpstreamAttenuation.AutoSize = true;
            this.labelUpstreamAttenuation.Location = new System.Drawing.Point(320, 362);
            this.labelUpstreamAttenuation.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelUpstreamAttenuation.Name = "labelUpstreamAttenuation";
            this.labelUpstreamAttenuation.Size = new System.Drawing.Size(42, 25);
            this.labelUpstreamAttenuation.TabIndex = 18;
            this.labelUpstreamAttenuation.Text = "0.0";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(185)))), ((int)(((byte)(255)))));
            this.label16.Location = new System.Drawing.Point(18, 31);
            this.label16.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(77, 37);
            this.label16.TabIndex = 46;
            this.label16.Text = "Line";
            // 
            // labelDSLStandard
            // 
            this.labelDSLStandard.AutoSize = true;
            this.labelDSLStandard.Location = new System.Drawing.Point(728, 98);
            this.labelDSLStandard.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelDSLStandard.Name = "labelDSLStandard";
            this.labelDSLStandard.Size = new System.Drawing.Size(98, 25);
            this.labelDSLStandard.TabIndex = 49;
            this.labelDSLStandard.Text = "unknown";
            // 
            // labelLinkUptime
            // 
            this.labelLinkUptime.AutoSize = true;
            this.labelLinkUptime.Location = new System.Drawing.Point(1178, 146);
            this.labelLinkUptime.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelLinkUptime.Name = "labelLinkUptime";
            this.labelLinkUptime.Size = new System.Drawing.Size(98, 25);
            this.labelLinkUptime.TabIndex = 60;
            this.labelLinkUptime.Text = "unknown";
            // 
            // labelDeviceUptime
            // 
            this.labelDeviceUptime.AutoSize = true;
            this.labelDeviceUptime.Location = new System.Drawing.Point(1178, 98);
            this.labelDeviceUptime.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelDeviceUptime.Name = "labelDeviceUptime";
            this.labelDeviceUptime.Size = new System.Drawing.Size(98, 25);
            this.labelDeviceUptime.TabIndex = 58;
            this.labelDeviceUptime.Text = "unknown";
            // 
            // labelGUIVersion
            // 
            this.labelGUIVersion.AutoSize = true;
            this.labelGUIVersion.Location = new System.Drawing.Point(1178, 315);
            this.labelGUIVersion.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelGUIVersion.Name = "labelGUIVersion";
            this.labelGUIVersion.Size = new System.Drawing.Size(98, 25);
            this.labelGUIVersion.TabIndex = 56;
            this.labelGUIVersion.Text = "unknown";
            // 
            // labelHardwareVersion
            // 
            this.labelHardwareVersion.AutoSize = true;
            this.labelHardwareVersion.Location = new System.Drawing.Point(1178, 208);
            this.labelHardwareVersion.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelHardwareVersion.Name = "labelHardwareVersion";
            this.labelHardwareVersion.Size = new System.Drawing.Size(98, 25);
            this.labelHardwareVersion.TabIndex = 55;
            this.labelHardwareVersion.Text = "unknown";
            // 
            // labelSoftwareVersion
            // 
            this.labelSoftwareVersion.AutoSize = true;
            this.labelSoftwareVersion.Location = new System.Drawing.Point(1178, 254);
            this.labelSoftwareVersion.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelSoftwareVersion.Name = "labelSoftwareVersion";
            this.labelSoftwareVersion.Size = new System.Drawing.Size(98, 25);
            this.labelSoftwareVersion.TabIndex = 54;
            this.labelSoftwareVersion.Text = "unknown";
            // 
            // panelLogin
            // 
            this.panelLogin.BackColor = System.Drawing.Color.Transparent;
            this.panelLogin.Controls.Add(this.label7);
            this.panelLogin.Controls.Add(this.fritzboxButton);
            this.panelLogin.Controls.Add(this.bbox3button);
            this.panelLogin.Controls.Add(this.label5);
            this.panelLogin.Controls.Add(this.label4);
            this.panelLogin.Controls.Add(this.bbox2button);
            this.panelLogin.Controls.Add(this.label1);
            this.panelLogin.Controls.Add(this.textBoxIpAddress);
            this.panelLogin.Controls.Add(this.label3);
            this.panelLogin.Controls.Add(this.textBoxUsername);
            this.panelLogin.Controls.Add(this.textBoxPassword);
            this.panelLogin.Controls.Add(this.buttonConnect);
            this.panelLogin.Controls.Add(this.label2);
            this.panelLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLogin.Location = new System.Drawing.Point(0, 0);
            this.panelLogin.Margin = new System.Windows.Forms.Padding(6);
            this.panelLogin.Name = "panelLogin";
            this.panelLogin.Size = new System.Drawing.Size(1388, 575);
            this.panelLogin.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(1001, 47);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(181, 25);
            this.label7.TabIndex = 12;
            this.label7.Text = "Step 3: Connect";
            // 
            // fritzboxButton
            // 
            this.fritzboxButton.Location = new System.Drawing.Point(72, 407);
            this.fritzboxButton.Name = "fritzboxButton";
            this.fritzboxButton.Size = new System.Drawing.Size(260, 102);
            this.fritzboxButton.TabIndex = 11;
            this.fritzboxButton.Text = "Fritz!Box";
            this.fritzboxButton.UseVisualStyleBackColor = true;
            this.fritzboxButton.Click += new System.EventHandler(this.fritzboxButton_Click);
            // 
            // bbox3button
            // 
            this.bbox3button.Location = new System.Drawing.Point(72, 264);
            this.bbox3button.Name = "bbox3button";
            this.bbox3button.Size = new System.Drawing.Size(260, 102);
            this.bbox3button.TabIndex = 10;
            this.bbox3button.Text = "BBOX3";
            this.bbox3button.UseVisualStyleBackColor = true;
            this.bbox3button.Click += new System.EventHandler(this.bbox3button_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(524, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(307, 25);
            this.label5.TabIndex = 9;
            this.label5.Text = "Step 2: Specify login details";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(78, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(242, 25);
            this.label4.TabIndex = 8;
            this.label4.Text = "Step 1: Select modem";
            // 
            // bbox2button
            // 
            this.bbox2button.Location = new System.Drawing.Point(72, 126);
            this.bbox2button.Name = "bbox2button";
            this.bbox2button.Size = new System.Drawing.Size(260, 102);
            this.bbox2button.TabIndex = 7;
            this.bbox2button.Text = "BBOX2";
            this.bbox2button.UseVisualStyleBackColor = true;
            this.bbox2button.Click += new System.EventHandler(this.bbox2button_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(616, 135);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 29);
            this.label1.TabIndex = 2;
            this.label1.Text = "IP address";
            // 
            // textBoxIpAddress
            // 
            this.textBoxIpAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxIpAddress.Location = new System.Drawing.Point(587, 170);
            this.textBoxIpAddress.Margin = new System.Windows.Forms.Padding(6);
            this.textBoxIpAddress.Name = "textBoxIpAddress";
            this.textBoxIpAddress.Size = new System.Drawing.Size(196, 35);
            this.textBoxIpAddress.TabIndex = 1;
            this.textBoxIpAddress.Text = "192.168.1.1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(624, 372);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 29);
            this.label3.TabIndex = 5;
            this.label3.Text = "Password";
            // 
            // textBoxUsername
            // 
            this.textBoxUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxUsername.Location = new System.Drawing.Point(587, 285);
            this.textBoxUsername.Margin = new System.Windows.Forms.Padding(6);
            this.textBoxUsername.Name = "textBoxUsername";
            this.textBoxUsername.Size = new System.Drawing.Size(196, 35);
            this.textBoxUsername.TabIndex = 4;
            this.textBoxUsername.Text = "User";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPassword.Location = new System.Drawing.Point(587, 407);
            this.textBoxPassword.Margin = new System.Windows.Forms.Padding(6);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(196, 35);
            this.textBoxPassword.TabIndex = 6;
            // 
            // buttonConnect
            // 
            this.buttonConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonConnect.Location = new System.Drawing.Point(987, 229);
            this.buttonConnect.Margin = new System.Windows.Forms.Padding(6);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(241, 137);
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
            this.label2.Location = new System.Drawing.Point(620, 254);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 29);
            this.label2.TabIndex = 3;
            this.label2.Text = "Username";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(185)))), ((int)(((byte)(255)))));
            this.label6.Location = new System.Drawing.Point(948, 31);
            this.label6.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(112, 37);
            this.label6.TabIndex = 50;
            this.label6.Text = "Device";
            // 
            // hwVersionLabel
            // 
            this.hwVersionLabel.AutoSize = true;
            this.hwVersionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(138)))));
            this.hwVersionLabel.Location = new System.Drawing.Point(950, 208);
            this.hwVersionLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.hwVersionLabel.Name = "hwVersionLabel";
            this.hwVersionLabel.Size = new System.Drawing.Size(180, 25);
            this.hwVersionLabel.TabIndex = 51;
            this.hwVersionLabel.Text = "Hardware version";
            // 
            // guiVersionLabel
            // 
            this.guiVersionLabel.AutoSize = true;
            this.guiVersionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(138)))));
            this.guiVersionLabel.Location = new System.Drawing.Point(950, 315);
            this.guiVersionLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.guiVersionLabel.Name = "guiVersionLabel";
            this.guiVersionLabel.Size = new System.Drawing.Size(124, 25);
            this.guiVersionLabel.TabIndex = 52;
            this.guiVersionLabel.Text = "GUI version";
            // 
            // softwareVersionLabel
            // 
            this.softwareVersionLabel.AutoSize = true;
            this.softwareVersionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(138)))));
            this.softwareVersionLabel.Location = new System.Drawing.Point(950, 254);
            this.softwareVersionLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.softwareVersionLabel.Name = "softwareVersionLabel";
            this.softwareVersionLabel.Size = new System.Drawing.Size(172, 25);
            this.softwareVersionLabel.TabIndex = 53;
            this.softwareVersionLabel.Text = "Software version";
            // 
            // deviceUptimeLabel
            // 
            this.deviceUptimeLabel.AutoSize = true;
            this.deviceUptimeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(138)))));
            this.deviceUptimeLabel.Location = new System.Drawing.Point(950, 100);
            this.deviceUptimeLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.deviceUptimeLabel.Name = "deviceUptimeLabel";
            this.deviceUptimeLabel.Size = new System.Drawing.Size(148, 25);
            this.deviceUptimeLabel.TabIndex = 57;
            this.deviceUptimeLabel.Text = "Device uptime";
            // 
            // lineUptimeLabel
            // 
            this.lineUptimeLabel.AutoSize = true;
            this.lineUptimeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(138)))));
            this.lineUptimeLabel.Location = new System.Drawing.Point(950, 146);
            this.lineUptimeLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lineUptimeLabel.Name = "lineUptimeLabel";
            this.lineUptimeLabel.Size = new System.Drawing.Size(123, 25);
            this.lineUptimeLabel.TabIndex = 59;
            this.lineUptimeLabel.Text = "Line uptime";
            // 
            // dslStandardLabel
            // 
            this.dslStandardLabel.AutoSize = true;
            this.dslStandardLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(138)))));
            this.dslStandardLabel.Location = new System.Drawing.Point(524, 98);
            this.dslStandardLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.dslStandardLabel.Name = "dslStandardLabel";
            this.dslStandardLabel.Size = new System.Drawing.Size(143, 25);
            this.dslStandardLabel.TabIndex = 48;
            this.dslStandardLabel.Text = "DSL standard";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label35.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(185)))), ((int)(((byte)(255)))));
            this.label35.Location = new System.Drawing.Point(522, 31);
            this.label35.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(107, 37);
            this.label35.TabIndex = 47;
            this.label35.Text = "Profile";
            // 
            // distanceLabel
            // 
            this.distanceLabel.AutoSize = true;
            this.distanceLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(138)))));
            this.distanceLabel.Location = new System.Drawing.Point(524, 417);
            this.distanceLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.distanceLabel.Name = "distanceLabel";
            this.distanceLabel.Size = new System.Drawing.Size(96, 25);
            this.distanceLabel.TabIndex = 44;
            this.distanceLabel.Text = "Distance";
            this.distanceLabel.Visible = false;
            // 
            // downstreamNoiseMarginLabel
            // 
            this.downstreamNoiseMarginLabel.AutoSize = true;
            this.downstreamNoiseMarginLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(138)))));
            this.downstreamNoiseMarginLabel.Location = new System.Drawing.Point(18, 417);
            this.downstreamNoiseMarginLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.downstreamNoiseMarginLabel.Name = "downstreamNoiseMarginLabel";
            this.downstreamNoiseMarginLabel.Size = new System.Drawing.Size(260, 25);
            this.downstreamNoiseMarginLabel.TabIndex = 19;
            this.downstreamNoiseMarginLabel.Text = "Downstream noise margin";
            // 
            // upstreamNoiseMarginLabel
            // 
            this.upstreamNoiseMarginLabel.AutoSize = true;
            this.upstreamNoiseMarginLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(138)))));
            this.upstreamNoiseMarginLabel.Location = new System.Drawing.Point(18, 463);
            this.upstreamNoiseMarginLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.upstreamNoiseMarginLabel.Name = "upstreamNoiseMarginLabel";
            this.upstreamNoiseMarginLabel.Size = new System.Drawing.Size(233, 25);
            this.upstreamNoiseMarginLabel.TabIndex = 20;
            this.upstreamNoiseMarginLabel.Text = "Upstream noise margin";
            // 
            // upstreamAttenuationLabel
            // 
            this.upstreamAttenuationLabel.AutoSize = true;
            this.upstreamAttenuationLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(138)))));
            this.upstreamAttenuationLabel.Location = new System.Drawing.Point(18, 362);
            this.upstreamAttenuationLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.upstreamAttenuationLabel.Name = "upstreamAttenuationLabel";
            this.upstreamAttenuationLabel.Size = new System.Drawing.Size(217, 25);
            this.upstreamAttenuationLabel.TabIndex = 16;
            this.upstreamAttenuationLabel.Text = "Upstream attenuation";
            // 
            // panelInfo
            // 
            this.panelInfo.BackColor = System.Drawing.Color.Transparent;
            this.panelInfo.Controls.Add(this.buttonClipboard);
            this.panelInfo.Controls.Add(this.label6);
            this.panelInfo.Controls.Add(this.buttonCancel);
            this.panelInfo.Controls.Add(this.hwVersionLabel);
            this.panelInfo.Controls.Add(this.guiVersionLabel);
            this.panelInfo.Controls.Add(this.labelSoftwareVersion);
            this.panelInfo.Controls.Add(this.labelVDSLProfile);
            this.panelInfo.Controls.Add(this.softwareVersionLabel);
            this.panelInfo.Controls.Add(this.labelHardwareVersion);
            this.panelInfo.Controls.Add(this.vdslProfileLabel);
            this.panelInfo.Controls.Add(this.labelGUIVersion);
            this.panelInfo.Controls.Add(this.labelProximusProfile);
            this.panelInfo.Controls.Add(this.proximusProfileLabel);
            this.panelInfo.Controls.Add(this.labelDeviceUptime);
            this.panelInfo.Controls.Add(this.labelVectoring);
            this.panelInfo.Controls.Add(this.deviceUptimeLabel);
            this.panelInfo.Controls.Add(this.labelRepair);
            this.panelInfo.Controls.Add(this.labelLinkUptime);
            this.panelInfo.Controls.Add(this.vectoringLabel);
            this.panelInfo.Controls.Add(this.lineUptimeLabel);
            this.panelInfo.Controls.Add(this.labelDSLStandard);
            this.panelInfo.Controls.Add(this.repairLabel);
            this.panelInfo.Controls.Add(this.dlmLabel);
            this.panelInfo.Controls.Add(this.labelDLM);
            this.panelInfo.Controls.Add(this.dslStandardLabel);
            this.panelInfo.Controls.Add(this.label16);
            this.panelInfo.Controls.Add(this.labelUpstreamCurrentBitRate);
            this.panelInfo.Controls.Add(this.label35);
            this.panelInfo.Controls.Add(this.upstreamCurrentBitRateLabel);
            this.panelInfo.Controls.Add(this.labelDistance);
            this.panelInfo.Controls.Add(this.distanceLabel);
            this.panelInfo.Controls.Add(this.labelDownstreamCurrentBitRate);
            this.panelInfo.Controls.Add(this.downstreamCurrentBitRateLabel);
            this.panelInfo.Controls.Add(this.labelUpstreamAttenuation);
            this.panelInfo.Controls.Add(this.downstreamNoiseMarginLabel);
            this.panelInfo.Controls.Add(this.labelUpstreamMaxBitRate);
            this.panelInfo.Controls.Add(this.upstreamMaxBitRateLabel);
            this.panelInfo.Controls.Add(this.labelDownstreamAttenuation);
            this.panelInfo.Controls.Add(this.upstreamNoiseMarginLabel);
            this.panelInfo.Controls.Add(this.labelDownstreamMaxBitRate);
            this.panelInfo.Controls.Add(this.labelDownstreamNoiseMargin);
            this.panelInfo.Controls.Add(this.downstreamMaxBitRateLabel);
            this.panelInfo.Controls.Add(this.downstreamAttenuationLabel);
            this.panelInfo.Controls.Add(this.labelUpstreamNoiseMargin);
            this.panelInfo.Controls.Add(this.upstreamAttenuationLabel);
            this.panelInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelInfo.Location = new System.Drawing.Point(0, 0);
            this.panelInfo.Margin = new System.Windows.Forms.Padding(6);
            this.panelInfo.Name = "panelInfo";
            this.panelInfo.Size = new System.Drawing.Size(1388, 575);
            this.panelInfo.TabIndex = 61;
            this.panelInfo.Visible = false;
            // 
            // labelDistance
            // 
            this.labelDistance.AutoSize = true;
            this.labelDistance.Location = new System.Drawing.Point(728, 417);
            this.labelDistance.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelDistance.Name = "labelDistance";
            this.labelDistance.Size = new System.Drawing.Size(98, 25);
            this.labelDistance.TabIndex = 45;
            this.labelDistance.Text = "unknown";
            this.labelDistance.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1388, 575);
            this.Controls.Add(this.panelLogin);
            this.Controls.Add(this.panelInfo);
            this.Controls.Add(this.panelDebug);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1414, 646);
            this.MinimumSize = new System.Drawing.Size(1414, 646);
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

        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.Panel panelDebug;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBoxDebugResult;
        private System.Windows.Forms.TextBox textBoxDebug;
        private System.Windows.Forms.Button buttonDebug;
        private System.Windows.Forms.Button buttonClipboard;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelVDSLProfile;
        private System.Windows.Forms.Label vdslProfileLabel;
        private System.Windows.Forms.Label labelProximusProfile;
        private System.Windows.Forms.Label proximusProfileLabel;
        private System.Windows.Forms.Label labelVectoring;
        private System.Windows.Forms.Label labelRepair;
        private System.Windows.Forms.Label vectoringLabel;
        private System.Windows.Forms.Label repairLabel;
        private System.Windows.Forms.Label dlmLabel;
        private System.Windows.Forms.Label labelDLM;
        private System.Windows.Forms.Label labelUpstreamCurrentBitRate;
        private System.Windows.Forms.Label upstreamCurrentBitRateLabel;
        private System.Windows.Forms.Label labelDownstreamCurrentBitRate;
        private System.Windows.Forms.Label downstreamCurrentBitRateLabel;
        private System.Windows.Forms.Label labelUpstreamMaxBitRate;
        private System.Windows.Forms.Label upstreamMaxBitRateLabel;
        private System.Windows.Forms.Label labelDownstreamMaxBitRate;
        private System.Windows.Forms.Label downstreamMaxBitRateLabel;
        private System.Windows.Forms.Label downstreamAttenuationLabel;
        private System.Windows.Forms.Label labelUpstreamNoiseMargin;
        private System.Windows.Forms.Label labelDownstreamNoiseMargin;
        private System.Windows.Forms.Label labelDownstreamAttenuation;
        private System.Windows.Forms.Label labelUpstreamAttenuation;
        private System.Windows.Forms.Panel panelLogin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxIpAddress;
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
        private System.Windows.Forms.Label hwVersionLabel;
        private System.Windows.Forms.Label guiVersionLabel;
        private System.Windows.Forms.Label softwareVersionLabel;
        private System.Windows.Forms.Label deviceUptimeLabel;
        private System.Windows.Forms.Label lineUptimeLabel;
        private System.Windows.Forms.Label dslStandardLabel;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label distanceLabel;
        private System.Windows.Forms.Label downstreamNoiseMarginLabel;
        private System.Windows.Forms.Label upstreamNoiseMarginLabel;
        private System.Windows.Forms.Label upstreamAttenuationLabel;
        private System.Windows.Forms.Panel panelInfo;
        private System.Windows.Forms.Label labelDistance;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button bbox2button;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button fritzboxButton;
        private System.Windows.Forms.Button bbox3button;
        private System.Windows.Forms.Label label5;
    }
}

