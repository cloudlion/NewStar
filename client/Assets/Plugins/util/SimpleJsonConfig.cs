using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace GameUtil
{
    public class SimpleJsonConfig
    {
        public delegate string StrFilter(string str);
        public delegate bool   TypeFilter(System.Type tp);
        public delegate string StrGetter();
        public delegate string StrGetter2(int i);
        
        // string filters.
        public StrFilter KeyFilter = null;
        public StrFilter bodyFilter = null;
        public TypeFilter typeFilter = null;
        
        // for null indicators.
        public StrGetter nullStringIndicator = null;
        public StrGetter nullObjectIndicator = null;
        
        // type filters
        public System.Type[] excludes = null;
        
        public bool ScreenType(System.Type t)
        {
            if (!pre_type_filter(t))
            {
                return false;
            }
            
            if (null != typeFilter)
            {
                return typeFilter(t);
            }
            else
            {
                return true;
            }
        }
        
        public bool pre_type_filter(System.Type tp)
        {
            if (null != excludes)
            {
                foreach (System.Type type in excludes)
                {
                    if (tp.Equals(type))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        
        // for pretty
        private bool isPretty = false;
        public bool IsPretty
        {
            get 
            { 
                return isPretty; 
            }
            set
            {
                isPretty = value;
                if (isPretty)
                {
                    seperatorFilter = PrettySeperator;
                    objBracketFilter = PrettyObjBracket;
                    arrBracketFilter = PrettyArrBracket;
                    linePrefix = PrettyPreModulePrefix;
                }else{
                    seperatorFilter = null;
                    objBracketFilter = null;
                    arrBracketFilter = null;
                    linePrefix = null;
                }
            }
        }
        
        public StrFilter seperatorFilter = null;
        public StrFilter objBracketFilter = null;
        public StrFilter arrBracketFilter = null;
        public StrGetter2 linePrefix = null;
        
        public static string PrettySeperator(string str)
        {
            return str + "\n";
        }
        
        public static string PrettyObjBracket(string str)
        {
            return str + "\n";
        }
        
        public static string PrettyArrBracket(string str)
        {
            return str + "\n";
        }
        
        public static string PrettyPreModulePrefix(int step)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < step; ++i)
            {
                sb.Append("\t");
            }
            
            return sb.ToString();
        }
    }
}

