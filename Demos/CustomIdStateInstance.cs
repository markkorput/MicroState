using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MicroState.Demos
{
	/// <summary>
	/// Generic C-Sharp class which contains our app-specific state
	/// Mark is as serializable so it is persisted between sessions
	/// and the default editor shows all fields
    /// </summary>
	[System.Serializable]
	public class CustomState
	{
		public string Name = "";
		public int Number = 0;
		public bool Approved = false;
	}
   
	/// <summary>
	/// Our IdState wrapper around our generic CustomState class
	/// In this class we'll defined attributes with names and getter/setter
	/// methods to propagate changes to/from our CustomState generic state instance
    /// </summary>
	public class CustomIdState : Id.IdState<CustomState>
	{
		public CustomIdState() : this(new CustomState()) {}

		public CustomIdState(CustomState instance) : base(instance)
		{
			base.CreateAttr<string>("Name",
									(state) => state.Name,
									(state, val) => state.Name = val);
         
			base.CreateAttr<int>("Number",
									(state) => state.Number,
									(state, val) => state.Number = val);

			base.CreateAttr<bool>("Approved",
								  (state) => state.Approved,
								  (s, b) => s.Approved = b);
		}
	}
   
	/// <summary>
    /// This Is our boilerplate MonoBehaviour for us in the Unity editor
	/// This instance will be used by state-observers to against, or to find
	/// their state instance.
    /// </summary>
	public class CustomIdStateInstance : Id.IdStateInstance<CustomState, CustomIdState>
	{
	}
   
//#if UNITY_EDITOR
//	/// <summary>
//    /// This Is our boilerplate Editor which is able to determine what (type of)
//	/// fields to render based on the attributes in gets from the IdState
//	/// </summary>
//	[CustomEditor(typeof(CustomIdStateInstance))]
//	public class CustomIdStateInstanceEditor : Id.IdStateInstanceEditor<CustomState, CustomIdState, CustomIdStateInstance>
//	{
//	}
//#endif
}