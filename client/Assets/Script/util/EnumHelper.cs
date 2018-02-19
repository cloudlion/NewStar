using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System;

namespace GameUtil
{
   public static class EnumHelper
    {
        public static string GetDescription(Enum value)
        {
            if (value == null)
            {
                throw new ArgumentException("value");
            }
            string description = value.ToString();
            var fieldInfo = value.GetType().GetField(description);
            var attributes =
                (EnumDescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(EnumDescriptionAttribute), false);
            if (attributes != null && attributes.Length > 0)
            {
                description = attributes[0].Description;
            }
            return description;
        }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class EnumDescriptionAttribute : Attribute
    {
        private string description;
        public string Description { get { return description; } }

        public EnumDescriptionAttribute(string description)
            : base()
        {
            this.description = description;
        }
    }
}