using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace MicroState.Id.Components
{
	public class StringValue : SingleValueBase<string>
	{
		[System.Serializable]
		public class ValueTypeEvent : UnityEvent<string> { }
		public ValueTypeEvent StringEvent; // = new ValueTypeEvent();

		#if UNITY_EDITOR
        [System.Serializable]
        public class Dinfo
        {
            public string Value;
        }
        public Dinfo DebugInfo;
        #endif
      
		private bool isRegistered = false;

        void OnEnable()
        {
            if (!isRegistered)
            {
                base.ValueEvent.AddListener(this.InvokeVal);
                isRegistered = true;
            }
        }

        private void InvokeVal(string v)
        {
            this.StringEvent.Invoke(v);
#if UNITY_EDITOR
            this.DebugInfo.Value = v;
#endif
        }
	}
}