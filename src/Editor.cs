using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MicroState
{
#if UNITY_EDITOR
	public class Editor<StateType> : UnityEditor.Editor where StateType : MicroState.State, new()
#else
	public class Editor<StateType> where StateType : MicroState.State, new()
#endif
	{
		private StateType editorState_ = new StateType();
		protected StateType EditorState { get { return editorState_; } }

		private int activePushes = 0;
		private int activePulls = 0;

		private List<SerializeField> SerializeFields = null;

		void OnEnable()
		{
#if UNITY_EDITOR
			var t = (StateInstance<StateType>)this.target;
            
			// Push changes from our EditorState to our Target's State
			this.EditorState.ChangeEvent.AddListener(() =>
			{
				if (t.PushChanges && activePushes == 0 && activePulls == 0)
				{
					activePushes += 1;
					t.State.TakeContentFrom(this.EditorState);
					activePushes -= 1;
				}
			});

			// Pull changes from our Target's State to our EditorState
			t.State.ChangeEvent.AddListener(() =>
			{
				if (t.PullChanges && activePushes == 0 && activePulls == 0)
				{
					activePulls += 1;
					this.EditorState.TakeContentFrom(t.State);
					activePulls -= 1;
				}
			});

			// Whenever our EditorState changes, make sure the Editor GUI is updated
			this.EditorState.ChangeEvent.AddListener(this.TriggerGuiUpdate);
#endif
		}

		void OnDisable()
		{
			this.EditorState.ChangeEvent.RemoveListener(this.TriggerGuiUpdate);
		}

#if UNITY_EDITOR
		public override void OnInspectorGUI()
#else
		public void OnInspectorGUI()
#endif

		{
#if UNITY_EDITOR
			var t = (StateInstance<StateType>)this.target;

			// TODO; make this configurable?
			t.PullChanges = EditorGUILayout.Toggle("Pull", t.PullChanges);
			t.PushChanges = EditorGUILayout.Toggle("Push", t.PushChanges);

			this.Fields();
#endif
		}

		private void TriggerGuiUpdate()
		{
#if UNITY_EDITOR
			EditorUtility.SetDirty(this.target);
#endif
		}


		protected virtual void Fields() { }

		protected void AddField(string label, MicroState.Attribute<string> attr)
		{
#if UNITY_EDITOR
            attr.Value = EditorGUILayout.TextField(label, attr.Value);
#endif
		}

		protected void AddField(string label, MicroState.Attribute<int> attr)
		{
#if UNITY_EDITOR
    		attr.Value = EditorGUILayout.IntField(label, attr.Value);
#endif
		}

		protected void AddField(string label, MicroState.Attribute<float> attr)
		{
#if UNITY_EDITOR
    		attr.Value = EditorGUILayout.FloatField(label, attr.Value);
#endif
		}

		protected void AddField(string label, MicroState.Attribute<bool> attr)
		{
#if UNITY_EDITOR
            attr.Value = EditorGUILayout.Toggle(label, attr.Value);
#endif
		}

		protected void AddField(System.Action func)
		{
#if UNITY_EDITOR
			func.Invoke();
#endif
		}
	}
}