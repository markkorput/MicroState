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
      
        void OnEnable() {
			base.ValueEvent.AddListener(this.InvokeVal);
        }      
      
		void OnDisable() {
			base.ValueEvent.RemoveListener(this.InvokeVal);
        }
      
		private void InvokeVal(string v) {
			Debug.Log("String invoke");
#if UNITY_EDITOR
			this.DebugInfo.Value = v;
#endif
			this.StringEvent.Invoke(v);
		}
	}
}