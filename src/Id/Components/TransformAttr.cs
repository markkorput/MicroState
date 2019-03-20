using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MicroState.Id.Components
{
	public class TransformAttr : BaseAttr<Transform>
	{
		[System.Serializable]
		public class ValueTypeEvent : UnityEvent<Transform> { }
		public ValueTypeEvent ValueEvent = new ValueTypeEvent();

		override protected void OnValue(Transform v)
		{
			if (!(this.InvokeWhenInactive || this.isActiveAndEnabled)) return;
			this.ValueEvent.Invoke(v);
		}
	}

#if UNITY_EDITOR
    [CustomEditor(typeof(TransformAttr))]
    [CanEditMultipleObjects]
    public class TransformAttrEditor : AttrRefEditor<Transform, TransformAttr>
    {

    }
#endif
}