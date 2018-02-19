using UnityEngine;
using System;
using System.IO;
using System.Text;

namespace GameUtil
{
    public class ReadStream
    {
        public ReadStream(byte[] bytes)
        {
            this.buffer = bytes;
            this.pos = 0;
        }

        public int Size
        {
            get
            {
                return buffer == null ? 0 : buffer.Length;
            }
        }

        public void ReadBytes(byte[] dest)
        {
            ReadBytes(dest, 0, dest.Length);
        }

        public void ReadBytes(byte[] dest, int start, int length)
        {
            CheckOutOfBound(length);
            Array.Copy(buffer, pos, dest, start, length);
            pos += length;
        }

        public byte ReadByte()
        {
            byte[] data = new byte[1];
            ReadBytes(data);
            return data[0];
        }

        public bool ReadBool()
        {
            CheckOutOfBound(sizeof(bool));
            bool val = BitConverter.ToBoolean(buffer, pos);
            pos += sizeof(bool);

            return val;
        }

        public short ReadShort()
        {
            CheckOutOfBound(sizeof(short));
            short val = BitConverter.ToInt16(buffer, pos);
            pos += sizeof(short);

            return val;
        }

        public int ReadInt()
        {
            CheckOutOfBound(sizeof(int));
            int val = BitConverter.ToInt32(buffer, pos);
            pos += sizeof(int);

            return val;
        }

        public long ReadLong()
        {
            CheckOutOfBound(sizeof(long));
            long val = BitConverter.ToInt64(buffer, pos);
            pos += sizeof(long);

            return val;
        }

        public float ReadFloat()
        {
            CheckOutOfBound(sizeof(float));
            float val = BitConverter.ToSingle(buffer, pos);
            pos += sizeof(float);

            return val;
        }

        public double ReadDouble()
        {
            CheckOutOfBound(sizeof(double));
            double val = BitConverter.ToDouble(buffer, pos);
            pos += sizeof(double);

            return val;
        }

        public string ReadString()
        {
            short len = ReadShort();
            if (len == 0)
            {
                return string.Empty;
            }

            CheckOutOfBound(len);
            string str = Encoding.UTF8.GetString(buffer, pos, len);
            pos += len;

            return str;
        }

        public Vector3 ReadVector3()
        {
            float x = ReadFloat();
            float y = ReadFloat();
            float z = ReadFloat();

            return new Vector3(x, y, z);
        }

        public Quaternion ReadQuat()
        {
            float x = ReadFloat();
            float y = ReadFloat();
            float z = ReadFloat();
            float w = ReadFloat();

            return new Quaternion(x, y, z, w);
        }

        public Matrix4x4 ReadMatrix3x3()
        {
            Matrix4x4 mtx = Matrix4x4.identity;

            mtx.m00 = ReadFloat();
            mtx.m01 = ReadFloat();
            mtx.m02 = ReadFloat();

            mtx.m10 = ReadFloat();
            mtx.m11 = ReadFloat();
            mtx.m12 = ReadFloat();

            mtx.m20 = ReadFloat();
            mtx.m21 = ReadFloat();
            mtx.m22 = ReadFloat();

            return mtx.transpose;
        }

        private void CheckOutOfBound(int len)
        {
            SysUtil.Assert(pos + len <= buffer.Length, "stream read out of bound");
        }

        private int pos = 0;
        private byte[] buffer = null;
    }
}