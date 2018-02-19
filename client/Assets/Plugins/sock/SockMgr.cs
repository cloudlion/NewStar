using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace GameNetWork
{
    public class SockMgr
    {
        private List<StreamSock> sockList = new List<StreamSock>();
		private List<StreamSock> removeList = new List<StreamSock>();

        public void Start() { }

        public void Shutdown()
        {
            foreach (StreamSock sock in sockList)
            {
                sock.Close();
            }
            sockList.Clear();
        }

        public void Update()
        {
//			Debug.Log ("socket update");
			for (int i = 0; i < sockList.Count;i++ )
            {
				sockList[i].Update();
            }

			foreach (StreamSock s in removeList)
			{
				this.RemoveSock(s);
			}
        }

        public StreamSock Connect(string ip, int port, Action connect_callback)
        {
            StreamSock sock = new StreamSock();
			sock.Connect(ip, port, connect_callback);
            this.AddSock(sock);
            return sock;
        }

        public void Disconnect(StreamSock sock)
        {
            if (sock == null)
            {
                return;
            }

//            this.RemoveSock(sock);
			removeList.Add (sock);
            sock.Close();
        }

        private void AddSock(StreamSock sock)
        {
            if (sock == null)
            {
                return;
            }

            lock (((ICollection)sockList).SyncRoot)
            {
                if (!sockList.Contains(sock))
                {
                    sockList.Add(sock);
                }
            }
        }

        private void RemoveSock(StreamSock sock)
        {
            if (sock == null)
            {
                return;
            }

            lock (((ICollection)sockList).SyncRoot)
            {
                if (sockList.Contains(sock))
                {
                    sockList.Remove(sock);
                }
            }
        }
    }
}
