using System;
using System.Globalization;
using System.IO;
using MinimalisticTelnet;

namespace BBox3Tool
{
    internal class FritzBoxSession : IModemSession
    {
        private VDSL2Profile _vdslProfile;

        private TelnetConnection tc;

        public FritzBoxSession()
	    {
            DeviceName = "Fritz!Box 7390";
	    }

        public bool OpenSession(String host, String username, String password)
        {
            // New connection
            tc = new TelnetConnection(host, 23);

            // Login
            var passwordPrompt = tc.Read(200);
            if (passwordPrompt.Contains("password:"))
            {
                tc.WriteLine(password);
            }
            else
            {
                return false;
            }

            return true;
        }

        public bool CloseSession()
        {
            // Close session if still connected
            if (tc.IsConnected)
            {
                tc.WriteLine("^C");
                tc.WriteLine("exit");
            }

            // Kill socket
            tc.CloseConnection();

            return true;
        }

        public void GetLineData()
        {
            // Exec 'vdsl' command
            if (tc.Read(500).EndsWith("# "))
            {
                tc.WriteLine("vdsl");
            }

            // Wait for cpe prompt
            if (tc.Read(1000).EndsWith("cpe>"))
            {
                // Request extended port status
                tc.WriteLine("11");
            }

            // Read reply
            var extendedPortStatusReply = tc.Read(2000);
            if (extendedPortStatusReply.Contains("Far-end ITU Vendor Id"))
            {
                // Parse results
                ParsePortStatus(extendedPortStatusReply);
            }
            else
            {
                throw new Exception("Unable to read extended port status. Try rebooting the Fritz!Box.");
            }

            // Wait for cpe prompt
            if (extendedPortStatusReply.EndsWith("cpe> "))
            {
                // Request near-end SNR margin and attenuation
                tc.WriteLine("13");
            }

            // Read reply
            var getsnrReply = tc.Read(2000);
            if (getsnrReply.Contains("Attenuation"))
            {
                // Parse results
                ParseVdslSnr(getsnrReply);
            }
        }

        private void ParsePortStatus(String extendedPortStatus)
        {
            var reader = new StringReader(extendedPortStatus);
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
                            var dsAttenuation = array[1].Trim().Replace(" dB", "");
                            DownstreamAttenuation = Convert.ToDecimal(dsAttenuation, CultureInfo.InvariantCulture);
                            break;
                        case "Bandplan Type...........":
                            _vdslProfile = VDSL2Profile.p8d;
                            if (array[1].Trim().Equals("0"))
                            {
                                _vdslProfile = VDSL2Profile.p17a;
                            }
                            break;
                        case "Far-end ITU Vendor Id":
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
                            var dsNoiseMargin = array[1].Trim().Replace(" dB", "");
                            DownstreamNoiseMargin = Convert.ToDecimal(dsNoiseMargin, CultureInfo.InvariantCulture);
                            break;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        public DSLStandard GetDslStandard()
        {
            return DSLStandard.unknown;
        }

        public DeviceInfo GetDeviceInfo()
        {
            var deviceInfo = new DeviceInfo();
            return deviceInfo;
        }

        public string GetDebugValue(string debugValue)
        {
            return "Not implemented yet!";
        }

        public int DownstreamCurrentBitRate { get; private set; }

        public int UpstreamCurrentBitRate { get; private set; }

        public int DownstreamMaxBitRate { get; private set; }

        public int UpstreamMaxBitRate { get; private set; }

        public decimal DownstreamAttenuation { get; private set; }

        public decimal UpstreamAttenuation { get; private set; }

        public decimal DownstreamNoiseMargin { get; private set; }

        public decimal UpstreamNoiseMargin { get; private set; }

        public decimal Distance { get; private set; }

        public string DeviceName { get; private set; }
    }
}