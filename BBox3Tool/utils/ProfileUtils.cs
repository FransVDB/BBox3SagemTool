using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using BBox3Tool.enums;
using BBox3Tool.profile;

namespace BBox3Tool.utils
{
    public class ProfileUtils
    {
        /// <summary>
        /// Load Proximus line profiles from embedded resource
        /// </summary>
        /// <returns>Proximus line profiles</returns>
        public static List<ProximusLineProfile> LoadEmbeddedProfiles()
        {
            //load xml doc
            XmlDocument profilesDoc = new XmlDocument();
            using (Stream stream = typeof(Form1).Assembly.GetManifestResourceStream("BBox3Tool.profile.profiles.xml"))
            {
                if (stream == null)
                    throw new Exception("BBox3Tool.profile.profiles.xml not found in resources.");

                using (StreamReader sr = new StreamReader(stream))
                {
                    profilesDoc.LoadXml(sr.ReadToEnd());
                }
            }

            //run trough all xml profiles
            return LoadProfilesFromXml(profilesDoc);
        }

        /// <summary>
        /// Load Proximus line profiles from xml document
        /// </summary>
        /// <param name="xmlDoc">Xml document that contains the profiles</param>
        /// <returns>Proximus line profiles</returns>
        public static List<ProximusLineProfile> LoadProfilesFromXml(XmlDocument xmlDoc)
        {
            //run trough all xml profiles
            List<ProximusLineProfile> listProfiles = new List<ProximusLineProfile>();

            XmlNodeList nodeProfiles = xmlDoc.SelectNodes("//document/profiles/profile");
            if (nodeProfiles != null)
            {
                foreach (XmlNode profileNode in nodeProfiles)
                {
                    List<int> confirmedDownloadList = new List<int>();
                    List<int> confirmedUploadList = new List<int>();

                    XmlNodeList nodeConfirmed = profileNode.SelectNodes("confirmed");
                    if (nodeConfirmed != null)
                    {
                        foreach (XmlNode confirmedNode in nodeConfirmed)
                        {
                            if (confirmedNode.Attributes != null)
                            {
                                confirmedDownloadList.Add(Convert.ToInt32(confirmedNode.Attributes["down"].Value));
                                confirmedUploadList.Add(Convert.ToInt32(confirmedNode.Attributes["up"].Value));
                            }
                        }
                    }

                    XmlNodeList nodeOfficial = profileNode.SelectNodes("official");
                    if (nodeOfficial != null)
                    {
                        XmlAttributeCollection attributesOfficial = nodeOfficial[0].Attributes;
                        if (attributesOfficial != null)
                        {
                            confirmedDownloadList.Add(Convert.ToInt32(attributesOfficial["down"].Value));
                            confirmedUploadList.Add(Convert.ToInt32(attributesOfficial["up"].Value));
                        }
                    }

                    if (profileNode.Attributes != null)
                    {
                        ProximusLineProfile profile = new ProximusLineProfile(
                            profileNode.Attributes["name"].Value, 
                            profileNode.Attributes["speedname"].Value, 
                            confirmedDownloadList.Last(),
                            confirmedUploadList.Last(),
                            Convert.ToBoolean(profileNode.Attributes["provisioning"].Value),
                            Convert.ToBoolean(profileNode.Attributes["dlm"].Value),
                            Convert.ToBoolean(profileNode.Attributes["repair"].Value),
                            Convert.ToBoolean(profileNode.Attributes["vectoring"].Value),
                            Convert.ToBoolean(profileNode.Attributes["vectoring-up"].Value),
                            (VDSL2Profile) Enum.Parse(typeof (VDSL2Profile), "p" + profileNode.Attributes["vdsl2"].Value),
                            confirmedDownloadList.Distinct().ToList(),
                            confirmedUploadList.Distinct().ToList(),
                            Convert.ToDecimal(profileNode.Attributes["min"].Value, CultureInfo.InvariantCulture),
                            Convert.ToDecimal(profileNode.Attributes["max"].Value, CultureInfo.InvariantCulture));

                        listProfiles.Add(profile);
                    }
                }
            }
            return listProfiles;
        }

        /// <summary>
        /// Get Proximus line profile 
        /// </summary>
        /// <param name="profiles">Proximus line profiles to choose from</param>
        /// <param name="uploadSpeed">Upload speed of current session</param>
        /// <param name="downloadSpeed">Download speed of current session</param>
        /// <param name="vectoringDownEnabled">Vectoring down enabled/disabled or null for Unknown</param>µ
        /// <param name="vectoringUpEnabled">Vectoring up enabled/disabled or null for Unknown</param>
        /// <param name="distance">Distance or null for Unknown</param>
        /// <returns>Proximus line profile of the current session</returns>
        public static ProximusLineProfile GetProfile(List<ProximusLineProfile> profiles, int uploadSpeed, int downloadSpeed, bool vectoringDownEnabled, bool vectoringUpEnabled, decimal? distance)
        {
            lock (profiles)
            {
                //check if speed matches with confirmed speeds
                /*List<ProximusLineProfile> confirmedMatches = _profiles.Where(x => x.ConfirmedDownloadSpeeds.Contains(downloadSpeed) && x.ConfirmedUploadSpeeds.Contains(uploadSpeed)).ToList();

                //if vectoringstatus could be determined, filter on vectoring
                if (vectoringEnabled != null)
                    confirmedMatches = confirmedMatches.Where(x => x.VectoringEnabled == vectoringEnabled).ToList();

                //1 match found
                if (confirmedMatches.Count == 1)
                    return confirmedMatches.First();

                //multiple matches found
                if (confirmedMatches.Count > 1)
                {
                    //get profile with closest distance
                    if (distance != null)
                    {
                        return confirmedMatches
                          .Select(x => new { x, diffDistance = (distance - x.DistanceMin), diffSpeed = Math.Abs(x.DownloadSpeed - downloadSpeed) })
                          .OrderBy(p => p.diffDistance < 0)
                          .ThenBy(p => p.diffDistance)
                          .ThenBy(p => p.diffSpeed)
                          .First().x;
                    }
                    //get profile with closest official download speed
                    else
                        return confirmedMatches.Select(x => new { x, diff = Math.Abs(x.DownloadSpeed - downloadSpeed) })
                          .OrderBy(p => p.diff)
                          .First().x;
                }*/

                //get profiles with closest speeds in range of 512kb
                List<ProximusLineProfile> rangeMatches = profiles.Select(x => new { x, diffDownload = Math.Abs(x.DownloadSpeed - downloadSpeed), diffUpload = Math.Abs(x.UploadSpeed - uploadSpeed) })
                    .Where(x => x.diffDownload <= 512 && x.diffUpload <= 512)
                    .OrderBy(p => p.diffDownload)
                    .ThenBy(p => p.diffUpload)
                    .Select(y => y.x)
                    .ToList();

                //check on vectoring
                rangeMatches = rangeMatches
                    .Where(x => x.VectoringDownDownEnabled == vectoringDownEnabled)
                    .OrderBy(x => x.VectoringUpEnabled == vectoringUpEnabled).ToList();

                //check on distance
                if (distance != null)
                    rangeMatches = rangeMatches
                        .Select(x => new { x, diffDistance = (distance - x.DistanceMin), diffSpeed = Math.Abs(x.DownloadSpeed - downloadSpeed) })
                        .OrderBy(p => p.diffDistance < 0)
                        .ThenBy(p => p.diffDistance)
                        .Select(y => y.x)
                        .ToList();

                //check matches found
                if (rangeMatches.Count > 0)
                {
                    if (rangeMatches.Count == 1)
                        return rangeMatches.First();
                    else
                    { 
                        //multiple found, check which values are certain

                        //provisioning
                        bool? provisioning = null;
                        if (rangeMatches.GroupBy(x => x.ProvisioningProfile).Count() == 1)
                            provisioning = rangeMatches.First().ProvisioningProfile;

                        //dlm
                        bool? dlm = null;
                        if (rangeMatches.GroupBy(x => x.DlmProfile).Count() == 1)
                            dlm = rangeMatches.First().DlmProfile;

                        //repair
                        bool? repair = null;
                        if (rangeMatches.GroupBy(x => x.RepairProfile).Count() == 1)
                            repair = rangeMatches.First().RepairProfile;

                        //vectoring
                        bool? vectoring = null;
                        if (rangeMatches.GroupBy(x => x.VectoringDownDownEnabled).Count() == 1)
                            vectoring = rangeMatches.First().VectoringDownDownEnabled;
                        bool? vectoringUp = null;
                        if (rangeMatches.GroupBy(x => x.VectoringUpEnabled).Count() == 1)
                            vectoringUp = rangeMatches.First().VectoringUpEnabled;

                        //vdsl profile
                        VDSL2Profile vdsl2Profile = VDSL2Profile.unknown;
                        if (rangeMatches.GroupBy(x => x.ProfileVDSL2).Count() == 1)
                            vdsl2Profile = rangeMatches.First().ProfileVDSL2;
                        
                        //LPXXX
                        string lp = "unknown";
                        if (rangeMatches.GroupBy(x => x.LpName).Count() == 1)
                            lp = rangeMatches.First().LpName;

                        //Name (100/20)
                        string name = "unknown";
                        if (rangeMatches.GroupBy(x => x.SpeedName).Count() == 1)
                            name = rangeMatches.First().SpeedName;

                        return new ProximusLineProfile(lp, name, downloadSpeed, uploadSpeed, provisioning, dlm, repair, vectoring, vectoringUp, vdsl2Profile, new List<int>(), new List<int>(), 0, 0);
                    }
                }
                    
            }

            //no matches found
            return null;
        }

    }
}