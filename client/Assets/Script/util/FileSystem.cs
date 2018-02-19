using System;
using System.IO;
using UnityEngine;

namespace GameUtil
{
	public static class FileSystem
	{		
		public static string CacheFileDir(string cacheType = "")
		{
			string fileDir = Application.persistentDataPath + "/cache/" + cacheType + "/";
			
			if (!Directory.Exists (fileDir)) {
				Directory.CreateDirectory (fileDir);
			}
			
			return fileDir;
		}
		
		public static string CacheFilePath(string cacheType = "", string fileCacheBreaker = "") 
		{
			return CacheFileDir(cacheType) + fileCacheBreaker + ".json";
		}
		
		public static bool CacheFileExists(string filePath) 
		{
			return File.Exists(filePath);
		}
		
		public static bool WriteToCacheFile(string cacheType, string filePath, string fileContent, bool clearDir = true)
		{
			try{
				if (clearDir == true) {
					CleanCacheDir(CacheFileDir(cacheType));
				}

				File.WriteAllText(Path.Combine(CacheFileDir(cacheType), filePath), fileContent, System.Text.Encoding.UTF8);
				#if UNITY_IPHONE
				UnityEngine.iOS.Device.SetNoBackupFlag(filePath);
				#endif

				return true;
			} catch(Exception) {
				return false;
			}
		}

		public static bool WriteToCacheFile(string cacheType, string filePath, byte[] fileContent, bool clearDir = true)
		{
			try{
				if (clearDir == true) {
					CleanCacheDir(CacheFileDir(cacheType));
				}
				
				File.WriteAllBytes(Path.Combine(CacheFileDir(cacheType), filePath), fileContent);
				#if UNITY_IPHONE
				UnityEngine.iOS.Device.SetNoBackupFlag(filePath);
				#endif

				return true;
			} catch(Exception) {
				return false;
			}
		}
		
		public static string ReadAllText(string filePath)
		{
			return File.ReadAllText(filePath, System.Text.Encoding.UTF8);	
		}

		public static byte[] ReadAll(string filePath)
		{
			FileStream stream = null;
			byte[] bytes = null;
			try
			{
				stream = new FileStream(filePath, FileMode.Open,FileAccess.Read);
				bytes = new byte[stream.Length];
				stream.Read(bytes, 0, (int)stream.Length);
			}
			catch (Exception ex)
			{

			}
			finally
			{
				if(stream != null)
					stream.Close();
			}

			return bytes;
		}

		public static byte[] ReadAll(string cacheType, string filePath)
		{
			string path = Path.Combine (CacheFileDir (cacheType), filePath);
			return ReadAll (path);
		}
		
		public static bool CleanCacheDir(string cacheDir)
		{
			try{
				string[] filesInDir = Directory.GetFiles(cacheDir);
				foreach(string filePath in filesInDir) {
					File.Delete(filePath);
				}
				
				return true;
			} catch (Exception) {
				return false;
			}
			
		}

		public static bool CleanCache()
		{
			try
			{
				DirectoryInfo cacheDI = new DirectoryInfo(Application.persistentDataPath + "/cache/");
				if (cacheDI.Exists)
					cacheDI.Delete(true);

				return true;
			}
			catch
			{
				return false;
			}
		}
		
		public static bool CleanAllCache()
		{
			try
			{
				DirectoryInfo cacheDI = new DirectoryInfo(Application.persistentDataPath);
				if (cacheDI.Exists)
					cacheDI.Delete(true);
//				PlayerPrefs.DeleteAll();
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}

