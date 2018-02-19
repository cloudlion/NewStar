using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.IO.Compression;

namespace GameNetWork
{
    public class HttpMgr
    {
		public const string POST = "POST";
		public const string PUT = "PUT";
		public const string GET = "GET";
		public const string DELETE = "DELETE";

		public Action requestFail = null;
        private HttpRouter router = new NullHttpRouter();

        private volatile bool running = false;
        //private Thread worker = null;

        private object syncRoot = new object();
        private List<HttpRequest> reqQ = new List<HttpRequest>();

        byte[] readStreamBuffer = new byte[64 * 1024];

        private CookieContainer cookie = new CookieContainer();

		private bool requestError = false;

        public HttpRouter Router
        {
            get
            {
                return router;
            }
            set
            {
                router.Shutdown();
                router = value;
                if (router == null)
                {
                    router = new NullHttpRouter();
                }
                router.Start();
            }
        }

        public void Start ()
        {
            this.running = true;
            //this.worker =
            Thread t = new Thread(new ThreadStart(this.Process));
            t.Start();
            //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; }; 
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
        }

        public void Shutdown ()
        {
            this.running = false;
            //clear the router
            Router = null;
			ServicePointManager.ServerCertificateValidationCallback = null;
        }

        public void Update()
        {
//			Debug.Log ("http update");
            router.Dispatch();
			if(requestError)
			{
				requestError = false;
				if(requestFail != null)
					requestFail.Invoke();
			}
        }

        public void Request(HttpRequest req)
        {
            lock (syncRoot)
            {
                reqQ.Add(req);
            }
        }

//        public HttpResponse RequestSync(HttpRequest req) {
//          return this.SendRequest(req);
//        }

        private void Process()
        {
            try
            {
                while (this.running)
                {
                    if (this.reqQ.Count == 0)
                    {
                        Thread.Sleep(10);
                        continue;
                    }

                    List<HttpRequest> reqs = null;
                    lock (syncRoot)
                    {
                        if (reqQ.Count > 0)
                        {
                            reqs = new List<HttpRequest>(reqQ);
                            reqQ.Clear();
                        }
                    }
                    if (reqs == null || reqs.Count == 0)
                    {
                        continue;
                    }

                    foreach (HttpRequest req in reqs)
                    {
                        HttpResponse rsp = this.SendRequest(req);
						if (null != rsp && rsp.status == HttpResponse.ST_OK)
                        {
                            router.OnRecvMsg(req, rsp);
                        }
						else if(req.needResponse)
						{
							Logger.LogError("response error");
							if(req.retryTimes >= 3)
							{
								requestError = true;
							}
							else
							{
								req.retryTimes++;
								Request(req);
							}
						}
                    }
                }
            }
            catch (System.Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private HttpResponse SendRequest(HttpRequest req)
        {
            try
            {
				string url = req.url;
				string postDataStr = req.data;
#if UNITY_ANDROID
				//url = url.Replace("https", "http").Replace("com:9900", "com:9000");

#endif 
				Debug.Log("http url " + url);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = req.method.ToUpper();
                request.CookieContainer = cookie;
                request.KeepAlive = true;       

                if (!String.IsNullOrEmpty(postDataStr))
                {
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.Accept = "*/*";
                    request.UserAgent = "link4age";
                    request.ServicePoint.Expect100Continue = false;

                    byte[] bytes = req.bytes;
                    request.ContentLength = bytes.Length;

                    Stream myRequestStream = request.GetRequestStream();

                    BufferedStream bfs = new BufferedStream(myRequestStream);

                    bfs.Write(bytes, 0, bytes.Length);

                    bfs.Flush();
                    bfs.Close();

                    myRequestStream.Flush();
                    myRequestStream.Close();

                    //return null;
                }
                
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
				Logger.Log("Get response");
                response.Cookies = cookie.GetCookies(response.ResponseUri);
                Stream myResponseStream = response.GetResponseStream();
			
				int streamLength = 0;//myResponseStream.Read(readStreamBuffer, 0, readStreamBuffer.Length);
				int count = 0;
				while( (count = myResponseStream.Read(readStreamBuffer, streamLength, readStreamBuffer.Length - streamLength)) > 0)
				{
					streamLength += count;
				}
                
                if (streamLength > 64 * 1024)
                {
                    throw new System.Exception("too large http response size!");
                }
                
				byte[] result = new byte[streamLength];
                Array.Copy(readStreamBuffer, 0, result, 0, streamLength);

		//		GetData(myResponseStream, out result);
                myResponseStream.Close();
		
                HttpResponse resp = new HttpResponse();
                int st = (int)response.StatusCode;
                resp.httpStatusCode = st;
                resp.result = result;
                if (st >= 300)
                {
                    resp.status = HttpResponse.ST_HTTP_ERROR;
                    Logger.LogError("http request error: " + st);
                }
                else
                {
                    resp.status = HttpResponse.ST_OK;
                }
                return resp;   
                 
            }
            catch(WebException e)
            {
                Logger.LogError("WebException! status : " + e.Status.ToString());
//                HttpResponse resp = new HttpResponse();
//                resp.status = HttpResponse.ST_REQ_ERROR;
//                resp.httpStatusCode = (int)e.Status;
                
                return null;
            }
        }

		void GetData( Stream sourceStream, out byte[] data)
		{
			DeflateStream decompressedStream = null;
			data = null;
//			byte[] quartetBuffer = null;
			try
			{
				// Create a compression stream pointing to the destiantion stream


				int streamLength = sourceStream.Read(readStreamBuffer, 0, readStreamBuffer.Length);

				if (streamLength > 32 * 1024)
                {
                    throw new System.Exception("too large http response size!");
                }


				MemoryStream ms = new MemoryStream(readStreamBuffer, 0, streamLength);
				decompressedStream = new DeflateStream ( ms, CompressionMode.Decompress, true );
				// Read the footer to determine the length of the destiantion file
//				quartetBuffer = new byte[4];
//				int position = (int)streamLength - 4;
//				sourceStream.Seek(position, SeekOrigin.Begin);
//				sourceStream.Position = position;
//				sourceStream.Read ( quartetBuffer, position, 4 );
//				sourceStream.Seek(0, SeekOrigin.Begin);
			
//				int checkLength = BitConverter.ToInt32 ( quartetBuffer, 0 );
				

				
				int offset = 0;
				int total = 0;
				byte[] quartetBuffer = new byte[32*1024];
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
				Debug.Log(System.Text.Encoding.UTF8.GetString(buffer));
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
		
		// public string Get(string Url, string postDataStr)
		// {
		//     HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
		//     request.Method = "GET";
		//     request.ContentType = "text/html;charset=UTF-8";
		
		//     HttpWebResponse response = (HttpWebResponse)request.GetResponse();
		//     Stream myResponseStream = response.GetResponseStream();
		//     StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
        //     string retString = myStreamReader.ReadToEnd();
        //     myStreamReader.Close();
        //     myResponseStream.Close();

        //     return retString;
        // }
    }
}