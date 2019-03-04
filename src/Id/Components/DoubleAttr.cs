using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MicroState.Id.Components
{
	public class DoubleAttr : BaseAttr<double>
	{
		[System.Serializable]
		public class ValueTypeEvent : UnityEvent<double> { }
		public ValueTypeEvent ValueEvent = new ValueTypeEvent();

		override protected void OnValue(double v)
		{
			if (!(this.InvokeWhenInactive || this.isActiveAndEnabled)) return;
			this.ValueEvent.Invoke(v);
		}
	}

#if UNITY_EDITOR
    [CustomEditor(typeof(DoubleAttr))]
    [CanEditMultipleObjects]
    public class DoubleAttrEditor : AttrRefEditor<double, DoubleAttr>
    {

    }
#endif
}