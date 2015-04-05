using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;
using System.Drawing;

namespace BBox3Tool
{
    public partial class Form1 : Form
    {
        private IModemSession _session;

        public Form1()
        {
            InitializeComponent();
            this.Text += " " + Application.ProductVersion;
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.WorkerReportsProgress = true;

            // Init for bbox3 by default
            _session = new Bbox3Session(backgroundWorker);
        }

        //buttons
        //-------

        private void bbox2button_Click(object sender, EventArgs e)
        {
            _session = new Bbox2Session();

            // Set default username
            textBoxUsername.Text = "admin";
            textBoxUsername.Enabled = true;
        }

        private void bbox3button_Click(object sender, EventArgs e)
        {
            _session = new Bbox3Session(backgroundWorker);

            // Set default username
            textBoxUsername.Text = "User";
            textBoxUsername.Enabled = true;
        }

        private void fritzboxButton_Click(object sender, EventArgs e)
        {
            _session = new FritzBoxSession();

            // Disable username textBox
            textBoxUsername.Text = "N/A";
            textBoxUsername.Enabled = false;
        }

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
            string host = textBoxIpAddress.Text;
            string username = "User"; //overwrite textbox value
            string password = textBoxPassword.Text;

            //init session
            _session = new Bbox3Session(backgroundWorker, true);
            if (_session.OpenSession(host, username, password))
            {
                buttonConnect.Enabled = false;
                panelDebug.Visible = true;
                panelInfo.Visible = false;
                panelLogin.Visible = false;
            }
            else
            {
                MessageBox.Show("Login incorrect.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void initNormalMode()
        {
            //get textbox values
            string host = textBoxIpAddress.Text;
            string username = textBoxUsername.Text;
            string password = textBoxPassword.Text;

            //init session
            if (_session.OpenSession(host, username, password))
            {
                buttonClipboard.Enabled = false;
                buttonConnect.Enabled = false;
                buttonCancel.Enabled = true;

                panelDebug.Visible = false;
                panelInfo.Visible = true;
                panelLogin.Visible = false;

                backgroundWorker.RunWorkerAsync();

                if (_session is Bbox3Session)
                    distanceLabel.Text += "\r\n(experimental)";
            }
            else
            {
                MessageBox.Show("Login incorrect.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync();
                buttonCancel.Enabled = false;
            }
        }

        private void buttonClipboard_Click(object sender, EventArgs e)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("[code]");
            builder.AppendLine("B-Box 3 Sagem Tool v" + Application.ProductVersion);
            builder.AppendLine("--------------------" + new String('-', Application.ProductVersion.Length));
            builder.AppendLine("Device:                        " + _session.DeviceName);
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
            builder.AppendLine("DSL standard:                  " + _session.GetDslStandard().ToString().Replace("plus", "+"));
            if (_session.GetDslStandard() == DSLStandard.VDSL2)
            {
                ProximusLineProfile currentProfile = _session.GetProfileInfo();
                if (currentProfile.Name == "unknown")
                {
                    builder.AppendLine("VDSL2 profile:                 unknown");
                    builder.AppendLine("Vectoring:                     unknown");
                    builder.AppendLine("Proximus profile:              unknown");
                    builder.AppendLine("DLM:                           unknown");
                    builder.AppendLine("Repair:                        unknown");
                }
                else
                {

                    builder.AppendLine("VDSL2 profile:                 " + currentProfile.ProfileVDSL2.ToString().Replace("p", ""));
                    //builder.AppendLine("Vectoring:                     " + (_session.VectoringEnabled ? "Yes" : "No"));
                    builder.AppendLine("Vectoring:                     " + (currentProfile.VectoringEnabled ? "Yes" : "No"));
                    builder.AppendLine("Proximus profile:              " + currentProfile.Name);
                    builder.AppendLine("DLM:                           " + (currentProfile.DlmProfile ? "Yes" : "No"));
                    builder.AppendLine("Repair:                        " + (currentProfile.RepairProfile ? "Yes" : "No"));
                }
            }

            if (_session is Bbox3Session)
                builder.AppendLine("Distance (experimental):       " + (_session.Distance < 0 ? "unknown" : _session.Distance.ToString("0 'm'")));
            else
                builder.AppendLine("Distance                       " + (_session.Distance < 0 ? "unknown" : _session.Distance.ToString("0 'm'")));

            builder.AppendLine("[/code]");

            Clipboard.SetText(builder.ToString());
        }

        //debug
        private void buttonDebug_Click(object sender, EventArgs e)
        {
            textBoxDebugResult.Text = _session.GetDebugValue(textBoxDebug.Text);
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
                // Get line data
                _session.GetLineData();

                // Get device info
                DeviceInfo deviceInfo = _session.GetDeviceInfo();
                setLabelText(labelHardwareVersion, deviceInfo.HardwareVersion);
                setLabelText(labelSoftwareVersion, deviceInfo.FirmwareVersion);
                setLabelText(labelGUIVersion, deviceInfo.GuiVersion);
                setLabelText(labelDeviceUptime, deviceInfo.DeviceUptime);
                setLabelText(labelLinkUptime, deviceInfo.LinkUptime);

                // Get dsl standard
                DSLStandard dslStandard = _session.GetDslStandard();
                setLabelText(labelDSLStandard, dslStandard.ToString().Replace("plus", "+"));

                // Get sync values
                setLabelText(labelDownstreamCurrentBitRate, "busy...");
                setLabelText(labelDownstreamCurrentBitRate, _session.DownstreamCurrentBitRate < 0 ? "unknown" : _session.DownstreamCurrentBitRate.ToString("###,###,##0 'kbps'"));

                setLabelText(labelUpstreamCurrentBitRate, "busy...");
                setLabelText(labelUpstreamCurrentBitRate, _session.UpstreamCurrentBitRate < 0 ? "unknown" : _session.UpstreamCurrentBitRate.ToString("###,###,##0 'kbps'"));

                // Get profile info
                if (_session.GetDslStandard() == DSLStandard.VDSL2)
                {
                    ProximusLineProfile currentProfile = _session.GetProfileInfo();

                    //TODO check why this is incorrect
                    //_session.getVectoringEnabled();
                    //setLabelText(labelVectoring, _session.VectoringEnabled ? "Yes" : "No");

                    if (currentProfile.Name == "unknown")
                    {
                        setLabelText(labelVectoring, "unknown");
                        setLabelText(labelDLM, "unknown");
                        setLabelText(labelRepair, "unknown");
                        setLabelText(labelProximusProfile, "unknown");
                        setLabelText(labelVDSLProfile, "unknown");
                    }
                    else
                    {
                        //get vectoring status fallback: get from profile list
                        setLabelText(labelVectoring, currentProfile.VectoringEnabled ? "Yes" : "No");
                        setLabelText(labelDLM, currentProfile.DlmProfile ? "Yes" : "No");
                        setLabelText(labelRepair, currentProfile.RepairProfile ? "Yes" : "No");
                        setLabelText(labelProximusProfile, currentProfile.Name.ToString());
                        setLabelText(labelVDSLProfile, currentProfile.ProfileVDSL2.ToString().Replace("p", ""));
                    }
                }
                else
                {
                    setLabelText(labelVDSLProfile, "n/a");
                    labelVDSLProfile.ForeColor = Color.Gray;
                    vdslProfileLabel.ForeColor = Color.Gray;
                    setLabelText(labelVectoring, "n/a");
                    labelVectoring.ForeColor = Color.Gray;
                    vectoringLabel.ForeColor = Color.Gray;
                    setLabelText(labelRepair, "n/a");
                    labelRepair.ForeColor = Color.Gray;
                    repairLabel.ForeColor = Color.Gray;
                    setLabelText(labelDLM, "n/a");
                    labelDLM.ForeColor = Color.Gray;
                    dlmLabel.ForeColor = Color.Gray;
                    setLabelText(labelProximusProfile, "n/a");
                    labelProximusProfile.ForeColor = Color.Gray;
                    proximusProfileLabel.ForeColor = Color.Gray;
                }

                //distance
                setLabelText(labelDistance, "busy...");
                setLabelText(labelDistance, _session.Distance.ToString("0 'm'"));

                //get line stats
                setLabelText(labelDownstreamAttenuation, "busy...");
                setLabelText(labelDownstreamAttenuation, _session.DownstreamAttenuation < 0 ? "unknown" : _session.DownstreamAttenuation.ToString("0.0 'dB'"));

                setLabelText(labelUpstreamAttenuation, "busy...");
                setLabelText(labelUpstreamAttenuation, _session.UpstreamAttenuation < 0 ? "unknown" : _session.UpstreamAttenuation.ToString("0.0 'dB'"));

                setLabelText(labelDownstreamNoiseMargin, "busy...");
                setLabelText(labelDownstreamNoiseMargin, _session.DownstreamNoiseMargin < 0 ? "unknown" : _session.DownstreamNoiseMargin.ToString("0.0 'dB'"));

                setLabelText(labelUpstreamNoiseMargin, "busy...");
                setLabelText(labelUpstreamNoiseMargin, _session.UpstreamNoiseMargin < 0 ? "unknown" : _session.UpstreamNoiseMargin.ToString("0.0 'dB'"));

                setLabelText(labelDownstreamMaxBitRate, "busy...");
                setLabelText(labelDownstreamMaxBitRate, _session.DownstreamMaxBitRate < 0 ? "unknown" : _session.DownstreamMaxBitRate.ToString("###,###,##0 'kbps'"));

                setLabelText(labelUpstreamMaxBitRate, "busy...");
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
                _session.CloseSession();
            }

            //worker.ReportProgress(100);
        }

        private void backgroundWorkerBbox_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            buttonClipboard.Enabled = true;
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
