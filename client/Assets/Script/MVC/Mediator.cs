/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/

#region Using

using System;
using System.Collections.Generic;
using UnityEngine;
using GameEngine;

#endregion

namespace MVC
{
    /// <summary>
    /// A base <c>IMediator</c> implementation
    /// </summary>
    /// <see cref="PureMVC.Core.View"/>
	public class Mediator :  MonoBehaviour, IMediator
	{
		#region Constants

		/// <summary>
        /// The name of the <c>Mediator</c>
        /// </summary>
        /// <remarks>
        ///     <para>Typically, a <c>Mediator</c> will be written to serve one specific control or group controls and so, will not have a need to be dynamically named</para>
        /// </remarks>
		public static string NAME = "Mediator";
		private bool quitApplication = false;
		#endregion


		#region Public Methods

		#region IMediator Members

		/// <summary>
		/// Called by the View when the Mediator is registered
		/// </summary>
		public virtual void OnRegister(MediatorMgr mgr)
		{
			m_owner = mgr;
			RegisterEventHandler ();
		}


		public virtual void Setup(GameObject  go)
		{
			m_viewComponent = go;
		}

		/// <summary>
		/// Called by the View when the Mediator is removed
		/// </summary>
		public virtual void OnRemove()
		{
			UnregisterEventHandler ();
		}



		public virtual void Start()
		{

		}

		public virtual void OnDestroy()
		{
			if(quitApplication)return;

			UnregisterEventHandler ();
            if(m_owner != null)
			    m_owner.UnRegister (MediatorName);
			m_viewComponent = null;
			Logger.Log ("remove mediator: "+ MediatorName);
		}
		#endregion

		#endregion

		#region Accessors

		/// <summary>
        /// The name of the <c>Mediator</c>
        /// </summary>
        /// <remarks><para>You should override this in your subclass</para></remarks>
		public virtual string MediatorName
		{
			get { return m_mediatorName; }
			set {
				m_mediatorName = value;
			}
		}

		/// <summary>
		/// The <code>IMediator</code>'s view component.
		/// </summary>
		/// <remarks>
		///     <para>Additionally, an implicit getter will usually be defined in the subclass that casts the view object to a type, like this:</para>
		///     <example>
		///         <code>
		///             private System.Windows.Form.ComboBox comboBox {
		///                 get { return viewComponent as ComboBox; }
		///             }
		///         </code>
		///     </example>
		/// </remarks>
		public UnityEngine.Object ViewComponent
		{
			get { return m_viewComponent; }
			set { m_viewComponent = value; }
		}

		protected void RegisterEventHandler(string name, EventMgr.EventHandler handler)
		{
            if(m_owner != null)
			    m_owner.RegisterEventHandler (name, handler);
		}

		protected void UnregisterEventHandler(string name, EventMgr.EventHandler handler)
		{
            if (m_owner != null)
			    m_owner.UnregisterEventHandler (name, handler);
		}

		protected virtual void RegisterEventHandler()
		{
			
		}
		
		protected virtual void UnregisterEventHandler()
		{
			
		}

		protected void SendEvent(GameEngine.Event e)
		{
            if (m_owner != null)
			    m_owner.SendEvent (e);
		}

		protected T GetMediator<T>(string name) where T: Mediator
		{
            if (m_owner != null)

                return m_owner.GetMediator(name) as T;
            else
                return null;
        }

		protected ProxyMgr proxyMgr
		{
			get
			{
				return ProxyMgr.Instance;
			}
		}

		void OnApplicationQuit()
		{
			quitApplication = true;
		}

		#endregion

		#region Members

		/// <summary>
        /// The mediator name
        /// </summary>
        protected string m_mediatorName;

        /// <summary>
        /// The view component being mediated
        /// </summary>
        protected UnityEngine.Object m_viewComponent;

		protected MediatorMgr m_owner;

		#endregion
	}
}
