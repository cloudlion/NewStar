using System;
using System.Reflection;
using System.Collections.Generic;

namespace GameUtil
{
    public class Reverser<T> : IComparer<T>
    {
        private Type type = null;
        private ReverserInfo info;

        public Reverser(Type type, string name, ReverserInfo.Direction direction)
        {
            this.type = type;
            this.info.name = name;
            if (direction != ReverserInfo.Direction.ASC)
            {
                this.info.direction = direction;
            }
        }

        public Reverser(string className, string name, ReverserInfo.Direction direction)
        {
            try
            {
                this.type = Type.GetType(className, true);
                this.info.name = name;
                this.info.direction = direction;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Reverser(T t, string name, ReverserInfo.Direction direction)
        {
            this.type = t.GetType();
            this.info.name = name;
            this.info.direction = direction;
        }

        int IComparer<T>.Compare(T t1, T t2)
        {
            object x = this.type.InvokeMember(this.info.name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty, null, t1, null);
            object y = this.type.InvokeMember(this.info.name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty, null, t2, null);
            if (this.info.direction != ReverserInfo.Direction.ASC)
            {
                Swap(ref x, ref y);
            }
            return (new System.Collections.CaseInsensitiveComparer()).Compare(x, y);
        }

        private void Swap(ref object x, ref object y)
        {
            object temp = null;
            temp = x;
            x = y;
            y = temp;
        }
    }


    public struct ReverserInfo
    {
        public enum Direction
        {
            ASC = 0,
            DESC,
        };

        public enum Target
        {
            CUSTOMER = 0,
            FORM,
            FIELD,
            SERVER,
        };

        public string name;
        public Direction direction;
        public Target target;
    }
}
