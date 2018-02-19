using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
#if MVC
using MVC;
#endif
public class NativeCaller{
#if UNITY_ANDROID
	private static AndroidJavaObject mNativeObject = null;
	private static AndroidJavaObject mDeviceNativeObject = null;
	private static AndroidJavaObject mActivity = null;
	private static AndroidJavaObject mAppRaterNativeObject = null;

	private static AndroidJavaObject GetNativeObject()
	{
		if(mNativeObject == null)
		{
			mNativeObject = new AndroidJavaObject("com.linkage.aok.UnityCallPlugin",new object[]{});
		}
		return mNativeObject;
	}
	
	private static AndroidJavaObject GetActivity()
	{
		if( mActivity == null)
		{
			AndroidJavaClass player = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
			mActivity = player.GetStatic<AndroidJavaObject>("currentActivity");
		}
		return mActivity;
	}

	public static void InitPayment()
	{
		AndroidJavaObject native = GetNativeObject();
		native.Call("InitPayment", new object[] { GameVersion.paymentDebug });
	}

	public static void GetServerProductIds()
	{
		List<GDSPayout> payouts = ProxyMgr.Instance.GetProxy<GDSProxy> ().GetPayouts ();
		string[] productIds = new string[payouts.Count];
		if (payouts != null && payouts.Count > 0) {
			for (int i = 0; i < payouts.Count; i++) {
				productIds [i] = payouts [i].productId;
			}
		}
		AndroidJavaObject native = GetNativeObject();
		native.Call("getServerProductIds",new object[]{productIds});
	}

	public static void TransitionFinish()
	{
		AndroidJavaObject native = GetNativeObject();
		native.Call("transitionFinish",new object[]{});
	}

	public static void BuyProduct(string sku,string verifyCode)
	{
		AndroidJavaObject native = GetNativeObject();
		native.Call("buyProduct",new object[]{sku,verifyCode});
	}

	private static void _copyToClipboard( string input)
	{
		AndroidJavaObject native = GetNativeObject();
		native.Call("CopyToClipboard",new object[]{GetActivity(), input});
	}

	public static void SendNotification(int id , long delayTime , string message)
	{
		AndroidJavaObject native = new AndroidJavaObject("com.linkage.aok.notification.UnityNotificationManager",new object[]{});
		native.Call("SetNotification",new object[]{id, delayTime , message });
	}
	public static void CancelNotification(int id)
	{
		AndroidJavaObject native = new AndroidJavaObject("com.linkage.aok.notification.UnityNotificationManager",new object[]{});
		native.Call("CancelNotification",new object[]{id});
	}
	public static void CancelAllNotification()
	{
		AndroidJavaObject native = new AndroidJavaObject("com.linkage.aok.notification.UnityNotificationManager",new object[]{});
		native.Call("CancelAll",new object[]{ });
	}
#endif


	[DllImport ("__Internal")]
	private static extern string _MobileId();
	public static string GetMobileId()
	{
		
		if(Application.isEditor)
		{
			return "";	
		}
#if UNITY_IPHONE
		return _MobileId();
#endif
		
#if UNITY_ANDROID
		AndroidJavaObject native = GetNativeObject();

		string mobileId = native.Call<String>("getMobileId",new object[]{GetActivity()});
		return getMD5Hash("59a1cc5e4b7af16131a80973739b783f"+mobileId);
#endif
		return "";
	}

	[DllImport ("__Internal")]
	private static extern string _CountryCode();
	public static string GetCountryCode()
	{
		if(Application.isEditor)
		{
			return "";	
		}
#if UNITY_IPHONE
		return _CountryCode();
#endif

#if UNITY_ANDROID
#endif

		return "";
	}

	public static string getMD5Hash(string str)
	{
		MD5 md = new MD5CryptoServiceProvider();
		byte[] bytes = Encoding.UTF8.GetBytes(str);
		bytes = md.ComputeHash(bytes);
		StringBuilder builder = new StringBuilder();
		int index = 0;
		byte[] buffer2 = bytes;
		int length = buffer2.Length;
		while (index < length)
		{
			builder.Append(buffer2[index].ToString("x2").ToLower());
			index++;
		}
		return builder.ToString();
	}

	[DllImport ("__Internal")]
	private static extern uint _MemoryUsage ();
	
	public static uint MemoryUsage ()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			return _MemoryUsage ();
		} else {
			return 0;
		}
	}

#if UNITY_IOS
	[DllImport ("__Internal")]
	private static extern float _IOSVersion();
#endif

	public static string OsVersion()
	{
		string osver = "0";
#if UNITY_IOS && !UNITY_EDITOR
			osver = _IOSVersion().ToString();
#endif
#if UNITY_ANDROID

#endif
		return osver;
	}

#if UNITY_IOS
	[DllImport ("__Internal")]
	private static extern void _copyToClipboard(string text);
#endif
	public static void CopyToClipboard( string input)
	{
		if(Application.isEditor)
		{
			return;	
		}
#if UNITY_IOS
		_copyToClipboard(input);
#endif
    }
}
