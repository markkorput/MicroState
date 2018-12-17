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
				{ // State Object Selector
					var objs = ((MonoBehaviour)this.target).gameObject.GetComponentsInParent<IdStateInstanceBase>();
					var objids = (from ob in objs select ob.Id).ToList();

					var selectedidx = objids.IndexOf(StateIdProp.stringValue);

					selectedidx = EditorGUILayout.Popup("Available States", selectedidx, objids.ToArray());
					if (selectedidx >= 0 && selectedidx < objids.Count)
					{
						StateIdProp.stringValue = objids[selectedidx];
						AttrIdProp.serializedObject.ApplyModifiedProperties();
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
					}
				}

				// Try to fetch initial value (if it fails with an InvalidCastException, this means that the
				// specified Attribute's value type does not match with our ValueT)
				try
				{
					if (attrRef.StateBase == null || attrRef.ValueAttr == null)
					{
						if (attrRef.StateBase == null) EditorGUILayout.HelpBox("Could not find State Container", MessageType.Warning);
						else if (attrRef.ValueAttr == null) EditorGUILayout.HelpBox("Could not find State Attribute", MessageType.Warning);
					} else {
						// EditorGUILayout.LabelField("Initial Value: "+attrRef.ValueAttr.Value.ToString());
						EditorGUILayout.HelpBox("Initial Value: " + attrRef.ValueAttr.Value.ToString(), MessageType.Info);
					}
				}
				catch (System.InvalidCastException exc)
				{
					EditorGUILayout.HelpBox("INVALID_TYPE", MessageType.Error);
				}
			}

			EditorGUILayout.Separator();
			this.DrawDefaultInspector();
		}
	}
}
#endif