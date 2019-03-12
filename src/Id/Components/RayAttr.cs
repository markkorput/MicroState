using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MicroState.Id.Components
{
	public class RayAttr : BaseAttr<Ray>
	{
		[System.Serializable]
		public class ValueTypeEvent : UnityEvent<Ray> { }
		public ValueTypeEvent ValueEvent = new ValueTypeEvent();

		override protected void OnValue(Ray v)
		{
			if (!(this.InvokeWhenInactive || this.isActiveAndEnabled)) return;
			this.ValueEvent.Invoke(v);
		}
	}

#if UNITY_EDITOR
    [CustomEditor(typeof(RayAttr))]
    [CanEditMultipleObjects]
    public class RayAttrEditor : AttrRefEditor<Ray, RayAttr>
    {

    }
#endif
}