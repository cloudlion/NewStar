using UnityEngine;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace GameUtil
{
    public class ProtoBufUtil
    {
        public static byte[] Serialize<T>(T data)
        {
            if (null == data)
            {
                Logger.LogError("Invalid input data!");
                return null;
            }
            
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            ProtoBuf.Serializer.Serialize<T>(ms, data);
            
            byte[] ret = ms.ToArray();
            ms.Close();
            
            return ret;
        }
        
        public static T Deserialize<T>(byte[] bytes)
        {
            if (null == bytes)
            {
                Logger.LogError("input bytes is null!");
                return default (T);
            }
            
            System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
            ms.Position = 0;
            
            T ret = ProtoBuf.Serializer.Deserialize<T>(ms);
            ms.Close();
            
            return ret;
        }
        
        public static object Deserialize(byte[] bytes, System.Type targetType)
        {
            System.Object obj = System.Activator.CreateInstance(targetType);
            
            MemoryStream ms = new MemoryStream(bytes);
            ms.Position = 0;
            
            obj =ProtoBuf.Meta.RuntimeTypeModel.Default.Deserialize(ms, obj, targetType, null); 
            
            ms.Close();
            
            return obj;
        }
        
        public static string ToUrlParams<T>(T data)
        {
            Type tp = typeof(T);
            if (null == data)
            {
                throw new Exception("Invalid arg!");
            }
            
            StringBuilder sb = new StringBuilder();
            
            foreach(FieldInfo field in tp.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                sb.Append('&');
                Type fieldType = field.FieldType;
                
                if (fieldType.IsValueType)
                {               
                    sb.Append(FieldNameFilter(fieldType.Name) + "=" + SimpleJsonSerializer.StringifyValue(field.GetValue(data)));
                }
                else if (fieldType.Equals(typeof(string)))
                {
                    sb.Append(FieldNameFilter(fieldType.Name) + "=" + SimpleJsonSerializer.StringifyString(field.GetValue(data)));
                }
                else if (fieldType.IsEnum)
                {
                    sb.Append(FieldNameFilter(fieldType.Name) + "=" + SimpleJsonSerializer.StringifyEnum(field.GetValue(data)));
                }
                else
                {
                    throw new Exception("Not supported! " + tp.FullName);
                }
            }
            
            return sb.ToString();
        }
        
        public static string ToJson<T>(T data)
        {
            SimpleJsonConfig cfg = new SimpleJsonConfig();
            cfg.KeyFilter = FieldNameFilter;
            cfg.excludes = excludes;
            
            return SimpleJsonSerializer.Stringify(data, cfg);
        }
        
        #region internal
        // Fight against c-sharp ProtoNet generation tool. avoiding
        // serialize IExtension to JSON output.
        private static Type[] excludes = new Type[1]
        {
            typeof(global::ProtoBuf.IExtension)
        };
        
        private static string FieldNameFilter(string fieldName)
        {
            if (fieldName.StartsWith("_"))
            {
                return fieldName.Substring(1);
            }
            return fieldName;
        }
        #endregion
    }
}

