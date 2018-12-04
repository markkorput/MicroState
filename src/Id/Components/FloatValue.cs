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

		private bool isRegistered = false; 

		void OnEnable()
        {
			if (!isRegistered)
			{
				base.ValueEvent.AddListener(this.InvokeVal);
				isRegistered = true;
			}
        }
      
        private void InvokeVal(float v)
        {
            this.FloatEvent.Invoke(v);
        }
	}
}