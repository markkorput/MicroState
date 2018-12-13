#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace MicroState.Id
{
    // [CustomEditor(typeof(Components.StringValue))]
	[CanEditMultipleObjects]
	public class AttrRefEditor<ValueT, ValueCompType> : Editor where ValueCompType : MonoBehaviour 
	{
		SerializedProperty StateIdProp, AttrIdProp;

		void OnEnable()
		{
			StateIdProp = serializedObject.FindProperty("StateId");
			AttrIdProp = serializedObject.FindProperty("AttrId");
		}
     
		public override void OnInspectorGUI()
		{
			var attrRef = new MicroState.Id.AttrRef<ValueT>(StateIdProp.stringValue, AttrIdProp.stringValue, ((MonoBehaviour)this.target).gameObject);
			{
				{   // State Selector
					var objs = ((MonoBehaviour)this.target).gameObject.GetComponentsInParent<IdStateInstanceBase>();
					var objids = (from ob in objs select ob.Id).ToList();

					var selectedidx = objids.IndexOf(StateIdProp.stringValue);
					selectedidx = EditorGUILayout.Popup("Available States", selectedidx, objids.ToArray());
					if (selectedidx >= 0 && selectedidx < objids.Count)
					{
                        StateIdProp.stringValue = objids[selectedidx];
						AttrIdProp.serializedObject.ApplyModifiedProperties();
						// ((ValueCompType)this.target).StateId = objids[selectedidx];
						//StateIdProp = ids[selectedidx];
					}
				}
            
				// Attr Selector
				if (attrRef.StateBase != null)
				{
					var ids = attrRef.StateBase.GetAttributeIds();
					var selectedidx = new List<string>(ids).IndexOf(attrRef.AttrId);
					selectedidx = EditorGUILayout.Popup("Available Attributes", selectedidx, ids);
					if (selectedidx >= 0 && selectedidx < ids.Length)
					{                     
						AttrIdProp.stringValue = ids[selectedidx];
						AttrIdProp.serializedObject.ApplyModifiedProperties();
						// ((ValueCompType)this.target).AttrId = ids[selectedidx];
					}
				}

				try
				{
					if (attrRef.StateBase == null || attrRef.ValueAttr == null)
					{
						if (attrRef.StateBase == null) EditorGUILayout.TextArea("Could not find State Container");
						if (attrRef.ValueAttr == null) EditorGUILayout.TextArea("Could not find State Attribute");
					} else {
						EditorGUILayout.LabelField("Initial Value: "+attrRef.ValueAttr.Value.ToString());                  
					}
				}
				catch (System.InvalidCastException exc)
				{
					EditorGUILayout.TextArea("INVALID TYPE");
				}
			}
        
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
        
			this.DrawDefaultInspector();
		}
	}
}
#endif