using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace MicroState.Id.Components
{
	public class IntValue : SingleValueBase<int>
	{
		public class ValueTypeEvent : UnityEvent<int> { }
		public ValueTypeEvent IntEvent; // = new ValueTypeEvent();
      
        void OnEnable()
        {
            base.ValueEvent.AddListener(this.InvokeVal);
        }

        void OnDisable()
        {
            base.ValueEvent.RemoveListener(this.InvokeVal);
        }

        private void InvokeVal(int v)
        {
            this.IntEvent.Invoke(v);
        }
	}
}