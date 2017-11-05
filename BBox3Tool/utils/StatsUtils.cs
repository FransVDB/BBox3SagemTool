using BBox3Tool.enums;

namespace BBox3Tool.utils
{
    public static class StatsUtils
    {
        /// <summary>
        /// Get VDSL2 profile, derived from current sync
        /// </summary>
        /// <param name="downstreamCurrentBitRate">Current downstream bitrate</param>
        /// <param name="upstreamCurrentBitRate">Current upstream bitrate</param>
        /// <returns>VDSL2 profile, if it could be derived</returns>
        public static VDSL2Profile GetVdsl2ProfileFallBack(int downstreamCurrentBitRate, int upstreamCurrentBitRate, decimal downStreamAttenuation, bool vectoringActive)
        {
            //check current bitrate
            if (downstreamCurrentBitRate >= 65000)
                return VDSL2Profile.p17a;
            else
                if (upstreamCurrentBitRate >= 9500)
                return VDSL2Profile.p17a;

            //check attenuation: longer lines = 8b / 8d
            if (downStreamAttenuation >= 22)
            {
                if (vectoringActive)
                    return VDSL2Profile.p8b;
                else
                    return VDSL2Profile.p8d;
            }

            //no way of knowing for sure
            return VDSL2Profile.unknown;
        }

    }
}
