using System;
using System.Collections.Generic;
using System.Net;
using System.ComponentModel;
using System.Web;
using System.Windows.Forms;
using System.Text;
using System.Drawing;

namespace BBox3Tool
{
    public partial class Form1 : Form
    {
        Bbox3Session _session;

        public Form1()
        {
            InitializeComponent();
            this.Text += " " + Application.ProductVersion;
            backgroundWorkerBbox.WorkerSupportsCancellation = true;
            backgroundWorkerBbox.WorkerReportsProgress = true;
            _session = new Bbox3Session();
        }

        //buttons
        //-------

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            //check mode
            bool debug = (textBoxUsername.Text.ToLower() == "debug");
            if (debug)
                initDebugMode();
            else
                initNormalMode();
        }

        private void initDebugMode()
        {
            //get textbox values
            string bboxUrl = "http://" + textBoxBboxUrl.Text;
            string user = "User";
            string password = textBoxPassword.Text;

            //init session
            _session = new Bbox3Session(bboxUrl, user, password, backgroundWorkerBbox, true);
            if (_session.openSession())
            {
                buttonConnect.Enabled = false;
                panelDebug.Visible = true;
                panelInfo.Visible = false;
                panelLogin.Visible = false;
            }
            else
                MessageBox.Show("Login incorrect.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void initNormalMode()
        {
            //get textbox values
            string bboxUrl = "http://" + textBoxBboxUrl.Text;
            string user = textBoxUsername.Text;
            string password = textBoxPassword.Text;

            //inint session
            _session = new Bbox3Session(bboxUrl, user, password, backgroundWorkerBbox);
            if (_session.openSession())
            {
                buttonConnect.Enabled = false;
                buttonCancel.Enabled = true;

                panelDebug.Visible = false;
                panelInfo.Visible = true;
                panelLogin.Visible = false;

                backgroundWorkerBbox.RunWorkerAsync();
            }
            else
                MessageBox.Show("Login incorrect.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (backgroundWorkerBbox.IsBusy)
            {
                backgroundWorkerBbox.CancelAsync();
                buttonCancel.Enabled = false;
            }
        }

        private void buttonClipboard_Click(object sender, EventArgs e)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("[code]");
            builder.AppendLine("B-Box 3 Sagem Tool v" + Application.ProductVersion);
            builder.AppendLine("-----------------------");
            //builder.AppendLine("Date:                          " + DateTime.Now.ToString("d/M/yyyy HH:mm:ss"));
            builder.AppendLine("");
            builder.AppendLine("Downstream current bit rate:   " + (_session.DownstreamCurrentBitRate < 0 ? "unknown" : _session.DownstreamCurrentBitRate.ToString("###,###,##0 'kbps'")));
            builder.AppendLine("Upstream current bit rate:     " + (_session.UpstreamCurrentBitRate < 0 ? "unknown" : _session.UpstreamCurrentBitRate.ToString("###,###,##0 'kbps'")));
            builder.AppendLine("");
            builder.AppendLine("Downstream max bit rate:       " + (_session.DownstreamMaxBitRate < 0 ? "unknown" : _session.DownstreamMaxBitRate.ToString("###,###,##0 'kbps'")));
            builder.AppendLine("Upstream max bit rate:         " + (_session.UpstreamMaxBitRate < 0 ? "unknown" : _session.UpstreamMaxBitRate.ToString("###,###,##0 'kbps'")));
            builder.AppendLine("");
            builder.AppendLine("Downstream attenuation:        " + (_session.DownstreamAttenuation < 0 ? "unknown" : _session.DownstreamAttenuation.ToString("0.0 'dB'")));
            builder.AppendLine("Upstream attenuation:          " + (_session.UpstreamAttenuation < 0 ? "unknown" : _session.UpstreamAttenuation.ToString("0.0 'dB'")));
            builder.AppendLine("");
            builder.AppendLine("Downstream noise margin:       " + (_session.DownstreamNoiseMargin < 0 ? "unknown" : _session.DownstreamNoiseMargin.ToString("0.0 'dB'")));
            builder.AppendLine("Upstream noise margin:         " + (_session.UpstreamNoiseMargin < 0 ? "unknown" : _session.UpstreamNoiseMargin.ToString("0.0 'dB'")));
            builder.AppendLine("");
            builder.AppendLine("DSL standard:                  " + _session.DslStandard.ToString().Replace("plus", "+"));
            if (_session.DslStandard == DSLStandard.VDSL2)
            {
                builder.AppendLine("VDSL2 profile:                 " + _session.CurrentProfile.ProfileVDSL2.ToString().Replace("p", ""));
                //builder.AppendLine("Vectoring:                     " + (_session.VectoringEnabled ? "Yes" : "No"));
                builder.AppendLine("Vectoring:                     " + (_session.CurrentProfile.VectoringEnabled ? "Yes" : "No")); 
                builder.AppendLine("Proximus profile:              " + _session.CurrentProfile.Name);
                builder.AppendLine("DLM:                           " + (_session.CurrentProfile.DlmProfile ? "Yes" : "No"));
                builder.AppendLine("Repair:                        " + (_session.CurrentProfile.RepairProfile ? "Yes" : "No"));

            }

            //builder.AppendLine("Distance:                      " + (_session.Distance == -1 ? "unknown" : _session.Distance.ToString("0 'm'")));
            builder.AppendLine("[/code]");

            Clipboard.SetText(builder.ToString());
        }

        //debug
        private void buttonDebug_Click(object sender, EventArgs e)
        {
            textBoxDebugResult.Text = _session.getDebugValue(textBoxDebug.Text);
        }
        
        //debug textbox on enter --> debug button click
        private void textBoxDebug_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                buttonDebug_Click((object)sender, (EventArgs)e);
        }

        //worker thread
        //-------------

        private void backgroundWorkerBbox_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            try
            {
                //get device stats
                _session.getDeviceCommonInfo();
                setLabelText(labelHardwareVersion, _session.HardwareVersion);
                setLabelText(labelSoftwareVersion, _session.InternalFirmwareVersion);
                setLabelText(labelGUIVersion, _session.GUIFirmwareVersion);
                setLabelText(labelDeviceUptime, _session.DeviceUptime.ToString("%d") + (_session.DeviceUptime.Days == 1 ? " day " : " days ") + _session.DeviceUptime.ToString("hh\\:mm\\:ss"));
                setLabelText(labelLinkUptime, _session.LinkUptime.ToString("%d") + (_session.LinkUptime.Days == 1 ? " day " : " days ") + _session.LinkUptime.ToString("hh\\:mm\\:ss"));

                //get dsl stats
                _session.getDSLStandard();
                setLabelText(labelDSLStandard, _session.DslStandard.ToString().Replace("plus", "+"));

                //get sync values
                setLabelText(labelDownstreamCurrentBitRate, "busy...");
                _session.getDownstreamCurrentBitRate();
                setLabelText(labelDownstreamCurrentBitRate, _session.DownstreamCurrentBitRate < 0 ? "unknown" : _session.DownstreamCurrentBitRate.ToString("###,###,##0 'kbps'"));

                setLabelText(labelUpstreamCurrentBitRate, "busy...");
                _session.getUpstreamCurrentBitRate();
                setLabelText(labelUpstreamCurrentBitRate, _session.UpstreamCurrentBitRate < 0 ? "unknown" : _session.UpstreamCurrentBitRate.ToString("###,###,##0 'kbps'"));

                //get profile info
                if (_session.DslStandard == DSLStandard.VDSL2)
                {
                    _session.getProfileInfo();

                    //TODO check why this is incorrect
                    //_session.getVectoringEnabled();
                    //setLabelText(labelVectoring, _session.VectoringEnabled ? "Yes" : "No");

                    //get vectoring status fallback: get from profile list
                    setLabelText(labelVectoring, _session.CurrentProfile.VectoringEnabled ? "Yes" : "No");

                    setLabelText(labelDLM, _session.CurrentProfile.DlmProfile ? "Yes" : "No");
                    setLabelText(labelRepair, _session.CurrentProfile.RepairProfile ? "Yes" : "No");
                    setLabelText(labelProximusProfile, _session.CurrentProfile.Name.ToString());
                    setLabelText(labelVDSLProfile, _session.CurrentProfile.ProfileVDSL2.ToString().Replace("p", ""));
                }
                else
                {
                    setLabelText(labelVDSLProfile, "n/a");
                    labelVDSLProfile.ForeColor = Color.Gray;
                    labelVDSLProfileLabel.ForeColor = Color.Gray;
                    setLabelText(labelVectoring, "n/a");
                    labelVectoring.ForeColor = Color.Gray;
                    labelVectoringLabel.ForeColor = Color.Gray;
                    setLabelText(labelRepair, "n/a");
                    labelRepair.ForeColor = Color.Gray;
                    labelRepairLabel.ForeColor = Color.Gray;
                    setLabelText(labelDLM, "n/a");
                    labelDLM.ForeColor = Color.Gray;
                    labelDLMLabel.ForeColor = Color.Gray;
                    setLabelText(labelProximusProfile, "n/a");
                    labelProximusProfile.ForeColor = Color.Gray;
                    labelProximusProfileLabel.ForeColor = Color.Gray;
                }

                //get line stats
                setLabelText(labelDownstreamAttenuation, "busy...");
                _session.getDownstreamAttenuation();
                setLabelText(labelDownstreamAttenuation, _session.DownstreamAttenuation < 0 ? "unknown" : _session.DownstreamAttenuation.ToString("0.0 'dB'"));
                //distance
                //setLabelText(labelDistance, _session.getEstimatedDistance());
                //return;

                setLabelText(labelUpstreamAttenuation, "busy...");
                _session.getUpstreamAttenuation();
                setLabelText(labelUpstreamAttenuation, _session.UpstreamAttenuation < 0 ? "unknown" : _session.UpstreamAttenuation.ToString("0.0 'dB'"));

                setLabelText(labelDownstreamNoiseMargin, "busy...");
                _session.getDownstreamNoiseMargin();
                setLabelText(labelDownstreamNoiseMargin, _session.DownstreamNoiseMargin < 0 ? "unknown" : _session.DownstreamNoiseMargin.ToString("0.0 'dB'"));

                setLabelText(labelUpstreamNoiseMargin, "busy...");
                _session.getUpstreamNoiseMargin();
                setLabelText(labelUpstreamNoiseMargin, _session.UpstreamNoiseMargin < 0 ? "unknown" : _session.UpstreamNoiseMargin.ToString("0.0 'dB'"));

                setLabelText(labelDownstreamMaxBitRate, "busy...");
                _session.getDownstreamMaxBitRate();
                setLabelText(labelDownstreamMaxBitRate, _session.DownstreamMaxBitRate < 0 ? "unknown" : _session.DownstreamMaxBitRate.ToString("###,###,##0 'kbps'"));

                setLabelText(labelUpstreamMaxBitRate, "busy...");
                _session.getUpstreamMaxBitRate();
                setLabelText(labelUpstreamMaxBitRate, _session.UpstreamMaxBitRate < 0 ? "unknown" : _session.UpstreamMaxBitRate.ToString("###,###,##0 'kbps'"));

            }
            catch (ThreadCancelledException)
            {
                //MessageBox.Show("Request cancelled.", "Info", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error occurred. Debug info: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _session.closeSession();
            }

            worker.ReportProgress(100);
        }

        private void backgroundWorkerBbox_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void backgroundWorkerBbox_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            buttonConnect.Enabled = true;
            buttonCancel.Enabled = false;
        }

        //util funtions
        //-------------

        private static void setLabelText(Label label, string text)
        {
            label.Invoke((MethodInvoker)delegate
            {
                label.Text = text;
            });
        }




    }
}
