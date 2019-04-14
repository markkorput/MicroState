using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MicroState.Id.Components
{
	public class IntAttr : BaseAttr<int>
	{
		[System.Serializable]
		public class ValueTypeEvent : UnityEvent<int> { }
		public ValueTypeEvent ValueEvent = new ValueTypeEvent();

		override protected void OnValue(int v)
		{
			this.ValueEvent.Invoke(v);
		}
	}

#if UNITY_EDITOR
    [CustomEditor(typeof(IntAttr))]
    [CanEditMultipleObjects]
    public class IntAttrEditor : AttrRefEditor<int, IntAttr>
    {

    }
#endif
}