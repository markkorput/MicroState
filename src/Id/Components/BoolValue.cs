using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace MicroState.Id.Components
{
	public class BoolValue : SingleValueBase<bool>
	{
		[System.Serializable]
		public class ValueTypeEvent : UnityEvent<bool> { }
		[Header("Bool Value")]
		public ValueTypeEvent BoolEvent; // = new ValueTypeEvent();
		public UnityEvent TrueEvent; // = new ValueTypeEvent();
		public UnityEvent FalseEvent;

		private bool isRegistered = false;
      
        void OnEnable()
        {
            if (!isRegistered)
            {
                base.ValueEvent.AddListener(this.InvokeVal);
                isRegistered = true;
            }
        }

        private void InvokeVal(bool v)
        {
            this.BoolEvent.Invoke(v);
			(v ? this.TrueEvent : this.FalseEvent).Invoke();
        }
	}
}