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
		public UnityEvent TrueEvent = new UnityEvent();
		public UnityEvent FalseEvent = new UnityEvent();

		override protected void OnValue(bool v)
		{
			this.ChangeEvent.Invoke(v);
			(v ? this.TrueEvent : this.FalseEvent).Invoke();
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