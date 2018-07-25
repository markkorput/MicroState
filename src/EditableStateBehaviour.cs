using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MicroState
{
    public class EditableStateBehaviour<StateType> : StateBehaviour<StateType> where StateType : MicroState.State, new()
	{
		public StateType EditorState { get; private set; }
		private List<System.Action> guiFuncs = new List<System.Action>();
      
		public EditableStateBehaviour() {
			this.EditorState = new StateType();

			this.EditorState.ChangeEvent.AddListener(() =>
			{
				if (this.PushChanges)
				{
					this.State.TakeContentFrom(this.EditorState);
				}
			});

			this.State.ChangeEvent.AddListener(() =>
            {
                if (this.PullChanges)
                {
                    this.EditorState.TakeContentFrom(this.State);
                }
            });  
         
			//if (PushChanges) {
			//	this.State.TakeContentFrom(this.EditorState);
			//}
		}
      
		public void OnInspectorGUI() {
#if UNITY_EDITOR
			// t.OnInspectorGUI();
			this.PullChanges = EditorGUILayout.Toggle("Pull", this.PullChanges);
			this.PushChanges = EditorGUILayout.Toggle("Push", this.PushChanges);

			this.EditorState.BatchUpdate(() =>
			{
				foreach (var func in guiFuncs) func.Invoke();
			});
#endif
		}

		protected void AddField(string label, MicroState.Attribute<string> attr)
        {
#if UNITY_EDITOR
            guiFuncs.Add(() =>
            {
				attr.Value = EditorGUILayout.TextField(label, attr.Value);
            });
#endif
        }

		protected void AddField(string label, MicroState.Attribute<int> attr) {
#if UNITY_EDITOR
			guiFuncs.Add(() =>
			{
				attr.Value = EditorGUILayout.IntField(label, attr.Value);
			});
#endif
		}

		protected void AddField(string label, MicroState.Attribute<float> attr) {
#if UNITY_EDITOR
			guiFuncs.Add(() =>
			{
				attr.Value = EditorGUILayout.FloatField(label, attr.Value);
			});
#endif
		}

		protected void AddField(string label, MicroState.Attribute<bool> attr) {
#if UNITY_EDITOR
			guiFuncs.Add(() =>
			{
				attr.Value = EditorGUILayout.Toggle(label, attr.Value);
			});
#endif
		}      
	}
}         