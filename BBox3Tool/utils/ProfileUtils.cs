using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;

namespace BBox3Tool.utils
{
    public class ProfileUtils
    {
        /// <summary>
        /// Load Proximus line profiles from embedded resource
        /// </summary>
        /// <returns>Proximus line profiles</returns>
        public static List<ProximusLineProfile> loadEmbeddedProfiles()
        {
            //load xml doc
            XmlDocument profilesDoc = new XmlDocument();
            using (Stream stream = typeof(Form1).Assembly.GetManifestResourceStream("BBox3Tool.profile.profiles.xml"))
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    profilesDoc.LoadXml(sr.ReadToEnd());
                }
            }

            //run trough all xml profiles
            return loadProfilesFromXML(profilesDoc);
        }

        /// <summary>
        /// Load Proximus line profiles from xml document
        /// </summary>
        /// <param name="xmlDoc">Xml document that contains the profiles</param>
        /// <returns>Proximus line profiles</returns>
        public static List<ProximusLineProfile> loadProfilesFromXML(XmlDocument xmlDoc)
        {
            //run trough all xml profiles
            List<ProximusLineProfile> listProfiles = new List<ProximusLineProfile>();
            foreach (XmlNode profileNode in xmlDoc.SelectNodes("//document/profiles/profile"))
            {
                List<int> confirmedDownloadList = new List<int>();
                List<int> confirmedUploadList = new List<int>();
                foreach (XmlNode confirmedNode in profileNode.SelectNodes("confirmed"))
                {
                    confirmedDownloadList.Add(Convert.ToInt32(confirmedNode.Attributes["down"].Value));
                    confirmedUploadList.Add(Convert.ToInt32(confirmedNode.Attributes["up"].Value));
                }
                confirmedDownloadList.Add(Convert.ToInt32(profileNode.SelectNodes("official")[0].Attributes["down"].Value));
                confirmedUploadList.Add(Convert.ToInt32(profileNode.SelectNodes("official")[0].Attributes["up"].Value));

                ProximusLineProfile profile = new ProximusLineProfile(
                    profileNode.Attributes["name"].Value,
                    confirmedDownloadList.Last(),
                    confirmedUploadList.Last(),
                    Convert.ToBoolean(profileNode.Attributes["provisioning"].Value),
                    Convert.ToBoolean(profileNode.Attributes["dlm"].Value),
                    Convert.ToBoolean(profileNode.Attributes["repair"].Value),
                    Convert.ToBoolean(profileNode.Attributes["vectoring"].Value),
                    (VDSL2Profile)Enum.Parse(typeof(VDSL2Profile), "p" + profileNode.Attributes["vdsl2"].Value),
                    confirmedDownloadList.Distinct().ToList(),
                    confirmedUploadList.Distinct().ToList(),
                    Convert.ToDecimal(profileNode.Attributes["min"].Value, CultureInfo.InvariantCulture),
                    Convert.ToDecimal(profileNode.Attributes["max"].Value, CultureInfo.InvariantCulture));

                listProfiles.Add(profile);
            }
            return listProfiles;
        }

        /// <summary>
        /// Get Proximus line profile 
        /// </summary>
        /// <param name="profiles">Proximus line profiles to choose from</param>
        /// <param name="uploadSpeed">Upload speed of current session</param>
        /// <param name="downloadSpeed">Download speed of current session</param>
        /// <param name="vectoringEnabled">Vectoring enabled/disabled or null for unknown</param>
        /// <param name="distance">Distance or null for unknown</param>
        /// <returns>Proximus line profile of the current session</returns>
        public static ProximusLineProfile getProfile(List<ProximusLineProfile> profiles, int uploadSpeed, int downloadSpeed, bool? vectoringEnabled, decimal? distance)
        {
            ProximusLineProfile profile = new ProximusLineProfile();

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

                //no matches found, get profile with closest speeds in range of +256kb
                List<ProximusLineProfile> rangeMatches = profiles.Select(x => new { x, diffDownload = Math.Abs(x.DownloadSpeed - downloadSpeed), diffUpload = Math.Abs(x.UploadSpeed - uploadSpeed) })
                    .Where(x => x.diffDownload <= 256 && x.diffUpload <= 256)
                    .OrderBy(p => p.diffDownload)
                    .ThenBy(p => p.diffUpload)
                    .Select(y => y.x)
                    .ToList();

                //check on vectoring
                if (vectoringEnabled != null)
                    rangeMatches = rangeMatches
                        .Where(x => x.VectoringEnabled == vectoringEnabled).ToList();

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
                    return rangeMatches.First();
            }

            //no matches found
            return null;
        }

    }
}