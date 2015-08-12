using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BBox3Tool
{
    public class DeviceInfo
    {
        private String _hardwareVersion;
        private String _firmwareVersion;
        private String _deviceUptime;
        private String _linkUptime;

        public string HardwareVersion
        {
            get { return _hardwareVersion; }
            set { _hardwareVersion = value; }
        }

        public string FirmwareVersion
        {
            get { return _firmwareVersion; }
            set { _firmwareVersion = value; }
        }

        public string DeviceUptime
        {
            get { return _deviceUptime; }
            set { _deviceUptime = value; }
        }

        public string LinkUptime
        {
            get { return _linkUptime; }
            set { _linkUptime = value; }
        }
    }
}
