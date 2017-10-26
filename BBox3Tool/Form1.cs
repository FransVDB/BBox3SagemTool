using BBox3Tool.objects;
using BBox3Tool.utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using BBox3Tool.enums;
using BBox3Tool.exception;
using BBox3Tool.profile;
using BBox3Tool.session;

namespace BBox3Tool
{
    public partial class Form1 : Form
    {
        private IModemSession _session;
        private List<ProximusLineProfile> _profiles;

        private Device _selectedModem = Device.Unknown;

        private readonly Color _colorSelected = Color.FromArgb(174, 204, 237);
        private readonly Color _colorMouseOver = Color.FromArgb(235, 228, 241);

        private readonly Uri _liveUpdateCheck = new Uri("http://www.cloudscape.be/userbasepyro85/latest.xml");
        private readonly Uri _liveUpdateProfiles = new Uri("http://www.cloudscape.be/userbasepyro85/profiles.xml");

        private bool _connecting;
        private bool _connected;

        public Form1()
        {
            InitializeComponent();

            //set form title
            Text += " " + Application.ProductVersion;

            //set worker thread properties
            backgroundWorkerGetLineData.WorkerSupportsCancellation = true;
            backgroundWorkerGetLineData.WorkerReportsProgress = true;

            //load embedded xml profiles
            _profiles = ProfileUtils.LoadEmbeddedProfiles();

            //do live update, update profiles
            backgroundWorkerLiveUpdate.RunWorkerAsync();

            //load settings if saved
            ToolSettings settings = SettingsUtils.LoadSettings();
            if (settings != null)
            {
                _selectedModem = settings.Device;
                textBoxIpAddress.Text = settings.Host;
                textBoxUsername.Text = settings.Username;
                textBoxPassword.Text = settings.Password;
                checkBoxSave.Checked = true;
                SetSelectedDevice();
            }
            else
            {
                //detect device
                backgroundWorkerDetectDevice.RunWorkerAsync();
                checkBoxSave.Checked = false;
            }
        }

        //buttons
        //-------
        private void buttonInfo_Click(object sender, EventArgs e)
        {
            Form check = Application.OpenForms["FormAbout"];
            if (check == null)
            {
                FormAbout form = new FormAbout();
                form.Show();
            }
            else
                check.Activate();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            //set session
            switch (_selectedModem)
            {
                case Device.BBOX3S:
                    _session = new Bbox3Session(backgroundWorkerGetLineData, _profiles);
                    break;
                case Device.BBOX2:
                    _session = new Bbox2Session();
                    break;
                case Device.FritzBox7390:
                    _session = new FritzBoxSession();
                    break;
                case Device.BBOX3T:
                    _session = null;
                    break;
                default:
                    _session = null;
                    break;
            }

            if (_session == null)
            {
                MessageBox.Show("Please select a device.", "Connection failure", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //set button text
            buttonConnect.Text = "Connecting, please wait...";
            buttonConnect.Enabled = false;

            //check mode
            bool debug = (textBoxUsername.Text.ToLower() == "debug");
            if (debug)
                InitDebugMode();
            else
                InitNormalMode();
        }

        private void InitDebugMode()
        {
            //get textbox values
            string host = textBoxIpAddress.Text;
            string username = "User"; //overwrite textbox value
            string password = textBoxPassword.Text;

            //init session
            _session = new Bbox3Session(backgroundWorkerGetLineData, _profiles, true);
            if (_session.OpenSession(host, username, password))
            {
                buttonConnect.Enabled = false;
                panelDebug.Visible = true;
                panelInfo.Visible = false;
                panelLogin.Visible = false;
            }
            else
            {
                //set button text
                buttonConnect.Text = "Connect";
                buttonConnect.Enabled = true;
                MessageBox.Show("Could not connect to device.", "Connection failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitNormalMode()
        {
            //init session
            backgroundWorkerConnect.RunWorkerAsync(new DeviceSettings
            {
                Host = textBoxIpAddress.Text,
                Username = textBoxUsername.Text,
                Password = textBoxPassword.Text,
            });
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (backgroundWorkerGetLineData.IsBusy)
            {
                backgroundWorkerGetLineData.CancelAsync();
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
            if (_session is Bbox3Session && new List<DSLStandard> { DSLStandard.ADSL, DSLStandard.ADSL2, DSLStandard.ADSL2plus }.Contains(_session.DSLStandard))
                builder.AppendLine("Upstream attenuation:          " + (_session.UpstreamAttenuation < 0 ? "unknown" : _session.UpstreamAttenuation.ToString("0.0 'dB'")));
            builder.AppendLine("");

            builder.AppendLine("Downstream noise margin:       " + (_session.DownstreamNoiseMargin < 0 ? "unknown" : _session.DownstreamNoiseMargin.ToString("0.0 'dB'")));
            if (_session is Bbox3Session || _session is FritzBoxSession)
                builder.AppendLine("Upstream noise margin:         " + (_session.UpstreamNoiseMargin < 0 ? "unknown" : _session.UpstreamNoiseMargin.ToString("0.0 'dB'")));
            builder.AppendLine("");


            builder.AppendLine("Vectoring down:                " + boolToString(_session.VectoringDown));
            builder.AppendLine("Vectoring up:                  " + boolToString(_session.VectoringUp));
            builder.AppendLine("Modem vectoring compatible:    " + boolToString(_session.VectoringDeviceCapable));
            builder.AppendLine("ROP vectoring compatible:      " + boolToString(_session.VectoringROPCapable));
            builder.AppendLine("");

            var dslStandardValue = _session.DSLStandard.ToString().Replace("plus", "+");
            if (_session.Annex != Annex.unknown)
                dslStandardValue += " Annex " + _session.Annex + "";
            builder.AppendLine("DSL standard:                  " + dslStandardValue);

            if (_session.DSLStandard == DSLStandard.VDSL2)
            {
                ProximusLineProfile currentProfile = ProfileUtils.GetProfile(_profiles, _session.UpstreamCurrentBitRate, _session.DownstreamCurrentBitRate, _session.VectoringDown, _session.VectoringUp, _session.Distance);
                if (currentProfile == null)
                {
                    builder.AppendLine("DSL profile:                   " + ProfileUtils.GetVdsl2ProfileFallBack(_session.DownstreamCurrentBitRate, _session.UpstreamCurrentBitRate).ToString().Replace("p", ""));
                    if (_session is Bbox3Session)
                        builder.AppendLine("Estimated distance:            " + (_session.Distance == null ? "unknown" : ((decimal)_session.Distance).ToString("0 'm'")));
                    else
                        builder.AppendLine("Distance                       " + (_session.Distance == null ? "unknown" : ((decimal)_session.Distance).ToString("0 'm'")));
                    builder.AppendLine("");
                    builder.AppendLine("Proximus profile name:         unknown");
                    builder.AppendLine("Proximus DLM profile:          unknown");
                    builder.AppendLine("Proximus repair profile:       unknown");
                }
                else
                {
                    builder.AppendLine("DSL profile:                   " + currentProfile.ProfileVDSL2.ToString().Replace("p", ""));
                    if (_session is Bbox3Session)
                        builder.AppendLine("Estimated distance:            " + (_session.Distance == null ? "unknown" : ((decimal)_session.Distance).ToString("0 'm'")));
                    else
                        builder.AppendLine("Distance                       " + (_session.Distance == null ? "unknown" : ((decimal)_session.Distance).ToString("0 'm'")));
                    builder.AppendLine("");
                    builder.AppendLine("Proximus profile name:         " + currentProfile.SpeedName);
                    builder.AppendLine("Proximus DLM profile:          " + boolToString(currentProfile.DlmProfile));
                    builder.AppendLine("Proximus repair profile:       " + boolToString(currentProfile.RepairProfile));
                }
            }
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
                buttonDebug_Click(sender, e);
        }

        //worker thread
        //-------------

        private void backgroundWorkerConnect_DoWork(object sender, DoWorkEventArgs e)
        {

            DeviceSettings sessionInfo = (DeviceSettings)e.Argument;

            try
            {
                _connecting = true;
                _connected = _session.OpenSession(sessionInfo.Host, sessionInfo.Username, sessionInfo.Password);

                //check remember settings
                if (_connected)
                {
                    if (checkBoxSave.Checked)
                        SettingsUtils.SaveSettings(new ToolSettings
                        {
                            Device = _selectedModem,
                            Username = sessionInfo.Username,
                            Password = sessionInfo.Password,
                            Host = sessionInfo.Host,
                        });
                    else
                        SettingsUtils.DeleteSettings();

                    //refresh
                    ThreadUtils.SetButtonTextFromThread(buttonConnect, "Refreshing, please wait...");
                    _session.RefreshData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error occurred. Debug info: " + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void backgroundWorkerConnect_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_connected)
            {
                //set button states
                buttonClipboard.Enabled = false;
                buttonCancel.Enabled = true;
                buttonConnect.Text = "Connect";

                //set labels empty
                labelDownstreamCurrentBitRate.Text = "...";
                labelUpstreamCurrentBitRate.Text = "...";
                labelDownstreamMaxBitRate.Text = "...";
                labelUpstreamMaxBitRate.Text = "...";
                labelDownstreamAttenuation.Text = "...";
                labelUpstreamAttenuation.Text = "...";
                labelDownstreamNoiseMargin.Text = "...";
                labelUpstreamNoiseMargin.Text = "...";

                labelDSLStandard.Text = "...";
                labelVDSLProfile.Text = "...";
                labelProximusProfile.Text = "...";
                labelVectoringDown.Text = "...";
                labelVectoringUp.Text = "...";
                labelVectoringModemCapable.Text = "...";
                labelVectoringROPCapable.Text = "...";
                labelDLM.Text = "...";
                labelRepair.Text = "...";
                labelDistance.Text = "...";

                labelDeviceUptime.Text = "...";
                labelLinkUptime.Text = "...";
                labelFirmwareVersion.Text = "...";
                labelHardwareVersion.Text = "...";


                //set panels visibility
                panelDebug.Visible = false;
                panelInfo.Visible = true;
                panelLogin.Visible = false;

                //start thread getLineData
                backgroundWorkerGetLineData.RunWorkerAsync();
            }
            else
            {
                buttonConnect.Enabled = true;
                buttonConnect.Text = "Connect";
                MessageBox.Show("Could not connect to device.", "Connection failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            _connecting = false;
        }

        private void backgroundWorkerGetLineData_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //disable connect button
                ThreadUtils.SetButtonEnabledFromThread(buttonConnect, false);

                if (_session is Bbox3Session)
                    ThreadUtils.SetLabelTextFromThread(distanceLabel, "Estimated distance");

                // Get device info
                //----------------
                SetLabelValueLoading(labelHardwareVersion);
                SetLabelValueLoading(labelFirmwareVersion);
                SetLabelValueLoading(labelDeviceUptime);
                SetLabelValueLoading(labelLinkUptime);
                DeviceInfo deviceInfo = _session.GetDeviceInfo();
                if (_session is Bbox3Session || _session is Bbox2Session)
                {
                    ThreadUtils.SetLabelTextFromThread(labelHardwareVersion, deviceInfo.HardwareVersion);
                    ThreadUtils.SetLabelTextFromThread(labelFirmwareVersion, deviceInfo.FirmwareVersion);
                    ThreadUtils.SetLabelTextFromThread(labelDeviceUptime, deviceInfo.DeviceUptime);
                    ThreadUtils.SetLabelTextFromThread(labelLinkUptime, deviceInfo.LinkUptime);
                }
                if (_session is FritzBoxSession)
                {
                    setLabelNotApplicable(hwVersionLabel, labelHardwareVersion);
                    setLabelNotApplicable(firmwareVersionLabel, labelFirmwareVersion);
                    setLabelNotApplicable(deviceUptimeLabel, labelDeviceUptime);
                    setLabelNotApplicable(lineUptimeLabel, labelLinkUptime);
                }

                // Check line connected
                //---------------------
                if (!_session.LineConnected)
                {
                    MessageBox.Show("Line status down, please check your connection and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {

                    // Get line data (BBox2 & FritzBox)
                    //---------------------------------
                    if (_session is Bbox2Session || _session is FritzBoxSession)
                    {
                        SetLabelValueLoading(labelDownstreamCurrentBitRate);
                        SetLabelValueLoading(labelUpstreamCurrentBitRate);
                        SetLabelValueLoading(labelDownstreamAttenuation);
                        SetLabelValueLoading(labelUpstreamAttenuation);
                        SetLabelValueLoading(labelDownstreamNoiseMargin);
                        SetLabelValueLoading(labelUpstreamNoiseMargin);
                        SetLabelValueLoading(labelDownstreamMaxBitRate);
                        SetLabelValueLoading(labelUpstreamMaxBitRate);

                        SetLabelValueLoading(labelDistance);
                        SetLabelValueLoading(labelDSLStandard);
                        SetLabelValueLoading(labelVDSLProfile);

                        SetLabelValueLoading(labelVectoringDown);
                        SetLabelValueLoading(labelVectoringUp);
                        SetLabelValueLoading(labelVectoringROPCapable);
                        SetLabelValueLoading(labelVectoringModemCapable);

                        SetLabelValueLoading(labelProximusProfile);
                        SetLabelValueLoading(labelDLM);
                        SetLabelValueLoading(labelRepair);

                    }
                    _session.GetLineData();

                    // Get dsl standard
                    //-----------------
                    var dslStandardValue = _session.DSLStandard.ToString().Replace("plus", "+");
                    if (_session.Annex != Annex.unknown)
                        dslStandardValue += " Annex " +_session.Annex + "";
                    ThreadUtils.SetLabelTextFromThread(labelDSLStandard, dslStandardValue);

                    // Get sync values
                    //----------------
                    SetLabelValueLoading(labelDownstreamCurrentBitRate);
                    setLabelValueAsDecimal(labelDownstreamCurrentBitRate, _session.DownstreamCurrentBitRate, "###,###,##0 'kbps'");
                    SetLabelValueLoading(labelUpstreamCurrentBitRate);
                    setLabelValueAsDecimal(labelUpstreamCurrentBitRate, _session.UpstreamCurrentBitRate, "###,###,##0 'kbps'");

                    //distance
                    //--------
                    SetLabelValueLoading(labelDistance);
                    setLabelValueAsDecimal(labelDistance, _session.Distance, "0 'm'");

                    //Proximus profile
                    //----------------
                    if (_session.DSLStandard == DSLStandard.VDSL2)
                    {
                        //get vectoring status
                        SetLabelValueLoading(labelVectoringDown);
                        ThreadUtils.SetLabelTextFromThread(labelVectoringDown, boolToString(_session.VectoringDown));
                        SetLabelValueLoading(labelVectoringUp);
                        ThreadUtils.SetLabelTextFromThread(labelVectoringUp, boolToString(_session.VectoringUp));

                        ProximusLineProfile currentProfile = ProfileUtils.GetProfile(_profiles, _session.UpstreamCurrentBitRate, _session.DownstreamCurrentBitRate, _session.VectoringDown, _session.VectoringUp, _session.Distance);
                        if (currentProfile == null)
                        {
                            ThreadUtils.SetLabelTextFromThread(labelDLM, "unknown");
                            ThreadUtils.SetLabelTextFromThread(labelRepair, "unknown");
                            ThreadUtils.SetLabelTextFromThread(labelProximusProfile, "unknown");
                            ThreadUtils.SetLabelTextFromThread(labelVDSLProfile, ProfileUtils.GetVdsl2ProfileFallBack(_session.DownstreamCurrentBitRate, _session.UpstreamCurrentBitRate).ToString().Replace("p", ""));
                        }
                        else
                        {
                            ThreadUtils.SetLabelTextFromThread(labelDLM, boolToString(currentProfile.DlmProfile));
                            ThreadUtils.SetLabelTextFromThread(labelRepair, boolToString(currentProfile.RepairProfile));
                            ThreadUtils.SetLabelTextFromThread(labelProximusProfile, currentProfile.SpeedName);
                            ThreadUtils.SetLabelTextFromThread(labelVDSLProfile, currentProfile.ProfileVDSL2.ToString().Replace("p", ""));
                        }
                    }
                    else
                    {
                        setLabelNotApplicable(vdslProfileLabel, labelVDSLProfile);
                        setLabelNotApplicable(vectoringLabel, labelVectoringDown);
                        setLabelNotApplicable(vectoringLabel, labelVectoringUp);
                        setLabelNotApplicable(repairLabel, labelRepair);
                        setLabelNotApplicable(dlmLabel, labelDLM);
                        setLabelNotApplicable(proximusProfileLabel, labelProximusProfile);
                    }

                    //vectoring ROP capable
                    SetLabelValueLoading(labelVectoringROPCapable);
                    ThreadUtils.SetLabelTextFromThread(labelVectoringROPCapable, boolToString(_session.VectoringROPCapable));
                    SetLabelValueLoading(labelVectoringModemCapable);
                    ThreadUtils.SetLabelTextFromThread(labelVectoringModemCapable, boolToString(_session.VectoringDeviceCapable));

                    //attenuation
                    //-----------
                    //downstream
                    SetLabelValueLoading(labelDownstreamAttenuation);
                    setLabelValueAsDecimal(labelDownstreamAttenuation, _session.DownstreamAttenuation, "0.0 'dB'");

                    //upstream: BBOX3 adsl only
                    if (_session is Bbox3Session && new List<DSLStandard> { DSLStandard.ADSL, DSLStandard.ADSL2, DSLStandard.ADSL2plus }.Contains(_session.DSLStandard))
                    {
                        SetLabelValueLoading(labelUpstreamAttenuation);
                        setLabelValueAsDecimal(labelUpstreamAttenuation, _session.UpstreamAttenuation, "0.0 'dB'");
                    }
                    else
                        setLabelNotApplicable(upstreamAttenuationLabel, labelUpstreamAttenuation);

                    //noise margin
                    //------------
                    //downstream
                    SetLabelValueLoading(labelDownstreamNoiseMargin);
                    setLabelValueAsDecimal(labelDownstreamNoiseMargin, _session.DownstreamNoiseMargin, "0.0 'dB'");

                    //upstream: not for BBOX2
                    if (_session is Bbox3Session || _session is FritzBoxSession)
                    {
                        SetLabelValueLoading(labelUpstreamNoiseMargin);
                        setLabelValueAsDecimal(labelUpstreamNoiseMargin, _session.UpstreamNoiseMargin, "0.0 'dB'");
                    }
                    else
                        setLabelNotApplicable(upstreamNoiseMarginLabel, labelUpstreamNoiseMargin);

                    //max bitrate
                    //-----------
                    //downstream
                    SetLabelValueLoading(labelDownstreamMaxBitRate);
                    setLabelValueAsDecimal(labelDownstreamMaxBitRate, _session.DownstreamMaxBitRate, "###,###,##0 'kbps'");

                    //upstream: not for BBOX2
                    if (_session is Bbox3Session || _session is FritzBoxSession)
                    {
                        SetLabelValueLoading(labelUpstreamMaxBitRate);
                        setLabelValueAsDecimal(labelUpstreamMaxBitRate, _session.UpstreamMaxBitRate, "###,###,##0 'kbps'");
                    }
                    else
                        setLabelNotApplicable(upstreamMaxBitRateLabel, labelUpstreamMaxBitRate);
                }
            }
            catch (ThreadCancelledException) { }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error occurred. Debug info: " + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (_connected)
                    _session.CloseSession();
            }
        }

        private void backgroundWorkerGetLineData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            buttonClipboard.Enabled = true;
            buttonConnect.Enabled = true;
            buttonCancel.Enabled = false;
        }

        private void setLabelNotApplicable(Label label, Label value)
        {
            ThreadUtils.SetLabelTextFromThread(value, "n/a");
            value.ForeColor = Color.Gray;
            label.ForeColor = Color.Gray;
        }

        private void SetLabelValueLoading(Label label)
        {
            //set loader
            PictureBox loader = new PictureBox
            {
                Image = Properties.Resources.loader,
                Visible = true,
                Size = new Size(16, 16),
                Location = label.Location
            };
            loader.Refresh();
            loader.BringToFront();

            ThreadUtils.AddLoaderToPanel((Panel)label.Parent, loader);
            ThreadUtils.SetLabelTextFromThread(label, "");

            //set eventhandler, when text changes, remove loading icon
            label.TextChanged += (sender, e) =>
            {
                ThreadUtils.RemoveLoaderFromPanel(panelInfo, loader);
            };
        }

        private void setLabelValueAsDecimal(Label label, decimal? value, string formatter)
        {
            if (value == null)
                ThreadUtils.SetLabelTextFromThread(label, "unknown");
            else
                ThreadUtils.SetLabelTextFromThread(label, value < 0 ? "unknown" : ((decimal)value).ToString(formatter));
        }

        private string boolToString(bool? value)
        {
            if (value == null)
                return "unknown";
            return ((bool)value) ? "yes" : "no";
        }

        //live update thread
        //------------------

        private void backgroundWorkerLiveUpdate_DoWork(object sender, DoWorkEventArgs e)
        {
            //disable connect button until
            buttonConnect.Enabled = false;
            try
            {
                //check latest profiles & distance
                string latest = NetworkUtils.SendRequest(_liveUpdateCheck);
                if (!string.IsNullOrEmpty(latest))
                {
                    XmlDocument latestDoc = new XmlDocument();
                    latestDoc.LoadXml(latest);
                    if (Decimal.Parse(latestDoc.SelectSingleNode("//document/version").InnerText, CultureInfo.InvariantCulture) == 1)
                    {
                        decimal latestOnlineProfile = Decimal.Parse(latestDoc.SelectSingleNode("//document/latest/profiles").InnerText, CultureInfo.InvariantCulture);
                        decimal latestBBox3Distance = Decimal.Parse(latestDoc.SelectSingleNode("//document/bbox3s/distance").InnerText, CultureInfo.InvariantCulture);

                        //check if online version is more recent then embedded verion
                        XmlDocument profilesDoc = new XmlDocument();
                        using (Stream stream = typeof(Form1).Assembly.GetManifestResourceStream("BBox3Tool.profile.profiles.xml"))
                        {
                            using (StreamReader sr = new StreamReader(stream))
                            {
                                profilesDoc.LoadXml(sr.ReadToEnd());
                            }
                        }
                        decimal latestembEddedProfile = Decimal.Parse(profilesDoc.SelectSingleNode("//document/version").InnerText, CultureInfo.InvariantCulture);

                        //more recent version found, update needed
                        if (latestOnlineProfile > latestembEddedProfile)
                        {
                            bool getOnlineProfiles = false;

                            //check if we need latest profiles
                            if (!File.Exists("BBox3Tool.profiles.xml"))
                                getOnlineProfiles = true;
                            else
                            {
                                XmlDocument localDoc = new XmlDocument();
                                localDoc.Load("BBox3Tool.profiles.xml");
                                decimal latestLocalProfile = Decimal.Parse(localDoc.SelectSingleNode("//document/version").InnerText, CultureInfo.InvariantCulture);
                                if (latestOnlineProfile > latestLocalProfile)
                                    getOnlineProfiles = true;
                            }

                            //check if profiles are already stored locally
                            if (getOnlineProfiles)
                            {
                                string latestProfiles = NetworkUtils.SendRequest(_liveUpdateProfiles);
                                if (!string.IsNullOrEmpty(latestProfiles))
                                {
                                    XmlDocument latestprofilesDoc = new XmlDocument();
                                    latestprofilesDoc.LoadXml(latestProfiles);
                                    latestprofilesDoc.Save("BBox3Tool.profiles.xml");
                                }
                            }

                            //check again, live update could have failed
                            if (File.Exists("BBox3Tool.profiles.xml"))
                            {
                                XmlDocument localDoc = new XmlDocument();
                                localDoc.Load("BBox3Tool.profiles.xml");
                                lock (_profiles)
                                {
                                    _profiles = ProfileUtils.LoadProfilesFromXml(localDoc);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                // ignored
            }
            finally
            {
                backgroundWorkerLiveUpdate.ReportProgress(100);
            }
        }

        private void backgroundWorkerLiveUpdate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            buttonConnect.Enabled = true;
        }

        //detect device thread
        //--------------------
        private void backgroundWorkerDetectDevice_DoWork(object sender, DoWorkEventArgs e)
        {
            Device detected = NetworkUtils.DetectDevice("192.168.1.1"); //default
            //try fritzbox default if not found
            if (detected == Device.Unknown)
                detected = NetworkUtils.DetectDevice("192.168.178.1");
            //do not overwrite user choice
            if (_selectedModem == Device.Unknown && detected != Device.Unknown)
                _selectedModem = detected;
            //detect bbox3 Technicolor
            if (detected == Device.BBOX3T)
                ThreadUtils.SetPanelVisibilityFromThread(panelUnsupported, true);
        }

        private void backgroundWorkerDetectDevice_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SetSelectedDevice();
        }

        private void SetSelectedDevice()
        {
            //preselect modem
            switch (_selectedModem)
            {
                case Device.BBOX3S:
                    panelThumb_Click(panelBBox3S, null);
                    break;
                case Device.BBOX2:
                    panelThumb_Click(panelBBox2, null);
                    break;
                case Device.FritzBox7390:
                    panelThumb_Click(panelFritzBox, null);
                    break;
            }
        }

        //gui
        //---

        private void panelThumb_MouseEnter(object sender, EventArgs e)
        {
            if (_connecting)
                return;

            Panel panel = getPanelFromThumb(sender);
            if (panel == null)
                return;

            Color color = _colorMouseOver;
            if (CheckPanelSelected(panel))
                color = _colorSelected;
            panel.BackColor = color;
        }

        private void panelThumb_MouseLeave(object sender, EventArgs e)
        {
            if (_connecting)
                return;

            Panel panel = getPanelFromThumb(sender);
            if (panel == null)
                return;

            Color color = Color.WhiteSmoke;
            if (CheckPanelSelected(panel))
                color = _colorSelected;

            panel.BackColor = color;
        }

        private void panelThumb_Click(object sender, EventArgs e)
        {
            if (_connecting)
                return;

            //reset colors
            panelBBox3S.BackColor = Color.WhiteSmoke;
            panelBBox2.BackColor = Color.WhiteSmoke;
            panelFritzBox.BackColor = Color.WhiteSmoke;

            //set color
            Panel panel = getPanelFromThumb(sender);
            if (panel == null)
                return;
            panel.BackColor = _colorSelected;

            //select modem
            if (panel == panelBBox3S)
            {
                _selectedModem = Device.BBOX3S;
                textBoxUsername.Text = "User";
                textBoxUsername.Enabled = true;
            }
            else if (panel == panelBBox2)
            {
                _selectedModem = Device.BBOX2;
                textBoxUsername.Text = "admin";
                textBoxUsername.Enabled = true;
            }
            else if (panel == panelFritzBox)
            {
                _selectedModem = Device.FritzBox7390;
                textBoxUsername.Text = "N/A";
                textBoxUsername.Enabled = false;
            }
            else
            {
                textBoxUsername.Text = "";
                textBoxUsername.Enabled = true;
                _selectedModem = Device.Unknown;
            }
        }

        private Panel getPanelFromThumb(object sender)
        {
            Panel panel = null;

            if (sender is Panel)
                panel = (Panel)sender;
            else if (sender is Label)
                panel = (Panel)((Label)sender).Parent;
            else if (sender is PictureBox)
                panel = (Panel)((PictureBox)sender).Parent;

            return panel;
        }

        private bool CheckPanelSelected(Panel panel)
        {
            if (panel == panelBBox3S && _selectedModem == Device.BBOX3S)
                return true;

            if (panel == panelBBox2 && _selectedModem == Device.BBOX2)
                return true;

            if (panel == panelFritzBox && _selectedModem == Device.FritzBox7390)
                return true;

            return false;
        }
    }
}