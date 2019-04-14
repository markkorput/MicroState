using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MicroState.Id.Components
{
	public class StringAttr : BaseAttr<string>
	{
		[System.Serializable]
		public class ValueTypeEvent : UnityEvent<string> { }
		public ValueTypeEvent ValueEvent = new ValueTypeEvent();

		override protected void OnValue(string v)
		{
			this.ValueEvent.Invoke(v);
		}
	}

#if UNITY_EDITOR
    [CustomEditor(typeof(StringAttr))]
    [CanEditMultipleObjects]
    public class StringAttrEditor : AttrRefEditor<string, StringAttr>
    {

    }
#endif
}