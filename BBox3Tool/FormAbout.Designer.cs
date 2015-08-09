namespace BBox3Tool
{
    partial class FormAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAbout));
            this.labelAboutTitle = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.linkLabelUserbase = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.linkLabelADSLBC = new System.Windows.Forms.LinkLabel();
            this.panelDisclaimer = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panelDisclaimer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelAboutTitle
            // 
            this.labelAboutTitle.AutoSize = true;
            this.labelAboutTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAboutTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(185)))), ((int)(((byte)(255)))));
            this.labelAboutTitle.Location = new System.Drawing.Point(11, 9);
            this.labelAboutTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelAboutTitle.Name = "labelAboutTitle";
            this.labelAboutTitle.Size = new System.Drawing.Size(184, 24);
            this.labelAboutTitle.TabIndex = 9;
            this.labelAboutTitle.Text = "B-Box 3 Sagem Tool";
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVersion.Location = new System.Drawing.Point(12, 45);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(51, 15);
            this.labelVersion.TabIndex = 10;
            this.labelVersion.Text = "Version ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(425, 90);
            this.label1.TabIndex = 11;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // linkLabelUserbase
            // 
            this.linkLabelUserbase.AutoSize = true;
            this.linkLabelUserbase.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabelUserbase.Location = new System.Drawing.Point(166, 246);
            this.linkLabelUserbase.Name = "linkLabelUserbase";
            this.linkLabelUserbase.Size = new System.Drawing.Size(60, 15);
            this.linkLabelUserbase.TabIndex = 12;
            this.linkLabelUserbase.TabStop = true;
            this.linkLabelUserbase.Text = "Userbase";
            this.linkLabelUserbase.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelUserbase_LinkClicked);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 246);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(295, 15);
            this.label2.TabIndex = 13;
            this.label2.Text = "Please provide feedback on Userbase and ADSL-BC.";
            // 
            // linkLabelADSLBC
            // 
            this.linkLabelADSLBC.AutoSize = true;
            this.linkLabelADSLBC.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabelADSLBC.Location = new System.Drawing.Point(247, 246);
            this.linkLabelADSLBC.Name = "linkLabelADSLBC";
            this.linkLabelADSLBC.Size = new System.Drawing.Size(58, 15);
            this.linkLabelADSLBC.TabIndex = 14;
            this.linkLabelADSLBC.TabStop = true;
            this.linkLabelADSLBC.Text = "ADSL-BC";
            this.linkLabelADSLBC.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelADSLBC_LinkClicked);
            // 
            // panelDisclaimer
            // 
            this.panelDisclaimer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(252)))), ((int)(((byte)(229)))));
            this.panelDisclaimer.Controls.Add(this.pictureBox1);
            this.panelDisclaimer.Controls.Add(this.label3);
            this.panelDisclaimer.Location = new System.Drawing.Point(15, 273);
            this.panelDisclaimer.Name = "panelDisclaimer";
            this.panelDisclaimer.Size = new System.Drawing.Size(607, 37);
            this.panelDisclaimer.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(68)))), ((int)(((byte)(0)))));
            this.label3.Location = new System.Drawing.Point(44, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(355, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "This tool is still in development, some results may not be correct.";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(8, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(27, 25);
            this.pictureBox1.TabIndex = 16;
            this.pictureBox1.TabStop = false;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 176);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(610, 62);
            this.label4.TabIndex = 16;
            this.label4.Text = resources.GetString("label4.Text");
            // 
            // FormAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(634, 321);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.panelDisclaimer);
            this.Controls.Add(this.linkLabelADSLBC);
            this.Controls.Add(this.linkLabelUserbase);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.labelAboutTitle);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(650, 360);
            this.MinimumSize = new System.Drawing.Size(650, 360);
            this.Name = "FormAbout";
            this.ShowIcon = false;
            this.Text = "Info";
            this.panelDisclaimer.ResumeLayout(false);
            this.panelDisclaimer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelAboutTitle;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabelUserbase;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel linkLabelADSLBC;
        private System.Windows.Forms.Panel panelDisclaimer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label4;
    }
}