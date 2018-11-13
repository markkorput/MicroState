using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace MicroState.Id.Components
{
	public class FloatValue : SingleValueBase<float>
	{
		[System.Serializable]
		public class ValueTypeEvent : UnityEvent<float> { }
		public ValueTypeEvent FloatEvent = new ValueTypeEvent();
      
		void OnEnable()
        {
            base.ValueEvent.AddListener(this.InvokeVal);
        }

        void OnDisable()
        {
            base.ValueEvent.RemoveListener(this.InvokeVal);
        }

        private void InvokeVal(float v)
        {
            this.FloatEvent.Invoke(v);
        }
	}
}