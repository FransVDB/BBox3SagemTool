using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml;
using System.Collections.Generic;
using BBox3Tool.objects;
using System.Net.NetworkInformation;

namespace BBox3Tool
{
    public partial class Form1 : Form
    {
        private IModemSession _session;
        private List<ProximusLineProfile> _profiles;

        public Form1()
        {
            InitializeComponent();
            this.Text += " " + Application.ProductVersion;
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.WorkerReportsProgress = true;

            //load xml profiles
            _profiles = loadEmbeddedProfiles();

            //load settings if saved
            if (loadSettings())
                checkBoxSave.Checked = true;
            else
            {
                // Init for bbox3 if not found
                _session = new Bbox3Session(backgroundWorker, _profiles);
                checkBoxSave.Checked = false;
            }
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
            _session = new Bbox3Session(backgroundWorker, _profiles);

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
            _session = new Bbox3Session(backgroundWorker, _profiles, true);
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
                //check remember settings
                if (checkBoxSave.Checked)
                    saveSettings(username, password, host);
                else
                    deleteSettings();
                
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
            if (_session is Bbox3Session || _session is FritzBoxSession)
                builder.AppendLine("Upstream max bit rate:         " + (_session.UpstreamMaxBitRate < 0 ? "unknown" : _session.UpstreamMaxBitRate.ToString("###,###,##0 'kbps'")));
            builder.AppendLine("");
            
            builder.AppendLine("Downstream attenuation:        " + (_session.DownstreamAttenuation < 0 ? "unknown" : _session.DownstreamAttenuation.ToString("0.0 'dB'")));
            if (_session is Bbox3Session && new List<DSLStandard> { DSLStandard.ADSL, DSLStandard.ADSL2, DSLStandard.ADSL2plus }.Contains(_session.GetDslStandard()))
                builder.AppendLine("Upstream attenuation:          " + (_session.UpstreamAttenuation < 0 ? "unknown" : _session.UpstreamAttenuation.ToString("0.0 'dB'")));
            builder.AppendLine("");
            
            builder.AppendLine("Downstream noise margin:       " + (_session.DownstreamNoiseMargin < 0 ? "unknown" : _session.DownstreamNoiseMargin.ToString("0.0 'dB'")));
            if (_session is Bbox3Session || _session is FritzBoxSession)
                builder.AppendLine("Upstream noise margin:         " + (_session.UpstreamNoiseMargin < 0 ? "unknown" : _session.UpstreamNoiseMargin.ToString("0.0 'dB'")));
            builder.AppendLine("");
           
            builder.AppendLine("DSL standard:                  " + _session.GetDslStandard().ToString().Replace("plus", "+"));
            if (_session.GetDslStandard() == DSLStandard.VDSL2)
            {
                ProximusLineProfile currentProfile = getProfile(_session.UpstreamCurrentBitRate, _session.DownstreamCurrentBitRate);
                if (currentProfile == null)
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
                    //TODO check why this is incorrect
                    //_session.getVectoringEnabled();
                    //setLabelText(labelVectoring, _session.VectoringEnabled ? "Yes" : "No");

                    ProximusLineProfile currentProfile = getProfile(_session.UpstreamCurrentBitRate, _session.DownstreamCurrentBitRate);
                    if (currentProfile == null)
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

                //upstream attenuation: BBOX3 adsl only
                if (_session is Bbox3Session && new List<DSLStandard> { DSLStandard.ADSL, DSLStandard.ADSL2, DSLStandard.ADSL2plus }.Contains(_session.GetDslStandard()))
                {
                    setLabelText(labelUpstreamAttenuation, "busy...");
                    setLabelText(labelUpstreamAttenuation, _session.UpstreamAttenuation < 0 ? "unknown" : _session.UpstreamAttenuation.ToString("0.0 'dB'"));
                }
                else
                {
                    setLabelText(labelUpstreamAttenuation, "n/a");
                    labelUpstreamAttenuation.ForeColor = Color.Gray;
                    upstreamAttenuationLabel.ForeColor = Color.Gray;
                }

                //downstream attenuation
                setLabelText(labelDownstreamNoiseMargin, "busy...");
                setLabelText(labelDownstreamNoiseMargin, _session.DownstreamNoiseMargin < 0 ? "unknown" : _session.DownstreamNoiseMargin.ToString("0.0 'dB'"));
                
                //upstream noise margin: not for BBOX2
                if (_session is Bbox3Session || _session is FritzBoxSession)
                {
                    setLabelText(labelUpstreamNoiseMargin, "busy...");
                    setLabelText(labelUpstreamNoiseMargin, _session.UpstreamNoiseMargin < 0 ? "unknown" : _session.UpstreamNoiseMargin.ToString("0.0 'dB'")); 
                }
                else
                {
                    setLabelText(labelUpstreamNoiseMargin, "n/a");
                    labelUpstreamNoiseMargin.ForeColor = Color.Gray;
                    upstreamNoiseMarginLabel.ForeColor = Color.Gray;
                }
                
                //downstream max bitrate
                setLabelText(labelDownstreamMaxBitRate, "busy...");
                setLabelText(labelDownstreamMaxBitRate, _session.DownstreamMaxBitRate < 0 ? "unknown" : _session.DownstreamMaxBitRate.ToString("###,###,##0 'kbps'"));
                
                //upstream max bit rate: not for BBOX2
                if (_session is Bbox3Session || _session is FritzBoxSession)
                {
                    setLabelText(labelUpstreamMaxBitRate, "busy...");
                    setLabelText(labelUpstreamMaxBitRate, _session.UpstreamMaxBitRate < 0 ? "unknown" : _session.UpstreamMaxBitRate.ToString("###,###,##0 'kbps'"));
                }
                else
                {
                    setLabelText(labelUpstreamMaxBitRate, "n/a");
                    labelUpstreamMaxBitRate.ForeColor = Color.Gray;
                    upstreamMaxBitRateLabel.ForeColor = Color.Gray;
                }
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

        private List<ProximusLineProfile> loadEmbeddedProfiles()
        {
            //load xml doc
            XmlDocument profilesDoc = new XmlDocument();
            using (Stream stream = typeof(Form1).Assembly.GetManifestResourceStream("BBox3Tool.profile.profiles.xml"))
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    profilesDoc.LoadXml(sr.ReadToEnd());
                }
            }

            //run trough all xml profiles
            List<ProximusLineProfile> listProfiles = new List<ProximusLineProfile>();
            foreach (XmlNode profileNode in profilesDoc.SelectNodes("//document/profiles/profile"))
            {
                List<int> confirmedDownloadList = new List<int>();
                List<int> confirmedUploadList = new List<int>();
                foreach (XmlNode confirmedNode in profileNode.SelectNodes("confirmed"))
                {
                    confirmedDownloadList.Add(Convert.ToInt32(confirmedNode.Attributes["down"].Value));
                    confirmedUploadList.Add(Convert.ToInt32(confirmedNode.Attributes["up"].Value));
                }
                confirmedDownloadList.Add(Convert.ToInt32(profileNode.SelectNodes("official")[0].Attributes["down"].Value));
                confirmedUploadList.Add(Convert.ToInt32(profileNode.SelectNodes("official")[0].Attributes["up"].Value));

                ProximusLineProfile profile = new ProximusLineProfile(
                    profileNode.Attributes["name"].Value,
                    confirmedDownloadList.Last(),
                    confirmedUploadList.Last(),
                    Convert.ToBoolean(profileNode.Attributes["provisioning"].Value),
                    Convert.ToBoolean(profileNode.Attributes["dlm"].Value),
                    Convert.ToBoolean(profileNode.Attributes["repair"].Value),
                    Convert.ToBoolean(profileNode.Attributes["vectoring"].Value),
                    (VDSL2Profile) Enum.Parse(typeof(VDSL2Profile), "p" + profileNode.Attributes["vdsl2"].Value),
                    confirmedDownloadList.Distinct().ToList(),
                    confirmedUploadList.Distinct().ToList());

                listProfiles.Add(profile);
            }
            return listProfiles;
        }

        private ProximusLineProfile getProfile(int uploadSpeed, int downloadSpeed)
        {
            ProximusLineProfile profile = new ProximusLineProfile();

            //check if speed matches with confirmed speeds
            List<ProximusLineProfile> confirmedMatches = _profiles.Where(x => x.ConfirmedDownloadSpeeds.Contains(downloadSpeed) && x.ConfirmedUploadSpeeds.Contains(uploadSpeed)).ToList();
            
            //1 match found
            if (confirmedMatches.Count == 1)
                return confirmedMatches.First();

            //multiple matches found, get profile with closest official download speed
            if (confirmedMatches.Count > 1)
                return confirmedMatches.Select(x => new { x, diff = Math.Abs(x.DownloadSpeed - downloadSpeed) })
                  .OrderBy(p => p.diff)
                  .First().x;

            //no matches found, get profile with closest speeds in range of +256kb
            List<ProximusLineProfile> rangeMatches = _profiles.Select(x => new { x, diffDownload = Math.Abs(x.DownloadSpeed - downloadSpeed), diffUpload = Math.Abs(x.UploadSpeed - uploadSpeed) })
                .Where(x => x.diffDownload <= 256 && x.diffUpload <= 256)
                .OrderBy(p => p.diffDownload)
                .ThenBy(p => p.diffUpload)
                .Select(y => y.x)
                .ToList();

            //check matches found
            if (rangeMatches.Count > 0)
                return rangeMatches.First();

            //no matches found
            return null;
        }

        //save & load settings
        //--------------------

        private void saveSettings(string username, string password, string host)
        {
            //load xml doc
            XmlDocument settingsDoc = new XmlDocument();
            using (Stream stream = typeof(Form1).Assembly.GetManifestResourceStream("BBox3Tool.settings.xml"))
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    settingsDoc.LoadXml(sr.ReadToEnd());
                }
            }
            settingsDoc.SelectSingleNode("//document/login/ip").InnerText = host;
            settingsDoc.SelectSingleNode("//document/login/user").InnerText = username;
            
            try
            {
                settingsDoc.SelectSingleNode("//document/login/password").InnerText = Crypto.EncryptStringAES(password, NetworkInterface.GetAllNetworkInterfaces().First().GetPhysicalAddress().ToString());
            }
            catch { }

            if (_session is Bbox3Session)
                settingsDoc.SelectSingleNode("//document/login/device").InnerText = "BBOX3S";
            else if (_session is Bbox2Session)
                settingsDoc.SelectSingleNode("//document/login/device").InnerText = "BBOX2";
            else if (_session is FritzBoxSession)
                settingsDoc.SelectSingleNode("//document/login/device").InnerText = "FRITZBOX";

            settingsDoc.Save("settings.xml");
        }

        private bool loadSettings()
        {
            //load xml doc
            try
            {
                if (File.Exists("settings.xml"))
                {
                    XmlDocument settingsDoc = new XmlDocument();
                    settingsDoc.Load("settings.xml");

                    //only support settings v1.0
                    if (settingsDoc.SelectSingleNode("//document/version").InnerText != "1.0")
                        return false;

                    textBoxIpAddress.Text = settingsDoc.SelectSingleNode("//document/login/ip").InnerText;
                    textBoxUsername.Text = settingsDoc.SelectSingleNode("//document/login/user").InnerText;
                    try
                    {
                        textBoxPassword.Text = Crypto.DecryptStringAES(settingsDoc.SelectSingleNode("//document/login/password").InnerText, NetworkInterface.GetAllNetworkInterfaces().First().GetPhysicalAddress().ToString());
                    }
                    catch { }
                    switch (settingsDoc.SelectSingleNode("//document/login/device").InnerText)
                    {
                        case "BBOX3S":
                            _session = new Bbox3Session(backgroundWorker, _profiles);
                            break;
                        case "BBOX2":
                            _session = new Bbox2Session();
                            break;
                        case "FRITZBOX":
                            _session = new FritzBoxSession();
                            break;
                        default:
                            break;
                    }
                    return true;
                }
            }
            catch
            { }
            return false;
        }

        private void deleteSettings() 
        {
            if (File.Exists("settings.xml"))
            {
                try {
                    File.Delete("settings.xml");
                }
                catch { }
            }
        }
    }
}
