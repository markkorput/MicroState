using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace MicroState.Id.Components
{
	public class IntValue : SingleValueBase<int>
	{
		[System.Serializable]
		public class ValueTypeEvent : UnityEvent<int> { }
		public ValueTypeEvent IntEvent; // = new ValueTypeEvent();

		private bool isRegistered = false;

        void OnEnable()
        {
            if (!isRegistered)
            {
                base.ValueEvent.AddListener(this.InvokeVal);
                isRegistered = true;
            }
        }
      
        private void InvokeVal(int v)
        {
            this.IntEvent.Invoke(v);
        }
	}
}