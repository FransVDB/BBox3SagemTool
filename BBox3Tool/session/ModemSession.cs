using System;

namespace BBox3Tool
{
    interface IModemSession
    {
        bool OpenSession(String host, String username, String password);
        bool CloseSession();
        
        void GetLineData();

        DeviceInfo GetDeviceInfo();
        String GetDebugValue(String debugValue);

        int DownstreamCurrentBitRate { get; }
        int UpstreamCurrentBitRate { get; }
        int DownstreamMaxBitRate { get; }
        int UpstreamMaxBitRate { get; }
        decimal DownstreamAttenuation { get; }
        decimal UpstreamAttenuation { get; }
        decimal DownstreamNoiseMargin { get; }
        decimal UpstreamNoiseMargin { get; }
        decimal? Distance { get; }
        string DeviceName { get; }
        bool? Vectoring { get; }
        DSLStandard DSLStandard { get; }
    }
}
