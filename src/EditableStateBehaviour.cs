using System;
using UnityEngine;

namespace MicroState
{
    public class EditableStateBehaviour<StateType> : StateBehaviour<StateType> where StateType : MicroState.State, new()
	{
		public State EditorState = new State();

		public EditableStateBehaviour()
		{
			EditorState.ChangeEvent.AddListener(this.SetDirty);
		}

    	override protected void Pull(StateType state)
    	{
    		this.EditorState.TakeContentFrom(base.State);
    	}

    	override protected void Push(StateType state)
    	{
    		base.State.TakeContentFrom(this.EditorState);
    	}
    }
}         