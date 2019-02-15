using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MicroState.Id.Components
{
	public class BoolAttr : BaseAttr<bool>
	{
		[System.Serializable]
		public class ValueTypeEvent : UnityEvent<bool> { }
		public ValueTypeEvent ChangeEvent = new ValueTypeEvent();
		public UnityEvent TrueEvent;
		public UnityEvent FalseEvent;

		override protected void OnValue(bool v)
		{
			if (!(this.InvokeWhenInactive || this.isActiveAndEnabled)) return;
			this.ChangeEvent.Invoke(v);
			var evt = (v ? this.TrueEvent : this.FalseEvent);
			if (evt != null) evt.Invoke(); // In unity 2018.3 this started giving NullReferenceExceptions in PlayMode tests...
		}

		#region Public Action Methods
			// 	public void Set(bool v) { this.AttrListener.Set(v); }
			public void Toggle() { this.AttrListener.Value = !this.AttrListener.Value; }
		#endregion
	}

#if UNITY_EDITOR
    [CustomEditor(typeof(BoolAttr))]
    [CanEditMultipleObjects]
    public class BoolAttrEditor : AttrRefEditor<bool, BoolAttr>
    {

    }
#endif
}