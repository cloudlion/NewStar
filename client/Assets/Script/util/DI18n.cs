using System;
//using System.IO;
using System.Net;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Bson;
using System.Collections.Generic;
using UnityEngine;
//using Mono.Data.Sqlite;
using System.Collections;
//using System.Text;
using System.Reflection;

namespace GameUtil
{	
	public class DI18n
	{
		public delegate void OutCompleteCallBack ();
		
		public static bool protobuf = false;
		public static DI18n _instance ;
//		private DbAccess db;
//		private SqliteDataReader sqReader;
		public string language;
		public Dictionary<String, String> diffTransDict;
		public Dictionary<String, String> transDict;
		public static OutCompleteCallBack completeCallBack;
//		public static Dictionary<String, kabam.ProtoLocaleIndex.LocaleHeadItem> localeIndexes;
//		public static FileStream localeHandler = null;
		public List<double> sqlite_costs;
		public List<double> protobuf_costs;
		
		private DI18n (string language)
		{
			this.language = language;
			this.diffTransDict = new Dictionary<string, string> ();
			this.transDict = new Dictionary<string, string> ();
//			sqlite_costs = new List<double>();
//			protobuf_costs = new List<double>();
		}
		
//		public static string DbPath(string dbCachebreaker)
//		{
//			string dbDirPath = Application.persistentDataPath + "/locales/";
//			if (!Directory.Exists (dbDirPath)) {
//				Directory.CreateDirectory (dbDirPath);
//			}
//			return dbDirPath + "locales_" + DoamBuild.GAME_VERSION + "_" + dbCachebreaker + ".sqdb";
//		}
//		
//		public DbAccess DbInstance ()
//		{
//			// Set db variable
//			string dbPath = DI18n.DbPath(ApplicationVO.localeDbCachebreaker);
//			// string dbPath = FileSystem.CacheFileDir ("locales") + "locales_" + ApplicationVO.localeDbCachebreaker + ".sqdb";
//			db = DbAccess.GetInstance (dbPath, DBType.LOCALE);
//
//			return db;
//		}
		
//		public static void Clear ()
//		{
////			int sqlite_cost_count = _instance.sqlite_costs.Count;
////			int proto_cost_count = _instance.protobuf_costs.Count;
////			double sqlite_total_cost = 0.0;
////			double proto_total_cost = 0.0;
////			
////			foreach(double i in _instance.sqlite_costs) {
////				sqlite_total_cost += i;
////			}
////			foreach(double i in _instance.protobuf_costs) {
////				proto_total_cost += i;
////			}
////			// DoamLogger.Fatal(StepStone.ErrorType.FeLogicErrorCrash, (int)type, "Error log: " + logString + stackTrace);
////			string log = "SQLite Total Count: " + sqlite_cost_count.ToString() + ", Total Cost: " + sqlite_total_cost.ToString() + ", Average Cost: " + (sqlite_total_cost / sqlite_cost_count).ToString() + " |||||| " + "Protobuf Total Count: " + proto_cost_count.ToString() + ", Total Cost: " + proto_total_cost.ToString() + ", Average Cost: " + (proto_total_cost / proto_cost_count).ToString();
////			
////			DoamLogger.Fatal(StepStone.ErrorType.NoError, 1, log);
//			
//			if (_instance != null) {
//				_instance.DbInstance().CloseSqlConnection();
//				_instance = null;
//			}
//		}


		public static void Load (string language, OutCompleteCallBack completeCallBack = null)
		{
			if (completeCallBack != null)
				DI18n.completeCallBack = completeCallBack;
			
			_instance = new DI18n(language);
//			if (DI18n.protobuf == true) {
//				LoadLocaleIndex(language);
//			} else {
//				LoadData (language);
//			}
		}

		public static void Update(string language, byte[] data, string version)
		{
#if COMPRESS
            //			GameUtil.FileSystem.WriteToCacheFile("lang", language, data, false);
            if (data != null) {
				_instance.transDict = JsonFx.Json.JsonReader.Deserialize<  Dictionary<string, string > >( System.Text.Encoding.UTF8.GetString(data) );
			}

#endif
        }

//		private static void LoadData (string language)
//		{
//			// string fileName = FileSystem.CacheFileDir ("locales") + "locales_" + ApplicationVO.localeDbCachebreaker + ".sqdb";
//			string fileName = DI18n.DbPath(ApplicationVO.localeDbCachebreaker);
//			
//			if (!FileSystem.CacheFileExists (fileName)) {
//				string offline_db = System.IO.Path.Combine (Application.streamingAssetsPath, "locales.sqdb");
//
//				FileSystem.CleanCacheDir(Application.persistentDataPath + "/locales/");
//				
//				#if UNITY_ANDROID && !UNITY_EDITOR
//					WWW reader = new WWW(offline_db);
//					while ( ! reader.isDone) {}
//					System.IO.File.WriteAllBytes(fileName, reader.bytes);
//				#else
//				if (File.Exists (offline_db)) {
//					File.Copy (offline_db, fileName);
//					#if UNITY_IPHONE
//					iPhone.SetNoBackupFlag(fileName);
//					#endif
//				}
//				#endif
//			}
//			
//			string stringCacheFileName = _instance.language + "_" + ApplicationVO.localeCachebreaker;
//			string cacheFilePath = FileSystem.CacheFilePath ("locales", stringCacheFileName);
//
//			bool cacheExists = false;
//			bool cacheIsBson = false;
//
//			if (FileSystem.CacheFileExists(cacheFilePath)) {
//			  	cacheExists = true;
//				byte[] localeJson = File.ReadAllBytes(cacheFilePath);
//					//UnityEngine.Debug.Log("manifestJson is " + cacheFilePath);
//				if(localeJson[localeJson.Length-1] == 0) //bson
//				{
//					cacheIsBson = true;
//					JsonSerializer serializer = new JsonSerializer();
//					var ms = new MemoryStream(localeJson);
//					BsonReader reader = new BsonReader(ms);
//					Dictionary<string,object> dic =  serializer.Deserialize<Dictionary<string,object>>(reader);
//					LoadDiffJson (dic);
//				}
//			} 
//
//			if( !cacheExists || !cacheIsBson) {
//				string url = DoamVersion.MAIN_VERSION + "/locales/" + ApplicationVO.realmId + "/" + language + "/" + ApplicationVO.localeDbCachebreaker + "_" + ApplicationVO.localeCachebreaker + "_" + DoamBuild.GAME_VERSION + ".json";
//				DServerConnection.instance.Get<Dictionary<String, object>> (url, LoadDiffJson, null, false, true, true);
//			}
//			
//		}
		
		static string MissingTrans(string key){
			return "&" + key;
		}
		
//		public static void LoadDefaultTrans ()
//		{
//			SqliteDataReader sqReader;
//			string findSql;
//			string key;
//			string trans;
//			findSql = "SELECT key, " + _instance.language + " FROM i18ns WHERE key LIKE 'research%' or  key like 'items%' or key like 'quests%' or key LIKE 'buildings%' or key LIKE 'common%' or key LIKE 'troops%'";
//			sqReader = _instance.DbInstance ().ExecuteQuery (findSql);
//			while (sqReader.Read ()) {
//				key = sqReader.GetString (0);
//				trans = sqReader.GetValue (1).ToString ();
//				if (trans == "") {
//					trans = MissingTrans(key);
//				} else {
//					
//					if (!_instance.transDict.ContainsKey (key)) {
//						_instance.transDict.Add (key, trans);
//					}
//				}
//			}
//			sqReader.Close ();
//			
//		}
		
		public static void ProcessDict (Dictionary<string, System.Object> dict, string prefix = null)
		{
			foreach (KeyValuePair<string, System.Object> item in dict) {
				if (item.Value == null)
					continue;
					
				Type val_type = item.Value.GetType ();
				Type dict_type = typeof(Dictionary<string,System.Object>);
				
				string nextP = prefix == null ? item.Key : prefix + "." + item.Key;
				if (val_type == dict_type) {
					ProcessDict (item.Value as Dictionary<string,System.Object>, nextP);
				} else if (val_type == typeof(string)) {
					_instance.diffTransDict.Add (nextP, item.Value.ToString ());
				}
			}
		}
		
		public static void LoadDiffJson (Dictionary<string, System.Object> out_dict)
		{
			if (out_dict != null) {
				ProcessDict (out_dict);
			}

			if (completeCallBack != null) {
				DI18n.completeCallBack ();
				DI18n.completeCallBack = null;
			}
		}
		
		public static bool IsInitialized ()
		{		
			return true;
			return _instance != null;
		}
		
		public static string T (string key)
		{
			return T (key, null);
		}

//		public static bool Exist(string key)
//		{
//			if (!IsInitialized ()) {
//				return HardCodeString.instance().Exist(key);
//			}
//			if (DI18n.protobuf) {
//				return _instance.ProtoLookupExist(key.ToLower().Trim());
//			} else {
//				return _instance.LookupExist(key.ToLower().Trim());
//			}
//		}
		
		public static string T (string key, System.Collections.Generic.IDictionary<String, System.Object> replace_pair)
		{
//			if (!IsInitialized ()) {
//				return HardCodeString.instance ().GetTranslation (key);
//			}
//			
			string i18n_value = null;//MissingTrans(key);
//			if (DI18n.protobuf == true) {
//				i18n_value = _instance.ProtoLookup (key.ToLower ().Trim ());
//			} else {
//				i18n_value = _instance.Lookup (key.ToLower ().Trim ());
//			}
//			return "jj";
			if(_instance == null)
				return i18n_value;
			i18n_value = _instance.Lookup (key.Trim ());
//			
			if (replace_pair == null) {
				i18n_value = i18n_value.Replace ("\\n", "\n");
			} else {
				i18n_value = ReplaceString (i18n_value, replace_pair).Replace ("\\n", "\n");
			}
			
			return i18n_value;
		}
		
		public static string ReplaceString (string s, System.Collections.Generic.IDictionary<String, System.Object> replace_pair)
		{
			string result = s;
			
			foreach (KeyValuePair<String,System.Object> pair in replace_pair) {
				if (typeof(String) == pair.Value.GetType ()) {
					result = result.Replace ("%{" + pair.Key + "}", pair.Value as String);
				} else if (typeof(int) == pair.Value.GetType ()) {
					result = result.Replace ("%{" + pair.Key + "}", ((int)pair.Value).ToString ());
				} else {
					result = result.Replace ("%{" + pair.Key + "}", pair.Value.ToString ());
				}
				
			}
			
			return result;
		}
		
//		public static bool Lookup (string key, out string result)
//		{
//			if (!IsInitialized ()) {
//				result = null;
//				return false;
//			} else {
//				if (DI18n.protobuf == true) {
//					return _instance.TryProtoLookup (key, out result);
//				} else {
//					return _instance.TryLookup (key, out result);
//				}
//				
//			}
//		}
//		
		private bool TryLookup (string key, out string result)
		{
//			key = key.Replace ("_", "-");
			string trans = null;
			result = null;
			
			try {
				if (diffTransDict.ContainsKey (key)) {
					trans = diffTransDict [key];
				} else if (transDict.ContainsKey (key)) {
					trans = transDict [key];
				} else {
//					string findSql = "SELECT " + language + " FROM i18ns WHERE key = '" + key + "'";
//					sqReader = DbInstance ().ExecuteQuery (findSql);
//					sqReader.Read ();
//					string re = sqReader.GetString (0);
//					
//					if (re != null && re != "") {
//						trans = re;
//						transDict.Add (key, trans);
//					}
					trans = MissingTrans(key);
				}
			} catch (Exception) {
				
			} finally {
//				if (sqReader != null) {
//					sqReader.Close ();
//				}
			}
			
			if (trans != null) {
				result = trans;
				return true;
			} else {
				return false;
			}
		}
//		
//		private bool TryProtoLookup (string key, out string result) {
//			key = key.Replace ("_", "-");
//			string trans = null;
//			result = null;
//			
//			if(!localeIndexes.ContainsKey(key)) {
//				return false;
//			}
//			
//			try {
//				kabam.ProtoLocaleIndex.LocaleHeadItem localeData = localeIndexes[key];
//				if (localeData == null) {
//					Debug.Log(key);
//					return false;
//				}
//				byte[] bytes = new byte[localeData.len];
//				localeHandler.Seek(localeData.offset, SeekOrigin.Begin);
//				localeHandler.Read(bytes, 0, localeData.len);
//				
//				trans = Encoding.UTF8.GetString(bytes);
//			} catch (Exception) {
//				
//			}
//			
//			if (trans != null) {
//				result = trans;
//				return true;
//			} else {
//				return false;
//			}
//		}
//
//		private bool LookupExist(string key)
//		{
//			key = key.Replace ("_", "-");
//			bool exist = false;
//			try {
//				if (diffTransDict.ContainsKey (key)) {
//					return true;
//				} else if (transDict.ContainsKey (key)) {
//					return true;
//				} else {
//					string findSql = "SELECT " + language + " FROM i18ns WHERE key = '" + key + "'";
//					sqReader = DbInstance ().ExecuteQuery (findSql);
//					if (sqReader.Read ()) {
//						string trans = sqReader.GetString(0);
//						if (trans == "") {
//							return false;
//						} else {
//							transDict.Add(key, trans);
//							return true;
//						}
//					}
//				}
//			} catch (Exception) {
//				return false;
//			} finally {
//				if (sqReader != null) {
//					sqReader.Close ();
//				}
//			}
//			return false;
//		}
//
		public  string Lookup (string key)
		{
	//		key = key.Replace ("_", "-");
			string trans = null;//MissingTrans(key);
//			try {
			/*	if (diffTransDict.ContainsKey (key)) {
					trans = diffTransDict [key];
				} 
				else*/ if (transDict.ContainsKey (key)) {
					trans = transDict [key];
//				} else {
//					string findSql = "SELECT " + language + " FROM i18ns WHERE key = '" + key + "'";
//					sqReader = DbInstance ().ExecuteQuery (findSql);
//					if (sqReader.Read ()) {
//						trans = sqReader.GetString (0);
//						if (trans == "") {
//							trans = MissingTrans(key);
//						} else {
//							transDict.Add (key, trans);
//						}
//					}
//					
				}
				else
				{
					trans = MissingTrans(key);
				}
//			} catch (Exception) {
//				trans = MissingTrans(key);
//			} 
			
			return trans;

		}
		
//		public static void LoadLocaleIndex(string language)
//		{
//			// string indexPath = System.IO.Path.Combine (Application.streamingAssetsPath, "locales/locales_index.bin");
//			string indexPath = System.IO.Path.Combine (Application.streamingAssetsPath, "locales/" + language + "_index.bin");
//			string localePath = System.IO.Path.Combine (Application.streamingAssetsPath, "locales/" + language + ".bin");
//			FileStream file = new FileStream(indexPath, FileMode.Open, FileAccess.Read);
//			localeHandler = new FileStream(localePath, FileMode.Open, FileAccess.Read);
//			
//			localeIndexes = new Dictionary<string, kabam.ProtoLocaleIndex.LocaleHeadItem>();
//			
//			kabam.ProtoLocaleIndex indexInstance = ProtoBuf.Serializer.Deserialize<kabam.ProtoLocaleIndex>(file);
//			foreach(kabam.ProtoLocaleIndex.LocaleHeadItem index in indexInstance.items) {
//				// localeIndexes[index.key] = index.locales_data.Find((mapping) => mapping.locale == language);
//				localeIndexes[index.key] = index;
//			}
//			if (completeCallBack != null) {
//				DI18n.completeCallBack ();
//				DI18n.completeCallBack = null;
//			}
//		}
//
//		private bool ProtoLookupExist(string key)
//		{
//			if (!localeIndexes.ContainsKey(key))
//				return false;
//			kabam.ProtoLocaleIndex.LocaleHeadItem localeData = localeIndexes[key];
//			if (localeData == null)
//				return false;
//			return true;
//		}
//		
//		private string ProtoLookup (string key)
//		{
//			key = key.Replace ("_", "-");
//			string trans = MissingTrans(key);
//			if(!localeIndexes.ContainsKey(key)) {
//				return trans;
//			}
//			kabam.ProtoLocaleIndex.LocaleHeadItem localeData = localeIndexes[key];
//			if (localeData == null) {
//				Debug.Log(key);
//				return trans;
//			}
//			byte[] bytes = new byte[localeData.len];
//			localeHandler.Seek(localeData.offset, SeekOrigin.Begin);
//			localeHandler.Read(bytes, 0, localeData.len);
//			
//			trans = Encoding.UTF8.GetString(bytes);
//			
//			return trans;
//
//		}
//		
//		public static void UpdateLocaleVersion ()
//		{
//			_instance.DbInstance ().UpdateIntoSpecific ("versions", new string[]{_instance.language}, new string[]{ApplicationVO.localeCachebreaker}, "id = 1");
//		}
		
	}
	
}

