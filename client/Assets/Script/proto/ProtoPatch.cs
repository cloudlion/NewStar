using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ProtoVO
{
	namespace common
	{

		public partial class packet : IProtocolHead
		{
		}
	}
}

namespace gameprotos
{
	public sealed partial class NewUser : IProtocolHead
	{
		public static UInt16 GetAppCode()
		{
			return 0;
		}

		public static UInt16 GetFnCode()
		{
			return 1;
		}

		public static byte GetFlag()
		{
			return 0;
		}

		public static string GetUrl()
		{
			return "";
		}

		public static string GetHttpMethod()
		{
			return "";
		}
	}

	public sealed partial class UserMessage : IProtocolHead
	{
		public static UInt16 GetAppCode()
		{
			return 0;
		}

		public static UInt16 GetFnCode()
		{
			return 3;
		}

		public static byte GetFlag()
		{
			return 0;
		}

		public static string GetUrl()
		{
			return "";
		}

		public static string GetHttpMethod()
		{
			return "";
		}
	}
}
