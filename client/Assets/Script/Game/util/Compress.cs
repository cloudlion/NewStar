using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.IO.Compression;

namespace  GameUtil
{
	public class Compress {

		public static void Decompress( byte[] source, out byte[] data )
		{
			data = null;
			Ionic.Zlib.DeflateStream decompressedStream = null;
			//			byte[] quartetBuffer = null;
			try
			{
				// Create a compression stream pointing to the destiantion stre
				
				
				MemoryStream ms = new MemoryStream(source);
				decompressedStream = new Ionic.Zlib.DeflateStream(ms, Ionic.Zlib.CompressionMode.Decompress, true);
				
				int offset = 0;
				int total = 0;
				byte[] quartetBuffer = new byte[1024*1024];
				// Read the compressed data into the buffer
				while ( true )
				{
					int bytesRead = decompressedStream.Read ( quartetBuffer, offset, 100 );
					
					if ( bytesRead == 0 )
						break;
					
					offset += bytesRead;
					total += bytesRead;
				}
				byte[] buffer = new byte[total];
				Array.Copy(quartetBuffer, 0, buffer, 0, total);
				data = buffer;
		//		Debug.Log(System.Text.Encoding.UTF8.GetString(buffer));
				
			}
			catch ( ApplicationException ex )
			{
				Console.WriteLine(ex.Message, "解压文件时发生错误：");
			}
			finally
			{
				// Make sure we allways close all streams
				if ( decompressedStream != null )
					decompressedStream.Close ( );
			}
		}

		public static void CompressByte(byte[] sourceByte,out byte[] dataByte)
		{
			dataByte = null;
			Ionic.Zlib.DeflateStream compressByte = null;
			try{
				using(MemoryStream ms = new MemoryStream())
				{
					using(compressByte = new Ionic.Zlib.DeflateStream(ms,Ionic.Zlib.CompressionMode.Compress,true))
					{
						compressByte.Write(sourceByte,0,sourceByte.Length);
					}
					dataByte = ms.ToArray();
				}
			}catch(ApplicationException ex){
				Console.WriteLine("compress byte is exception !!");
			}finally{
				if(compressByte != null)
				{
					compressByte.Close();
				}
			}
		}
	}
}