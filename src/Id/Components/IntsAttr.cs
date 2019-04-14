using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MicroState.Id.Components
{
	public class IntsAttr : BaseAttr<int[]>
	{
		[System.Serializable]
		public class ValueTypeEvent : UnityEvent<int[]> { }
		public ValueTypeEvent ValueEvent = new ValueTypeEvent();

		override protected void OnValue(int[] v)
		{
			this.ValueEvent.Invoke(v);
		}
	}

#if UNITY_EDITOR
    [CustomEditor(typeof(IntsAttr))]
    [CanEditMultipleObjects]
    public class IntsAttrEditor : AttrRefEditor<int[], IntsAttr>
    {

    }
#endif
}