using UnityEngine;
using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace GameUtil
{
    static public class SysUtil
    {
        public static bool isDebug = true;

        public static string CreateGUID()
        {
            return Guid.NewGuid().ToString("N");
        }

        public static void DebugBreak()
        {
            if (isDebug)
            {
                Debug.Break();
            }
        }

        public static void Assert(bool val, string msg = "")
        {
            if (isDebug && !val)
            {
                Logger.Log("assert failed:\n" + msg);
                Debug.Break();
            }
        }

        public static int MilliSecToFps(long time)
        {
            return 1000 / (int)time;
        }

        public static long FpsToMilliSec(int fps)
        {
            return 1000 / fps;
        }

        public static long TickToMilliSec(long tick)
        {
            return tick / (10 * 1000);
        }

        public static long MilliSecToTick(long time)
        {
            return time * 10 * 1000;
        }

        public static float MilliSecToSec(long ms)
        {
            return ((float)ms) / 1000;
        }

        public static long SecToMilliSec(float s)
        {
            return (long)(s * 1000);
        }

        public static string GetAddr(string ip, int port)
        {
            return ip + ":" + port;
        }

        public static string GetMD5Str(string str)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] encryptedBytes = md5.ComputeHash(Encoding.ASCII.GetBytes(str));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < encryptedBytes.Length; i++)
            {
                sb.AppendFormat("{0:x2}", encryptedBytes[i]);
            }
            return sb.ToString();
        }

        public static void DrawDebugRect(Rect rt, float d, Color col)
        {
            if (isDebug)
            {
                Vector3 src = Vector3.zero;
                Vector3 dst = Vector3.zero;

                src.z = d;
                dst.z = d;

                //left
                src.x = rt.x;
                src.y = rt.y;
                dst.x = rt.x;
                dst.y = rt.y + rt.height;
                UnityEngine.Debug.DrawLine(src, dst, col);

                //right
                src.x = rt.x + rt.width;
                src.y = rt.y;
                dst.x = rt.x + rt.width;
                dst.y = rt.y + rt.height;
                UnityEngine.Debug.DrawLine(src, dst, col);

                //bottom
                src.x = rt.x;
                src.y = rt.y;
                dst.x = rt.x + rt.width;
                dst.y = rt.y;
                UnityEngine.Debug.DrawLine(src, dst, col);

                //top
                src.x = rt.x;
                src.y = rt.y + rt.height;
                dst.x = rt.x + rt.width;
                dst.y = rt.y + rt.height;
                UnityEngine.Debug.DrawLine(src, dst, col);
            }
        }

        public static void DrawDebugDimension(Vector3 dim, MathUtil.BoundOriginType origin,
                                              Vector3 pos, Color col)
        {
            if (isDebug)
            {
                Bounds bd = MathUtil.DimensionToBound(dim, origin);
                Matrix4x4 mtx = Matrix4x4.identity;
                mtx.SetTRS(pos, Quaternion.identity, Vector3.one);
                DrawDebugBounds(bd, mtx, col);
            }
        }

        public static void DrawDebugBounds(Bounds bound, Matrix4x4 mtx, Color col)
        {
            if (isDebug)
            {
                Vector3 src = Vector3.zero;
                Vector3 dst = Vector3.zero;

                //bottom
                src.x = bound.min.x;
                src.y = bound.min.y;
                src.z = bound.min.z;
                dst.x = bound.max.x;
                dst.y = bound.min.y;
                dst.z = bound.min.z;
                src = mtx.MultiplyPoint(src);
                dst = mtx.MultiplyPoint(dst);
                UnityEngine.Debug.DrawLine(src, dst, col);

                src.x = bound.min.x;
                src.y = bound.min.y;
                src.z = bound.min.z;
                dst.x = bound.min.x;
                dst.y = bound.min.y;
                dst.z = bound.max.z;
                src = mtx.MultiplyPoint(src);
                dst = mtx.MultiplyPoint(dst);
                UnityEngine.Debug.DrawLine(src, dst, col);

                src.x = bound.max.x;
                src.y = bound.min.y;
                src.z = bound.max.z;
                dst.x = bound.max.x;
                dst.y = bound.min.y;
                dst.z = bound.min.z;
                src = mtx.MultiplyPoint(src);
                dst = mtx.MultiplyPoint(dst);
                UnityEngine.Debug.DrawLine(src, dst, col);

                src.x = bound.max.x;
                src.y = bound.min.y;
                src.z = bound.max.z;
                dst.x = bound.min.x;
                dst.y = bound.min.y;
                dst.z = bound.max.z;
                src = mtx.MultiplyPoint(src);
                dst = mtx.MultiplyPoint(dst);
                UnityEngine.Debug.DrawLine(src, dst, col);

                //top
                src.x = bound.max.x;
                src.y = bound.max.y;
                src.z = bound.max.z;
                dst.x = bound.max.x;
                dst.y = bound.max.y;
                dst.z = bound.min.z;
                src = mtx.MultiplyPoint(src);
                dst = mtx.MultiplyPoint(dst);
                UnityEngine.Debug.DrawLine(src, dst, col);

                src.x = bound.max.x;
                src.y = bound.max.y;
                src.z = bound.max.z;
                dst.x = bound.min.x;
                dst.y = bound.max.y;
                dst.z = bound.max.z;
                src = mtx.MultiplyPoint(src);
                dst = mtx.MultiplyPoint(dst);
                UnityEngine.Debug.DrawLine(src, dst, col);

                src.x = bound.min.x;
                src.y = bound.max.y;
                src.z = bound.min.z;
                dst.x = bound.max.x;
                dst.y = bound.max.y;
                dst.z = bound.min.z;
                src = mtx.MultiplyPoint(src);
                dst = mtx.MultiplyPoint(dst);
                UnityEngine.Debug.DrawLine(src, dst, col);

                src.x = bound.min.x;
                src.y = bound.max.y;
                src.z = bound.min.z;
                dst.x = bound.min.x;
                dst.y = bound.max.y;
                dst.z = bound.max.z;
                src = mtx.MultiplyPoint(src);
                dst = mtx.MultiplyPoint(dst);
                UnityEngine.Debug.DrawLine(src, dst, col);

                //side
                src.x = bound.min.x;
                src.y = bound.min.y;
                src.z = bound.min.z;
                dst.x = bound.min.x;
                dst.y = bound.max.y;
                dst.z = bound.min.z;
                src = mtx.MultiplyPoint(src);
                dst = mtx.MultiplyPoint(dst);
                UnityEngine.Debug.DrawLine(src, dst, col);

                src.x = bound.max.x;
                src.y = bound.min.y;
                src.z = bound.min.z;
                dst.x = bound.max.x;
                dst.y = bound.max.y;
                dst.z = bound.min.z;
                src = mtx.MultiplyPoint(src);
                dst = mtx.MultiplyPoint(dst);
                UnityEngine.Debug.DrawLine(src, dst, col);

                src.x = bound.max.x;
                src.y = bound.max.y;
                src.z = bound.max.z;
                dst.x = bound.max.x;
                dst.y = bound.min.y;
                dst.z = bound.max.z;
                src = mtx.MultiplyPoint(src);
                dst = mtx.MultiplyPoint(dst);
                UnityEngine.Debug.DrawLine(src, dst, col);

                src.x = bound.min.x;
                src.y = bound.min.y;
                src.z = bound.max.z;
                dst.x = bound.min.x;
                dst.y = bound.max.y;
                dst.z = bound.max.z;
                src = mtx.MultiplyPoint(src);
                dst = mtx.MultiplyPoint(dst);
                UnityEngine.Debug.DrawLine(src, dst, col);
            }
        }

        public static KeyCode TransKeyToKeyCode(int key)
        {
            return (KeyCode)key;
        }

        public static int TransKeyCodeToKey(KeyCode code)
        {
            return (int)code;
        }

        //lower str, then rm all space
        public static string GetPureStr(string txt)
        {
            string pt = txt.ToLower();
            pt = pt.Trim();
            pt = pt.Replace('\t', '_');
            pt = pt.Replace(' ', '_');
            pt = pt.Replace('-', '_');
            return pt;
        }

        public static bool StrPureEqual(string txt1, string txt2)
        {
            string pt1 = GetPureStr(txt1);
            string pt2 = GetPureStr(txt2);
            return pt1 == pt2;
        }

        public static bool DetectEnterKeyInStr(string str)
        {
            if (str == null || str == string.Empty)
            {
                return false;
            }

            return str[str.Length - 1] == '\n';
        }

        public static string RmEnterKeyInStr(string str)
        {
            if (str == null || str == string.Empty)
            {
                return str;
            }

            string nStr = str;
            while (nStr.Length > 0)
            {
                if (nStr[nStr.Length - 1] == '\n' || nStr[nStr.Length - 1] == '\r')
                {
                    nStr = nStr.Remove(nStr.Length - 1);
                }
                else
                {
                    break;
                }
            }

            return nStr;
        }

        public static void ClearGuiContent(GUIContent content)
        {
            content.image = null;
            content.text = string.Empty;
            content.tooltip = string.Empty;
        }

        public static bool IsEqual(float f1,float f2)
        {
            return System.Math.Abs(f1 - f2) < 0.00001f;
        }

        public static bool IsEqual(Vector3 v1,Vector3 v2)
        {
            return IsEqual(v1.x, v2.x) && IsEqual(v1.y, v2.y) && IsEqual(v1.z, v2.z);
        }

        public static GameObject CreateEmptyGameObj(string name)
        {
            GameObject go = new GameObject();
            go.name = name;
            UnityEngine.Object.DontDestroyOnLoad(go);
            return go;
        }
        public static Dictionary<string, GameObject> GetNGUIElementControls(GameObject source)
        {
            Dictionary<string, GameObject> controls = null;
            if (controls == null)
            {
                List<GameObject> items = new List<GameObject>();
                var widgets = source.GetComponentsInChildren<MonoBehaviour>(true);
                for (int i = 0; i < widgets.Length; ++i)
                {
                    if (!items.Contains(widgets[i].gameObject))
                    {
                        items.Add(widgets[i].gameObject);
                    }
                }
                controls = new Dictionary<string, GameObject>();
                for (int i = 0; i < items.Count; ++i)
                {
                    if (!controls.ContainsKey(items[i].name))
                    {
                        controls.Add(items[i].name, items[i]);
                    }
                }
            }
            return controls;
        }

        public static int StringToHash(string ss)
        {
            // todo: need a better solution for this hash function
            char[] str = ss.ToCharArray();
            uint hash = 0;
            uint x  = 0;
            uint i =0;
            uint seed = 131;
            while (i < str.Length)
            {
                hash = (hash << 4) * seed + (str[i++]);
                if ((x = hash & 0xF0000000) != 0)
                {
                    hash ^= (x >> 24);
                    hash &= ~x;
                }
            }

            return (int)(hash & 0x7FFFFFFF);
        }

        public static string SecToStr(float sec)
        {
            int t = (int)sec;
            int h = t / 3600;
            int m = (t % 3600) / 60;
            int s = (t % 3600) % 60;

            string sh = h.ToString();
            if (h < 10)
            {
                sh = "0" + sh;
            }

            string sm = m.ToString();
            if (m < 10)
            {
                sm = "0" + sm;
            }

            string ss = s.ToString();
            if (s < 10)
            {
                ss = "0" + ss;
            }

            string str = "";
            if (h != 0)
            {
                str = str + sh + ":";
            }
            if (h != 0 || m != 0)
            {
                str = str + sm + ":";
            }
            if (h != 0 || m != 0 || s != 0)
            {
                str = str + ss;
            }

            return str;
        }

        public static long currentTimeMillis()
        {
            DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan span = DateTime.UtcNow - Jan1st1970;
            return (long)span.TotalMilliseconds;
        }

		public static int CompareVerision(string version1, string version2)
		{
			if (string.IsNullOrEmpty (version1) && string.IsNullOrEmpty (version2)) {
				return 0;		
			}

			if (string.IsNullOrEmpty (version1) )
			    return -1;

			  if (string.IsNullOrEmpty (version2))
			    return 1;  

			string[] num1 = version1.Split('.');
			string[] num2 = version2.Split ('.');

			int mainVersion1 = int.Parse (num1 [0]);
			int mainVersion2 = int.Parse (num2 [0]);
			if (mainVersion1 > mainVersion2)
				return 1;
			if (mainVersion1 < mainVersion2)
				return -1;

			int subversion1 = int.Parse (num1 [1]);
			int subversion2 = int.Parse (num1 [1]);
			if (subversion1 > subversion2)
				return 1;
			if (subversion1 < subversion2)
				return -1;
			else
				return 0;
		}

		public static string GetMD5(byte[] data)
		{
			MD5 md5 = new MD5CryptoServiceProvider ();
			byte[] result = md5.ComputeHash (data);
			StringBuilder sb = new StringBuilder ();
			for (int i = 0; i < result.Length; i++) {
				byte b = result [i];
				sb.Append (Convert.ToString (b, 16));
			}
			return sb.ToString ();

//			return BitConverter.ToString(result).Replace("-", "").ToLower();
		}
    }
}
