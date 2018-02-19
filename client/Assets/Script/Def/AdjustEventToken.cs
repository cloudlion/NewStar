using UnityEngine;
using System.Collections;

namespace com.aok.sdk {
	public class AdjustEventToken {

		public static string GetRegisterToken()
		{
			string registerToken = "";
			#if UNITY_ANDROID && !UNITY_EDITOR
			registerToken = "wm5xie";
			#else
			registerToken = "kdxfi8";
			#endif
			return registerToken;
		}

		public static string GetTutorial()
		{
			string tutorialToken = "";
			#if UNITY_ANDROID && !UNITY_EDITOR
			tutorialToken = "2vdx3k";
			#else
			tutorialToken = "26y2ay";
			#endif
			return tutorialToken;
		}
	}
}
