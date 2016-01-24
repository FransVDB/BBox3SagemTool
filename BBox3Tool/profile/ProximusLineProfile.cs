using System.Collections.Generic;
using BBox3Tool.enums;

namespace BBox3Tool.profile
{
    public class ProximusLineProfile
    {
        //private members

        #region getters&setters
        
        /// <summary>
        /// Proximus profile number (LPXXX)
        /// </summary>
        public string LpName { get; set; }

        /// <summary>
        /// Name of the Proximus profile
        /// </summary>
        public string SpeedName { get; set; }

        /// <summary>
        /// Downloadspeed in kbps
        /// </summary>
        public int DownloadSpeed { get; set; }

        /// <summary>
        /// List of confirmed download bitrates (feedback from users)
        /// </summary>
        public List<int> ConfirmedDownloadSpeeds { get; set; }

        /// <summary>
        /// Uploadspeed in kbps
        /// </summary>
        public int UploadSpeed { get; set; }

        /// <summary>
        /// List of confirmed upload bitrates (feedback from users)
        /// </summary>
        public List<int> ConfirmedUploadSpeeds { get; set; }

        /// <summary>
        /// Indicated if this is a DLM profile or not 
        /// </summary>
        public bool? DlmProfile { get; set; }

        /// <summary>
        /// Indicated if this is a repair profile or not
        /// </summary>
        public bool? RepairProfile { get; set; }

        /// <summary>
        /// Indicated if this is a provisioning profile or not
        /// </summary>
        public bool? ProvisioningProfile { get; set; }

        /// <summary>
        /// Indicate if vectoring down is enabled or not
        /// </summary>
        public bool? VectoringDownDownEnabled { get; set; }

        /// <summary>
        /// Indicate if vectoring up is enabled or not
        /// </summary>
        public bool? VectoringUpEnabled { get; set; }

        /// <summary>
        /// VDSL2 profile (17a or 8d)
        /// </summary>
        public VDSL2Profile ProfileVDSL2 { get; set; }

        /// <summary>
        /// Minimum distance for this profile
        /// </summary>
        public decimal DistanceMin { get; set; }

        /// <summary>
        /// Maximum distance for this profile
        /// </summary>
        public decimal DistanceMax { get; set; }

        #endregion

        #region constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProximusLineProfile()
        {
            LpName = string.Empty;
            SpeedName = string.Empty;
            DownloadSpeed = 0;
            UploadSpeed = 0;
            DlmProfile = false;
            RepairProfile = false;
            ProvisioningProfile = false;
            VectoringDownDownEnabled = false;
            VectoringUpEnabled = false;
            ConfirmedDownloadSpeeds = new List<int>();
            ConfirmedUploadSpeeds = new List<int>();
            ProfileVDSL2 = VDSL2Profile.unknown;
            DistanceMax = 0;
            DistanceMin = 0;
        }

        /// <summary>
        /// Proximus line profile Constructor
        /// </summary>
        /// <param name="lpName">Line Profile number (LPXXX)</param>
        /// <param name="speedName">Profile speedname (100/20)</param>
        /// <param name="downloadSpeed">Downloadspeed in kbps</param>
        /// <param name="uploadSpeed">Uploadspeed in kbps</param>
        /// <param name="provisioning">Indicated if this is a provisioning profile or not</param>
        /// <param name="dlm">Indicated if this is a DLM profile or not</param>
        /// <param name="repair">Indicated if this is a repair profile or not</param>
        /// <param name="vectoringDown">Indicate if vectoring down is enabled or not</param>
        /// <param name="vectoringUp">Indicate if vectoring up is enabled or not</param>
        /// <param name="profile">VDSL2 profile (17a or 8d)</param>
        /// <param name="confirmedDownloadSpeeds">List of confirmed download speeds (feedback from users)</param>
        /// <param name="confirmedUploadSpeeds">List of confirmed upload speeds (feedback from users)</param>
        /// <param name="distanceMin">Minimim distance for this profile</param>
        /// <param name="distanceMax">Maximum distance for this profile</param>
        public ProximusLineProfile(string lpName, string speedName, int downloadSpeed, int uploadSpeed, bool? provisioning, bool? dlm, bool? repair, bool? vectoringDown, bool? vectoringUp, VDSL2Profile profile, List<int> confirmedDownloadSpeeds, List<int> confirmedUploadSpeeds, decimal distanceMin, decimal distanceMax)
        {
            LpName = lpName;
            SpeedName = speedName;
            DownloadSpeed =downloadSpeed;
            UploadSpeed = uploadSpeed;
            DlmProfile = dlm;
            RepairProfile = repair;
            ProvisioningProfile = provisioning;
            VectoringDownDownEnabled = vectoringDown;
            VectoringUpEnabled = vectoringUp;
            ProfileVDSL2 = profile;
            ConfirmedDownloadSpeeds = confirmedDownloadSpeeds;
            ConfirmedUploadSpeeds = confirmedUploadSpeeds;
            DistanceMin = distanceMin;
            DistanceMax = distanceMax;
        }
  
        #endregion

    }
}
