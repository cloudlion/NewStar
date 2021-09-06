using UnityEngine;
using System.Collections;
using Google.Protobuf;

namespace GameUtil
{
	public class DataParser {

		public static byte[] Serialize<T>(T data) where T : IMessage<T>
		{
			return ProtoBufUtil.Serialize<T> (data);
		}
		
		public static T Deserialize<T>(byte[] bytes) where T : IMessage<T>, new()
		{
			return ProtoBufUtil.Deserialize<T>(bytes);
		}

		public static object Deserialize(byte[] bytes, System.Type targetType)
		{
			return ProtoBufUtil.Deserialize( bytes, targetType );
		}

	}
}
