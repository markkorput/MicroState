using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MicroState.Id.Components
{
	public class GameObjectAttr : BaseAttr<GameObject>
	{
		[System.Serializable]
		public class ValueTypeEvent : UnityEvent<GameObject> { }
		public ValueTypeEvent ValueEvent = new ValueTypeEvent();

		override protected void OnValue(GameObject v)
		{
			this.ValueEvent.Invoke(v);
		}
	}

#if UNITY_EDITOR
    [CustomEditor(typeof(GameObjectAttr))]
    [CanEditMultipleObjects]
    public class GameObjectAttrEditor : AttrRefEditor<GameObject, GameObjectAttr>
    {

    }
#endif
}