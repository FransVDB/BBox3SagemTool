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
using BBox3Tool.enums;
using BBox3Tool.exception;
using BBox3Tool.objects;
using BBox3Tool.profile;
using BBox3Tool.utils;
// ReSharper disable RedundantArgumentDefaultValue

namespace BBox3Tool.session
{
    public class Bbox3Session : IModemSession
    {
        #region private members

        //worker thread
        private readonly BackgroundWorker _worker;

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
        private bool _lineConnected;
        private int _downstreamCurrentBitRate;
        private int _upstreamCurrentBitRate;
        private int _downstreamMaxBitRate;
        private int _upstreamMaxBitRate;
        private decimal _downstreamAttenuation;
        private decimal _upstreamAttenuation;
        private decimal _downstreamNoiseMargin;
        private decimal _upstreamNoiseMargin;
        private decimal? _distance;
        private bool _vectoringDown;
        private bool _vectoringUp;
        private bool _vectoringROPCapable;
        private DSLStandard _DSLStandard;
        private Annex _annex;

        //booleans for stats
        private bool _dsCurrBitRateDone, _usCurrBitRateDone;
        private bool _dsMaxBitRateDone, _usMaxBitRateDone;
        private bool _dsAttenuationDone, _usAttenuationDone;
        private bool _dsNoiseMarginDone, _usNoiseMarginDone;
        private bool _distanceDone;
        private bool _vectoringDownDone, _vectoringUpDone, _vectoringROPCapableDone;
        private bool _dslStandardDone;

        #endregion

        #region getters&setters

        public bool LoggedIn { get; private set; }

        public bool LineConnected
        {
            get
            {
                return CheckLineConnected();
            }
        }

        public decimal DownstreamAttenuation
        {
            get
            {
                if (!_dsAttenuationDone)
                    GetDownstreamAttenuation();
                return _downstreamAttenuation;
            }
            set { _downstreamAttenuation = value; }
        }

        public decimal UpstreamAttenuation
        {
            get
            {
                if (!_usAttenuationDone)
                    GetUpstreamAttenuation();
                return _upstreamAttenuation;
            }
            set { _upstreamAttenuation = value; }
        }

        public decimal DownstreamNoiseMargin
        {
            get
            {
                if (!_dsNoiseMarginDone)
                    GetDownstreamNoiseMargin();
                return _downstreamNoiseMargin;
            }
            set { _downstreamNoiseMargin = value; }
        }

        public decimal UpstreamNoiseMargin
        {
            get
            {
                if (!_usNoiseMarginDone)
                    GetUpstreamNoiseMargin();
                return _upstreamNoiseMargin;
            }
            set { _upstreamNoiseMargin = value; }
        }

        public int DownstreamMaxBitRate
        {
            get
            {
                if (!_dsMaxBitRateDone)
                    GetDownstreamMaxBitRate();
                return _downstreamMaxBitRate;
            }
            set { _downstreamMaxBitRate = value; }
        }

        public int UpstreamMaxBitRate
        {
            get
            {
                if (!_usMaxBitRateDone)
                    GetUpstreamMaxBitRate();
                return _upstreamMaxBitRate;
            }
            set { _upstreamMaxBitRate = value; }
        }

        public int DownstreamCurrentBitRate
        {
            get
            {
                if (!_dsCurrBitRateDone)
                    GetDownstreamCurrentBitRate();
                return _downstreamCurrentBitRate;
            }
            set { _downstreamCurrentBitRate = value; }
        }

        public int UpstreamCurrentBitRate
        {
            get
            {
                if (!_usCurrBitRateDone)
                    GetUpstreamCurrentBitRate();
                return _upstreamCurrentBitRate;
            }
            set { _upstreamCurrentBitRate = value; }
        }

        public decimal? Distance
        {
            get
            {
                if (!_distanceDone)
                    GetEstimatedDistance();
                return _distance;
            }
            set { _distance = value; }
        }

        public bool VectoringDown
        {
            get
            {
                if (!_vectoringDownDone)
                    GetVectoringDownEnabled();
                return _vectoringDown;
            }
            private set { _vectoringDown = value; }
        }

        public bool VectoringUp
        {
            get
            {
                if (!_vectoringUpDone)
                    GetVectoringUpEnabled();
                return _vectoringUp;
            }
            private set { _vectoringUp = value; }
        }

        public bool VectoringROPCapable
        {
            get
            {
                if (!_vectoringROPCapableDone)
                    GetVectoringROPCapable();
                return _vectoringROPCapable;
            }
            private set { _vectoringROPCapable = value; }
        }

        public bool VectoringDeviceCapable { get; private set; }

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
                if (!_dslStandardDone)
                    GetDslStandard();
                return _DSLStandard;
            }
            private set { _DSLStandard = value; }
        }

        public Annex Annex
        {
            get
            {
                if (!_dslStandardDone)
                    GetDslStandard();
                return _annex;
            }
            private set { _annex = value; }
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
            VectoringDown = false;
            VectoringUp = false;
            VectoringDeviceCapable = true;
            VectoringROPCapable = false;
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

            var deviceInfo = new DeviceInfo
            {
                HardwareVersion = jsonObject["reply"]["actions"][0]["callbacks"][0]["parameters"]["value"].ToString(),
                FirmwareVersion =
                    jsonObject["reply"]["actions"][2]["callbacks"][0]["parameters"]["value"].ToString() + " / " +
                    jsonObject["reply"]["actions"][1]["callbacks"][0]["parameters"]["value"].ToString()
            };

            InternalFirmwareVersion = jsonObject["reply"]["actions"][2]["callbacks"][0]["parameters"]["value"].ToString();
            GUIFirmwareVersion = jsonObject["reply"]["actions"][1]["callbacks"][0]["parameters"]["value"].ToString();

            try
            {
                double seconds = Convert.ToDouble(jsonObject["reply"]["actions"][3]["callbacks"][0]["parameters"]["value"]);
                DeviceUptime = TimeSpan.FromSeconds(seconds);
                seconds = Convert.ToDouble(jsonObject["reply"]["actions"][4]["callbacks"][0]["parameters"]["value"]);
                LinkUptime = TimeSpan.FromSeconds(seconds);

                deviceInfo.LinkUptime = LinkUptime.ToString("%d") + (LinkUptime.Days == 1 ? " day " : " days ") +
                                        LinkUptime.ToString("h\\:mm\\:ss");
                deviceInfo.DeviceUptime = DeviceUptime.ToString("%d") + (DeviceUptime.Days == 1 ? " day " : " days ") +
                                          DeviceUptime.ToString("h\\:mm\\:ss");
            }
            catch
            {
                // ignored
            }

            return deviceInfo;
        }

        #endregion

        #region debug

        public string GetDebugValue(string xpath)
        {
            //prepare actions
            var actions = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"id", 0},
                    {"method", "getValue"},
                    {"xpath", xpath},
                    {"options", new Dictionary<string, object>
                        {
                            {"depth", 10}
                        }
                    }
                }
            };
            return SendActionsToBBox(actions);
        }

        #endregion

        #region test

        public int GetDownstreamMaxBitRate2()
        {
            DownstreamMaxBitRate =
                (int)GetDslValueExponentional("Device/DSL/Lines/Line[{0}]/Status", "DownstreamMaxBitRate", 24);
            return DownstreamMaxBitRate;
        }

        public void GetTest()
        {
            var valuesToCheck = new List<string>();
            CultureInfo enUs = new CultureInfo("en-US");
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
                        valuesToCheck.Add(m1.ToString("0.0", enUs) + "," + m2.ToString("0.0", enUs) + "," + m3.ToString("0.0", enUs));
                    }
                }
            }

            Stopwatch watch = new Stopwatch();

            watch.Start();
            var test = GetDslValueLinear("Device/DSL/Lines/Line[{0}]/LastChange", "SNRMpbds", valuesToCheck, 5);
            watch.Stop();
            Debug.WriteLine("GetDslValueLinear 5: " + watch.Elapsed);

            //var valuesToCheck = Enumerable.Range(0, 1000).ToList();

            //watch.Start();
            //var test = GetDslValueLinear("Device/DSL/Lines/Line[{0}]/LastChange", "UpstreamNoiseMargin", valuesToCheck, 5) / 10;
            //watch.Stop();
            //Debug.WriteLine("GetDslValueLinear 5: " + watch.Elapsed.ToString());

            //watch.Restart();
            //test = GetDslValueParallel("Device/DSL/Lines/Line[{0}]/LastChange", "UpstreamNoiseMargin", valuesToCheck, 5, 5) / 10;
            //watch.Stop();
            //Debug.WriteLine("GetDslValueParallel 5 5: " + watch.Elapsed.ToString());

            //watch.Restart();
            //test = GetDslValueParallel("Device/DSL/Lines/Line[{0}]/LastChange", "UpstreamNoiseMargin", valuesToCheck, 10, 5) / 10;
            //watch.Stop();
            //Debug.WriteLine("GetDslValueParallel 10 5: " + watch.Elapsed.ToString());

            //watch.Restart();
            //test = GetDslValueParallel("Device/DSL/Lines/Line[{0}]/LastChange", "UpstreamNoiseMargin", valuesToCheck, 20, 5) / 10;
            //watch.Stop();
            //Debug.WriteLine("GetDslValueParallel 20 5: " + watch.Elapsed.ToString());

            //watch.Restart();
            //test = GetDslValueParallel("Device/DSL/Lines/Line[{0}]/LastChange", "UpstreamNoiseMargin", valuesToCheck, 50, 5) / 10;
            //watch.Stop();
            //Debug.WriteLine("GetDslValueParallel 50 5: " + watch.Elapsed.ToString());

            //watch.Restart();
            //valuesToCheck.Clear();
            //valuesToCheck.AddRange(Enumerable.Range(90000, 10000).ToList());
            //test = GetDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "DownstreamMaxBitRate", valuesToCheck, 75, 5);
            //watch.Stop();
            //Debug.WriteLine(" DownstreamMaxBitRate getDslValueParallel2: " + test + ", " + watch.Elapsed.ToString());


            //watch.Restart();
            //test = GetDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "DownstreamMaxBitRate", 90000, 100000, 1000);
            //watch.Stop();
            //Debug.WriteLine(" DownstreamMaxBitRate GetDslValueParallel: " + test + ", " + watch.Elapsed.ToString());

            //watch.Restart();
            //test = getDslValueSingleLinear("Device/DSL/Lines/Line[{0}]/LastChange", "UpstreamNoiseMargin", valuesToCheck) / 10;
            //watch.Stop();
            //Debug.WriteLine("getDslValueSingleLinear: " + watch.Elapsed.ToString());
        }

        public string GetDslam()
        {
            // Check known Broadcom ROP
            dynamic jsonObject = BBoxGetValue(new List<string>
            {
                "Device/DSL/Lines/Line[IDDSLAM='BDCM:0xb1ae']/Status", //BDCM
            });

            if (jsonObject["reply"]["actions"][0]["error"]["description"] == "Applied")
            {
                return "BDCM:0xb1ae";
            }

            //Check Broadcom/Ikanos rops
            string dslam;
            var valuesToCheck = new List<string>();
            for (int i = 0; i < 65535; i++)
            {
                valuesToCheck.Add("BDCM:0x" + i.ToString("x4"));
                valuesToCheck.Add("IKNS:0x" + i.ToString("x4"));
            }
            dslam = GetDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "IDDSLAM", valuesToCheck);

            if (!string.IsNullOrEmpty(dslam))
                return dslam;

            return "unknown";
        }

        #endregion


        #region login&logout

        /// <summary>
        ///     Login with given credentials
        /// </summary>
        /// <returns>Login successfull or not</returns>
        public bool OpenSession(string host, string username, string password)
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
                _localNonce = SagemUtils.GetLocalNonce();

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
                                                    {"depth", _debug ? 99 : 6}, //default 2, 6 = refresh
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
                        {"auth-key", SagemUtils.CalcAuthKey(_username, _password, _requestID, _serverNonce, _localNonce)}
                    }
                };

                //prepare data to send
                var jsonString = new JavaScriptSerializer().Serialize(jsonLogin);
                var data = new Dictionary<string, string> { { "req", jsonString } };

                //send request & get response
                var response = NetworkUtils.SendRequest(_cgiReq, GetCookies(), data, WebRequestMode.Post);

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

                //GetTest();
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
                _localNonce = SagemUtils.GetLocalNonce();

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
                            "auth-key", SagemUtils.CalcAuthKey(_username, _password, _requestID, _serverNonce, _localNonce)
                        }
                    }
                };

                //prepare data to send
                var jsonString = new JavaScriptSerializer().Serialize(jsonLogout);
                var data = new Dictionary<string, string> { { "req", jsonString } };

                //send request & get response
                var response = NetworkUtils.SendRequest(_cgiReq, GetCookies(), data, WebRequestMode.Post);

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

        /// <summary>
        /// Refresh line data
        /// </summary>
        public void RefreshData()
        {
            //request everything --> triggers refresh somehow
            BBoxGetValue(new List<string> { "*" }, 10);
        }

        #endregion

        #region profile

        public void GetLineData()
        {
            // Do nothing
        }

        /// <summary>
        ///     Get current download sync speed in kbps
        /// </summary>
        public void GetDownstreamCurrentBitRate()
        {
            if (DSLStandard == DSLStandard.VDSL2)
            {
                var checkedDownloadSpeeds = new List<int>();

                // Add all official speeds                
                var knownDownloadBitrates = _profiles.Select(x => x.DownloadSpeed).ToList();

                // Add all official speeds -1 / -2 / +63 / +64
                knownDownloadBitrates.AddRange(_profiles.Select(x => x.DownloadSpeed - 1));
                knownDownloadBitrates.AddRange(_profiles.Select(x => x.DownloadSpeed - 2));
                knownDownloadBitrates.AddRange(_profiles.Select(x => x.DownloadSpeed + 63));
                knownDownloadBitrates.AddRange(_profiles.Select(x => x.DownloadSpeed +64));

                //Add all user confirmed speeds
                knownDownloadBitrates.AddRange(_profiles.SelectMany(x => x.ConfirmedDownloadSpeeds));

                //Check these speeds
                knownDownloadBitrates = knownDownloadBitrates.Distinct().OrderByDescending(x => x).ToList();
                _downstreamCurrentBitRate = Convert.ToInt32(GetDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "../../Channels/Channel/DownstreamCurrRate", knownDownloadBitrates, 10, 5));

                checkedDownloadSpeeds.AddRange(knownDownloadBitrates);

                //speed found, return
                if (_downstreamCurrentBitRate >= 0)
                {
                    _dsCurrBitRateDone = true;
                    return;
                }

                //fallback 1
                //----------
                //speed not found in confirmed bitrate list, check profile download speeds, but with margin of -128 to +128
                knownDownloadBitrates.Clear();
                knownDownloadBitrates.AddRange(_profiles.Select(x => x.DownloadSpeed).SelectMany(x => Enumerable.Range(x - 128, 256)));
                knownDownloadBitrates = knownDownloadBitrates.Distinct().ToList();
                
                //remove checked speeds
                knownDownloadBitrates = knownDownloadBitrates.Except(checkedDownloadSpeeds).ToList();
                checkedDownloadSpeeds.AddRange(knownDownloadBitrates);

                //check speed
                _downstreamCurrentBitRate = Convert.ToInt32(GetDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "../../Channels/Channel/DownstreamCurrRate", knownDownloadBitrates, 5));

                //fallback 2
                //----------
                //check every mb, with a margin of -128 to +128
                if (_downstreamCurrentBitRate < 0)
                {
                    var valuesToCheck = new List<int>();
                    for (var i = 1; i < 140; i++)
                    {
                        valuesToCheck.AddRange(Enumerable.Range((i*1000) - 128, 256).ToList());
                    }

                    //remove checked speeds
                    valuesToCheck = valuesToCheck.Except(checkedDownloadSpeeds).ToList();
                    checkedDownloadSpeeds.AddRange(valuesToCheck);

                    //check speed
                    _downstreamCurrentBitRate = (int)GetDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "../../Channels/Channel/DownstreamCurrRate", valuesToCheck, 10, 5);   
                }

                //fallback 3
                //----------
                //check every speed possible (very slow)
                if (_downstreamCurrentBitRate < 0)
                {
                    var valuesToCheck = Enumerable.Range(0, 140000).ToList();
                    valuesToCheck = valuesToCheck.Except(checkedDownloadSpeeds).ToList();

                    //check speed
                    _downstreamCurrentBitRate = (int)GetDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "../../Channels/Channel/DownstreamCurrRate", valuesToCheck, 10, 5);
                }
            }
            else
            {
                var knownDownloadBitrates = _profiles.Where(x => x.LpName == "ADSL").Select(x => x.DownloadSpeed).ToList();
                knownDownloadBitrates = knownDownloadBitrates.Distinct().OrderByDescending(x => x).ToList();
                _downstreamCurrentBitRate = Convert.ToInt32(GetDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "../../Channels/Channel/DownstreamCurrRate", knownDownloadBitrates, 10, 5));

                //speed found, return
                if (_downstreamCurrentBitRate >= 0)
                {
                    _dsCurrBitRateDone = true;
                    return;
                }
                //not found, check every speed from 0 to 30000
                var valuesToCheck = Enumerable.Range(0, 30000).ToList();
                _downstreamCurrentBitRate = (int)GetDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "../../Channels/Channel/DownstreamCurrRate", valuesToCheck, 10, 5);

            }
            _dsCurrBitRateDone = true;
        }

        /// <summary>
        ///     Get current upload sync speed in kbps
        /// </summary>
        public void GetUpstreamCurrentBitRate()
        {
            if (DSLStandard == DSLStandard.VDSL2)
            {
                var checkedUploadSpeeds = new List<int>();

                // Add all official speeds                
                var knownUploadBitrates = _profiles.Select(x => x.UploadSpeed).ToList();

                // Add all official speeds -1 / -2 / +63 / +64
                knownUploadBitrates.AddRange(_profiles.Select(x => x.UploadSpeed - 1));
                knownUploadBitrates.AddRange(_profiles.Select(x => x.UploadSpeed - 2));
                knownUploadBitrates.AddRange(_profiles.Select(x => x.UploadSpeed + 63));
                knownUploadBitrates.AddRange(_profiles.Select(x => x.UploadSpeed + 64));

                //Add all user confirmed speeds
                knownUploadBitrates.AddRange(_profiles.SelectMany(x => x.ConfirmedUploadSpeeds));

                //Check these speeds
                knownUploadBitrates = knownUploadBitrates.Distinct().OrderByDescending(x => x).ToList();
                _upstreamCurrentBitRate = Convert.ToInt32(GetDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "../../Channels/Channel/UpstreamCurrRate", knownUploadBitrates, 10, 5));

                checkedUploadSpeeds.AddRange(knownUploadBitrates);

                //speed found, return
                if (_upstreamCurrentBitRate >= 0)
                {
                    _usCurrBitRateDone = true;
                    return;
                }

                //fallback 1
                //----------
                //speed not found in confirmed bitrate list, check profile download speeds, but with margin of -128 to +128
                knownUploadBitrates.Clear();
                knownUploadBitrates.AddRange(_profiles.Select(x => x.UploadSpeed).SelectMany(x => Enumerable.Range(x - 128, 256)));
                knownUploadBitrates = knownUploadBitrates.Distinct().ToList();

                //remove checked speeds
                knownUploadBitrates = knownUploadBitrates.Except(checkedUploadSpeeds).ToList();
                checkedUploadSpeeds.AddRange(knownUploadBitrates);

                //check speed
                _upstreamCurrentBitRate = Convert.ToInt32(GetDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "../../Channels/Channel/UpstreamCurrRate", knownUploadBitrates, 10, 5));


                //fallback 2
                //----------
                //check every mb, with a margin of -128 to +128
                if (_upstreamCurrentBitRate < 0)
                {
                    var valuesToCheck = new List<int>();
                    for (var i = 1; i < 50; i++)
                    {
                        valuesToCheck.AddRange(Enumerable.Range((i * 1000) - 128, 256).ToList());
                    }

                    //remove checked speeds
                    valuesToCheck = valuesToCheck.Except(checkedUploadSpeeds).ToList();
                    checkedUploadSpeeds.AddRange(valuesToCheck);

                    //check speed
                    _upstreamCurrentBitRate = (int)GetDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "../../Channels/Channel/UpstreamCurrRate", valuesToCheck, 10, 5);
                }

                //fallback 3
                //----------
                //check every speed possible (very slow)
                if (_upstreamCurrentBitRate < 0)
                {
                    var valuesToCheck = Enumerable.Range(0, 40000).ToList();
                    valuesToCheck = valuesToCheck.Except(checkedUploadSpeeds).ToList();
                    //check speed
                    _upstreamCurrentBitRate = (int)GetDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "../../Channels/Channel/UpstreamCurrRate", valuesToCheck, 10, 5);
                }
            }
            else
            {
                var knownUploadBitrates = _profiles.Where(x => x.LpName == "ADSL").Select(x => x.UploadSpeed).ToList();
                knownUploadBitrates = knownUploadBitrates.Distinct().OrderByDescending(x => x).ToList();
                _upstreamCurrentBitRate = Convert.ToInt32(GetDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "../../Channels/Channel/UpstreamCurrRate", knownUploadBitrates, 10, 5));

                //speed found, return
                if (_upstreamCurrentBitRate >= 0)
                {
                    _usCurrBitRateDone = true;
                    return;
                }
                //not found, check every speed from 0 to 6000
                var valuesToCheck = Enumerable.Range(0, 6000).ToList();
                _upstreamCurrentBitRate = (int)GetDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "../../Channels/Channel/UpstreamCurrRate", valuesToCheck, 10 ,5);

            }

            _usCurrBitRateDone = true;
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
                "Device/DSL/Lines/Line[@uid=\"1\" and StandardUsed=\"G_992_3_ANNEX_L\"]/Status",
                "Device/DSL/Lines/Line[@uid=\"1\" and StandardUsed=\"G_992_1\"]/Status",
                "Device/DSL/Lines/Line[@uid=\"1\" and StandardUsed=\"G_992_1_ANNEX_A\"]/Status",
                "Device/DSL/Lines/Line[@uid=\"1\" and StandardUsed=\"G_992_1_ANNEX_B\"]/Status",

            });

            //check standard
            for (var i = 0; i < 13; i++)
            {
                if (jsonObject["reply"]["actions"][i]["error"]["description"] == "Applied")
                {
                    switch (i)
                    {
                        //VDSL2
                        case 0:
                            Annex = Annex.unknown;
                            DSLStandard = DSLStandard.VDSL2;
                            break;
                        case 1:
                            Annex = Annex.A;
                            DSLStandard = DSLStandard.VDSL2;
                            break;
                        case 2:
                            Annex = Annex.B;
                            DSLStandard = DSLStandard.VDSL2;
                            break;

                        //ADSL2plus
                        case 3:
                            Annex = Annex.unknown;
                            DSLStandard = DSLStandard.ADSL2plus;
                            break;
                        case 4:
                            Annex = Annex.A;
                            DSLStandard = DSLStandard.ADSL2plus;
                            break;
                        case 5:
                            Annex = Annex.B;
                            DSLStandard = DSLStandard.ADSL2plus;
                            break;

                        //ADSL2
                        case 6:
                            Annex = Annex.unknown;
                            DSLStandard = DSLStandard.ADSL2;
                            break;
                        case 7:
                            Annex = Annex.A;
                            DSLStandard = DSLStandard.ADSL2;
                            break;
                        case 8:
                            Annex = Annex.B;
                            DSLStandard = DSLStandard.ADSL2;
                            break;
                        case 9:
                            Annex = Annex.L;
                            DSLStandard = DSLStandard.ADSL2;
                            break;

                        //ADSL
                        case 10:
                            Annex = Annex.unknown;
                            DSLStandard = DSLStandard.ADSL;
                            break;
                        case 11:
                            Annex = Annex.A;
                            DSLStandard = DSLStandard.ADSL;
                            break;
                        case 12:
                            Annex = Annex.B;
                            DSLStandard = DSLStandard.ADSL;
                            break;

                        default:
                            Annex = Annex.unknown;
                            DSLStandard = DSLStandard.unknown;
                            break;
                    }
                    break;
                }
            }
            _dslStandardDone = true;

        }

        /// <summary>
        ///     Check if vectoring down is enabled on this line
        /// </summary>
        public void GetVectoringDownEnabled()
        {
            if (DSLStandard == DSLStandard.VDSL2)
            {
                if (DownstreamCurrentBitRate >= 71000 || VectoringUp)
                    VectoringDown = true;
                else
                {
                    //only correct after line reset
                    var valuesToCheck = new List<int>();
                    valuesToCheck.AddRange(Enumerable.Range(0, 60).ToList());
                    var inpDownstream = GetDslValueParallel("Device/DSL/Lines/Line/Status[{0}]", "../../../Channels/Channel[@uid=\"1\"]/ACTINP", valuesToCheck, 10, 5);
                    VectoringDown = (inpDownstream >= 10);
                }
            }
            else
                VectoringDown = false;

            _vectoringDownDone = true;
        }

        /// <summary>
        ///     Check if vectoring up is enabled on this line
        /// </summary>
        public void GetVectoringUpEnabled()
        {
            if (DSLStandard == DSLStandard.VDSL2)
            {
                if (UpstreamCurrentBitRate >= 11000)
                    VectoringUp = true;
                else
                {
                    //only correct after line reset
                    var valuesToCheck = new List<int>();
                    valuesToCheck.AddRange(Enumerable.Range(0, 60).ToList());
                    var inpUpstream = GetDslValueParallel("Device/DSL/Lines/Line/Status[{0}]", "../../../Channels/Channel[@uid=\"1\"]/ACTINPus", valuesToCheck, 10, 5);
                    VectoringUp = (inpUpstream >= 10);
                }
            }
            else
                VectoringUp = false;

            _vectoringUpDone = true;
        }

        /// <summary>
        ///     Check if ROP is vectoring capable
        /// </summary>
        public void GetVectoringROPCapable()
        {
            if (VectoringDown || VectoringUp)
            {
                // Obviously...
                VectoringROPCapable = true;
            }
            else
            {
                // Check IDDSLAM
                dynamic jsonObject = BBoxGetValue(new List<string>
                {
                    "Device/DSL/Lines/Line[IDDSLAM='BDCM:0xb1ae']/Status", //BDCM
                    "Device/DSL/Lines/Line[IDDSLAM='IKNS:0x0000']/Status"  //IKNS
                 });

                // Broadcom ROP: vectoring capable
                if (jsonObject["reply"]["actions"][0]["error"]["description"] == "Applied")
                {
                    VectoringROPCapable = true;
                }

                // Ikanos ROP: not vectoring capable
                if (jsonObject["reply"]["actions"][1]["error"]["description"] == "Applied")
                {
                    VectoringROPCapable = false;
                }

                    //Unknown vendor, assume false(default value)
            }

            _vectoringROPCapableDone = true;
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
            _downstreamAttenuation = GetDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "DownstreamAttenuation", valuesToCheck, 10, 5) / 10;
            _dsAttenuationDone = true;
        }

        /// <summary>
        ///     Get upstream attenuation, check from 0 to 100 dB
        /// </summary>
        public void GetUpstreamAttenuation()
        {
            //special case, upstream attenuation always seems to be 0, so check 0 first to save requests
            var valuesToCheck = new List<int> { 0 };
            _upstreamAttenuation = GetDslValueLinear("Device/DSL/Lines/Line[{0}]/Status", "UpstreamAttenuation", valuesToCheck, 1) / 10;

            if (_upstreamAttenuation < 0)
            {
                //check attenuation from 0.1 to 100.0
                valuesToCheck = Enumerable.Range(1, 1000).ToList();
                _upstreamAttenuation = GetDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "UpstreamAttenuation", valuesToCheck, 10, 5) / 10;
            }
            _usAttenuationDone = true;
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
            _downstreamNoiseMargin = GetDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "DownstreamNoiseMargin", valuesToCheck, 10, 5) / 10;
            _dsNoiseMarginDone = true;
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
            _upstreamNoiseMargin = GetDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "UpstreamNoiseMargin", valuesToCheck, 10, 5) / 10;
            _usNoiseMarginDone = true;
        }

        //---

        /// <summary>
        ///     Get downstream max bit rate, check from 0 to 150.000 kbps
        /// </summary>
        public void GetDownstreamMaxBitRate()
        {
            _dsMaxBitRateDone = true;
            try
            {
                if (DSLStandard == DSLStandard.VDSL2 && _downstreamNoiseMargin > 0)
                {
                    //get VDSL2 profile
                    ProximusLineProfile profile = ProfileUtils.GetProfile(_profiles, UpstreamCurrentBitRate, DownstreamCurrentBitRate, VectoringDown, VectoringUp, Distance);
                    VDSL2Profile vdsl2Profile;
                    decimal maxDistance;
                    if (profile == null)
                    {
                        vdsl2Profile = ProfileUtils.GetVdsl2ProfileFallBack(DownstreamCurrentBitRate, UpstreamCurrentBitRate);
                        maxDistance = 0;
                    }
                    else
                    {
                        vdsl2Profile = profile.ProfileVDSL2;
                        maxDistance = profile.DistanceMax;
                    }

                    switch (vdsl2Profile)
                    {
                        case VDSL2Profile.p17a:
                            {
                                _downstreamMaxBitRate = _downstreamCurrentBitRate + Convert.ToInt32((_downstreamNoiseMargin - 6m) * 2900) + Convert.ToInt32(5000 / _downstreamAttenuation);

                                //corrections
                                if (_downstreamMaxBitRate >= 140000)
                                    _downstreamMaxBitRate = Convert.ToInt32(_downstreamMaxBitRate * 0.98);
                                else if (_downstreamMaxBitRate >= 138000)
                                    _downstreamMaxBitRate = Convert.ToInt32(_downstreamMaxBitRate * 0.985);
                                else if (_downstreamMaxBitRate >= 136000)
                                    _downstreamMaxBitRate = Convert.ToInt32(_downstreamMaxBitRate * 0.995);

                                return;
                            }
                        case VDSL2Profile.p8b:
                        case VDSL2Profile.p8d:
                            {
                                //zone 3
                                if (maxDistance <= 1000)
                                    _downstreamMaxBitRate = _downstreamCurrentBitRate + Convert.ToInt32((_downstreamNoiseMargin - 6m) * 1710);
                                //zone 4 & 5
                                else
                                    _downstreamMaxBitRate = _downstreamCurrentBitRate + Convert.ToInt32((_downstreamNoiseMargin - 6m) * 900);
                                return;
                            }
                    }
                }

                //ADSL mode of VDSL2 profile could not be determined, get max bitrate from bbox
                var valuesToCheck = new List<int>();
                if (_downstreamCurrentBitRate >= 39990)
                {
                    var startValue = _downstreamCurrentBitRate + Convert.ToInt32((_downstreamNoiseMargin - 6.5m) * 3200) + Convert.ToInt32(5000 / _downstreamAttenuation);
                    if (startValue < _downstreamCurrentBitRate)
                        startValue = _downstreamCurrentBitRate + 15000;

                    //check range + - 15.000 of predicted value
                    for (int i = 0; i < 30; i++)
                    {
                        valuesToCheck.AddRange(Enumerable.Range(startValue + (i * 500), 500));
                        valuesToCheck.AddRange(Enumerable.Range(startValue - (i * 500), 500));
                    }
                    _downstreamMaxBitRate = (int)GetDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "DownstreamMaxBitRate", valuesToCheck);

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

                        _downstreamMaxBitRate = (int)GetDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "DownstreamMaxBitRate", valuesToCheck);
                    }
                }
                else
                {
                    valuesToCheck.AddRange(Enumerable.Range(_downstreamCurrentBitRate, 150000 - _downstreamCurrentBitRate));
                    _downstreamMaxBitRate = (int)GetDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "DownstreamMaxBitRate", valuesToCheck);

                    if (_downstreamMaxBitRate < 0)
                    {
                        valuesToCheck.Clear();
                        valuesToCheck.AddRange(Enumerable.Range(0, _downstreamCurrentBitRate));
                        _downstreamMaxBitRate = (int)GetDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "DownstreamMaxBitRate", valuesToCheck);
                    }
                }

                //fix max values that are below current values
                if (_downstreamMaxBitRate < _downstreamCurrentBitRate)
                    _downstreamMaxBitRate = _downstreamCurrentBitRate;
            }
            catch(Exception ex)
            {
                _downstreamMaxBitRate = _downstreamCurrentBitRate;
            }
        }

        /// <summary>
        ///     Get upstream max bit rate, check from 0 to 50.000 kbps
        /// </summary>
        /// <returns>Upstream max bit rate in kbps, or 'Unknown' if not found</returns>
        public void GetUpstreamMaxBitRate()
        {
            _usMaxBitRateDone = true;
            try
            {
                if (DSLStandard == DSLStandard.VDSL2 && _upstreamNoiseMargin > 0)
                {
                    //get VDSL2 profile
                    ProximusLineProfile profile = ProfileUtils.GetProfile(_profiles, UpstreamCurrentBitRate, DownstreamCurrentBitRate, VectoringDown, VectoringUp, Distance);
                    VDSL2Profile vdsl2Profile;
                    decimal maxDistance;
                    if (profile == null)
                    {
                        vdsl2Profile = ProfileUtils.GetVdsl2ProfileFallBack(DownstreamCurrentBitRate, UpstreamCurrentBitRate);
                        maxDistance = 0;
                    }
                    else
                    {
                        vdsl2Profile = profile.ProfileVDSL2;
                        maxDistance = profile.DistanceMax;
                    }

                    switch (vdsl2Profile)
                    {
                        case VDSL2Profile.p17a:
                            {
                                //zone 1 & 2
                                _upstreamMaxBitRate = _upstreamCurrentBitRate + Convert.ToInt32((_upstreamNoiseMargin - 5m) * 1250 + 1000 * (1 + (8 - _downstreamAttenuation) / 15));
                                return;
                            }
                        case VDSL2Profile.p8b:
                        case VDSL2Profile.p8d:
                            {
                                //zone 3
                                if (maxDistance <= 1000)
                                    _upstreamMaxBitRate = _upstreamCurrentBitRate + Convert.ToInt32((_upstreamNoiseMargin - 6m) * 440);
                                //zone 4 & 5
                                else
                                    _upstreamMaxBitRate = _upstreamCurrentBitRate + Convert.ToInt32((_upstreamNoiseMargin - 6m) * 140);
                                return;
                            }
                    }
                }

                //17a profiles
                var valuesToCheck = new List<int>();
                if (_upstreamCurrentBitRate >= 6000)
                {
                    var startValue = Convert.ToInt32(_upstreamCurrentBitRate + Convert.ToInt32((_upstreamNoiseMargin - 6) * 1300 + 4000 * (1 + (8 - _downstreamAttenuation) / 15)));
                    if (startValue < _upstreamCurrentBitRate)
                        startValue = _upstreamCurrentBitRate + 7500;

                    //check range + - 7.500 of predicted value
                    for (int i = 0; i < 30; i++)
                    {
                        valuesToCheck.AddRange(Enumerable.Range(startValue + (i * 250), 250));
                        valuesToCheck.AddRange(Enumerable.Range(startValue - (i * 250), 250));
                    }

                    _upstreamMaxBitRate = (int)GetDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "UpstreamMaxBitRate", valuesToCheck);

                    //out of predicted range
                    if (_upstreamMaxBitRate < 0)
                    {
                        valuesToCheck.Clear();

                        //add values form predicted value +7.500 to 60.000
                        if (startValue + 7500 < 60000)
                            valuesToCheck.AddRange(Enumerable.Range(startValue + 7500, 60000 - (startValue + 7500)));

                        //add values from predicted value -7.500 to 0
                        if (startValue - 7500 > 0)
                            valuesToCheck.AddRange(Enumerable.Range(0, startValue - 7500).OrderByDescending(x => x));

                        _upstreamMaxBitRate = (int)GetDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "UpstreamMaxBitRate", valuesToCheck);
                    }
                }
                else
                {
                    valuesToCheck.AddRange(Enumerable.Range(_upstreamCurrentBitRate, 60000 - _upstreamCurrentBitRate));
                    _upstreamMaxBitRate = (int)GetDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "UpstreamMaxBitRate", valuesToCheck);

                    if (_upstreamMaxBitRate < 0)
                    {
                        valuesToCheck.Clear();
                        valuesToCheck.AddRange(Enumerable.Range(0, _upstreamCurrentBitRate));
                        _upstreamMaxBitRate = (int)GetDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "UpstreamMaxBitRate", valuesToCheck);
                    }
                }

                //fix max values that are below current values
                if (_upstreamMaxBitRate < _upstreamCurrentBitRate)
                    _upstreamMaxBitRate = _upstreamCurrentBitRate;
            }
            catch (Exception)
            {
                _upstreamMaxBitRate = _upstreamCurrentBitRate;
            }
        }

        //---

        /// <summary>
        /// Get estimated distande, based on UPBOKLE
        /// </summary>
        public void GetEstimatedDistance()
        {
            switch (DSLStandard)
            {
                case DSLStandard.ADSL:
                    //based on stats
                    Distance = (1000m / 9.6m) * DownstreamAttenuation;
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
                    decimal upbokle = GetDslValueParallel("Device/DSL/Lines/Line[{0}]/Status", "UPBOKLE", valuesToCheck, 10, 5) / 100;

                    //v0.11
                    if (upbokle > 0)
                        Distance = (upbokle / (20m + (upbokle / 4.1m))) * 1000;

                    //v0.10
                    /*if (upbokle > 0)
                        Distance = (upbokle / (20m + (upbokle/4.5m))) * 1000;*/

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
                default:
                    Distance = null;
                    break;
            }

            _distanceDone = true;
        }

        #endregion

        #region private

        /// <summary>
        ///     Get line status (connected/disconnected)
        /// </summary>
        private bool CheckLineConnected()
        {
            dynamic response = BBoxGetValue(new List<string> { "Device/DSL/Lines/Line/Status" });
            return (response["reply"]["actions"][0]["callbacks"][0]["parameters"]["value"].ToString() == "UP");
        }

        private decimal GetDslValueParallel(string xpathBase, string node, List<int> valuesToCheck, int orCount = 25, int xpathCount = 10)
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
                    var list = new List<string>();
                    for (var j = 0; j < orCount; j++)
                    {
                        var rangeIndex = (i * orCount) + j;
                        if (rangeIndex >= rangeValues.Count)
                            break;
                        list.Add(node + "=" + rangeValues[rangeIndex]);
                    }
                    var xpathSub = string.Join(" or ", list);
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
            while (preciseRange.Count == 0 && index < valuesToCheck.Count);

            //not found
            if (preciseRange.Count == 0)
                return -1;

            //get precise value
            return GetDslValueLinear(xpathBase, node, preciseRange, 5);
        }

        private string GetDslValueParallel(string xpathBase, string node, List<string> valuesToCheck, int orCount = 25, int xpathCount = 10)
        {
            var index = 0;
            var preciseRange = new List<string>();
            do
            {
                //prepare requests
                var range = Math.Min(orCount * xpathCount, valuesToCheck.Count - index);
                var rangeValues = valuesToCheck.GetRange(index, range);

                var actions = new List<string>();
                for (var i = 0; i < xpathCount; i++)
                {
                    var list = new List<string>();
                    for (var j = 0; j < orCount; j++)
                    {
                        var rangeIndex = (i * orCount) + j;
                        if (rangeIndex >= rangeValues.Count)
                            break;
                        list.Add(node + "='" + rangeValues[rangeIndex] + "'");
                    }
                    var xpathSub = string.Join(" or ", list);
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
            while (preciseRange.Count == 0 && index < valuesToCheck.Count);

            //not found
            if (preciseRange.Count == 0)
                return "";

            //get precise value
            return GetDslValueLinear(xpathBase, node, preciseRange, 5);
        }
        
        /// <summary>
        ///     Get DSL value using linear algorithm
        /// </summary>
        /// <param name="xpathBase">Xpath base with {0} placeholder for Xpath node</param>
        /// <param name="node">Xpath node to check</param>
        /// <param name="valuesToCheck">List of values to check</param>
        /// <param name="requestRange">Ammount of Xpath requests to send at bbox in one webrequest (max 100)</param>
        /// <returns>DSL value, -1 if not found</returns>
        private decimal GetDslValueLinear(string xpathBase, string node, List<int> valuesToCheck, int requestRange = 80)
        {
            decimal dslValue = -1;

            var from = 0;
            do
            {
                var ammount = Math.Min(requestRange, valuesToCheck.Count - from);

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
            } while (dslValue == -1 && from < valuesToCheck.Count);

            return dslValue;
        }

        private string GetDslValueLinear(string xpathBase, string node, List<string> valuesToCheck, int requestRange = 80)
        {
            string dslValue = "";

            var from = 0;
            do
            {
                var ammount = Math.Min(requestRange, valuesToCheck.Count - from);

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
            } while (dslValue == "" && from < valuesToCheck.Count);

            return dslValue;
        }

        /// <summary>
        ///     Get DSL value using exponential algorithm
        /// </summary>
        /// <param name="xpathBase">Xpath base with {0} placeholder for Xpath node</param>
        /// <param name="node">Xpath node to check</param>
        /// <param name="power">Check value to 2^power</param>
        /// <returns>DSL value, -1 if not found</returns>
        private decimal GetDslValueExponentional(string xpathBase, string node, int power)
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
        /// <param name="depth">How deep to check</param>
        /// <returns>JSON reply from bbox</returns>
        private dynamic BBoxGetValue(List<string> xpaths, int depth = 1)
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
                    {"xpath", xpath},
                    {"options", new Dictionary<string, object>
                        {
                            {"depth", depth}
                        }
                    }
                });
                i++;
            }

            var response = SendActionsToBBox(actions);
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

            var response = SendActionsToBBox(actions);

            //deserialize object
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<dynamic>(response);
        }

        /// <summary>
        ///     Make request to bbox and return the JSON-object as a string
        /// </summary>
        /// <param name="actions">Actions to execute</param>
        /// <returns>JSON reply from bbox as string</returns>
        private string SendActionsToBBox(List<Dictionary<string, object>> actions)
        {
            //check thread cancel
            if (_worker.CancellationPending)
                throw new ThreadCancelledException("Request cancelled.");

            //calc local nonce
            _localNonce = SagemUtils.GetLocalNonce();

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
                    {"auth-key", SagemUtils.CalcAuthKey(_username, _password, _requestID, _serverNonce, _localNonce)}
                }
            };

            //prepare data to send
            var jsonString = new JavaScriptSerializer().Serialize(jsonGetValue);
            var data = new Dictionary<string, string> { { "req", jsonString } };

            //send request & get response
            var response = NetworkUtils.SendRequest(_cgiReq, GetCookies(), data, WebRequestMode.Post);

            //increase request id
            _requestID++;

            return response;
        }

        /// <summary>
        ///     Create the cookies to send with each webrequest
        /// </summary>
        /// <returns></returns>
        private CookieCollection GetCookies()
        {
            var cookies = new CookieCollection { new Cookie("lang", "en", "/", _bboxUrl.Host) };

            //set language cookie

            //set session cookie
            var jsonCookie = new
            {
                request = new Dictionary<string, object>
                {
                    {"req_id", (_requestID + 1)},
                    {"sess_id", _sessionID},
                    {"basic", _basicAuth},
                    {"user", _username},
                    {"nonce", _serverNonce},
                    {"ha1", SagemUtils.CalcHa1Cookie(_username, _password, _serverNonce)},
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