using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
        private bool _gotWebLog;
        private bool _gotUptime;
        private string _adminHTML;
        private string _webLog;
        private string _uptimeLog;

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
                return (GetAdminHTML().IndexOf("NOT CONNECTED", StringComparison.Ordinal) == -1);
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

        public bool OpenSession(string host, string username, string password)
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
                var passwordPrompt = _tc.Read(500);
                if (passwordPrompt.Contains("Password:"))
                {
                    _tc.WriteLine(password);
                }
                else
                {
                    return false;
                }

                //check login successfull
                var result = _tc.Read(500);
                if (result.ToLower().Contains("admin @ home"))
                {
                    _host = host;
                    _username = username;
                    _password = password;

                    // Exec 'shell' command
                    _tc.WriteLine("shell");
                    _tc.Read(500);
                    return true;
                }

                return false;
            }
            catch
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
            _gotWebLog = false;
            _gotAdminHTML = false;
            _gotUptime = false;

            GetAdminHTML();
            GetWebLog();
        }

        public void GetLineData()
        {
            try
            {
                //get vdsl stats
                //--------------
                _tc.WriteLine("vdsl pstatex");
                var pstatexReply = _tc.Read(2000);
                if (pstatexReply.Contains("Far-end ITU Vendor Id"))
                {
                    //vdsl command successfull, so line obviously vdsl2
                    DSLStandard = DSLStandard.VDSL2;
                    // Parse results
                    ParsePstatex(pstatexReply);
                }
                else
                    throw new Exception("Unable to read extended port status.");

                //get SNR and ATTN
                //----------------
                _tc.WriteLine("vdsl getsnr");
                var getsnrReply = _tc.Read(2000);
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
            var deviceInfo = new DeviceInfo
            {
                DeviceUptime = "unknown",
                LinkUptime = "unknown",
                HardwareVersion = "unknown",
                FirmwareVersion = "unknown"
            };

            try
            {
                //get hardware and firmware versions
                string homePage = GetAdminHTML();
                deviceInfo.HardwareVersion = GetValueFromHTML(homePage, "Hardware Version");
                deviceInfo.FirmwareVersion = GetValueFromHTML(homePage, "Runtime Code Version");

                //get device uptime
                var uptime = GetUptime();
                deviceInfo.DeviceUptime = GetDeviceUptime(uptime);

                //get link uptime
                var dhcpLog = GetWebLog();
                deviceInfo.LinkUptime = GetLinkUptime(dhcpLog);

            }
            catch
            {
                // ignored
            }
            return deviceInfo;
        }

        public string GetDebugValue(string debugValue)
        {
            return "Not implemented yet!";
        }

        //private functions

        private void ParsePstatex(string pstatex)
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
                            var dsCurrentBitRate = array[1].Replace("kbps", "").Trim();
                            DownstreamCurrentBitRate = Convert.ToInt32(dsCurrentBitRate);
                            break;
                        case "Bearer1 Upstream payload rate":
                            var usCurrentBitRate = array[1].Replace("kbps", "").Trim();
                            UpstreamCurrentBitRate = Convert.ToInt32(usCurrentBitRate);
                            break;
                        case "Downstream attainable payload rate":
                            var dsMaxBitRate = array[1].Replace("kbps", "").Trim();
                            DownstreamMaxBitRate = Convert.ToInt32(dsMaxBitRate);
                            break;
                        /*case "Upstream line rate":
                            var usMaxBitRate = array[1].Replace("kbps", "").Trim();
                            UpstreamMaxBitRate = Convert.ToInt32(usMaxBitRate);
                            break;*/
                        case "Downstream Training Margin":
                            var dsNoiseMargin = array[1].Trim().Replace(" dB", "");
                            DownstreamNoiseMargin = Convert.ToDecimal(dsNoiseMargin, CultureInfo.InvariantCulture);
                            break;
                        case "Bandplan Type...........":
                            _vdslProfile = VDSL2Profile.p8d;
                            if (array[1].Trim().Equals("0"))
                                _vdslProfile = VDSL2Profile.p17a;
                            break;
                        case "VDSL Estimated Loop Length ":
                            var loopLength = array[1].Trim().Replace("ft", "").Trim();
                            Distance = Convert.ToDecimal(loopLength, CultureInfo.InvariantCulture) * 0.3048m;
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

        private void ParseVdslSnr(string snr)
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

        private string GetDeviceUptime(string uptime)
        {
            uptime = uptime.ToLower().Trim(); //uptime\r\r\n 14:35:12 up 12 days,  2:01, load average: 0.16, 0.03, 0.01\r\r\n# 

            int start = uptime.IndexOf("up ") + "up ".Length;
            int end = uptime.IndexOf(", load");
            uptime = uptime.Substring(start, end - start); //12 days,  2:01
            string[] time = uptime.Substring(uptime.IndexOf("days, ") + "days, ".Length).Trim().Split(':');

            int days = Convert.ToInt32(uptime.Substring(0, uptime.IndexOf("days")).Trim()); //12
            int hours = 0;
            int minutes = 0;
            if (time.Length == 2)
            {
                hours = Convert.ToInt32(time[0]);
                minutes = Convert.ToInt32(time[1]);
            }
            if (time.Length == 1)
                minutes = Convert.ToInt32(time[0].Replace("min", "").Trim());

            double totalSeconds = 0;
            totalSeconds += (days * 24 * 60 * 60);
            totalSeconds += (hours * 60 * 60);
            totalSeconds += (minutes * 60);
            TimeSpan deviceUptime = TimeSpan.FromSeconds(totalSeconds);

            return deviceUptime.ToString("%d") + (deviceUptime.Days == 1 ? " day " : " days ") + deviceUptime.ToString("h\\:mm\\:ss");
        }

        private string GetLinkUptime(string log)
        {
            //Wed, 01 Jan 2003 01:01:30 GMT IP=192.168.1.4 MAC=00:21:6b:14:d4:da
            //Wed, 01 Jan 2003 01:00:41 GMT IP = 192.168.1.2 MAC = 3c: cd: 93:cc: 91:53

            CultureInfo enUs = new CultureInfo("en-US");

            List<string> loglines = log.Split('\n').ToList();
            List<DateTime> pppoe = loglines.Where(x => x.Contains("PPPOE: session established")).Select(x => DateTime.Parse(x.Substring(0, x.IndexOf("GMT")).Trim(), enUs)).ToList();

            if (pppoe.Count > 0)
            {
                TimeSpan linkUptime = DateTime.Now - pppoe.First();
                return linkUptime.ToString("%d") + (linkUptime.Days == 1 ? " day " : " days ") + linkUptime.ToString("h\\:mm\\:ss");
            }
            return "unknown";
        }

        private string GetAdminHTML()
        {
            if (_gotAdminHTML)
                return _adminHTML;

            Uri bbox2Uri = new Uri("http://" + _host);
            Dictionary<string, string> getData = new Dictionary<string, string>
            {
                {"user_name", _username},
                {"password", _password}
            };
            _adminHTML = NetworkUtils.SendRequest(bbox2Uri, null, getData);
            _gotAdminHTML = true;

            return _adminHTML;
        }

        private string GetWebLog()
        {
            if (_gotWebLog)
                return _webLog;

            Uri bbox2Uri = new Uri("http://" + _host + "/WebGui.txt");
            _webLog = NetworkUtils.SendRequest(bbox2Uri);

            _gotWebLog = true;

            return _webLog;
        }

        private string GetUptime()
        {
            if (_gotUptime)
                return _uptimeLog;

            _tc.WriteLine("uptime");
            _uptimeLog = _tc.Read(500);

            _gotUptime = true;

            return _uptimeLog;
        }

        private string GetValueFromHTML(string html, string value)
        {
            //get firmware version
            int valueIndex = html.IndexOf("<td class=\"libelle\">" + value + "</td>", StringComparison.Ordinal);
            if (valueIndex > 0)
            {
                valueIndex = html.IndexOf("<td class=\"status\">", valueIndex, StringComparison.Ordinal);
                if (valueIndex > 0)
                {
                    valueIndex += "<td class=\"status\">".Length;
                    int valueIndexEnd = html.IndexOf("<", valueIndex, StringComparison.Ordinal);
                    return html.Substring(valueIndex, valueIndexEnd - valueIndex).Replace("&nbsp;", " ").Trim();
                }
            }
            return string.Empty;
        }

        private string FromHexString(string hexString)
        {
            hexString = hexString.Trim().ToLower().Replace("0x", "");
            try
            {
                var bytes = new byte[hexString.Length / 2];
                for (var i = 0; i < bytes.Length; i++)
                    bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);

                return Encoding.Default.GetString(bytes);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}