using System;
using UnityEngine;
using UnityEditor;

namespace MicroState
{
	public class Editor<StateType> : UnityEditor.Editor where StateType : MicroState.State, new()
    {
        void OnEnable()
        {
            if (this.target != null)
            {
                var t = (EditableStateBehaviour<StateType>)this.target;
                t.EditorState.ChangeEvent.AddListener(() =>
                {
                    EditorUtility.SetDirty(this.target);
                });
            }
        }

        public override void OnInspectorGUI()
        {
            var t = (EditableStateBehaviour<StateType>)this.target;

            t.PullChanges = EditorGUILayout.Toggle("Pull", t.PullChanges);
            t.PushChanges = EditorGUILayout.Toggle("Push", t.PushChanges);


			var attrs = t.EditorState.ValueTypeProviders;

			foreach(var attr in attrs) {
				switch(attr.GetValueType().ToString()) {
					case "int":
						EditorGUILayout.IntField("int", 0);
						break;
					case "float":
						EditorGUILayout.FloatField("float", 0.0f);
                        break;
					case "bool":                  
                        EditorGUILayout.Toggle("bool", false);
                        break;
				}            
			}
            //t.experience = EditorGUILayout.IntField("Experience", t.experience);
            // EditorGUILayout.LabelField("Level", t.Level.ToString());
            //t.EditorState.AngleAttr.Value = EditorGUILayout.FloatField("Angle", t.EditorState.AngleAttr.Value);
        }
    }
}         