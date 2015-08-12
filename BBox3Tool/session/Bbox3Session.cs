using BBox3Tool.utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;

namespace BBox3Tool
{
    public class Bbox3Session : IModemSession
    {
        #region private members

        //worker thread
        private BackgroundWorker _worker;

        //bbox url
        private Uri _bboxUrl;
        private Uri _cgiReq;

        //authentication
        private string _serverNonce;
        private string _localNonce;
        private int _sessionID;
        private int _requestID;
        private int _notificationID;
        private string _username;
        private string _password;
        private bool _basicAuth;
        private readonly bool _debug;

        //profiles
        private readonly List<ProximusLineProfile> _profiles;

        //stats
        private int _downstreamCurrentBitRate;
        private int _upstreamCurrentBitRate;
        private int _downstreamMaxBitRate;
        private int _upstreamMaxBitRate;
        private decimal _downstreamAttenuation;
        private decimal _upstreamAttenuation;
        private decimal _downstreamNoiseMargin;
        private decimal _upstreamNoiseMargin;
        private decimal? _distance;
        private bool? _vectoring;
        private DSLStandard _DSLStandard;

        //booleans for stats
        private bool dsCurrBitRateDone, usCurrBitRateDone;
        private bool dsMaxBitRateDone, usMaxBitRateDone;
        private bool dsAttenuationDone, usAttenuationDone;
        private bool dsNoiseMarginDone, usNoiseMarginDone;
        private bool distanceDone;
        private bool vectoringDone;
        private bool DSLStandardDone;

        #endregion

        #region getters&setters

        public bool LoggedIn { get; private set; }

        public decimal DownstreamAttenuation
        {
            get
            {
                if (!dsAttenuationDone)
                    GetDownstreamAttenuation();
                return _downstreamAttenuation;
            }
            set { this._downstreamAttenuation = value; }
        }

        public decimal UpstreamAttenuation
        {
            get
            {
                if (!usAttenuationDone)
                    GetUpstreamAttenuation();
                return _upstreamAttenuation;
            }
            set { this._upstreamAttenuation = value; }
        }

        public decimal DownstreamNoiseMargin
        {
            get
            {
                if (!dsNoiseMarginDone)
                    GetDownstreamNoiseMargin();
                return _downstreamNoiseMargin;
            }
            set { this._downstreamNoiseMargin = value; }
        }

        public decimal UpstreamNoiseMargin
        {
            get
            {
                if (!usNoiseMarginDone)
                    GetUpstreamNoiseMargin();
                return _upstreamNoiseMargin;
            }
            set { this._upstreamNoiseMargin = value; }
        }

        public int DownstreamMaxBitRate
        {
            get
            {
                if (!dsMaxBitRateDone)
                    GetDownstreamMaxBitRate();
                return _downstreamMaxBitRate;
            }
            set { this._downstreamMaxBitRate = value; }
        }

        public int UpstreamMaxBitRate
        {
            get
            {
                if (!usMaxBitRateDone)
                    GetUpstreamMaxBitRate();
                return _upstreamMaxBitRate;
            }
            set { this._upstreamMaxBitRate = value; }
        }

        public int DownstreamCurrentBitRate
        {
            get
            {
                if (!dsCurrBitRateDone)
                    GetDownstreamCurrentBitRate();
                return _downstreamCurrentBitRate;
            }
            set { this._downstreamCurrentBitRate = value; }
        }

        public int UpstreamCurrentBitRate
        {
            get
            {
                if (!usCurrBitRateDone)
                    GetUpstreamCurrentBitRate();
                return _upstreamCurrentBitRate;
            }
            set { this._upstreamCurrentBitRate = value; }
        }

        public decimal? Distance
        {
            get
            {
                if (!distanceDone)
                    GetEstimatedDistance();
                return _distance;
            }
            set { this._distance = value; }
        }

        public bool? Vectoring
        {
            get
            {
                if (!vectoringDone)
                    GetVectoringEnabled();
                return _vectoring;
            }
            private set { this._vectoring = value; }
        }

        public string DeviceName { get; private set; }

        public ProximusLineProfile CurrentProfile { get; private set; }

        public string HardwareVersion { get; private set; }

        public string GUIFirmwareVersion { get; private set; }

        public string InternalFirmwareVersion { get; private set; }

        public TimeSpan DeviceUptime { get; private set; }

        public TimeSpan LinkUptime { get; private set; }

        public DSLStandard DSLStandard
        {
            get
            {
                if (!DSLStandardDone)
                    GetDslStandard();
                return _DSLStandard;
            }
            private set { this._DSLStandard = value; }
        }

        #endregion

        #region constructors

        public Bbox3Session(BackgroundWorker worker, List<ProximusLineProfile> profiles, bool debug = false)
        {
            LoggedIn = false;
            _debug = debug;
            _worker = worker;

            //auth
            _sessionID = 0;
            _requestID = 0;
            _notificationID = 1;
            _basicAuth = false;
            _serverNonce = "";
            _localNonce = "";

            //stats
            DownstreamAttenuation = -1;
            UpstreamAttenuation = -1;
            DownstreamNoiseMargin = -1;
            UpstreamNoiseMargin = -1;
            DownstreamMaxBitRate = -1;
            UpstreamMaxBitRate = -1;
            DownstreamCurrentBitRate = -1;
            UpstreamCurrentBitRate = -1;

            //device
            DeviceName = "B-Box 3 Sagem";
            HardwareVersion = "";
            GUIFirmwareVersion = "";
            InternalFirmwareVersion = "";
            DeviceUptime = new TimeSpan(0);
            LinkUptime = new TimeSpan(0);

            CurrentProfile = new ProximusLineProfile();
            Distance = null;
            Vectoring = null;
            DSLStandard = DSLStandard.unknown;

            //load profiles
            _profiles = profiles;
        }

        #endregion constructors

        #region device

        /// <summary>
        ///     Get device info: software & hardware versions, link and device uptime
        /// </summary>
        public DeviceInfo GetDeviceInfo()
        {
            dynamic jsonObject = BBoxGetValue(new List<string>
            {
                "Device/DeviceInfo/HardwareVersion",
                "Device/DeviceInfo/GUIFirmwareVersion",
                "Device/DeviceInfo/InternalFirmwareVersion",
                "Device/DeviceInfo/UpTime",
                "Device/DSL/Lines/Line[Alias=\"DSL0\"]/LastChange"
            });

            var deviceInfo = new DeviceInfo();
            deviceInfo.HardwareVersion = jsonObject["reply"]["actions"][0]["callbacks"][0]["parameters"]["value"].ToString();
            deviceInfo.FirmwareVersion = jsonObject["reply"]["actions"][2]["callbacks"][0]["parameters"]["value"].ToString()  + " / " + jsonObject["reply"]["actions"][1]["callbacks"][0]["parameters"]["value"].ToString();

            try
            {
                double seconds = Convert.ToDouble(jsonObject["reply"]["actions"][3]["callbacks"][0]["parameters"]["value"]);
                DeviceUptime = TimeSpan.FromSeconds(seconds);
                seconds = Convert.ToDouble(jsonObject["reply"]["actions"][4]["callbacks"][0]["parameters"]["value"]);
                LinkUptime = TimeSpan.FromSeconds(seconds);

                deviceInfo.LinkUptime = LinkUptime.ToString("%d") + (LinkUptime.Days == 1 ? " day " : " days ") +
                                        LinkUptime.ToString("hh\\:mm\\:ss");
                deviceInfo.DeviceUptime = DeviceUptime.ToString("%d") + (DeviceUptime.Days == 1 ? " day " : " days ") +
                                          DeviceUptime.ToString("hh\\:mm\\:ss");
            }
            catch
            {
            }

            return deviceInfo;
        }

        #endregion

        #region debug

        public string GetDebugValue(string xpath)
        {
            //prepare actions
            var actions = new List<Dictionary<string, object>>();
            actions.Add(new Dictionary<string, object>
            {
                {"id", 0},
                {"method", "getValue"},
                {"xpath", xpath},
                /*{"parameters", new Dictionary<string, object> {
                    {"current-value", true},
                    {"no-default", true}
                }}*/
            });

            //actions.Add(new Dictionary<string, object>
            //{
            //    {"id", 0},
            //    {"method", "subscribeForNotification"},
            //    {"xpath", xpath},
            //    {"parameters", new Dictionary<string, object> {
            //        {"id", _notificationID},
            //        {"type", "value-change"},
            //        {"current-value", true}
            //    }}
            //});
            //_notificationID++;

            return sendActionsToBBox(actions);
        }

        #endregion

        #region test

        public int getDownstreamMaxBitRate2()
        {
            DownstreamMaxBitRate =
                (int)getDslValueExponentional("Device/DSL/Lines/Line[{0}]/Status", "DownstreamMaxBitRate", 24);
            return DownstreamMaxBitRate;
        }

        public void getTest()
        {
            var valuesToCheck = new List<string>();
            CultureInfo enUS = new CultureInfo("en-US");
            decimal marginBase = 10m;
            for (int i = 0; i < 150; i++)
            {
                for (int j = -10; j < 10; j++)
                {
                    for (int k = -10; k < 10; k++)
                    {
                        decimal m1 = marginBase + Convert.ToDecimal(i) / 10m;
                        decimal m2 = m1 + Convert.ToDecimal(j) / 10m;
                        decimal m3 = m1 + Convert.ToDecimal(k) / 10m;
                        valuesToCheck.Add(m1.ToString("0.0", enUS) + "," + m2.ToString("0.0", enUS) + "," + m3.ToString("0.0", enUS));
                    }
                }
            }

            Stopwatch watch = new Stopwatch();

            watch.Start();
            var test = getDslValueLinear("Device/DSL/Lines/Line[{0}]/LastChange", "SNRMpbds", valuesToCheck, 5);
            watch.Stop();
            Debug.WriteLine("getDslValueLinear 5: " + watch.Elapsed.ToString());

            //var valuesToCheck = Enumerable.Range(0, 1000).ToList();

            //watch.Start();
            //var test = getDslValueLinear("Device/DSL/Lines/Line[{0}]/LastChange", "UpstreamNoiseMargin", valuesToCheck, 5) / 10;
            //watch.Stop();
            //Debug.WriteLine("getDslValueLinear 5: " + watch.Elapsed.ToString());

            //watch.Restart();
            //test = getDslValueParallel("Device/DSL/Lines/Line[{0}]/LastChange", "UpstreamNoiseMargin", valuesToCheck, 5, 5) / 10;
            //watch.Stop();
            //Debug.WriteLine("getDslValueParallel 5 5: " + watch.Elapsed.ToString());

            //watch.Restart();
            //test = getDslValueParallel("Device/DSL/Lines/Line[{0}]/LastChange", "UpstreamNoiseMargin", valuesToCheck, 10, 5) / 10;
            //watch.Stop();
            //Debug.WriteLine("getDslValueParallel 10 5: " + watch.Elapsed.ToString());

            //watch.Restart();
            //test = getDslValueParallel("Device/DSL/Lines/Line[{0}]/LastChange", "UpstreamNoiseMargin", valuesToCheck, 20, 5) / 10;
            //watch.Stop();
            //Debug.WriteLine("getDslValueParallel 20 5: " + watch.Elapsed.ToString());

            //watch.Restart();
            //test = getDslValueParallel("Device/DSL/Lines/Line[{0}]/LastChange", "UpstreamNoiseMargin", valuesToCheck, 50, 5) / 10;
            //watch.Stop();
            //Debug.WriteLine("getDslValueParallel 50 5: " + watch.Elapsed.ToString());

            //watch.Restart();
            //valuesToCheck.Clear();
            //valuesToCheck.AddRange(Enumerable.Range(90000, 10000).ToList());
            //test = getDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "DownstreamMaxBitRate", valuesToCheck, 75, 5);
            //watch.Stop();
            //Debug.WriteLine(" DownstreamMaxBitRate getDslValueParallel2: " + test + ", " + watch.Elapsed.ToString());


            //watch.Restart();
            //test = getDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "DownstreamMaxBitRate", 90000, 100000, 1000);
            //watch.Stop();
            //Debug.WriteLine(" DownstreamMaxBitRate getDslValueParallel: " + test + ", " + watch.Elapsed.ToString());

            //watch.Restart();
            //test = getDslValueSingleLinear("Device/DSL/Lines/Line[{0}]/LastChange", "UpstreamNoiseMargin", valuesToCheck) / 10;
            //watch.Stop();
            //Debug.WriteLine("getDslValueSingleLinear: " + watch.Elapsed.ToString());
        }

        #endregion


        #region login&logout

        /// <summary>
        ///     Login with given credentials
        /// </summary>
        /// <returns>Login successfull or not</returns>
        public bool OpenSession(String host, String username, String password)
        {
            _username = username;
            _password = password;
            _bboxUrl = new Uri("http://" + host);
            _cgiReq = new Uri(_bboxUrl, Path.Combine("cgi", "json-req"));

            try
            {
                //reset member vars
                _sessionID = 0;
                _requestID = 0;
                _basicAuth = false;
                _serverNonce = "";
                _localNonce = SagemUtils.getLocalNonce();

                //create json object
                var jsonLogin = new
                {
                    request = new Dictionary<string, object>
                    {
                        {"id", _requestID},
                        {"session-id", _sessionID.ToString()}, // !! must be string
                        {"priority", true},
                        {
                            "actions", new[]
                            {
                                new Dictionary<string, object>
                                {
                                    {"id", 0},
                                    {"method", "logIn"},
                                    {
                                        "parameters", new Dictionary<string, object>
                                        {
                                            {"user", _username},
                                            //{"password", _password  },
                                            //{"basic", _basicAuth },
                                            {"persistent", "true"}, // !! must be string
                                            {
                                                "session-options", new Dictionary<string, object>
                                                {
                                                    {
                                                        "nss", new[]
                                                        {
                                                            new
                                                            {
                                                                name = "gtw",
                                                                uri = "http://sagemcom.com/gateway-data"
                                                            }
                                                        }
                                                    },
                                                    {"language", "ident"},
                                                    {
                                                        "context-flags", new Dictionary<string, object>
                                                        {
                                                            {"get-content-name", true}, //default true
                                                            {"local-time", true},//default true
                                                            {"no-default", true} //default false
                                                        }
                                                    },
                                                    {"capability-depth", 0}, //default 1
                                                    {
                                                        "capability-flags", new Dictionary<string, object>
                                                        {
                                                            {"name", false}, //default true
                                                            {"default-value", false}, //default true
                                                            {"restriction", false}, //default true
                                                            {"description", false}, //default false
                                                            {"flags", false}, //default true
                                                            {"type", false}, //default true
                                                            
                                                        }
                                                    },
                                                    {"time-format", "ISO_8601"},
                                                    {"depth", _debug ? 2 : -1}, //default 2
                                                    {"max-add-events", 5},
                                                    {"write-only-string", "_XMO_WRITE_ONLY_"},
                                                    {"undefined-write-only-string", "_XMO_UNDEFINED_WRITE_ONLY_"}
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        {"cnonce", Convert.ToInt32(_localNonce)},
                        {"auth-key", SagemUtils.calcAuthKey(_username, _password, _requestID, _serverNonce, _localNonce)}
                    }
                };

                //prepare data to send
                var jsonString = new JavaScriptSerializer().Serialize(jsonLogin);
                var data = new Dictionary<string, string>();
                data.Add("req", jsonString);

                //send request & get response
                var response = NetworkUtils.sendRequest(_cgiReq, getCookies(), data, WebRequestMode.Post);

                //deserialize object
                var serializer = new JavaScriptSerializer();
                dynamic jsonObject = serializer.Deserialize<dynamic>(response);
                Dictionary<string, object> parameters = jsonObject["reply"]["actions"][0]["callbacks"][0]["parameters"];

                //set session id and server nonce
                _sessionID = Convert.ToInt32(parameters["id"]);
                _serverNonce = Convert.ToString(parameters["nonce"]);
                _requestID++;

                //successfully logged in
                LoggedIn = true;

                //BBoxSubscribeForNotification(new List<string> { "Device/DSL/Lines/Line[Alias=\"DSL0\"]/Status" });
                //getTest();
                return true;
            }
            catch (Exception ex)
            {
                Console.Write("Exception: " + ex);
                LoggedIn = false;
                return false;
            }
        }

        /// <summary>
        ///     Logout, close Bbox session
        /// </summary>
        public bool CloseSession()
        {
            try
            {
                //calc local nonce
                _localNonce = SagemUtils.getLocalNonce();

                //create json object
                var jsonLogout = new
                {
                    request = new Dictionary<string, object>
                    {
                        {"id", _requestID},
                        {"priority", false},
                        {"session-id", _sessionID},
                        {
                            "actions", new[]
                            {
                                new
                                {
                                    id = 0,
                                    method = "logOut"
                                }
                            }
                        },
                        {"cnonce", Convert.ToUInt32(_localNonce)},
                        {
                            "auth-key", SagemUtils.calcAuthKey(_username, _password, _requestID, _serverNonce, _localNonce)
                        }
                    }
                };

                //prepare data to send
                var jsonString = new JavaScriptSerializer().Serialize(jsonLogout);
                var data = new Dictionary<string, string>();
                data.Add("req", jsonString);

                //send request & get response
                var response = NetworkUtils.sendRequest(_cgiReq, getCookies(), data, WebRequestMode.Post);

                //deserialize object
                var serializer = new JavaScriptSerializer();
                dynamic jsonObject = serializer.Deserialize<dynamic>(response);
                string loggedOut = Convert.ToString(jsonObject["reply"]["error"]["description"]);
                if (loggedOut != "Ok")
                    throw new Exception("Logout unsuccessfull!");

                // Logout succesful
                LoggedIn = false;
                return true;
            }
            catch
            {
                // Logout failed
                return false;
            }
        }

        public void GetLineData()
        {
            // Do nothing
        }

        #endregion

        #region profile

        /// <summary>
        ///     Get current download sync speed in kbps
        /// </summary>
        public void GetDownstreamCurrentBitRate()
        {
            if (DSLStandard == BBox3Tool.DSLStandard.VDSL2)
            {

                //check confirmed bitrates first (feedback from users)
                var knownDownloadBitrates = _profiles.SelectMany(x => x.ConfirmedDownloadSpeeds).ToList();
                knownDownloadBitrates.AddRange(_profiles.Select(x => x.DownloadSpeed));
                knownDownloadBitrates = knownDownloadBitrates.Distinct().OrderByDescending(x => x).ToList();
                _downstreamCurrentBitRate = Convert.ToInt32(getDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "../../Channels/Channel/DownstreamCurrRate", knownDownloadBitrates, 10, 5));

                //speed found, return
                if (_downstreamCurrentBitRate >= 0)
                {
                    dsCurrBitRateDone = true;
                    return;
                }

                //speed not found in confirmed bitrate list, check profile download speeds, but with margin of -64 to +64
                knownDownloadBitrates.Clear();
                knownDownloadBitrates.AddRange(_profiles.Select(x => x.DownloadSpeed).SelectMany(x => Enumerable.Range(x - 64, 128)));
                knownDownloadBitrates = knownDownloadBitrates.Distinct().ToList();
                _downstreamCurrentBitRate = Convert.ToInt32(getDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "../../Channels/Channel/DownstreamCurrRate", knownDownloadBitrates, 5));

                //fallback: speed not found in profile list, check every speed (very slow)
                if (_downstreamCurrentBitRate < 0)
                {
                    var valuesToCheck = Enumerable.Range(0, 100000).ToList();
                    _downstreamCurrentBitRate = (int)getDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "../../Channels/Channel/DownstreamCurrRate", valuesToCheck, 75, 5);
                }
            }
            else
            {
                var knownDownloadBitrates = _profiles.Where(x => x.Name == "ADSL").Select(x => x.DownloadSpeed).ToList();
                knownDownloadBitrates = knownDownloadBitrates.Distinct().OrderByDescending(x => x).ToList();
                _downstreamCurrentBitRate = Convert.ToInt32(getDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "../../Channels/Channel/DownstreamCurrRate", knownDownloadBitrates, 10, 5));

                //speed found, return
                if (_downstreamCurrentBitRate >= 0)
                {
                    dsCurrBitRateDone = true;
                    return;
                }
                //not found, check every speed from 0 to 30000
                var valuesToCheck = Enumerable.Range(0, 30000).ToList();
                _downstreamCurrentBitRate = (int)getDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "../../Channels/Channel/DownstreamCurrRate", valuesToCheck, 75, 5);

            }
            dsCurrBitRateDone = true;
        }

        /// <summary>
        ///     Get current upload sync speed in kbps
        /// </summary>
        public void GetUpstreamCurrentBitRate()
        {
            if (DSLStandard == BBox3Tool.DSLStandard.VDSL2)
            {
                //check confirmed bitrates first (feedback from users)
                var knownUploadBitrates = _profiles.SelectMany(x => x.ConfirmedUploadSpeeds).ToList();
                knownUploadBitrates.AddRange(_profiles.Select(x => x.UploadSpeed));
                knownUploadBitrates = knownUploadBitrates.Distinct().OrderByDescending(x => x).ToList();
                _upstreamCurrentBitRate = Convert.ToInt32(getDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "../../Channels/Channel/UpstreamCurrRate", knownUploadBitrates, 10, 5));

                //speed found, return
                if (_upstreamCurrentBitRate >= 0)
                {
                    usCurrBitRateDone = true;
                    return;
                }

                //speed not found in confirmed bitrate list, check profile download speeds, but with margin of -64 to +64
                knownUploadBitrates.Clear();
                knownUploadBitrates.AddRange(_profiles.Select(x => x.UploadSpeed).SelectMany(x => Enumerable.Range(x - 64, 128)));
                knownUploadBitrates = knownUploadBitrates.Distinct().ToList();
                _upstreamCurrentBitRate = Convert.ToInt32(getDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "../../Channels/Channel/UpstreamCurrRate", knownUploadBitrates, 10, 5));

                //fallback: speed not found in profile list, check every speed (slow)
                if (_upstreamCurrentBitRate < 0)
                {
                    var valuesToCheck = Enumerable.Range(0, 50000).ToList();
                    _upstreamCurrentBitRate = (int)getDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "../../Channels/Channel/UpstreamCurrRate", valuesToCheck, 75, 5);
                }
            }
            else
            {
                var knownUploadBitrates = _profiles.Where(x => x.Name == "ADSL").Select(x => x.UploadSpeed).ToList();
                knownUploadBitrates = knownUploadBitrates.Distinct().OrderByDescending(x => x).ToList();
                _upstreamCurrentBitRate = Convert.ToInt32(getDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "../../Channels/Channel/UpstreamCurrRate", knownUploadBitrates, 10, 5));

                //speed found, return
                if (_upstreamCurrentBitRate >= 0)
                {
                    usCurrBitRateDone = true;
                    return;
                }
                //not found, check every speed from 0 to 6000
                var valuesToCheck = Enumerable.Range(0, 6000).ToList();
                _upstreamCurrentBitRate = (int)getDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "../../Channels/Channel/UpstreamCurrRate", valuesToCheck, 75, 5);

            }

            usCurrBitRateDone = true;
        }

        /// <summary>
        ///     Get DSL standard (VDSL2 / ADSL2+ / ADSL2 / ADSL
        /// </summary>
        public void GetDslStandard()
        {
            dynamic jsonObject = BBoxGetValue(new List<string>
            {
                "Device/DSL/Lines/Line[@uid=\"1\" and StandardUsed=\"G_993_2\"]/Status",
                "Device/DSL/Lines/Line[@uid=\"1\" and StandardUsed=\"G_993_2_ANNEX_A\"]/Status",
                "Device/DSL/Lines/Line[@uid=\"1\" and StandardUsed=\"G_993_2_ANNEX_B\"]/Status",
                "Device/DSL/Lines/Line[@uid=\"1\" and StandardUsed=\"G_992_5\"]/Status",
                "Device/DSL/Lines/Line[@uid=\"1\" and StandardUsed=\"G_992_5_ANNEX_A\"]/Status",
                "Device/DSL/Lines/Line[@uid=\"1\" and StandardUsed=\"G_992_5_ANNEX_B\"]/Status",
                "Device/DSL/Lines/Line[@uid=\"1\" and StandardUsed=\"G_992_3\"]/Status",
                "Device/DSL/Lines/Line[@uid=\"1\" and StandardUsed=\"G_992_3_ANNEX_A\"]/Status",
                "Device/DSL/Lines/Line[@uid=\"1\" and StandardUsed=\"G_992_3_ANNEX_B\"]/Status",
                "Device/DSL/Lines/Line[@uid=\"1\" and StandardUsed=\"G_992_1\"]/Status",
                "Device/DSL/Lines/Line[@uid=\"1\" and StandardUsed=\"G_992_1_ANNEX_A\"]/Status",
                "Device/DSL/Lines/Line[@uid=\"1\" and StandardUsed=\"G_992_1_ANNEX_B\"]/Status",
                "Device/DSL/Lines/Line[@uid=\"1\" and StandardUsed=\"G_992_3_ANNEX_L\"]/Status"
            });

            //check standard
            for (var i = 0; i < 13; i++)
            {
                if (jsonObject["reply"]["actions"][i]["error"]["description"] == "Applied")
                {
                    switch (i)
                    {
                        case 0:
                        case 1:
                        case 2:
                            DSLStandard = DSLStandard.VDSL2;
                            break;
                        case 3:
                        case 4:
                        case 5:
                            DSLStandard = DSLStandard.ADSL2plus;
                            break;
                        case 6:
                        case 7:
                        case 8:
                            DSLStandard = DSLStandard.ADSL2;
                            break;
                        case 9:
                        case 10:
                        case 11:
                        case 12:
                            DSLStandard = DSLStandard.ADSL;
                            break;
                        default:
                            DSLStandard = DSLStandard.unknown;
                            break;
                    }
                    break;
                }
            }
            DSLStandardDone = true;

        }

        /// <summary>
        ///     Check if vectoring is enabled on this line
        /// </summary>
        public void GetVectoringEnabled()
        {
            if (DSLStandard == BBox3Tool.DSLStandard.VDSL2)
            {
                //only correct after line reset
                dynamic jsonObject = BBoxGetValue(new List<string> { "Device/DSL/Lines/Line[VectoringState=\"RUNNING\"]/Status" });
                if (jsonObject["reply"]["actions"][0]["error"]["description"] == "Applied")
                    Vectoring = true;
                else
                {
                    var valuesToCheck = new List<int> { };
                    valuesToCheck.AddRange(Enumerable.Range(0, 50).ToList());
                    var inpDownstream = getDslValueParallel("Device/DSL/Lines/Line/Status[{0}]", "../../../Channels/Channel[@uid=\"1\"]/ACTINP", valuesToCheck, 10, 5);
                    if (inpDownstream > 30)
                        Vectoring = true;
                    else
                        Vectoring = null; // !! not false, because the above checks don't always return correct values
                }
            }
            else
                Vectoring = false;

            vectoringDone = true;
        }

        #endregion profile

        #region stats

        /// <summary>
        ///     Get downstream attenuation, check from 0 to 100 dB
        /// </summary>
        public void GetDownstreamAttenuation()
        {
            //check attenuation from 0.0 to 100.0
            var valuesToCheck = Enumerable.Range(0, 1000).ToList();
            _downstreamAttenuation = getDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "DownstreamAttenuation", valuesToCheck, 10, 5) / 10;
            dsAttenuationDone = true;
        }

        /// <summary>
        ///     Get upstream attenuation, check from 0 to 100 dB
        /// </summary>
        public void GetUpstreamAttenuation()
        {
            //special case, upstream attenuation always seems to be 0, so check 0 first to save requests
            var valuesToCheck = new List<int> { 0 };
            _upstreamAttenuation = getDslValueLinear("Device/DSL/Lines/Line[{0}]/Status", "UpstreamAttenuation", valuesToCheck, 1) / 10;

            if (_upstreamAttenuation < 0)
            {
                //check attenuation from 0.1 to 100.0
                valuesToCheck = Enumerable.Range(1, 1000).ToList();
                _upstreamAttenuation = getDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "UpstreamAttenuation", valuesToCheck, 10, 5) / 10;
            }
            usAttenuationDone = true;
        }

        /// <summary>
        ///     Get downstream noise margin, check from 0 to 100 dB
        /// </summary>
        public void GetDownstreamNoiseMargin()
        {
            //check noise margin from 0.0 to 50.0
            var valuesToCheck = new List<int> { 0 };
            valuesToCheck.AddRange(Enumerable.Range(60, 500).ToList());
            valuesToCheck.AddRange(Enumerable.Range(1, 59).ToList());
            _downstreamNoiseMargin = getDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "UpstreamNoiseMargin", valuesToCheck, 10, 5) / 10;
            dsNoiseMarginDone = true;
        }

        /// <summary>
        ///     Get upstream noise margin, check from 0 to 100 dB
        /// </summary>
        public void GetUpstreamNoiseMargin()
        {
            //check noise margin from 0.0 to 50.0
            var valuesToCheck = new List<int> { 0 };
            valuesToCheck.AddRange(Enumerable.Range(60, 500).ToList());
            valuesToCheck.AddRange(Enumerable.Range(1, 59).ToList());
            _upstreamNoiseMargin = getDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "DownstreamNoiseMargin", valuesToCheck, 10, 5) / 10;
            usNoiseMarginDone = true;
        }

        //---

        /// <summary>
        ///     Get downstream max bit rate, check from 0 to 150.000 kbps
        /// </summary>
        /// <returns>Downstream max bit rate in kbps, or 'unknown' if not found</returns>
        public void GetDownstreamMaxBitRate()
        {
            var valuesToCheck = new List<int>();

            //17a profiles
            if (_downstreamCurrentBitRate >= 39990)
            {
                var restMarginDown = _downstreamNoiseMargin - 6m;
                var startValue = _downstreamCurrentBitRate + Convert.ToInt32(restMarginDown * 3000);

                //check range + - 15.000 of predicted value
                for (int i = 0; i < 30; i++)
                {
                    valuesToCheck.AddRange(Enumerable.Range(startValue + (i * 500), 500));
                    valuesToCheck.AddRange(Enumerable.Range(startValue - (i * 500), 500));
                }
                _downstreamMaxBitRate = (int)getDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "DownstreamMaxBitRate", valuesToCheck);

                //out of predicted range

                if (_downstreamMaxBitRate < 0)
                {
                    valuesToCheck.Clear();

                    //add values form predicted value +15.000 to 150.000
                    if (startValue + 15000 < 150000)
                        valuesToCheck.AddRange(Enumerable.Range(startValue + 15000, 150000 - (startValue + 15000)));

                    //add values from predicted value -15.000 to 0
                    if (startValue - 15000 > 0)
                        valuesToCheck.AddRange(Enumerable.Range(0, startValue - 15000).OrderByDescending(x => x));

                    _downstreamMaxBitRate = (int)getDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "DownstreamMaxBitRate", valuesToCheck);
                }
            }
            //other profiles
            else
            {

                valuesToCheck.AddRange(Enumerable.Range(_downstreamCurrentBitRate, 150000 - _downstreamCurrentBitRate));
                _downstreamMaxBitRate = (int)getDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "DownstreamMaxBitRate", valuesToCheck);

                if (_downstreamMaxBitRate < 0)
                {
                    valuesToCheck.Clear();
                    valuesToCheck.AddRange(Enumerable.Range(0, _downstreamCurrentBitRate));
                    _downstreamMaxBitRate = (int)getDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "DownstreamMaxBitRate", valuesToCheck);
                }

            }
            dsMaxBitRateDone = true;

            //fix vectoring state
            if (_vectoring == null && vectoringDone)
            {
                if (_downstreamCurrentBitRate >= 69000 && _downstreamMaxBitRate >= 100000)
                    _vectoring = true;
            }
        }

        /// <summary>
        ///     Get upstream max bit rate, check from 0 to 50.000 kbps
        /// </summary>
        /// <returns>Upstream max bit rate in kbps, or 'unknown' if not found</returns>
        public void GetUpstreamMaxBitRate()
        {
            //17a profiles
            var valuesToCheck = new List<int>();
            if (_upstreamCurrentBitRate >= 6000)
            {
                var startValue = Convert.ToInt32(_upstreamCurrentBitRate + (_upstreamNoiseMargin * 1000m * (1m + (0.1m * (3m / _upstreamAttenuation)))));
                //adjustment <= 20.000
                if (startValue <= 20000)
                    startValue = Convert.ToInt32(startValue * 0.95);

                //check range + - 7.500 of predicted value
                for (int i = 0; i < 30; i++)
                {
                    valuesToCheck.AddRange(Enumerable.Range(startValue + (i * 250), 250));
                    valuesToCheck.AddRange(Enumerable.Range(startValue - (i * 250), 250));
                }
                _upstreamMaxBitRate = (int)getDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "UpstreamMaxBitRate", valuesToCheck);
                
                //out of predicted range
                if (_upstreamMaxBitRate < 0)
                {
                    valuesToCheck.Clear();

                    //add values form predicted value +7.500 to 50.000
                    if (startValue + 7500 < 50000)
                        valuesToCheck.AddRange(Enumerable.Range(startValue + 7500, 50000 - (startValue + 7500)));

                    //add values from predicted value -7.500 to 0
                    if (startValue - 7500 > 0)
                        valuesToCheck.AddRange(Enumerable.Range(0, startValue - 7500).OrderByDescending(x => x));

                    _upstreamMaxBitRate = (int)getDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "UpstreamMaxBitRate", valuesToCheck);
                }
            }
            else
            {
                valuesToCheck.AddRange(Enumerable.Range(_upstreamCurrentBitRate, 50000 - _upstreamCurrentBitRate));
                _upstreamMaxBitRate = (int)getDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "UpstreamMaxBitRate", valuesToCheck);

                if (_upstreamMaxBitRate < 0)
                {
                    valuesToCheck.Clear();
                    valuesToCheck.AddRange(Enumerable.Range(0, _upstreamCurrentBitRate));
                    _upstreamMaxBitRate = (int)getDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "UpstreamMaxBitRate", valuesToCheck);
                }
            }
            usMaxBitRateDone = true;
        }

        /// <summary>
        /// Get estimated distande, based on UPBOKLE
        /// </summary>
        public void GetEstimatedDistance()
        {
            switch (DSLStandard)
            {
                case DSLStandard.ADSL:
                    //based on stats
                    Distance = (1000m / 8.75m) * DownstreamAttenuation;
                    break;
                case DSLStandard.ADSL2:
                    Distance = null;
                    break;
                case DSLStandard.ADSL2plus:
                    Distance = null;
                    break;
                case DSLStandard.VDSL2:
                    var valuesToCheck = Enumerable.Range(0, 1280).ToList();
                    valuesToCheck = valuesToCheck.Select(x => x * 10).ToList();
                    decimal upbokle = getDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "UPBOKLE", valuesToCheck, 10, 5) / 100;

                    //v0.10
                    if (upbokle > 0)
                        Distance = (upbokle / (20m + (upbokle/5m))) * 1000;

                    //v0.9
                    /*if (upbokle > 0)
                        Distance = (upbokle / (17m + (upbokle / 2.2m))) * 1000;*/

                    //v0.8
                    /*if (upbokle > 0)
                        Distance = (upbokle / (20m + (upbokle/3m))) * 1000;*/

                    //v0.7
                    /*if (upbokle > 0)
                        Distance = (upbokle / 20m) * 1000;*/

                    break;
                case DSLStandard.unknown:
                default:
                    Distance = null;
                    break;
            }

            distanceDone = true;
        }

        #endregion

        #region private

        private decimal getDslValueParallel(string xpathBase, string node, int from, int to, int subStep)
        {
            var requestCount = 30;
            var xpathCount = 10;
            //--> 300 checks per request

            decimal dslValue = -1;
            var limitCheck = from + subStep;

            do
            {
                //prepare requests
                var actions = new List<string>();
                for (var i = from; i < (from + requestCount); i++)
                {
                    var xpathSub = string.Empty;
                    var list = new List<string>();
                    for (var j = 0; j < xpathCount; j++)
                        list.Add(node + "=\"" + ((j * subStep) + i) + "\"");
                    xpathSub = string.Join(" or ", list);

                    actions.Add(string.Format(xpathBase, xpathSub));
                }

                //get values from bbox
                dynamic jsonObject = BBoxGetValue(actions);

                //check values
                for (var i = 0; i < requestCount; i++)
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
            } while (dslValue == -1 && from < to);

            //not found
            if (dslValue == -1)
                return -1;

            //get precise value
            //prepare requests
            var actionsPrecise = new List<string>();
            for (var i = 0; i < xpathCount; i++)
                actionsPrecise.Add(string.Format(xpathBase, node + "=" + ((i * subStep) + dslValue)));

            //get values from bbox
            dynamic jsonObjectPrecise = BBoxGetValue(actionsPrecise);

            //check values
            for (var i = 0; i < xpathCount; i++)
            {
                if (jsonObjectPrecise["reply"]["actions"][i]["error"]["description"] == "Applied")
                {
                    dslValue += (i * subStep);
                    break;
                }
            }

            return dslValue;
        }

        private decimal getDslValueParallel(string xpathBase, string node, List<int> valuesToCheck, int orCount = 75, int xpathCount = 5)
        {
            var index = 0;
            var preciseRange = new List<int>();
            do
            {
                //prepare requests
                var range = Math.Min(orCount * xpathCount, valuesToCheck.Count - index);
                var rangeValues = valuesToCheck.GetRange(index, range);

                var actions = new List<string>();
                for (var i = 0; i < xpathCount; i++)
                {
                    var xpathSub = string.Empty;
                    var list = new List<string>();
                    for (var j = 0; j < orCount; j++)
                    {
                        var rangeIndex = ((i * orCount) + j);
                        if (rangeIndex >= rangeValues.Count)
                            break;
                        list.Add(node + "=" + rangeValues[rangeIndex]);
                    }
                    xpathSub = string.Join(" or ", list);
                    if (xpathSub != string.Empty)
                        actions.Add(string.Format(xpathBase, xpathSub));
                }

                //get values from bbox
                dynamic jsonObject = BBoxGetValue(actions);

                //check values
                for (var i = 0; i < actions.Count; i++)
                {
                    if (jsonObject["reply"]["actions"][i]["error"]["description"] == "Applied")
                    {
                        preciseRange = rangeValues.GetRange(i * orCount, Math.Min(orCount, rangeValues.Count - (i * orCount)));
                        break;
                    }
                }

                //increase start
                index += range;

            }
            while (preciseRange.Count == 0 && index < valuesToCheck.Count());

            //not found
            if (preciseRange.Count == 0)
                return -1;

            //get precise value
            return getDslValueLinear(xpathBase, node, preciseRange, 5);
        }

        /// <summary>
        ///     Get DSL value using linear algorithm
        /// </summary>
        /// <param name="xpathBase">Xpath base with {0} placeholder for Xpath node</param>
        /// <param name="node">Xpath node to check</param>
        /// <param name="valuesToCheck">List of values to check</param>
        /// <param name="requestRange">Ammount of Xpath requests to send at bbox in one webrequest (max 100)</param>
        /// <returns>DSL value, -1 if not found</returns>
        private decimal getDslValueLinear(string xpathBase, string node, List<int> valuesToCheck, int requestRange = 80)
        {
            decimal dslValue = -1;

            var from = 0;
            var ammount = 0;

            do
            {
                ammount = Math.Min(requestRange, valuesToCheck.Count() - from);

                //get subrange from list
                var subrange = valuesToCheck.GetRange(from, ammount);

                //prepare requests
                var actions =
                    subrange.Select(x => string.Format(xpathBase, node + "=\"" + x.ToString() + "\"")).ToList();

                //get values from bbox
                dynamic jsonObject = BBoxGetValue(actions);

                //check values
                for (var i = 0; i < actions.Count; i++)
                {
                    if (jsonObject["reply"]["actions"][i]["error"]["description"] == "Applied")
                    {
                        dslValue = subrange[i];
                        break; //from loop
                    }
                }

                //increase from
                from += ammount;
            } while (dslValue == -1 && from < valuesToCheck.Count());

            return dslValue;
        }

        private string getDslValueLinear(string xpathBase, string node, List<string> valuesToCheck, int requestRange = 80)
        {
            string dslValue = "";

            var from = 0;
            var ammount = 0;

            do
            {
                ammount = Math.Min(requestRange, valuesToCheck.Count() - from);

                //get subrange from list
                var subrange = valuesToCheck.GetRange(from, ammount);

                //prepare requests
                var actions =
                    subrange.Select(x => string.Format(xpathBase, node + "=\"" + x + "\"")).ToList();

                //get values from bbox
                dynamic jsonObject = BBoxGetValue(actions);

                //check values
                for (var i = 0; i < actions.Count; i++)
                {
                    if (jsonObject["reply"]["actions"][i]["error"]["description"] == "Applied")
                    {
                        dslValue = subrange[i];
                        break; //from loop
                    }
                }

                //increase from
                from += ammount;
            } while (dslValue == "" && from < valuesToCheck.Count());

            return dslValue;
        }


        /// <summary>
        ///     Get DSL value using exponential algorithm
        /// </summary>
        /// <param name="xpathBase">Xpath base with {0} placeholder for Xpath node</param>
        /// <param name="node">Xpath node to check</param>
        /// <param name="power">Check value to 2^power</param>
        /// <returns>DSL value, -1 if not found</returns>
        private decimal getDslValueExponentional(string xpathBase, string node, int power)
        {
            var greaterThen = Convert.ToInt32(Math.Pow(2, power));
            do
            {
                //get values from bbox
                dynamic jsonObject =
                    BBoxGetValue(new List<string> { string.Format(xpathBase, node + " > " + greaterThen) });
                //doens't work, always true :(  

                //check values
                power--;
                bool greater = (jsonObject["reply"]["actions"][0]["error"]["description"] == "Applied");
                if (greater)
                    greaterThen += Convert.ToInt32(Math.Pow(2, power));
                else
                    greaterThen -= Convert.ToInt32(Math.Pow(2, power));
            } while (power >= 1);

            return greaterThen;
        }

        /// <summary>
        ///     Make request for getValue to bbox and return the JSON-object as a dynamic
        /// </summary>
        /// <param name="xpaths">Xpaths to check</param>
        /// <returns>JSON reply from bbox</returns>
        private dynamic BBoxGetValue(List<string> xpaths)
        {
            //prepare actions
            var actions = new List<Dictionary<string, object>>();
            var i = 0;
            foreach (var xpath in xpaths)
            {
                actions.Add(new Dictionary<string, object>
                {
                    {"id", i},
                    {"method", "getValue"},
                    {"xpath", xpath}
                });
                i++;
            }

            var response = sendActionsToBBox(actions);
            response = response.Replace(",}", "}");
            //deserialize object
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<dynamic>(response);
        }

        /// <summary>
        ///     Make request for SubscribeForNotification to bbox and return the JSON-object as a dynamic
        /// </summary>
        /// <param name="xpaths">Xpaths to check</param>
        /// <returns>JSON reply from bbox</returns>
        private dynamic BBoxSubscribeForNotification(List<string> xpaths)
        {
            //prepare actions
            var actions = new List<Dictionary<string, object>>();
            var i = 0;
            foreach (var xpath in xpaths)
            {
                actions.Add(new Dictionary<string, object>
                {
                    {"id", i},
                    {"method", "subscribeForNotification"},
                    {"xpath", xpath},
                    {"parameters", new Dictionary<string, object> {
                        {"id", _notificationID},
                        {"type", "value-change"},
                        {"current-value", true}
                    }}
                });
                i++;
                _notificationID++;
            }

            var response = sendActionsToBBox(actions);

            //deserialize object
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<dynamic>(response);
        }

        /// <summary>
        ///     Make request to bbox and return the JSON-object as a string
        /// </summary>
        /// <param name="xpaths">Xpaths to check</param>
        /// <returns>JSON reply from bbox as string</returns>
        private string sendActionsToBBox(List<Dictionary<string, object>> actions)
        {
            //check thread cancel
            if (_worker.CancellationPending)
                throw new ThreadCancelledException("Request cancelled.");

            //calc local nonce
            _localNonce = SagemUtils.getLocalNonce();

            //create json object
            var jsonGetValue = new
            {
                request = new Dictionary<string, object>
                {
                    {"id", _requestID},
                    {"session-id", _sessionID},
                    {"priority", false},
                    {"actions", actions.ToArray()},
                    {"cnonce", Convert.ToUInt32(_localNonce)},
                    {"auth-key", SagemUtils.calcAuthKey(_username, _password, _requestID, _serverNonce, _localNonce)}
                }
            };

            //prepare data to send
            var jsonString = new JavaScriptSerializer().Serialize(jsonGetValue);
            var data = new Dictionary<string, string>();
            data.Add("req", jsonString);

            //send request & get response
            var response = NetworkUtils.sendRequest(_cgiReq, getCookies(), data, WebRequestMode.Post);

            //increase request id
            _requestID++;

            return response;
        }

        /// <summary>
        ///     Create the cookies to send with each webrequest
        /// </summary>
        /// <returns></returns>
        private CookieCollection getCookies()
        {
            var cookies = new CookieCollection();

            //set language cookie
            cookies.Add(new Cookie("lang", "en", "/", _bboxUrl.Host));

            //set session cookie
            var jsonCookie = new
            {
                request = new Dictionary<string, object>
                {
                    //{"request", (_requestID + 1)},
                    {"req_id", (_requestID + 1)},
                    {"sess_id", _sessionID},
                    {"basic", _basicAuth},
                    {"user", _username},
                    {"nonce", _serverNonce},
                    {"ha1", SagemUtils.calcHa1Cookie(_username, _password, _serverNonce)},
                    {
                        "dataModel",
                        new
                        {
                            name = "Internal",
                            nss = new[]
                            {
                                new
                                {
                                    name = "gtw",
                                    uri = "http://sagem.com/gateway-data"
                                }
                            }
                        }
                    }
                }
            };
            var cookieStr = new JavaScriptSerializer().Serialize(jsonCookie);
            cookies.Add(new Cookie("session", HttpUtility.UrlEncode(cookieStr), "/", _bboxUrl.Host));

            cookies[0].Expires = DateTime.Now.AddYears(1);
            cookies[1].Expires = DateTime.Now.AddDays(1);

            return cookies;
        }

        #endregion
    }
}