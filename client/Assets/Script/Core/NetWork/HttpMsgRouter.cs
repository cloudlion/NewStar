using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
//using System.Net;
using GameUtil;

namespace GameNetWork
{
    public class HttpMsgRouter : HttpRouter
    {
        private string _authToken = "";
        public string AuthToken
        {
            get { return _authToken; }
            set { _authToken = value; }
        }
        
        public HttpMsgRouter(string _urlPrefix)
        {
            this._urlPrefix = _urlPrefix;
        }

        private string _urlPrefix = "";
        public string UrlPrefix
        {
            get { return _urlPrefix; }
            set { _urlPrefix = value; }
        }

        public delegate void MsgHandler(string url, IProtocolHead ph);
        public delegate void ErrorHandler(string url, int status);
        
        private Dictionary<int, Delegate> handlerDic = new Dictionary<int, Delegate>();
        private ErrorHandler errorHandler;
        public ErrorHandler OnError
        {
            get { return errorHandler; }
            set { errorHandler = value; }
        }

        public void RegisterMsgHandler(UInt16 appCode, UInt16 funcCode, MsgHandler handler)
        {
            int id = PackageUtils.GetProtocolID(appCode, funcCode);
            if (!handlerDic.ContainsKey(id))
            {
                handlerDic[id] = null;
            }
            handlerDic[id] = (MsgHandler)handlerDic[id] + handler;
        }

        public void UnRegisterMsgHandler(UInt16 appCode, UInt16 funcCode, MsgHandler handler)
        {
            int id = PackageUtils.GetProtocolID(appCode, funcCode);
            if (!handlerDic.ContainsKey(id))
            {
                Logger.LogError("Id not found! : " + id.ToString());
                return;
            }
            handlerDic[id] = (MsgHandler)handlerDic[id] - handler;
        }

		public void SendMsg(string httpMethod, string directory, string body, string parameters, MsgHandler callback = null, bool getOriginData = false, byte[] bytes = null, bool needResp = true)
        {
            StringBuilder urlBuilder = new StringBuilder(UrlPrefix);
            
            urlBuilder.Append(directory);
//            urlBuilder.Append("/?authToken=");
//            urlBuilder.Append(AuthToken);
            
            if (!String.IsNullOrEmpty(parameters)){
                urlBuilder.Append(parameters);
            }
            
			string url = urlBuilder.ToString ();//HttpUtility.UrlEncode(urlBuilder.ToString());
			HttpRequest req = new HttpRequest(httpMethod, url, body, getOriginData,bytes, needResp);
            
//  TODO http GameMain.Instance.HttpMgr.Request(req);
        }

		protected override void HandleMsg(string url, MsgWrap mwrap)
        {
            UInt16 appCode = 0, funcCode = 0;
			int opCode = 0;

			if (!mwrap.req.needResponse)
				return;
			byte[] data = mwrap.rsp.result;
			IProtocolHead ph = PackageUtils.Deserialize2Proto(data, ref appCode, ref funcCode, ref opCode);
            if (null == ph)
            {
				Debug.LogError("package error: " + url);
				Debug.LogError(Convert.ToBase64String(data));
                return;
            }
            
            int id = PackageUtils.GetProtocolID(appCode, funcCode);

//			if(mwrap.req._callback != null)
//			{
//				mwrap.req._callback(url, ph);
//			}

            if (!handlerDic.ContainsKey(id)){
                Logger.LogWarning("Can't find handler for " + appCode + " | " + funcCode);
                return;
            }
            
            Delegate handler = handlerDic[id];
            if (null != handler)
            {
                ((MsgHandler)handler)(url, ph);
            }
        }

        protected override void HandleError(string url, int status, int httpStatusCode)
        {
            Logger.LogError("response error: " + status);
            Logger.LogError("http error: " + ((HttpStatusCode)httpStatusCode).ToString());
            if (null != errorHandler)
            {
                errorHandler(url, status);
            }
        }
    }
}

