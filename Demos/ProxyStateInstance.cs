using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MicroState.Demos
{
	[System.Serializable]
	public class ProxyData
	{
		public string Postfix = "";
	}

	public class ProxyState : Id.IdStateProxy<ProxyData, CustomState>
	{
		public ProxyState() : this(new ProxyData()) {}

		public ProxyState(ProxyData instance) : base(instance)
		{
			base.CreateAttr<string>("PostfixedName",
				(state, origin) => origin.Name + state.Postfix);
		}
	}

	/// <summary>
    /// This Is our boilerplate MonoBehaviour for use in the Unity editor
	/// This instance will be used by state-observers to link against, or to find
	/// their state instance.
    /// </summary>
	public class ProxyStateInstance : Id.IdStateProxyInstance<ProxyData, ProxyState, CustomState, CustomIdState>
	{
	}
}