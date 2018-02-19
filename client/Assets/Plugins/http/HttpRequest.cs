using System;

namespace GameNetWork
{
    public class HttpRequest
    {
        public string url = "";
        public string data = "";
        public string method = "";
        public object userdata;
        public byte[] bytes;
        public bool getOriginData = false;
		public bool needResponse = true;
        public HttpRequest() {}
		public int retryTimes = 0;

        public HttpRequest(string method, string url, string data)
        {
            this.url = url;
            this.data = data;
            this.method = method;
            this.userdata = null;
        }
		public HttpRequest(string method, string url, string data, object userdata,byte[] bytes = null, bool needResponse = true) 
        {
            this.url = url;
            this.data = data;
            this.method = method;
            this.userdata = userdata;
            this.bytes = bytes;
			this.needResponse = needResponse;
        }
    }

    public class HttpResponse
    {
        public const int ST_OK = 0;
        public const int ST_HTTP_ERROR = -1;
        public const int ST_REQ_ERROR = -2;

        public int status = 0;
        public int httpStatusCode = 200;
        public byte[] result = null;

        public HttpResponse() {}
        public HttpResponse(int st, int stcode, byte[] result)
        {
            this.status = st;
            this.httpStatusCode = stcode;
            this.result = result;
        }
    }

    public delegate void HttpRequestCallback(HttpRequest req, HttpResponse rsp);
}