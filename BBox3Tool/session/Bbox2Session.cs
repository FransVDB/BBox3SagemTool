using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using BBox3Tool.enums;
using BBox3Tool.objects;
using BBox3Tool.utils;

namespace BBox3Tool.session
{
    internal class Bbox2Session : IModemSession
    {
        private VDSL2Profile _vdslProfile;
        private TelnetConnection _tc;

        private string _host;
        private string _username;
        private string _password;

        private bool _gotAdminHTML;
        private string _adminHTML;

        #region getters & setters

        public int DownstreamCurrentBitRate { get; private set; }

        public int UpstreamCurrentBitRate { get; private set; }

        public int DownstreamMaxBitRate { get; private set; }

        public int UpstreamMaxBitRate { get; private set; }

        public decimal DownstreamAttenuation { get; private set; }

        public decimal UpstreamAttenuation { get; private set; }

        public decimal DownstreamNoiseMargin { get; private set; }

        public decimal UpstreamNoiseMargin { get; private set; }

        public decimal? Distance { get; private set; }

        public string DeviceName { get; private set; }

        public bool VectoringDown { get; private set; }

        public bool VectoringUp { get; private set; }

        public bool VectoringDeviceCapable { get; private set; }

        public bool? VectoringROPCapable { get; private set; }

        public DSLStandard DSLStandard { get; private set; }

        public bool LineConnected
        {
            get
            {
                return (GetAdminHTML().IndexOf("NOT CONNECTED") == -1);
            }
        }

        #endregion

        public Bbox2Session()
	    {
            DeviceName = "B-Box 2";
            Distance = null;
            VectoringDown = false;
            VectoringUp = false;
            VectoringDeviceCapable = false;

            _host = string.Empty;
            _username = string.Empty;
            _password = string.Empty;
	    }

        public bool OpenSession(String host, String username, String password)
        {
            try
            {
                // New connection
                _tc = new TelnetConnection(host, 23);

                // Login
                var usernamePrompt = _tc.Read(2000);
                if (usernamePrompt.Contains("login:"))
                {
                    _tc.WriteLine(username);
                }
                else
                {
                    return false;
                }
                var passwordPrompt = _tc.Read(200);
                if (passwordPrompt.Contains("Password:"))
                {
                    _tc.WriteLine(password);
                }
                else
                {
                    return false;
                }

                //check login successfull
                var result = _tc.Read(200);
                if (result.ToLower().Contains("admin @ home"))
                {
                    _host = host;
                    _username = username;
                    _password = password;
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool CloseSession()
        {
            // Close session if still connected
            if (_tc.IsConnected)
            {
                _tc.WriteLine("^C");
                _tc.WriteLine("exit");
            }

            // Kill socket
            _tc.CloseConnection();

            return true;
        }
        
        public void RefreshData()
        {
            //not implemented
        }

        public void GetLineData()
        {
            try
            {
                // Exec 'shell' command
                _tc.WriteLine("shell");

                // Wait for shell prompt
                if (_tc.Read(1000).EndsWith("# "))
                {
                    // Send 'vdsl pstatex' command
                    _tc.WriteLine("vdsl pstatex");
                }

                // Read reply
                var pstatexReply = _tc.Read(1000);
                if (pstatexReply.Contains("Far-end ITU Vendor Id"))
                {
                    // Parse results
                    ParsePstatex(pstatexReply);
                }
                else
                {
                    throw new Exception("Unable to read extended port status.");
                }

                // Wait for shell prompt
                if (pstatexReply.EndsWith("# "))
                {
                    // Send 'vdsl getsnr' command
                    _tc.WriteLine("vdsl getsnr");
                }

                // Read reply
                var getsnrReply = _tc.Read(1000);
                if (getsnrReply.Contains("Attenuation"))
                {
                    // Parse results
                    ParseVdslSnr(getsnrReply);
                }
            }
            //vdsl pstatex connection failed --> adsl  line
            catch
            {
                DownstreamCurrentBitRate = 0;
                UpstreamCurrentBitRate = 0;
                DownstreamMaxBitRate = 0;
                UpstreamMaxBitRate = 0;
                DownstreamNoiseMargin = 0;
                UpstreamNoiseMargin = 0;
                DownstreamAttenuation = 0;
                UpstreamAttenuation = 0;
                Distance = 0;
                DSLStandard = DSLStandard.unknown;
                _vdslProfile = VDSL2Profile.unknown;
                //TODO read adsl stats from telnet
                //_tc.WriteLine("adslstat");
            }
        }

        public DeviceInfo GetDeviceInfo()
        {
            var deviceInfo = new DeviceInfo();
            deviceInfo.DeviceUptime = "Unknown";
            deviceInfo.LinkUptime = "Unknown";
            deviceInfo.HardwareVersion = "Unknown";
            deviceInfo.FirmwareVersion = "Unknown";

            try
            {
                string homePage = GetAdminHTML();

                //get hardware version
                int hwIndex = homePage.IndexOf("<td class=\"libelle\">Hardware Version</td>");
                if (hwIndex > 0)
                {
                    hwIndex = homePage.IndexOf("<td class=\"status\">", hwIndex);
                    if (hwIndex > 0)
                    {
                        hwIndex += "<td class=\"status\">".Length;
                        int hwIndexEnd = homePage.IndexOf("<", hwIndex);

                        string hwVersion = homePage.Substring(hwIndex, hwIndexEnd - hwIndex).Replace("&nbsp;", " ");
                        deviceInfo.HardwareVersion = hwVersion;
                    }
                }

                //get firmware version
                int fwIndex = homePage.IndexOf("<td class=\"libelle\">Runtime Code Version</td>");
                if (fwIndex > 0)
                {
                    fwIndex = homePage.IndexOf("<td class=\"status\">", fwIndex);
                    if (fwIndex > 0)
                    {
                        fwIndex += "<td class=\"status\">".Length;
                        int fwIndexEnd = homePage.IndexOf("<", fwIndex);
                        string fwVersion = homePage.Substring(fwIndex, fwIndexEnd - fwIndex).Replace("&nbsp;", " ");
                        deviceInfo.FirmwareVersion = fwVersion;
                    }
                }
            }
            catch { }
            return deviceInfo;
        }

        public string GetDebugValue(string debugValue)
        {
            return "Not implemented yet!";
        }

        private void ParsePstatex(String pstatex)
        {
            var reader = new StringReader(pstatex);
            while (true)
            {
                var line = reader.ReadLine();
                if (line != null)
                {
                    var array = line.Split(':');
                    switch (array[0])
                    {
                        case "Bearer1 Downstream payload rate":
                            var dsCurrentBitRate = array[1].Trim().Replace(" kbps", "");
                            DownstreamCurrentBitRate = Convert.ToInt32(dsCurrentBitRate);
                            break;
                        case "Bearer1 Upstream payload rate":
                            var usCurrentBitRate = array[1].Trim().Replace(" kbps", "");
                            UpstreamCurrentBitRate = Convert.ToInt32(usCurrentBitRate);
                            break;
                        case "Downstream attainable payload rate":
                            var dsMaxBitRate = array[1].Trim().Replace(" kbps", "");
                            DownstreamMaxBitRate = Convert.ToInt32(dsMaxBitRate);
                            break;
                        case "Downstream Training Margin":
                            var dsNoiseMargin = array[1].Trim().Replace(" dB", "");
                            DownstreamNoiseMargin = Convert.ToDecimal(dsNoiseMargin, CultureInfo.InvariantCulture);
                            break;
                        case "Bandplan Type...........":
                            _vdslProfile = VDSL2Profile.p8d;
                            if (array[1].Trim().Equals("0"))
                            {
                                _vdslProfile = VDSL2Profile.p17a;
                            }
                            break;
                        case "VDSL Estimated Loop Length ":
                            var loopLength = array[1].Trim().Replace("ft", "").Trim();
                            Distance = Convert.ToDecimal(loopLength, CultureInfo.InvariantCulture) * 0.3048m;
                            break;
                        case "Line Type":
                            if (array[1].Trim() == "0x00800000#" || array[1].Trim() == "0x04000000#" || array[1].Trim() == "0x00200000#")
                                DSLStandard = DSLStandard.VDSL2;
                            else
                                DSLStandard = DSLStandard.unknown;
                            break;
                        case "Far-end ITU Vendor Id":
                            VectoringROPCapable = FromHexString(array[1]).Contains("BDCM"); //0xb5004244434da45f
                            break;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        private void ParseVdslSnr(String snr)
        {
            var reader = new StringReader(snr);
            while (true)
            {
                var line = reader.ReadLine();
                if (line != null)
                {
                    var array = line.Split(':');
                    switch (array[0])
                    {
                        case "Attenuation":
                            var dsAttenuation = array[1].Trim().Replace(" dB", "");
                            DownstreamAttenuation = Convert.ToDecimal(dsAttenuation, CultureInfo.InvariantCulture);
                            break;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        private string GetAdminHTML()
        {
            if (!_gotAdminHTML)
            {
                Uri bbox2Uri = new Uri("http://" + _host);
                Dictionary<string, string> getData = new Dictionary<string, string>();
                getData.Add("user_name", _username);
                getData.Add("password", _password);

                _adminHTML = NetworkUtils.SendRequest(bbox2Uri, null, getData);
                _gotAdminHTML = true;
            }
            return _adminHTML;
        }

        private string GetValueFromHTML(string html, string value)
        {
            //get firmware version
            int valueIndex = html.IndexOf("<td class=\"libelle\">" + value + "</td>");
            if (valueIndex > 0)
            {
                valueIndex = html.IndexOf("<td class=\"status\">", valueIndex);
                if (valueIndex > 0)
                {
                    valueIndex += "<td class=\"status\">".Length;
                    int valueIndexEnd = html.IndexOf("<", valueIndex);
                    return html.Substring(valueIndex, valueIndexEnd - valueIndex).Replace("&nbsp;", " ").Trim();
                }
            }
            return string.Empty;
        }

        public string FromHexString(string hexString)
        {
            var bytes = new byte[hexString.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);

            return Encoding.Unicode.GetString(bytes);
        }
    }
}