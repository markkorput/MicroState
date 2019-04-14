using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MicroState.Id.Components
{
	public class QuaternionAttr : BaseAttr<Quaternion>
	{
		[System.Serializable]
		public class ValueTypeEvent : UnityEvent<Quaternion> { }
		public ValueTypeEvent ValueEvent = new ValueTypeEvent();

		override protected void OnValue(Quaternion v)
		{
			this.ValueEvent.Invoke(v);
		}
	}

#if UNITY_EDITOR
    [CustomEditor(typeof(QuaternionAttr))]
    [CanEditMultipleObjects]
    public class QuaternionAttrEditor : AttrRefEditor<Quaternion, QuaternionAttr>
    {

    }
#endif
}