using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;

namespace BBox3Tool
{
    public class Bbox3Session
    {
        #region private members

        //worker thread
        private BackgroundWorker _worker;

        //bbox url
        private Uri _bboxUrl;
        private Uri _cgiReq;

        //authentication
        private bool _loggedIn;
        private string _serverNonce;
        private string _localNonce;
        private int _sessionID;
        private int _requestID;
        private string _user;
        private string _password;
        private bool _basicAuth;

        //profiles
        private List<ProximusLineProfile> _profiles;

        //stats
        private decimal _downstreamAttenuation;
        private decimal _upstreamAttenuation;
        private decimal _downstreamNoiseMargin;
        private decimal _upstreamNoiseMargin;
        private int _downstreamMaxBitRate;
        private int _upstreamMaxBitRate;
        private int _downstreamCurrentBitRate;
        private int _upstreamCurrentBitRate;

        //profile
        private ProximusLineProfile _currentProfile;

        #endregion

        #region getters&setters

        public bool LoggedIn
        {
            get { return _loggedIn; }
        }

        public Uri BboxUrl
        {
            get { return _bboxUrl; }
            set { _bboxUrl = value; }
        }

        public string User
        {
            get { return _user; }
            set { _user = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public decimal DownstreamAttenuation
        {
            get { return _downstreamAttenuation; }
        }

        public decimal UpstreamAttenuation
        {
            get { return _upstreamAttenuation; }
        }

        public decimal DownstreamNoiseMargin
        {
            get { return _downstreamNoiseMargin; }
        }

        public decimal UpstreamNoiseMargin
        {
            get { return _upstreamNoiseMargin; }
        }

        public int DownstreamMaxBitRate
        {
            get { return _downstreamMaxBitRate; }
        }

        public int UpstreamMaxBitRate
        {
            get { return _upstreamMaxBitRate; }
        }

        public int DownstreamCurrentBitRate
        {
            get { return _downstreamCurrentBitRate; }
        }

        public int UpstreamCurrentBitRate
        {
            get { return _upstreamCurrentBitRate; }
        }

        internal ProximusLineProfile CurrentProfile
        {
            get { return _currentProfile; }
        }

        #endregion

        #region constructors

        public Bbox3Session()
        {
            _loggedIn = false;
        }

        public Bbox3Session(string bboxUrl, string user, string password, BackgroundWorker worker)
        {
            _loggedIn = false;
            _user = user;
            _password = password;
            _bboxUrl = new Uri(bboxUrl);
            _cgiReq = new Uri(BboxUrl, Path.Combine("cgi", "json-req"));

            //worker thread
            _worker = worker;

            //auth
            _sessionID = 0;
            _requestID = 0;
            _basicAuth = false;
            _serverNonce = "";
            _localNonce = "";

            //stats
            _downstreamAttenuation = -1;
            _upstreamAttenuation = -1;
            _downstreamNoiseMargin = -1;
            _upstreamNoiseMargin = -1;
            _downstreamMaxBitRate = -1;
            _upstreamMaxBitRate = -1;
            _downstreamCurrentBitRate = -1;
            _upstreamCurrentBitRate = -1;

            _currentProfile = new ProximusLineProfile();

            //load profiles
            _profiles = loadProfiles();
        }

        #endregion constructors

        #region login&logout

        /// <summary>
        /// Login with given credentials 
        /// </summary>
        /// <returns>Login successfull or not</returns>
        public bool openSession()
        {
            try
            {
                //reset member vars
                _sessionID = 0;
                _requestID = 0;
                _basicAuth = false;
                _serverNonce = "";
                _localNonce = Bbox3Utils.getLocalNonce();

                //create json object
                var jsonLogin = new
                {
                    request = new Dictionary<string, object>{ 
                        { "id", _requestID },
                        { "priority", true },
                        { "session-id", _sessionID.ToString() }, // !! must be string
                        { "actions", new[]{
                            new Dictionary<string, object>{
                                {"id", 0},
                                {"method", "logIn"},
                                {"parameters", new Dictionary<string, object>{ 
                                    {"user", _user},
                                    //{"password", _password  },
                                    //{"basic", _basicAuth },
			                        {"persistent", "true"}, // !! must be string
                                    {"session-options", new Dictionary<string, object>{
                                        {"nss", new[] { new{
                                            name = "gtw",
                                            uri = "http://sagem.com/gateway-data"
                                        }}},
                                        {"language", "ident"},
                                        {"context-flags",new Dictionary<string, object>{ 
					                        {"get-content-name",true},
					                        {"local-time",true},
					                        {"no-default",true}
                                        }},
                                        {"capability-depth",1}, //default 1
                                        {"capability-flags",new Dictionary<string, object>{ 
					                        {"name",false}, //default true
                                            {"default-value",false}, //default true
                                            {"restriction",false}, //default true
                                            {"description",false}, //default false
                                            {"flags",false}, //default true
                                            {"type",false} //default true
                                        }},
                                        {"time-format","ISO_8601"},
                                        {"depth",1},  //default 2
                                        {"max-add-events",5},
                                        {"write-only-string","_XMO_WRITE_ONLY_"},
                                        {"undefined-write-only-string","_XMO_UNDEFINED_WRITE_ONLY_"}
                                    }}
                                }}
                            }
                        }},
                        { "cnonce", Convert.ToUInt32(_localNonce)},
                        { "auth-key" , Bbox3Utils.calcAuthKey(User, Password, _requestID, _serverNonce, _localNonce)}
                    }
                };

                //prepare data to send
                var jsonString = new JavaScriptSerializer().Serialize(jsonLogin);
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("req", jsonString);

                //send request & get response
                string response = Bbox3Utils.sendRequest(_cgiReq, getCookies(), data, WebRequestMode.Post);

                //deserialize object
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                dynamic jsonObject = serializer.Deserialize<dynamic>(response);
                Dictionary<string, object> parameters = jsonObject["reply"]["actions"][0]["callbacks"][0]["parameters"];

                //set session id and server nonce
                _sessionID = Convert.ToInt32(parameters["id"]);
                _serverNonce = Convert.ToString(parameters["nonce"]);
                _requestID++;

                _loggedIn = true;
                //successfully logged in
                return true;
            }
            catch
            {
                _loggedIn = false;
                return false;
            }
        }
        
        /// <summary>
        /// Logout, close Bbox session
        /// </summary>
        public void closeSession()
        {
            try
            {
                //calc local nonce
                _localNonce = Bbox3Utils.getLocalNonce();

                //create json object
                var jsonLogout = new
                {
                    request = new Dictionary<string, object>{ 
                    { "id", _requestID },
                    { "priority", false },
                    { "session-id", _sessionID },
                    { "actions", new[]{
                        new {
                            id = 0,
                            method = "logOut"
                        }
                    }},
                    { "cnonce", Convert.ToUInt32(_localNonce)},
                    { "auth-key" , Bbox3Utils.calcAuthKey(User, Password, _requestID, _serverNonce, _localNonce)}}
                };

                //prepare data to send
                var jsonString = new JavaScriptSerializer().Serialize(jsonLogout);
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("req", jsonString);

                //send request & get response
                string response = Bbox3Utils.sendRequest(_cgiReq, getCookies(), data, WebRequestMode.Post);

                //deserialize object
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                dynamic jsonObject = serializer.Deserialize<dynamic>(response);
                string loggedOut = Convert.ToString(jsonObject["reply"]["error"]["description"]);
                if (loggedOut != "Ok")
                    throw new Exception("Logout unsuccessfull!");

                _loggedIn = false;
            }
            catch { }
        }
        
        #endregion

        #region profile

        /// <summary>
        /// Get current download sync speed in kbps
        /// </summary>
        /// <returns>Speed in kbps, 'unknown' if not found</returns>
        public string getDownstreamCurrentBitRate()
        {
            List<int> knownDownloadBitrates = new List<int>();

            //check confirmed bitrates first (feedback from users)
            //TODO add confirmed bitrates in profile
            knownDownloadBitrates.AddRange(new List<int> { 69999, 60198, 50200, 50199, 30063, 30057, 25063, 20061, 16559, 16550, 12063, 4448, 2240 });
            _downstreamCurrentBitRate = Convert.ToInt32(getDslValueLinear("Device/DSL/Lines/Line[{0}]/Status", "../../Channels/Channel/DownstreamCurrRate", knownDownloadBitrates, 1));

            //speed found, return
            if (_downstreamCurrentBitRate != -1)
                return _downstreamCurrentBitRate.ToString("###,###,##0 'kbps'");

            //speed not found in confirmed bitrate list, check profile download speeds, but with margin of -64 to +64
            knownDownloadBitrates.Clear();
            knownDownloadBitrates.AddRange(_profiles.Select(x => x.DownloadSpeed).SelectMany(x => Enumerable.Range(x-64, 128)));
            knownDownloadBitrates = knownDownloadBitrates.Distinct().ToList();
            _downstreamCurrentBitRate = Convert.ToInt32( getDslValueLinear("Device/DSL/Lines/Line[{0}]/Status", "../../Channels/Channel/DownstreamCurrRate", knownDownloadBitrates));

            //fallback: speed not found in profile list, check every speed (very slow)
            if (_downstreamCurrentBitRate == -1)
                _downstreamCurrentBitRate = (int)getDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "../../Channels/Channel/DownstreamCurrRate", 0, 100000, 1000);
            
            if (_downstreamCurrentBitRate == -1)
                return "unknown";
            else
                return _downstreamCurrentBitRate.ToString("###,###,##0 'kbps'"); ;
        }

        /// <summary>
        /// Get current upload sync speed in kbps
        /// </summary>
        /// <returns>Speed in kbps, 'unknown' if not found</returns>
        public string getUpstreamCurrentBitRate()
        {
              List<int> knownUploadBitrates = new List<int>();

            //check confirmed bitrates first (feedback from users)
            //TODO add confirmed bitrates in profile
            knownUploadBitrates.AddRange(new List<int> { 10064, 10054, 10049, 10004, 6063, 2063, 2054, 2045, 1044, 512, 384 });
            _upstreamCurrentBitRate = Convert.ToInt32(getDslValueLinear("Device/DSL/Lines/Line[{0}]/Status", "../../Channels/Channel/UpstreamCurrRate", knownUploadBitrates, 1));
            
            //speed found, return
            if (_upstreamCurrentBitRate != -1)
                return _upstreamCurrentBitRate.ToString("###,###,##0 'kbps'");

            //speed not found in confirmed bitrate list, check profile download speeds, but with margin of -64 to +64
            knownUploadBitrates.Clear();
            knownUploadBitrates.AddRange(_profiles.Select(x => x.UploadSpeed).SelectMany(x => Enumerable.Range(x - 64, 128)));
            knownUploadBitrates = knownUploadBitrates.Distinct().ToList();
            _upstreamCurrentBitRate = Convert.ToInt32(getDslValueLinear("Device/DSL/Lines/Line[{0}]/Status", "../../Channels/Channel/UpstreamCurrRate", knownUploadBitrates));

            //fallback: speed not found in profile list, check every speed (slow)
            if (_upstreamCurrentBitRate == -1)
                _upstreamCurrentBitRate = (int)getDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "../../Channels/Channel/UpstreamCurrRate", 0, 20000, 1000);

            if (_upstreamCurrentBitRate == -1)
                return "unknown";
            else
                return _upstreamCurrentBitRate.ToString("###,###,##0 'kbps'");
        }

        /// <summary>
        /// Get Proximus Line profile, based on current download and upload speeds
        /// </summary>
        /// <returns>Proximus line profile</returns>
        public ProximusLineProfile getProfileInfo()
        {
            //find closest profile values
            int downloadProfile = _profiles.Aggregate((x, y) => Math.Abs(x.DownloadSpeed - _downstreamCurrentBitRate) < Math.Abs(y.DownloadSpeed - _downstreamCurrentBitRate) ? x : y).DownloadSpeed;
            int uploadProfile = _profiles.Aggregate((x, y) => Math.Abs(x.UploadSpeed - _upstreamCurrentBitRate) < Math.Abs(y.UploadSpeed - _upstreamCurrentBitRate) ? x : y).UploadSpeed;
            
            //find closest profile
            _currentProfile = _profiles.Where(x => x.UploadSpeed == uploadProfile && x.DownloadSpeed == downloadProfile).FirstOrDefault();
            if (_currentProfile != null && Math.Abs(_currentProfile.DownloadSpeed - _downstreamCurrentBitRate) <= 256 && Math.Abs(_currentProfile.UploadSpeed - _upstreamCurrentBitRate) <= 256){ }
            else
                _currentProfile = new ProximusLineProfile("unknown", _downstreamCurrentBitRate, _upstreamCurrentBitRate, false, false, false, false, VDSL2Profile.unknown);

            return _currentProfile;
        }

        #endregion profile

        #region stats

        /// <summary>
        /// Get downstream attenuation, check from 0 to 100 dB
        /// </summary>
        /// <returns>Downstream attenuation in dB, or 'unknown' if not found</returns>
        public string getDownstreamAttenuation()
        {
            //check attenuation from 0.0 to 100.0
            List<int> valuesToCheck = Enumerable.Range(0, 1000).ToList();
            _downstreamAttenuation = getDslValueLinear("Device/DSL/Lines/Line[{0}]/Status", "DownstreamAttenuation", valuesToCheck, 20) / 10;

            if (_downstreamAttenuation == -1)
                return "unknown";

            return _downstreamAttenuation.ToString("0.0 'dB'");
        }
        
        /// <summary>
        /// Get upstream attenuation, check from 0 to 100 dB
        /// </summary>
        /// <returns>Upstream attenuation in dB, or 'unknown' if not found</returns>
        public string getUpstreamAttenuation()
        {
            //special case, upstream attenuation always seems to be 0, so check 0 first to save requests
            List<int> valuesToCheck = new List<int> { 0 };
            _upstreamAttenuation = getDslValueLinear("Device/DSL/Lines/Line[{0}]/Status", "UpstreamAttenuation", valuesToCheck, 20) / 10;

            if (_upstreamAttenuation == -1)
            {
                //check attenuation from 0.1 to 100.0
                valuesToCheck = Enumerable.Range(1, 1000).ToList();
                _upstreamAttenuation = getDslValueLinear("Device/DSL/Lines/Line[{0}]/Status", "UpstreamAttenuation", valuesToCheck, 20) / 10;
            }

            if (_upstreamAttenuation == -1)
                return "unknown";

            return _upstreamAttenuation.ToString("0.0 'dB'");
        }
        
        /// <summary>
        /// Get downstream noise margin, check from 0 to 100 dB
        /// </summary>
        /// <returns>Downstream noise margin in dB, or 'unknown' if not found</returns>
        public string getDownstreamNoiseMargin()
        {
            //check noise margin from 0.0 to 100.0
            List<int> valuesToCheck = Enumerable.Range(0, 1000).ToList();
            _downstreamNoiseMargin = getDslValueLinear("Device/DSL/Lines/Line[{0}]/Status", "DownstreamNoiseMargin", valuesToCheck, 20) / 10;

            if (_downstreamNoiseMargin == -1)
                return "unknown";

            return _downstreamNoiseMargin.ToString("0.0 'dB'");
        }

        /// <summary>
        /// Get upstream noise margin, check from 0 to 100 dB
        /// </summary>
        /// <returns>Upstream noise margin in dB, or 'unknown' if not found</returns>
        public string getUpstreamNoiseMargin()
        {
            //check noise margin from 0.0 to 100.0
            List<int> valuesToCheck = Enumerable.Range(0, 1000).ToList();
            _upstreamNoiseMargin = getDslValueLinear("Device/DSL/Lines/Line[{0}]/Status", "UpstreamNoiseMargin", valuesToCheck) / 10;

            if (_upstreamNoiseMargin == -1)
                return "unknown";

            return _upstreamNoiseMargin.ToString("0.0 'dB'");
        }
        
        /// <summary>
        /// Get downstream max bit rate, check from 0 to 150.000 kbps
        /// </summary>
        /// <returns>Downstream max bit rate in kbps, or 'unknown' if not found</returns>
        public string getDownstreamMaxBitRate()
        {
            int startValue = (_downstreamCurrentBitRate > 0) ? _downstreamCurrentBitRate : 0;
            startValue = Convert.ToInt32(Math.Floor(Convert.ToDecimal(startValue + 1) / 1000) * 1000);
            _downstreamMaxBitRate = (int)getDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "DownstreamMaxBitRate", startValue, 150000, 1000);

            if (_downstreamMaxBitRate == -1)
                _downstreamMaxBitRate = (int)getDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "DownstreamMaxBitRate",0, startValue, 1000);

            if (_downstreamMaxBitRate == -1)
                return "unknown";

            return _downstreamMaxBitRate.ToString("###,###,##0 'kbps'");
        }
        
        /// <summary>
        /// Get upstream max bit rate, check from 0 to 50.000 kbps
        /// </summary>
        /// <returns>Upstream max bit rate in kbps, or 'unknown' if not found</returns>
        public string getUpstreamMaxBitRate()
        {
            int startValue = (_upstreamCurrentBitRate > 0) ? _upstreamCurrentBitRate : 0;
            startValue = Convert.ToInt32(Math.Floor(Convert.ToDecimal(startValue + 1) / 1000) * 1000);
            _upstreamMaxBitRate = (int)getDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "UpstreamMaxBitRate", startValue, 50000, 1000);

            if (_upstreamMaxBitRate == -1)
                _upstreamMaxBitRate = (int)getDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "UpstreamMaxBitRate",0, startValue, 1000);

            if (_upstreamMaxBitRate == -1)
                return "unknown";

            return _upstreamMaxBitRate.ToString("###,###,##0 'kbps'");
        }

        #endregion

        //test

        public void getTestValues()
        {
            //calc local nonce
            _localNonce = Bbox3Utils.getLocalNonce();

            //create json object
            var jsonGetValue = new
            {
                request = new Dictionary<string, object>{ 
                { "id", _requestID },
                { "session-id", _sessionID },
                { "priority", false },
                { "actions", new Dictionary<string, object>[]{
                    new Dictionary<string, object> {
                        {"id",0},
                        {"method","getValue"},
                        //test xpaths here
                        {"xpath", "Device/DSL/Lines/Line[UpstreamMaxBitRate = 70000]/Status"}, 
                    },
                }},
                { "cnonce", Convert.ToUInt32(_localNonce)},
                { "auth-key" , Bbox3Utils.calcAuthKey(User, Password, _requestID, _serverNonce, _localNonce)}}
            };

            //prepare data to send
            string jsonString = new JavaScriptSerializer().Serialize(jsonGetValue);
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("req", jsonString);

            //send request & get response
            string response = Bbox3Utils.sendRequest(_cgiReq, getCookies(), data, WebRequestMode.Post);
                
            //TO TEST: set breakpoint on next line

            //deserialize object
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            dynamic jsonObject = serializer.Deserialize<dynamic>(response);

            _requestID++;
        }

        public int getDownstreamMaxBitRate2()
        {
            _downstreamMaxBitRate = (int)getDslValueExponentional("Device/DSL/Lines/Line[{0}]/Status", "DownstreamMaxBitRate", 24);
            return _downstreamMaxBitRate;
        }

        //private functions
        //-----------------
        #region private

        private decimal getDslValueParallel(string xpathBase, string node, int from, int to, int subStep)
        {

            int requestCount = 30;
            int xpathCount = 10;
            //--> 300 checks per request

            decimal dslValue = -1;
            int limitCheck = from + subStep;

            do
            {
                //prepare requests
                List<string> actions = new List<string>();
                for (int i = from; i < (from + requestCount); i++)
                {
                    string xpathSub = string.Empty;
                    List<string> list = new List<string>();
                    for (int j = 0; j < xpathCount; j++)
                        list.Add(node + "=\"" + ((j * subStep) + i).ToString() + "\"");
                    xpathSub = string.Join(" or ", list);

                    actions.Add(string.Format(xpathBase, xpathSub));
                }

                //get values from bbox
                dynamic jsonObject = getValuesFromBBox(actions);

                //check values
                for (int i = 0; i < requestCount; i++)
                {
                    if (jsonObject["reply"]["actions"][i]["error"]["description"] == "Applied")
                    {
                        dslValue = from + Convert.ToDecimal(jsonObject["reply"]["actions"][i]["id"]);
                        break;
                    }
                }

                if (from >= limitCheck)
                {
                    from += (xpathCount * subStep) - subStep;
                    limitCheck += (xpathCount * subStep);
                }

                //increase start
                from += requestCount;

            }
            while (dslValue == -1 && from < to);

            //not found
            if (dslValue == -1)
                return -1;

            //get precise value
            //prepare requests
            List<string> actionsPrecise = new List<string>();
            for (int i = 0; i < xpathCount; i++)
                actionsPrecise.Add(string.Format(xpathBase, node + "=" + ((i * subStep) + dslValue)));

            //get values from bbox
            dynamic jsonObjectPrecise = getValuesFromBBox(actionsPrecise);

            //check values
            for (int i = 0; i < xpathCount; i++)
            {
                if (jsonObjectPrecise["reply"]["actions"][i]["error"]["description"] == "Applied")
                {
                    dslValue += (i * subStep);
                    break;
                }
            }

            return dslValue;
        }

        /// <summary>
        /// Get DSL value using linear algorithm
        /// </summary>
        /// <param name="xpathBase">Xpath base with {0} placeholder for Xpath node</param>
        /// <param name="node">Xpath node to check</param>
        /// <param name="valuesToCheck">List of values to check</param>
        /// <param name="requestRange">Ammount of Xpath requests to send at bbox in one webrequest (max 100)</param>
        /// <returns>DSL value, -1 if not found</returns>
        private decimal getDslValueLinear(string xpathBase, string node, List<int> valuesToCheck, int requestRange = 80)
        {

            decimal dslValue = -1;

            int from = 0;
            int ammount = 0;

            do
            {
                ammount = Math.Min(requestRange, valuesToCheck.Count() - from);

                //get subrange from list
                List<int> subrange = valuesToCheck.GetRange(from, ammount);

                //prepare requests
                List<string> actions = subrange.Select(x => string.Format(xpathBase, node + "=\"" + x.ToString() + "\"")).ToList();

                //get values from bbox
                dynamic jsonObject = getValuesFromBBox(actions);

                //check values
                for (int i = 0; i < actions.Count; i++)
                {
                    if (jsonObject["reply"]["actions"][i]["error"]["description"] == "Applied")
                    {
                        dslValue = subrange[i];
                        break; //from loop
                    }
                }

                //increase from
                from += ammount;

            }
            while (dslValue == -1 && from < valuesToCheck.Count());

            return dslValue;
        }

        /// <summary>
        /// Get DSL value using exponential algorithm
        /// </summary>
        /// <param name="xpathBase">Xpath base with {0} placeholder for Xpath node</param>
        /// <param name="node">Xpath node to check</param>
        /// <param name="power">Check value to 2^power</param>
        /// <returns>DSL value, -1 if not found</returns>
        private decimal getDslValueExponentional(string xpathBase, string node, int power)
        {
            int greaterThen = Convert.ToInt32(Math.Pow(2, power));
            do
            {

                //get values from bbox
                dynamic jsonObject = getValuesFromBBox(new List<string> { string.Format(xpathBase, node + " > " + greaterThen.ToString()) }); //doens't work, always true :(  

                //check values
                power--;
                bool greater = (jsonObject["reply"]["actions"][0]["error"]["description"] == "Applied");
                if (greater)
                    greaterThen += Convert.ToInt32(Math.Pow(2, power));
                else
                    greaterThen -= Convert.ToInt32(Math.Pow(2, power));
            }
            while (power >= 1);

            return greaterThen;
        }

        /// <summary>
        /// Make request to bbox and return the JSON-object as a dynamic
        /// </summary>
        /// <param name="xpaths">Xpaths to check</param>
        /// <returns>JSON reply from bbox</returns>
        private dynamic getValuesFromBBox(List<string> xpaths)
        {
            //check thread cancel
            if (_worker.CancellationPending)
                throw new ThreadCancelledException("Request cancelled.");

            //calc local nonce
            _localNonce = Bbox3Utils.getLocalNonce();

            //prepare actions
            List<Dictionary<string, object>> actions = new List<Dictionary<string, object>>();
            int i = 0;
            foreach (string xpath in xpaths)
            {
                actions.Add(new Dictionary<string, object> {
                    {"id",i},
                    {"method","getValue"},
                    {"xpath", xpath},                             
                });
                i++;
            }
            //create json object
            var jsonGetValue = new
            {
                request = new Dictionary<string, object>{ 
                    { "id", _requestID },
                    { "session-id", _sessionID },
                    { "priority", false },
                    { "actions", actions.ToArray()},
                    { "cnonce", Convert.ToUInt32(_localNonce)},
                    { "auth-key" , Bbox3Utils.calcAuthKey(User, Password, _requestID, _serverNonce, _localNonce)}}
            };

            //prepare data to send
            string jsonString = new JavaScriptSerializer().Serialize(jsonGetValue);
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("req", jsonString);

            //send request & get response
            string response = Bbox3Utils.sendRequest(_cgiReq, getCookies(), data, WebRequestMode.Post);

            //increase request id
            _requestID++;

            //deserialize object
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Deserialize<dynamic>(response);
        }

        /// <summary>
        /// Create the cookies to send with each webrequest
        /// </summary>
        /// <returns></returns>
        private CookieCollection getCookies()
        {

            CookieCollection cookies = new CookieCollection();

            //set language cookie
            cookies.Add(new Cookie("lang", "en", "/", BboxUrl.Host));

            //set session cookie
            var jsonCookie = new
            {
                request = new Dictionary<string, object>{ 
                    { "req_id", (_requestID+1) },
                    { "sess_id", _sessionID },
                    { "basic", _basicAuth },
                    { "user", _user},
                    { "nonce", _serverNonce},
                    { "ha1", Bbox3Utils.calcHa1Cookie(User, Password, _serverNonce)},
                    { "datamodel",
                        new{
                        name = "internal",
                        nss = new[]{ 
                                new{ 
                                    name = "gtw",
                                    uri = "http://sagem.com/gateway-data"
                                }
                            }
                        }
                    }
                }
            };
            string cookieStr = new JavaScriptSerializer().Serialize(jsonCookie);
            cookies.Add(new Cookie("session", HttpUtility.UrlEncode(cookieStr), "/", BboxUrl.Host));

            cookies[0].Expires = DateTime.Now.AddYears(1);
            cookies[1].Expires = DateTime.Now.AddDays(1);

            return cookies;
        }

        /// <summary>
        /// Get the Proximus line profiles
        /// </summary>
        /// <returns>List of profiles</returns>
        private List<ProximusLineProfile> loadProfiles()
        {
            //TODO get profiles from (online) xml

            List<ProximusLineProfile> profiles = new List<ProximusLineProfile>();

            //17a vectoring profiles
            profiles.Add(new ProximusLineProfile("LP145", 70000, 10064, true, false, false, true, VDSL2Profile.p17a));
            profiles.Add(new ProximusLineProfile("LP141", 70000, 8064, false, false, false, true, VDSL2Profile.p17a));
            profiles.Add(new ProximusLineProfile("LP810", 70000, 6064, false, false, false, true, VDSL2Profile.p17a));
            //17a vectoring repair
            profiles.Add(new ProximusLineProfile("LP820", 70000, 4064, false, false, true, true, VDSL2Profile.p17a));
            profiles.Add(new ProximusLineProfile("LP830", 50000, 4064, false, false, true, true, VDSL2Profile.p17a));
            profiles.Add(new ProximusLineProfile("LP840", 50000, 2064, false, false, true, true, VDSL2Profile.p17a));
            profiles.Add(new ProximusLineProfile("LP850", 30000, 2064, false, false, true, true, VDSL2Profile.p17a));
            //8d vectoring fallback (not possible with bbox3...)
            profiles.Add(new ProximusLineProfile("LP725", 7544, 576, true, false, false, false, VDSL2Profile.p8d));

            //17a profiles
            profiles.Add(new ProximusLineProfile("LP056", 30064, 10064, true, false, false, false, VDSL2Profile.p17a));
            //17a repair
            profiles.Add(new ProximusLineProfile("LP048", 30064, 8064, false, false, true, false, VDSL2Profile.p17a));
            profiles.Add(new ProximusLineProfile("LP705", 30064, 6064, false, false, true, false, VDSL2Profile.p17a));
            profiles.Add(new ProximusLineProfile("LP706", 25064, 6064, false, false, true, false, VDSL2Profile.p17a));
            profiles.Add(new ProximusLineProfile("LP707", 20064, 6064, false, false, true, false, VDSL2Profile.p17a));
            profiles.Add(new ProximusLineProfile("LP708", 14564, 4064, false, false, true, false, VDSL2Profile.p17a));
            //17a dlm
            profiles.Add(new ProximusLineProfile("LP060", 70200, 10064, false, true, false, false, VDSL2Profile.p17a));
            profiles.Add(new ProximusLineProfile("LP052", 70200, 8064, false, true, false, false, VDSL2Profile.p17a));
            profiles.Add(new ProximusLineProfile("LP059", 60200, 10064, false, true, false, false, VDSL2Profile.p17a));
            profiles.Add(new ProximusLineProfile("LP051", 60200, 8064, false, true, false, false, VDSL2Profile.p17a));
            profiles.Add(new ProximusLineProfile("LP058", 50200, 10064, false, true, false, false, VDSL2Profile.p17a));
            profiles.Add(new ProximusLineProfile("LP050", 50200, 8064, false, true, false, false, VDSL2Profile.p17a));
            profiles.Add(new ProximusLineProfile("LP057", 40200, 10064, false, true, false, false, VDSL2Profile.p17a));
            profiles.Add(new ProximusLineProfile("LP049", 40200, 8064, false, true, false, false, VDSL2Profile.p17a));
            profiles.Add(new ProximusLineProfile("LP709", 50200, 6064, false, true, false, false, VDSL2Profile.p17a));
            profiles.Add(new ProximusLineProfile("LP710", 40200, 6064, false, true, false, false, VDSL2Profile.p17a));
            profiles.Add(new ProximusLineProfile("LP723", 70200, 6064, false, true, false, false, VDSL2Profile.p17a));
            profiles.Add(new ProximusLineProfile("LP724", 60200, 6064, false, true, false, false, VDSL2Profile.p17a));

            //17a high upload
            profiles.Add(new ProximusLineProfile("LP715", 16564, 10064, true, false, false, false, VDSL2Profile.p17a));
            //17a high upload repair
            profiles.Add(new ProximusLineProfile("LP716", 16564, 8064, false, false, true, false, VDSL2Profile.p17a));
            profiles.Add(new ProximusLineProfile("LP717", 14564, 6064, false, false, true, false, VDSL2Profile.p17a));
            profiles.Add(new ProximusLineProfile("LP718", 12064, 4064, false, false, true, false, VDSL2Profile.p17a));

            //8d profiles
            profiles.Add(new ProximusLineProfile("LP701", 20064 , 2064, true, false, false, false, VDSL2Profile.p8d));
            profiles.Add(new ProximusLineProfile("LP702", 16564 , 2064, true, false, false, false, VDSL2Profile.p8d));
            //8d repair
            profiles.Add(new ProximusLineProfile("LP703", 14564 , 1064, false, false, true, false, VDSL2Profile.p8d));
            profiles.Add(new ProximusLineProfile("LP704", 9064 , 576, false, false, true, false, VDSL2Profile.p8d));
            //8d dlm
            profiles.Add(new ProximusLineProfile("LP719", 30200 , 2064, false, true, false, false, VDSL2Profile.p8d));
            profiles.Add(new ProximusLineProfile("LP720", 25200 , 2064, false, true, false, false, VDSL2Profile.p8d));
            profiles.Add(new ProximusLineProfile("LP722", 20200 , 2064, false, true, false, false, VDSL2Profile.p8d));

            //8d profiles long reach
            profiles.Add(new ProximusLineProfile("LP711", 12064, 1064, true, false, false, false, VDSL2Profile.p8d));
            //8d profiles long reach repair
            profiles.Add(new ProximusLineProfile("LP712", 12064, 576, false, false, true, false, VDSL2Profile.p8d));
            profiles.Add(new ProximusLineProfile("LP713", 7064, 576, false, false, true, false, VDSL2Profile.p8d));
            profiles.Add(new ProximusLineProfile("LP714", 10100, 576, false, false, true, false, VDSL2Profile.p8d));

            //8d profiles very long reach
            profiles.Add(new ProximusLineProfile("LP730", 9564, 704, true, false, false, false, VDSL2Profile.p8d));
            //8d profiles very long reach repair
            profiles.Add(new ProximusLineProfile("LP731", 5064, 576, false, false, true, false, VDSL2Profile.p8d));

            return profiles;
        }

        #endregion
    }
}
