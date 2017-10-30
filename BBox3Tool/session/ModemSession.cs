using System;
using BBox3Tool.enums;
using BBox3Tool.objects;

namespace BBox3Tool.session
{
    interface IModemSession
    {
        bool OpenSession(String host, String username, String password);

        bool CloseSession();

        void RefreshData();

        void GetLineData();

        DeviceInfo GetDeviceInfo();

        String GetDebugValue(String debugValue);

        //getters

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
        bool VectoringDown { get; }
        bool VectoringUp { get; }
        bool VectoringDeviceCapable { get; }
        bool VectoringROPCapable { get; }
        bool LineConnected { get; }
        DSLStandard DSLStandard { get; }
        Annex Annex { get; }
    }
}
