using BBox3Tool.objects;
using System;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Xml;
using BBox3Tool.enums;

namespace BBox3Tool.utils
{
    public class SettingsUtils
    {
        /// <summary>
        /// Save tool settings in extrernal xml file
        /// </summary>
        /// <param name="settings">Tool settings (username, password, devide, host)</param>
        public static void SaveSettings(ToolSettings settings)
        {
            //load xml doc
            XmlDocument settingsDoc = new XmlDocument();
            using (Stream stream = typeof(Form1).Assembly.GetManifestResourceStream("BBox3Tool.settings.xml"))
            {
                if (stream == null)
                    throw new Exception("BBox3Tool.settings.xml not found in resources.");
                using (StreamReader sr = new StreamReader(stream))
                {
                    settingsDoc.LoadXml(sr.ReadToEnd());
                }
            }

            XmlNode nodeLoginIp = settingsDoc.SelectSingleNode("//document/login/ip");
            XmlNode nodeLoginUser = settingsDoc.SelectSingleNode("//document/login/user");
            XmlNode nodeLoginPassword = settingsDoc.SelectSingleNode("//document/login/password");
            XmlNode nodeLoginDevice = settingsDoc.SelectSingleNode("//document/login/device");

            if (nodeLoginIp != null)
                nodeLoginIp.InnerText = settings.Host;
            if (nodeLoginUser != null)
                nodeLoginUser.InnerText = settings.Username;

            try
            {
                if (nodeLoginPassword != null)
                    nodeLoginPassword.InnerText = Crypto.EncryptStringAES(settings.Password, NetworkInterface.GetAllNetworkInterfaces().First().GetPhysicalAddress().ToString());
            }
            catch
            {
                // ignored
            }

            if (nodeLoginDevice != null)
            {
                switch (settings.Device)
                {
                    case Device.BBOX3S:
                        nodeLoginDevice.InnerText = "BBOX3S";
                        break;
                    case Device.BBOX2:
                        nodeLoginDevice.InnerText = "BBOX2";
                        break;
                    case Device.FritzBox7390:
                        nodeLoginDevice.InnerText = "FRITZBOX";
                        break;
                }
            }

            settingsDoc.Save("BBox3Tool.settings.xml");
        }

        /// <summary>
        /// Load settings form external xml file
        /// </summary>
        /// <returns>Toolsettings, or null if settings could not be loaded</returns>
        public static ToolSettings LoadSettings()
        {
            ToolSettings settings = new ToolSettings();
            //load xml doc
            try
            {
                if (File.Exists("BBox3Tool.settings.xml"))
                {
                    XmlDocument settingsDoc = new XmlDocument();
                    settingsDoc.Load("BBox3Tool.settings.xml");

                    XmlNode nodeVersion = settingsDoc.SelectSingleNode("//document/version");

                    //only support settings v1.0
                    if (nodeVersion != null && nodeVersion.InnerText != "1.0")
                        return null;

                    XmlNode nodeLoginIp = settingsDoc.SelectSingleNode("//document/login/ip");
                    XmlNode nodeLoginUser = settingsDoc.SelectSingleNode("//document/login/user");
                    XmlNode nodeLoginPassword = settingsDoc.SelectSingleNode("//document/login/password");
                    XmlNode nodeLoginDevice = settingsDoc.SelectSingleNode("//document/login/device");

                    if (nodeLoginIp != null) 
                        settings.Host = nodeLoginIp.InnerText;
                    if (nodeLoginUser != null) 
                        settings.Username = nodeLoginUser.InnerText;

                    try
                    {
                        if (nodeLoginPassword != null)
                            settings.Password = Crypto.DecryptStringAES(nodeLoginPassword.InnerText, NetworkInterface.GetAllNetworkInterfaces().First().GetPhysicalAddress().ToString());
                    }
                    catch (Exception)
                    {
                        // ignored
                    }

                    if (nodeLoginDevice != null)
                    {
                        switch (nodeLoginDevice.InnerText)
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
                                settings.Device = Device.Unknown;
                                break;
                        }
                    }

                    return settings;
                }
            }
            catch (Exception)
            {
                // ignored
            }
            return null;
        }

        /// <summary>
        /// Delete the tool settings
        /// </summary>
        public static void DeleteSettings()
        {
            if (File.Exists("BBox3Tool.settings.xml"))
            {
                try
                {
                    File.Delete("BBox3Tool.settings.xml");
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}
