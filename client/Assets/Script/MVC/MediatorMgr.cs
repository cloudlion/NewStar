using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameEngine;

namespace MVC
{
	public class MediatorMgr {

		private Dictionary<string, Mediator> _mediatorMap;
		// Use this for initialization
		public void Start () {
			
			_mediatorMap = new Dictionary<string, Mediator> ();
		}

		public void Register(string id, Mediator mediator)
		{
			if (_mediatorMap.ContainsKey (id)) {
				
				Logger.Log("Mediator id is existed: " + id);
				return;
			}
			
			_mediatorMap.Add (id, mediator);
			mediator.MediatorName = id;
			mediator.OnRegister (this);
		
		}
		
		public void UnRegister(string id)
		{
			_mediatorMap.Remove (id);
		} 

		public Mediator GetMediator(string id)
		{
			Mediator m = null;
			_mediatorMap.TryGetValue(id, out m);
			
			return m;
		}

		public void RegisterEventHandler(string name, EventMgr.EventHandler handler)
		{
            if (ProxyMgr.Instance.EventManager != null)
                ProxyMgr.Instance.EventManager.RegisterHandler(name, handler);
        }
		
		public void UnregisterEventHandler(string name, EventMgr.EventHandler handler)
		{
            if (ProxyMgr.Instance.EventManager != null)
                ProxyMgr.Instance.EventManager.UnRegisterHandler(name, handler);
        }

		public void SendEvent(GameEngine.Event e)
		{
            ProxyMgr.Instance.EventManager.SendEvent(e);
        }

		public void ShutDown()
		{
			foreach(var item in _mediatorMap)
			{
				Object.Destroy( item.Value.ViewComponent );
			}
			_mediatorMap.Clear ();
		}
	}
}








