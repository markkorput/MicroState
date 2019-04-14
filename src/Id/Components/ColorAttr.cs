using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MicroState.Id.Components
{
	public class ColorAttr : BaseAttr<Color>
	{
		[System.Serializable]
		public class ValueTypeEvent : UnityEvent<Color> { }
		public ValueTypeEvent ValueEvent = new ValueTypeEvent();

		override protected void OnValue(Color v)
		{
			this.ValueEvent.Invoke(v);
		}
	}

#if UNITY_EDITOR
    [CustomEditor(typeof(ColorAttr))]
    [CanEditMultipleObjects]
    public class ColorAttrEditor : AttrRefEditor<Color, ColorAttr>
    {

    }
#endif
}