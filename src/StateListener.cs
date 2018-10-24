using UnityEngine;
using System.Collections;

namespace MicroState
{

	public class StateListener<StateType, InstanceType> : MonoBehaviour where StateType : State, new() where InstanceType : StateInstance<StateType>
	{
		[Header("State Listener Attributes")]
		public InstanceType Instance;
		public bool InvokeEventsWhenInactive = false;
		private System.IDisposable subscription1, subscription2;
      
        void Start()
        {
			this.subscription1 = Utils.Subscribe<StateType>(this.Instance, this.gameObject, this.PrivateProcessState);
			this.subscription2 = Utils.Subscribe<StateType>(this.Instance, this.gameObject, this.PrivateProcessStateChange);
        }

        private void OnDestroy()
        {
            if (this.subscription1 != null) this.subscription1.Dispose();
			if (this.subscription2 != null) this.subscription2.Dispose();
        }

		private void PrivateProcessState(StateType state) {
			if (this.isActiveAndEnabled || this.InvokeEventsWhenInactive)
				this.ProcessState(state);
		}

		private void PrivateProcessStateChange(StateType prev, StateType state) {
			if (this.isActiveAndEnabled || this.InvokeEventsWhenInactive)
				this.ProcessStateChange(prev, state);
		}

		protected virtual void ProcessState(StateType state) {}
		protected virtual void ProcessStateChange(StateType prev, StateType state) {}

		protected void WithState(System.Action<StateType> func) {
			Utils.With<StateType>(this.Instance, this.gameObject, func);
		}
	}
}