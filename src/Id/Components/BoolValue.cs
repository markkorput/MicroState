using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace MicroState.Id.Components
{
	public class BoolValue : SingleValueBase<bool>
	{
		public class ValueTypeEvent : UnityEvent<bool> { }
		public ValueTypeEvent BoolEvent; // = new ValueTypeEvent();

        void OnEnable()
        {
            base.ValueEvent.AddListener(this.InvokeVal);
        }

        void OnDisable()
        {
            base.ValueEvent.RemoveListener(this.InvokeVal);
        }
      
        private void InvokeVal(bool v)
        {
            this.BoolEvent.Invoke(v);
        }
	}
}