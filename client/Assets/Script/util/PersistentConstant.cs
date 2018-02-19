using UnityEngine;
using System.Collections;


namespace GameUtil
{
    public static class PersistentConstant
    {
        public static void Write(string key, string val)
        {
            PlayerPrefs.SetString(key, val);
        }

        public static string Read(string key, string val)
        {
            if (!PlayerPrefs.HasKey(key))
                return "";
            return PlayerPrefs.GetString(key);
        }

    }

}