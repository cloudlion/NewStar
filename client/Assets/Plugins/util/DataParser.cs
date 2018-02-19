using UnityEngine;
using System.Collections;

namespace GameUtil{
	public class DataParser {

		public static byte[] Serialize<T>(T data)
		{
			return ProtoBufUtil.Serialize<T> (data);
		}
		
		public static T Deserialize<T>(byte[] bytes)
		{
			return ProtoBufUtil.Deserialize<T> ( bytes );
		}

		public static object Deserialize(byte[] bytes, System.Type targetType)
		{
			return ProtoBufUtil.Deserialize( bytes, targetType );
		}

	}
}
