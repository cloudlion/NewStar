using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GameUtil
{  
    public class SimpleJsonSerializer
    {
        const string DEFAULT_NULL_STRING_INDICATOR = "\"\"";
        const string DEFAULT_NULL_OBJECT_INDICATOR = "null";
        const string DEFAULT_SEPERATOR = ",";
        
        public static string Stringify(Object obj, SimpleJsonConfig cfg = null)
        {
            if (null == obj)
            { 
                return GetNullObjectIndicator(cfg);
            }
            
            Type T = obj.GetType();
            if (!IsValidType(T, cfg))
            {
                return GetNullObjectIndicator(cfg);
            }
            
            return StringifyInternal(T, obj, cfg);
        }
        
        private static string StringifyInternal(Type T, Object obj, SimpleJsonConfig cfg)
        {
            // Dispatch
            if (TypeHelper.IsEnum(T))
            {
            
                return StringifyEnum(obj);
                
            }
            else if (TypeHelper.IsValue(T))
            {
            
                return StringifyValue(obj);
                
            }
            else if (TypeHelper.IsString(T))
            {
            
                return StringifyString(obj);
                
            }
            else if (TypeHelper.IsArrayType(T))
            {
            
                return StringifyArray((Array)obj, cfg);
                
            }
            else if (TypeHelper.IsList(T))
            {
            
                return StringifyList((IList)obj, cfg);
                
            }
            else if (TypeHelper.IsDictionary(T))
            {
            
                return StringifyDict((IDictionary)obj, cfg);
                
            }
            else
            {
            
                return StringifyObject(obj, cfg);
                
            }
        }
        
        private static string StringifyObject(Object obj, SimpleJsonConfig cfg)
        {
            if (null == obj) 
            {
                return GetNullObjectIndicator(cfg);
            }
            
            Type T = obj.GetType();
            StringBuilder json = new StringBuilder("");
            json.Append(GetObjBracketIndicator(cfg, true));
            
            bool isEmpty = true;
            string seperator = GetSeperator(cfg);
            
            foreach(FieldInfo field in T.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                Type fieldType = field.FieldType;
                
                if (!IsValidType(fieldType, cfg))
                {
                    continue;
                }
                
                string fieldKey = GetKey(field.Name, cfg);
                json.AppendFormat("\"{0}\":", fieldKey);
                
                object fieldObj = field.GetValue(obj);
                
                json.Append(StringifyInternal(fieldType, fieldObj, cfg));
                json.Append(seperator);
                isEmpty = false;
            }
            
            if (!isEmpty)
            {
                json.Remove(json.Length - seperator.Length, seperator.Length);
            }
            
            json.Append(GetObjBracketIndicator(cfg, false));
            return json.ToString();
        }
        
        public static string StringifyValue(object v)
        {
            return v.ToString();
        }
        
        public static string StringifyString(object v, SimpleJsonConfig cfg = null)
        {
            if (null == v)
            {
                return GetNullStringIndicator(cfg);
            }
            else
            {
                string str = (v as string);
                return ("\"" + str + "\"");
            }
        }
        
        public static string StringifyEnum(Object v)
        {
            int i = (int)v;
            return StringifyValue(i);
        }
        
        public static string StringifyList(IList list, SimpleJsonConfig cfg)
        {
            if (null == list)
            {
                return GetNullObjectIndicator(cfg);
            }
            
            string seperator = GetSeperator(cfg);
            StringBuilder json = new StringBuilder("");
            json.Append(GetArrBracketIndicator(cfg, true));
            
            bool isEmpty = true;
            foreach (Object o in list)
            {
                json.Append(StringifyInternal(o.GetType(), o, cfg));
                json.Append(seperator);
                isEmpty = false;
            }
            
            if (!isEmpty)
            {
                json.Remove(json.Length - seperator.Length, seperator.Length);
            }
            
            json.Append(GetArrBracketIndicator(cfg, false));
            return json.ToString();
        }
        
        public static string StringifyArray(Array arr, SimpleJsonConfig cfg)
        {
            if (null == arr)
            {
                return GetNullObjectIndicator(cfg);
            }
            
            string seperator = GetSeperator(cfg);
            StringBuilder json = new StringBuilder("");
            json.Append(GetArrBracketIndicator(cfg, true));
            
            bool isEmpty = true;
            foreach (Object o in arr)
            {
                json.Append(StringifyInternal(o.GetType(), o, cfg));
                json.Append(seperator);
                isEmpty = false;
            }
            
            if (!isEmpty)
            {
                json.Remove(json.Length - seperator.Length, seperator.Length);
            }
            
            json.Append(GetArrBracketIndicator(cfg, false));
            return json.ToString();
        }
        
        public static string StringifyDict(IDictionary dict, SimpleJsonConfig cfg)
        {
            if (null == dict)
            {
                return GetNullObjectIndicator(cfg);
            }
            
            string seperator = GetSeperator(cfg);
            StringBuilder json = new StringBuilder("");
            json.Append(GetObjBracketIndicator(cfg, true));
            
            bool isEmpty = true;
            Type[] temp = dict.GetType().GetGenericArguments();
            Type keyType = temp[0];
            Type valueType = temp[1];
            IDictionaryEnumerator enumlator = dict.GetEnumerator();
            
            while (enumlator.MoveNext())
            {
                string keyStr = StringifyInternal(keyType, enumlator.Key, cfg);
                if (!keyStr.StartsWith("\""))
                {
                    keyStr = "\"" + keyStr + "\"";
                }
                
                string valueStr = StringifyInternal(valueType, enumlator.Value, cfg);
                json.AppendFormat("{0}:{1}", keyStr, valueStr);
                json.Append(seperator);
                isEmpty = false;
            }

            if (!isEmpty)
            {
                json.Remove(json.Length - seperator.Length, seperator.Length);
            }
            
            json.Append(GetObjBracketIndicator(cfg, false));
            return json.ToString();
        }
        
        // internal procs
        static bool IsValidType(Type t, SimpleJsonConfig cfg)
        {
            if (null == cfg)
            {
                return true;
            }
            Type target_type = TypeHelper.IsGenericType(t) ? TypeHelper.GetElementType(t) : t;
            return cfg.ScreenType(target_type);
        }
        
        static string GetKey(string metaKey, SimpleJsonConfig cfg)
        {
            if (null != cfg && null != cfg.KeyFilter)
            {
                return cfg.KeyFilter(metaKey);
            }
            return metaKey;
        }
        
        static string GetContent(string metaContent, SimpleJsonConfig cfg)
        {
            if (null != cfg && null != cfg.bodyFilter)
            {
                return cfg.KeyFilter(metaContent);
            }
            return metaContent;
        }
        
        static string GetSeperator(SimpleJsonConfig cfg)
        {
            string ret = DEFAULT_SEPERATOR;
            if (null != cfg && null != cfg.seperatorFilter)
            {
                ret = cfg.seperatorFilter(ret);
            }
            return ret;
        }
        
        static string GetNullStringIndicator(SimpleJsonConfig cfg)
        {
            if (null != cfg && null != cfg.nullStringIndicator)
            {
                return cfg.nullStringIndicator();
            }
            return DEFAULT_NULL_STRING_INDICATOR;
        }
        
        static string GetNullObjectIndicator(SimpleJsonConfig cfg)
        {
            if (null != cfg && null != cfg.nullObjectIndicator)
            {
                return cfg.nullObjectIndicator();
            }
            return DEFAULT_NULL_OBJECT_INDICATOR;
        }
        
        static string GetObjBracketIndicator(SimpleJsonConfig cfg, bool is_left)
        {
            string bracket = is_left ? "{" : "}";
            if (null != cfg && null != cfg.objBracketFilter)
            {
                bracket = cfg.objBracketFilter(bracket);
            }
            return bracket;
        }
        
        static string GetArrBracketIndicator(SimpleJsonConfig cfg, bool is_left)
        {
            string bracket = is_left ? "[" : "]";
            if (null != cfg && null != cfg.arrBracketFilter)
            {
                bracket = cfg.arrBracketFilter(bracket);
            }
            return bracket;
        }
        
        static string GetPerModulePrefix(SimpleJsonConfig cfg, int step)
        {
            if (null != cfg && null != cfg.linePrefix)
            {
                return cfg.linePrefix(step);
            }
            return "";
        }
    }
    
    
    class TypeHelper
    {
        public static bool IsValue(Type t)
        {
            return t.IsValueType;
        }
        
        public static bool IsString(Type t)
        {
            return t.Equals(typeof(string));
        }
        
        public static bool IsEnum(Type t)
        {
            return t.IsEnum;
        }
        
        public static bool IsArrayType(Type t)
        {
            return t.IsArray;
        }
        
        public static bool IsList(Type t)
        {
            Type typeInterface = t.GetInterface("System.Collections.IList", true);
            return (typeInterface != null);
        }
        
        public static bool IsDictionary(Type t)
        {
            Type typeInterface = t.GetInterface("System.Collections.IDictionary", true);
            return (typeInterface != null);
        }
        
        public static object GetDefaultValue(Type t)
        {
            return IsValue(t) ? Activator.CreateInstance(t) : null;
        }
        
        public static bool IsGenericType(Type t)
        {
            return t.IsGenericType;
        }
        
        public static Type GetElementType(Type t)
        {
            if (!t.IsGenericType)
            {
                return t;
            }
            else
            {
                if (IsList(t))
                {
                    return t.GetGenericArguments()[0];
                }
                else if (IsArrayType(t))
                {
                    return t.GetGenericArguments()[0];
                }
                else if (IsDictionary(t))
                {
                    return t.GetGenericArguments()[1];
                }
                else
                {
                    throw new Exception("NOT supported type : " + t.Name);
                }
            }
        }
    }
}

