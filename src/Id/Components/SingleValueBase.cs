using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace MicroState.Id.Components
{
	public class SingleValueBase<ValueT> : SingleValueActions<ValueT>
	{
		[Header("Listener")]
		public bool InvokeWhenInactive = false;
      
		[System.Serializable]
        public class ValTypeEvent : UnityEvent<ValueT> {}      
		public ValTypeEvent ValueEvent = new ValTypeEvent();      

		private bool isFirstValue = true;
		private ValueT lastValue;

		private void Start()
		{
			var statebase = base.ResolvedStateBase;
			if (statebase != null) {
				statebase.ChangeEvent += this.OnStateChange;
                this.ProcessAttr(statebase);
			} else {
				Debug.LogWarning("[MicroState.Id.SingleValueBase] Could not find State");
			}         
		}

		private void OnDestroy() {
			var statebase = base.ResolvedStateBase;
			if (statebase != null)
			{
				statebase.ChangeEvent -= this.OnStateChange;
				// statebase = null;
			}
		}
      
		private void OnStateChange()
		{
			this.ProcessAttr(base.ResolvedStateBase);
		}
      
		private void ProcessAttr(IdStateBase state)
		{
			var attr = base.ResolvedValueAttr;
    		if (attr == null) return;
         
			var val = attr.Value;
         
			if (this.isFirstValue || !lastValue.Equals(val))
			{
				if (this.InvokeWhenInactive || this.isActiveAndEnabled)
					this.ValueEvent.Invoke(val);

				this.lastValue = val;
				this.isFirstValue = false;
			}
		}
	}
}