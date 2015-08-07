using BBox3Tool.objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Xml;

namespace BBox3Tool.utils
{
    public class SettingsUtils
    {
        /// <summary>
        /// Save tool settings in extrernal xml file
        /// </summary>
        /// <param name="settings">Tool settings (username, password, devide, host)</param>
        public static void saveSettings(ToolSettings settings)
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
            settingsDoc.SelectSingleNode("//document/login/ip").InnerText = settings.Host;
            settingsDoc.SelectSingleNode("//document/login/user").InnerText = settings.Username;

            try
            {
                settingsDoc.SelectSingleNode("//document/login/password").InnerText = Crypto.EncryptStringAES(settings.Password, NetworkInterface.GetAllNetworkInterfaces().First().GetPhysicalAddress().ToString());
            }
            catch { }

            switch (settings.Device)
            {
                case Device.BBOX3S:
                    settingsDoc.SelectSingleNode("//document/login/device").InnerText = "BBOX3S";
                    break;
                case Device.BBOX2:
                    settingsDoc.SelectSingleNode("//document/login/device").InnerText = "BBOX2";
                    break;
                case Device.FritzBox7390:
                    settingsDoc.SelectSingleNode("//document/login/device").InnerText = "FRITZBOX";
                    break;
                case Device.unknown:
                case Device.BBOX3T:
                default:
                    break;
            }
            settingsDoc.Save("BBox3Tool.settings.xml");
        }

        /// <summary>
        /// Load settings form external xml file
        /// </summary>
        /// <returns>Toolsettings, or null if settings could not be loaded</returns>
        public static ToolSettings loadSettings()
        {
            ToolSettings settings = new ToolSettings();
            //load xml doc
            try
            {
                if (File.Exists("BBox3Tool.settings.xml"))
                {
                    XmlDocument settingsDoc = new XmlDocument();
                    settingsDoc.Load("BBox3Tool.settings.xml");

                    //only support settings v1.0
                    if (settingsDoc.SelectSingleNode("//document/version").InnerText != "1.0")
                        return null;

                    settings.Host = settingsDoc.SelectSingleNode("//document/login/ip").InnerText;
                    settings.Username = settingsDoc.SelectSingleNode("//document/login/user").InnerText;
                    try
                    {
                        settings.Password = Crypto.DecryptStringAES(settingsDoc.SelectSingleNode("//document/login/password").InnerText, NetworkInterface.GetAllNetworkInterfaces().First().GetPhysicalAddress().ToString());
                    }
                    catch (Exception) { }
                    switch (settingsDoc.SelectSingleNode("//document/login/device").InnerText)
                    {
                        case "BBOX3S":
                            settings.Device = Device.BBOX3S;
                            break;
                        case "BBOX2":
                            settings.Device = Device.BBOX2;
                            break;
                        case "FRITZBOX":
                            settings.Device = Device.FritzBox7390;
                            break;
                        default:
                            settings.Device = Device.unknown;
                            break;
                    }
                    return settings;
                }
            }
            catch (Exception) { }
            return null;
        }

        /// <summary>
        /// Delete the tool settings
        /// </summary>
        public static void deleteSettings()
        {
            if (File.Exists("BBox3Tool.settings.xml"))
            {
                try
                {
                    File.Delete("BBox3Tool.settings.xml");
                }
                catch { }
            }
        }
    }
}
