using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MicroState.Demos
{
	// Generic C-Sharp class which contains the state
	public class CustomState
	{
		public string Name = "";
		public int Number = 0;
		public bool Approved = false;
	}
   
	// Our IdState wrapper around our generic CustomState class
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
   
	// Our IdStateInstance MonoBehaviour class
	public class CustomIdStateInstance : Id.IdStateInstance<CustomState, CustomIdState>
	{
	}
   
#if UNITY_EDITOR
	// The Editor for our state
	[CustomEditor(typeof(CustomIdStateInstance))]
	public class CustomIdStateInstanceEditor : Id.IdStateInstanceEditor<CustomState, CustomIdState>
	{

	}
#endif
}