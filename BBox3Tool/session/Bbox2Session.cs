using System;
using System.Globalization;
using System.IO;
using MinimalisticTelnet;

namespace BBox3Tool
{
    internal class Bbox2Session : IModemSession
    {
        private VDSL2Profile _vdslProfile;
        private TelnetConnection tc;

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
        public bool? Vectoring { get; private set; }
        public DSLStandard DSLStandard { get; private set; }

        public Bbox2Session ()
	    {
            DeviceName = "B-Box 2";
            Distance = null;
            Vectoring = false;
	    }

        public bool OpenSession(String host, String username, String password)
        {
            try
            {
                // New connection
                tc = new TelnetConnection(host, 23);

                // Login
                var usernamePrompt = tc.Read(2000);
                if (usernamePrompt.Contains("login:"))
                {
                    tc.WriteLine(username);
                }
                else
                {
                    return false;
                }
                var passwordPrompt = tc.Read(200);
                if (passwordPrompt.Contains("Password:"))
                {
                    tc.WriteLine(password);
                }
                else
                {
                    return false;
                }

                //check login successfull
                var result = tc.Read(200);
                if (result.ToLower().Contains("admin @ home"))
                    return true;

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
            // Exec 'shell' command
            if (tc.Read(500).EndsWith("$ "))
            {
                tc.WriteLine("shell");
            }

            // Wait for shell prompt
            if (tc.Read(1000).EndsWith("# "))
            {
                // Send 'vdsl pstatex' command
                tc.WriteLine("vdsl pstatex");
            }

            // Read reply
            var pstatexReply = tc.Read(1000);
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
                tc.WriteLine("vdsl getsnr");
            }

            // Read reply
            var getsnrReply = tc.Read(1000);
            if (getsnrReply.Contains("Attenuation"))
            {
                // Parse results
                ParseVdslSnr(getsnrReply);
            }
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
                            if (array[1].Trim() == "0x00800000#" || array[1].Trim() == "0x04000000#")
                                DSLStandard = DSLStandard.VDSL2;
                            else
                                DSLStandard = DSLStandard.unknown;
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
    }
}