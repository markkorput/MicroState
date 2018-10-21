using System;
using UnityEngine.Events;

namespace MicroState
{
	public class StateHandler<StateType> where StateType : State, new()
    {
		private StateType MasterState;
		private StateType PreviousState = new StateType();
		private StateType FirstState = new StateType();
		private System.Action<StateType, StateType> func;
		private bool isDirty = true;
      
		private class UpdateEvent : UnityEvent<StateType, StateType> { }
        private UpdateEvent Event = new UpdateEvent();

		public bool IsDirty { get { return this.isDirty; }}
      
		public StateHandler(StateType state)
        {
			this.MasterState = state;
			this.MasterState.ChangeEvent.AddListener(this.OnMasterStateChange);
        }

		public void Dispose() {
			this.Event.RemoveAllListeners();
         
			if (this.MasterState != null) {
				this.MasterState.ChangeEvent.RemoveListener(this.OnMasterStateChange);
			}
		}

		public void Add(UnityAction<StateType,StateType> func, bool invokeDirectly = true) {
			if (invokeDirectly) func.Invoke(FirstState, MasterState);
			Event.AddListener(func);
		}

		public void Remove(UnityAction<StateType, StateType> func) {
			Event.RemoveListener(func);
		}

		private void OnMasterStateChange() {
			Event.Invoke(this.PreviousState, this.MasterState);
			this.PreviousState.TakeContentFrom(this.MasterState);
		}
    }
}
