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
		SerializedProperty StateInstanceProp, StateIdProp, AttrIdProp;
      
		void OnEnable()
		{
            StateInstanceProp = serializedObject.FindProperty("StateId");
			StateIdProp = serializedObject.FindProperty("StateId");
			AttrIdProp = serializedObject.FindProperty("AttrId");
		}

		public override void OnInspectorGUI()
		{
			this.DrawDefaultInspector();
			EditorGUILayout.Separator();

			var style = GUI.skin.label;
            style.fontStyle = FontStyle.Bold;         
			EditorGUILayout.LabelField("State/Attribute Form", new GUIStyle(style));         
			this.DrawStateForm();
		}

        private IdStateInstanceBase GetStateInst(SerializedProperty prop) {
            var targetObject = prop.serializedObject.targetObject;
            var targetObjectClassType = targetObject.GetType();
            var field = targetObjectClassType.GetField(prop.propertyPath);
            if (field == null) return null;
            var value = field.GetValue(targetObject);
            // Debug.Log(value.s);
            return (IdStateInstanceBase)value;
        }

		protected void DrawStateForm() {
            var stateinst = ((Components.BaseAttrBase)this.target).StateInstance;

			var attrRef = stateinst != null
                ? new MicroState.Id.AttrRef<ValueT>(stateinst.GetState(), AttrIdProp.stringValue)
                : new MicroState.Id.AttrRef<ValueT>(StateIdProp.stringValue, AttrIdProp.stringValue, ((MonoBehaviour)this.target).gameObject);

            // var attrRef = new MicroState.Id.AttrRef<ValueT>(StateIdProp.stringValue, AttrIdProp.stringValue, ((MonoBehaviour)this.target).gameObject);

            if (stateinst == null) {
                {   // State Object ID Selector
                    var objs = ((MonoBehaviour)this.target).gameObject.GetComponentsInParent<IdStateInstanceBase>();
                    var objids = (from ob in objs select ob.Id).ToList();
                
                    var selectedidx = objids.IndexOf(StateIdProp.stringValue);
                    var newselectedidx = EditorGUILayout.Popup("Available States", selectedidx, objids.ToArray());            
                
                    if (newselectedidx != selectedidx && newselectedidx >= 0 && newselectedidx < objids.Count)
                    {
                        StateIdProp.stringValue = objids[newselectedidx];
                        AttrIdProp.serializedObject.ApplyModifiedProperties();
                    }
                }

                {
                    // State Object Selector
                    var stateobj = attrRef.StateInstanceBase;
                    var newstateobj = (IdStateInstanceBase)EditorGUILayout.ObjectField("State Instance Object",
                        stateobj, typeof(IdStateInstanceBase), true);

                    if (newstateobj != stateobj) {
                        StateIdProp.stringValue = newstateobj == null ? "" : newstateobj.Id;
                        AttrIdProp.serializedObject.ApplyModifiedProperties();               
                    }
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
                }
                else
                {
                    var val = attrRef.ValueAttr.Value;
					EditorGUILayout.HelpBox(val == null ? "null" : val.ToString(), MessageType.Info);
                }
            }
            catch (System.InvalidCastException exc)
            {
                EditorGUILayout.HelpBox("INVALID_TYPE", MessageType.Error);
            }
		}
	}
}
#endif