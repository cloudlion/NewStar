/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/

#region Using

using System;
using System.Collections.Generic;

#endregion

namespace MVC
{
    public interface IMediator
	{
		/// <summary>
        /// Tthe <c>IMediator</c> instance name
        /// </summary>
		string MediatorName { get; }
		
        /// <summary>
        /// The <c>IMediator</c>'s view component
        /// </summary>
		UnityEngine.Object ViewComponent { get; set; }

		/// <summary>
		/// Called by the View when the Mediator is registered
		/// </summary>
		void OnRegister(MediatorMgr mgr);

		/// <summary>
		/// Called by the View when the Mediator is removed
		/// </summary>
		void OnRemove();
	
	}
}
