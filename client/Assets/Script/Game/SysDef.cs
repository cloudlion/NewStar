using UnityEngine;
using System;
using System.IO;


class SysDef
{
    //sys
    public const int TARGET_FPS = 30;
    public const float TARGET_FRAME_TIME = 1000.0f / TARGET_FPS;

    //version
    public static string Version = "0.0.0.0";

    //debug
    public static bool useSysDebug = true;
    public static bool isDebug = true;
    public static bool logToFile = false;
    public static bool logToGUI = false;
    public static bool showSysInfo = false;

    //config file
    public const string CONFIG_FILE = "config.txt";

    //res server
	public static string ASSET_MAP_FILE = "assets_manifest";
    public static string BUNDLE_MAP_FILE = "bundle_map.txt";
    public static string CHECKSUM_FILE = "bundle_info.txt";
    public static string LOCAL_RES_URL = "";
    public static string CACHE_FOLDER = "";
    public static string MAC_RES_SERVER_URL = "";
    public static string IPHONE_RES_SERVER_URL = "";
    public static string ANDROID_RES_SERVER_URL = "";

    //game server
    //public static string HTTP_SERVER_URL = "http://adam.link4age.com:9000/";
    //public static string SOCK_SERVER_IP = "adam.link4age.com";//"36.110.7.66";//;
    //public static int SOCK_SERVER_PORT = 6000;
    //public static string OTA_SERVER_URL = "http://adam.link4age.com:9000/";

    //public static string CHAT_SOCK_IP = "adam.link4age.com";//"36.110.7.66";

    //public static int CHAT_SOCK_PORT = 4000;


    //public static string HTTP_SERVER_URL = "http://adam.link4age.com:7100/";
    //public static string SOCK_SERVER_IP = //"36.110.7.66";
    //                                      "adam.link4age.com";
    //public static int SOCK_SERVER_PORT = 9500;
    //public static string OTA_SERVER_URL = "http://adam.link4age.com:7100/";

    //public static string CHAT_SOCK_IP = //"36.110.7.66";
    //                                    "adam.link4age.com";
    //public static int CHAT_SOCK_PORT = 4500;



    //beta game server
	public static string HTTP_SERVER_URL = "http://beta2.linkagegame.com:9000/";
    public static string SOCK_SERVER_IP = //"101.201.211.127";
											"beta2.linkagegame.com";
    public static int SOCK_SERVER_PORT = 1800;
	public static string OTA_SERVER_URL = "http://beta2.linkagegame.com:9000/";

    public static string CHAT_SOCK_IP =  //"101.201.211.127";
										"beta2.linkagegame.com";
    public static int CHAT_SOCK_PORT = 1900;


    public const string LANGUAGE = "en";
    public const string GAME_CONF_BUNDLE = "";

	public const int TimeOut = 60;
    public static void Init()
    {
//        Application.runInBackground = true;
        Application.targetFrameRate = TARGET_FPS;

        if (useSysDebug)
        {
            isDebug = Debug.isDebugBuild;
        }

        InitResPath();
    }

    public static void InitResPath()
    {
        LOCAL_RES_URL = Path.Combine(Application.streamingAssetsPath, "res");
        CACHE_FOLDER = Path.Combine(Application.persistentDataPath, "res");
        MAC_RES_SERVER_URL = "file://" + Path.GetFullPath(Path.Combine(Application.dataPath, "../../../mad_release/res/mac"));
//            IPHONE_RES_SERVER_URL = LOCAL_RES_URL;
        IPHONE_RES_SERVER_URL = "0.0.0.0";
        ANDROID_RES_SERVER_URL = "0.0.0.0";

        if (!Directory.Exists(CACHE_FOLDER))
        {
            Directory.CreateDirectory(CACHE_FOLDER);
#if UNITY_IPHONE
            UnityEngine.iOS.Device.SetNoBackupFlag(CACHE_FOLDER);
#endif
        }
    }
	

    public static void PrintInfo() {}

	public static void InitSysEnv()
	{
//		if (Debug.isDebugBuild) {
//			HTTP_SERVER_URL = "http://adam.link4age.com:9000/";
//			SOCK_SERVER_IP = "adam.link4age.com";//"36.110.7.66";//;
//			SOCK_SERVER_PORT = 6000;
//			OTA_SERVER_URL = "http://adam.link4age.com:9000/";
//			CHAT_SOCK_IP = "adam.link4age.com";//"36.110.7.66";
//			CHAT_SOCK_PORT = 4000;
//		} else {
//			HTTP_SERVER_URL = "http://cvc.linkagegame.com/";
//			SOCK_SERVER_IP = "cvc.linkagegame.com";
//			SOCK_SERVER_PORT = 1800;
//			OTA_SERVER_URL = "http://cvc.linkagegame.com/";
//			CHAT_SOCK_IP = "cvc.linkagegame.com";
//			CHAT_SOCK_PORT = 1900;
//		}

/*		if (GameVersion.IsDevelopVersion) {
			HTTP_SERVER_URL = "http://adam.link4age.com:9000/";
			SOCK_SERVER_IP = "adam.link4age.com";
			SOCK_SERVER_PORT = 1800;
			OTA_SERVER_URL = "http://adam.link4age.com:9000/";
			CHAT_SOCK_IP = "adam.link4age.com";
			CHAT_SOCK_PORT = 1900;
		}
		else {
            string prefix = "goc-v";
            if (RuntimePlatform.Android == Application.platform)
            {
                prefix = "goc-g";
            }
            HTTP_SERVER_URL = "https://" + prefix + GameVersion.Version + ".linkagegame.com:9900/";
			SOCK_SERVER_IP = prefix + GameVersion.Version + ".linkagegame.com";
			SOCK_SERVER_PORT = 1800;
			OTA_SERVER_URL = "https://" + prefix + GameVersion.Version + ".linkagegame.com:9900/";
			CHAT_SOCK_IP = prefix + GameVersion.Version + ".linkagegame.com";
			CHAT_SOCK_PORT = 1900;
		}*/
	}

	public static void ChooseServer(string name)
	{
        PlayerPrefs.SetString("ServerName", name);
		switch(name)
		{
		case "dev":
			HTTP_SERVER_URL = "http://adam.link4age.com:9000/";
			SOCK_SERVER_IP = "adam.link4age.com";
			SOCK_SERVER_PORT = 1800;
			OTA_SERVER_URL = "http://adam.link4age.com:9000/";
			CHAT_SOCK_IP = "adam.link4age.com";
			CHAT_SOCK_PORT = 1900;
			break;
		case "alpha":
			HTTP_SERVER_URL = "http://adam-alpha.link4age.com:9100/";
			SOCK_SERVER_IP = "adam-alpha.link4age.com";
			SOCK_SERVER_PORT = 2000;
			OTA_SERVER_URL = "http://adam-alpha.link4age.com:9100/";
			CHAT_SOCK_IP = "adam-alpha.link4age.com";
			CHAT_SOCK_PORT = 2100;
			break;
		case "beta":
			HTTP_SERVER_URL = "http://beta2.linkagegame.com:9000/";
			SOCK_SERVER_IP = "beta2.linkagegame.com";
			SOCK_SERVER_PORT = 1800;
			OTA_SERVER_URL = "https://beta2.linkagegame.com:9000/";
			CHAT_SOCK_IP = "beta2.linkagegame.com";
			CHAT_SOCK_PORT = 1900;
			break;
/*		case "product":
            string prefix = "goc-v";
            if (RuntimePlatform.Android == Application.platform)
            {
                prefix = "goc-g";
            }

            HTTP_SERVER_URL = "https://" + prefix + GameVersion.Version + ".linkagegame.com:9900/";
			SOCK_SERVER_IP = prefix + GameVersion.Version + ".linkagegame.com";
			SOCK_SERVER_PORT = 1800;
			OTA_SERVER_URL = "https://" + prefix + GameVersion.Version + ".linkagegame.com:9900/";
			CHAT_SOCK_IP = prefix + GameVersion.Version + ".linkagegame.com";
			CHAT_SOCK_PORT = 1900;
			break;*/
		}
	}
}

