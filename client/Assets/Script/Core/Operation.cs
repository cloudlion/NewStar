using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using MVC;
using System;

namespace GameNetWork
{
	public interface IOperation
	{
		void Execute ();
		void Undo();
	}

	public class Operation {

		private int _opCode;
		private byte[] _data;
		private int sendTime;
		private bool _mask;
        public IProtocolHead _ph;

        public int retryTime = 0;
		public bool dirty = false;
        private UInt16 _funcCode;
        public Operation(int opCode, UInt16 funcCode, IProtocolHead ph,byte[] data, bool mask = false)
		{
	//		_owner = proxy;
			this._opCode = opCode;
			this._data = data;
			this._mask = mask;
            this._funcCode = funcCode;
            this._ph = ph;
//			sendTime = Time.realtimeSinceStartup;//TimeProxy.CurrentServerTime;
		}

		public int OpCode
		{
			get{
				return _opCode;
			}
		}

        public UInt16 FuncCode
        {
            get
            {
                return _funcCode;
            }
        }

        public IProtocolHead Ph
        {
            get
            {
                return _ph;
            }

        }


        public byte[] Data
		{
			get{
				return _data;
			}

		}

		public int TimeStamp
		{
			get
			{
				return sendTime;
			}
			set
			{
				sendTime = value;
			}
		}

		public bool IsBlockGame
		{
			get {
				return _mask;
			}
		}
	//	public virtual void Execute()
	//	{
	//	}
	//
	//	public virtual void Undo()
	//	{
	//
	//	}
	}

	public class OperationQueue
	{
		public static int RetryTime = 3;
		private List<Operation> opList = new List<Operation>();
		private SockMsgRouter router;

		public OperationQueue(SockMsgRouter router)
		{
			this.router = router;
		}


		public void Add(Operation op)
		{
			opList.Add (op);
		}

		public void Update()
		{
//			for(int i = 0; i < opList.Count; i++)
//			{
//				Operation op = opList[i];
//				if(op.dirty)
//					continue;
//				if (TimeProxy.CurrentServerTime - op.TimeStamp > SysDef.TimeOut)
//				{
//					if (op.retryTime < RetryTime)
//						Resend (op);
//					else 
//					{
//						if (router.timeoutHandler != null)
//							router.timeoutHandler.Invoke ();
//						if (router.blockGameMsgBack != null)
//							router.blockGameMsgBack.Invoke ();
//						op.dirty = true;
//					}
//				}
//			}
		}

		public Operation OnRecieve(int opcode)
		{
			for(int i = 0; i < opList.Count; i++)
			{
				Operation op = opList[i];
				if(op.OpCode == opcode)
				{
					opList.RemoveAt(i);
					if (op.IsBlockGame) {
						if (router.blockGameMsgBack != null)
							router.blockGameMsgBack.Invoke ();
					}
                    return op;
				}
			}
            return null;
		}

		public void ResendAll()
		{
			for(int i = 0; i < opList.Count; i++)
			{
				Operation op = opList[i];
				op.dirty = false;
				Resend(op);
			}
			ClearRetryTimes ();
		}

		public void ClearRetryTimes()
		{
			for(int i = 0; i < opList.Count; i++)
			{
				Operation op = opList[i];
				op.retryTime = 0;
			}
		}

		public void Clear()
		{
			opList.Clear ();
		}

		public void Stash()
		{
			for(int i = 0; i < opList.Count; i++)
			{
				Operation op = opList[i];
				op.dirty = true;
			}
		}

		void Resend(Operation op)
		{
			Logger.LogWarning ("resend: " + op.OpCode + op._ph);
			router.Send (op.Data);
			op.retryTime++;
//			op.TimeStamp = TimeProxy.CurrentServerTime;
		}
	}		

}














