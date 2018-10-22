using UnityEngine;
using System.Collections;

namespace MicroState
{

	public class StateListener<StateType, InstanceType> : MonoBehaviour where StateType : State, new() where InstanceType : StateInstance<StateType>
	{
		public InstanceType Instance;
		private System.IDisposable subscription1, subscription2;
      
        void Start()
        {
			this.subscription1 = Utils.Subscribe<StateType>(this.Instance, this.gameObject, this.ProcessState);
			this.subscription2 = Utils.Subscribe<StateType>(this.Instance, this.gameObject, this.ProcessStateChange);
        }

        private void OnDestroy()
        {
            if (this.subscription1 != null) this.subscription1.Dispose();
			if (this.subscription2 != null) this.subscription2.Dispose();
        }

		protected virtual void ProcessState(StateType state) {}
		protected virtual void ProcessStateChange(StateType prev, StateType state) {}

		protected void WithState(System.Action<StateType> func) {
			Utils.With<StateType>(this.Instance, this.gameObject, func);
		}
	}
}