using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MicroState.Id.Components
{
	public class FloatAttr : BaseAttr<float>
	{
		[System.Serializable]
		public class ValueTypeEvent : UnityEvent<float> { }
		public ValueTypeEvent ValueEvent = new ValueTypeEvent();

		override protected void OnValue(float v)
		{
			if (!(this.InvokeWhenInactive || this.isActiveAndEnabled)) return;
			this.ValueEvent.Invoke(v);
		}
	}

#if UNITY_EDITOR
    [CustomEditor(typeof(FloatAttr))]
    [CanEditMultipleObjects]
    public class FloatAttrEditor : AttrRefEditor<float, FloatAttr>
    {

    }
#endif
}