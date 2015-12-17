using System.Collections.Generic;
using BBox3Tool.enums;

namespace BBox3Tool.profile
{
    public class ProximusLineProfile
    {
        //private members
        private readonly string _name;
        private readonly int _downloadSpeed;
        private readonly int _uploadSpeed;
        private readonly List<int> _confirmedDownloadSpeeds;
        private readonly List<int> _confirmedUploadSpeeds;
        private readonly bool? _dlmProfile;
        private readonly bool? _repairProfile;
        private readonly bool? _provisioningProfile;
        private readonly bool? _vectoringDownDownEnabled;
        private readonly bool? _vectoringUpEnabled;
        private readonly VDSL2Profile _profileVDSL2;
        private readonly decimal _distanceMin;
        private readonly decimal _distanceMax;

        #region getters&setters

        /// <summary>
        /// Name of the Proximus profile
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Downloadspeed in kbps
        /// </summary>
        public int DownloadSpeed
        {
            get { return _downloadSpeed; }
        }

        /// <summary>
        /// List of confirmed download bitrates (feedback from users)
        /// </summary>
        public List<int> ConfirmedDownloadSpeeds
        {
            get { return _confirmedDownloadSpeeds; }
        }

        /// <summary>
        /// Uploadspeed in kbps
        /// </summary>
        public int UploadSpeed
        {
            get { return _uploadSpeed; }
        }

        /// <summary>
        /// List of confirmed upload bitrates (feedback from users)
        /// </summary>
        public List<int> ConfirmedUploadSpeeds
        {
            get { return _confirmedUploadSpeeds; }
        }

        /// <summary>
        /// Indicated if this is a DLM profile or not 
        /// </summary>
        public bool? DlmProfile
        {
            get { return _dlmProfile; }
        }
       
        /// <summary>
        /// Indicated if this is a repair profile or not
        /// </summary>
        public bool? RepairProfile
        {
            get { return _repairProfile; }
        }

        /// <summary>
        /// Indicated if this is a provisioning profile or not
        /// </summary>
        public bool? ProvisioningProfile
        {
            get { return _provisioningProfile; }
        }

        /// <summary>
        /// Indicate if vectoring down is enabled or not
        /// </summary>
        public bool? VectoringDownDownEnabled
        {
            get { return _vectoringDownDownEnabled; }
        }

        /// <summary>
        /// Indicate if vectoring up is enabled or not
        /// </summary>
        public bool? VectoringUpEnabled
        {
            get { return _vectoringUpEnabled; }
        }

        /// <summary>
        /// VDSL2 profile (17a or 8d)
        /// </summary>
        public VDSL2Profile ProfileVDSL2
        {
            get { return _profileVDSL2; }
        }

        /// <summary>
        /// Minimum distance for this profile
        /// </summary>
        public decimal DistanceMin
        {
            get { return _distanceMin; }
        }

        /// <summary>
        /// Maximum distance for this profile
        /// </summary>
        public decimal DictanceMax
        {
            get { return _distanceMax; }
        }

        #endregion

        #region constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProximusLineProfile() {
            _name = string.Empty;
            _downloadSpeed = 0;
            _uploadSpeed = 0;
            _dlmProfile = false;
            _repairProfile = false;
            _provisioningProfile = false;
            _vectoringDownDownEnabled = false;
            _vectoringUpEnabled = false;
            _confirmedDownloadSpeeds = new List<int>();
            _confirmedUploadSpeeds = new List<int>();
            _profileVDSL2 = VDSL2Profile.unknown;
            _distanceMax = 0;
            _distanceMin = 0;
        }

        /// <summary>
        /// Proximus line profile Constructor
        /// </summary>
        /// <param name="name"></param>
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
        public ProximusLineProfile(string name, int downloadSpeed, int uploadSpeed, bool? provisioning, bool? dlm, bool? repair, bool? vectoringDown, bool? vectoringUp, VDSL2Profile profile, List<int> confirmedDownloadSpeeds, List<int> confirmedUploadSpeeds, decimal distanceMin, decimal distanceMax)
        {
            _name = name;
            _downloadSpeed =downloadSpeed;
            _uploadSpeed = uploadSpeed;
            _dlmProfile = dlm;
            _repairProfile = repair;
            _provisioningProfile = provisioning;
            _vectoringDownDownEnabled = vectoringDown;
            _vectoringUpEnabled = vectoringUp;
            _profileVDSL2 = profile;
            _confirmedDownloadSpeeds = confirmedDownloadSpeeds;
            _confirmedUploadSpeeds = confirmedUploadSpeeds;
            _distanceMin = distanceMin;
            _distanceMax = distanceMax;
        }
  
        #endregion

    }
}
