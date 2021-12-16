using UnityEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GameEngine
{
     public class Event
    {
        private string name;

        public string Name
        {
            get
            {
                return name;
            }
        }

        public Event(string name)
        {
            this.name = name;
            //SysUtil.StringToHash(name);
        }
    }

    public class EventMgr
    {
        public EventMgr(bool hasUpdate = true){
            this.hasUpdate = hasUpdate;
        }
        private bool hasUpdate;
        public delegate void EventHandler(Event e);

        private Queue<Event> events = new Queue<Event>();
        private Dictionary<string, Delegate> handlerDic = new Dictionary<string, Delegate>();

		private List<string> eventsLog = new List<string>();
		private int logCnt = 10;
        public void Start() {}

        public void Shutdown()
        {
            events.Clear();
            handlerDic.Clear(); 
        }

        public void RegisterHandler(string name, EventHandler handler)
        {
            if (string.IsNullOrEmpty(name))
            {
				Logger.LogWarning("event name is null");
                return;
            }
            if (handler == null)
            {

				Logger.LogWarning("event handler is null: " + handler);
                return;
            }

            //int id = SysUtil.StringToHash(name);

            if (!handlerDic.ContainsKey(name))
            {
                handlerDic.Add(name, null);
            }

            handlerDic[name] = (EventHandler)handlerDic[name] + handler;
        }

        public void UnRegisterHandler(string name, EventHandler handler)
        {
            if (string.IsNullOrEmpty(name))
            {
                Logger.LogWarning("event name is null");
                return;
            }
            if (handler == null)
            {
                Logger.LogWarning("event handler is null: " + handler);
                return;
            }

            //int id = SysUtil.StringToHash(name);

            if (!handlerDic.ContainsKey(name) || handlerDic[name] == null)
            {
                return;
            }

            handlerDic[name] = (EventHandler)handlerDic[name] - handler;

            if (handlerDic[name] == null)
            {
                handlerDic.Remove(name);
            }
        }

        //queue the event, handle it next frame
        public void SendEvent(Event e)
        {
            if (e == null)
            {
                return;
            }
            
            if (hasUpdate){
                events.Enqueue(e);
            }else{
                HandleEvent(e);
            }
			#if DEBUG
			StackTrace trace = new StackTrace(true);
			StringBuilder str = new StringBuilder ();
			str.Append ("Send Event: ");
			str.Append (e.Name);
			str.AppendLine (trace.ToString ());
			eventsLog.Add (str.ToString());
			if (eventsLog.Count > logCnt)
				eventsLog.RemoveAt (0);
			#endif
        }

        //handle the event immediate
        public void HandleEvent(Event e)
        {
            if (e == null)
            {
                return;
            }

            if (!handlerDic.ContainsKey(e.Name) || handlerDic[e.Name] == null)
            {
                return;
            }

            ((EventHandler)handlerDic[e.Name])(e);
        }

        public void Update()
        {
//			Debug.Log ("event update");
            if (!hasUpdate){
                Logger.LogError("Should not reach here!");
                return;
            }
            while (true)
            {
                if (events.Count == 0)
                {
                    break;
                }

                Event e = events.Dequeue();
                HandleEvent(e);
            }
        }

		public string GetEventLog()
		{
			StringBuilder str = new StringBuilder ("Event Log=================");
			for(int i = eventsLog.Count - 1; i >= 0; i--)
			{
				str.AppendLine(eventsLog[i]);
			}
			return str.ToString ();
		}
    }
}



