using System;
using System.Collections.Generic;
using System.Net;
using System.ComponentModel;
using System.Web;
using System.Windows.Forms;
using System.Text;

namespace BBox3Tool
{
    public partial class Form1 : Form
    {
        Bbox3Session _session;

        public Form1()
        {
            InitializeComponent();
            backgroundWorkerBbox.WorkerSupportsCancellation = true;
            backgroundWorkerBbox.WorkerReportsProgress = true;
            _session = new Bbox3Session();
        }

        //buttons
        //-------

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (!backgroundWorkerBbox.IsBusy)
            {
                buttonConnect.Enabled = false;
                buttonCancel.Enabled = true;
                backgroundWorkerBbox.RunWorkerAsync();
                tabControlMain.SelectTab("tabPageLineInfo");
            }
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
            builder.AppendLine("Bbox 3 Sagem Tool v0.4");
            builder.AppendLine("----------------------");
            builder.AppendLine("");
            builder.AppendLine("Downstream current bit rate:   " + (_session.DownstreamCurrentBitRate == -1 ? "unknown" : _session.DownstreamCurrentBitRate.ToString("###,###,##0 'kbps'")));
            builder.AppendLine("Upstream current bit rate:     " + (_session.UpstreamCurrentBitRate == -1 ? "unknown" : _session.UpstreamCurrentBitRate.ToString("###,###,##0 'kbps'")));
            builder.AppendLine("");
            builder.AppendLine("Downstream max bit rate:       " + (_session.DownstreamMaxBitRate == -1 ? "unknown" : _session.DownstreamMaxBitRate.ToString("###,###,##0 'kbps'")));
            builder.AppendLine("Upstream max bit rate:         " + (_session.UpstreamMaxBitRate == -1 ? "unknown" : _session.UpstreamMaxBitRate.ToString("###,###,##0 'kbps'")));
            builder.AppendLine("");
            builder.AppendLine("Downstream attenuation:        " + (_session.DownstreamAttenuation == -1 ? "unknown" : _session.DownstreamAttenuation.ToString("0.0 'dB'")));
            builder.AppendLine("Upstream attenuation:          " + (_session.UpstreamAttenuation == -1 ? "unknown" : _session.UpstreamAttenuation.ToString("0.0 'dB'")));
            builder.AppendLine("");
            builder.AppendLine("Downstream noise margin:       " + (_session.DownstreamNoiseMargin == -1 ? "unknown" : _session.DownstreamNoiseMargin.ToString("0.0 'dB'")));
            builder.AppendLine("Upstream noise margin:         " + (_session.UpstreamNoiseMargin == -1 ? "unknown" : _session.UpstreamNoiseMargin.ToString("0.0 'dB'")));
            builder.AppendLine("");
            builder.AppendLine("Proximus profile:              " + _session.CurrentProfile.Name);
            builder.AppendLine("VDSL2 profile:                 " + _session.CurrentProfile.ProfileVDSL2.ToString().Replace("p", ""));
            builder.AppendLine("DLM:                           " + _session.CurrentProfile.DlmProfile);
            builder.AppendLine("Repair:                        " + _session.CurrentProfile.RepairProfile);
            builder.AppendLine("Vectoring:                     " + _session.CurrentProfile.VectoringEnabled);
            builder.AppendLine("[/code]");

            Clipboard.SetText(builder.ToString());
        }

        //worker thread
        //-------------

        private void backgroundWorkerBbox_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            //get textbox values
            string bboxUrl = "http://" + textBoxBboxUrl.Text;
            string user = textBoxUsername.Text;
            string password = textBoxPassword.Text;

            _session = new Bbox3Session(bboxUrl, user, password, worker);

            if (_session.openSession())
            {
                try
                {
                   // _session.getTestValues();

                   // _session.getDownstreamMaxBitRate2();
                   // return;

                    //get sync values
                    setLabelText(labelDownstreamCurrentBitRate, "busy...");
                    setLabelText(labelDownstreamCurrentBitRate, _session.getDownstreamCurrentBitRate());

                    setLabelText(labelUpstreamCurrentBitRate, "busy...");
                    setLabelText(labelUpstreamCurrentBitRate, _session.getUpstreamCurrentBitRate());

                    //get profile info
                    _session.getProfileInfo();
                    setLabelText(labelDLM, _session.CurrentProfile.DlmProfile.ToString());
                    setLabelText(labelRepair, _session.CurrentProfile.RepairProfile.ToString());
                    setLabelText(labelVectoring, _session.CurrentProfile.VectoringEnabled.ToString());
                    setLabelText(labelProximusProfile, _session.CurrentProfile.Name.ToString());
                    setLabelText(labelVDSLProfile, _session.CurrentProfile.ProfileVDSL2.ToString().Replace("p",""));

                    setLabelText(labelDownstreamAttenuation, "busy...");
                    setLabelText(labelDownstreamAttenuation, _session.getDownstreamAttenuation());

                    setLabelText(labelUpstreamAttenuation, "busy...");
                    setLabelText(labelUpstreamAttenuation, _session.getUpstreamAttenuation());

                    setLabelText(labelDownstreamNoiseMargin, "busy...");
                    setLabelText(labelDownstreamNoiseMargin, _session.getDownstreamNoiseMargin());

                    setLabelText(labelUpstreamNoiseMargin, "busy...");
                    setLabelText(labelUpstreamNoiseMargin, _session.getUpstreamNoiseMargin());

                    setLabelText(labelDownstreamMaxBitRate, "busy...");
                    setLabelText(labelDownstreamMaxBitRate, _session.getDownstreamMaxBitRate());

                    setLabelText(labelUpstreamMaxBitRate, "busy...");
                    setLabelText(labelUpstreamMaxBitRate, _session.getUpstreamMaxBitRate());

                }
                catch(ThreadCancelledException)
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
            }
            else
            {
                MessageBox.Show("Login failure.","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private static void setLabelText(Label label, string text)
        {
            label.Invoke((MethodInvoker)delegate
            {
                label.Text = text;
            });
        }

    }
}
