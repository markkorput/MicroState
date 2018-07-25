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
				t.EditorState.ChangeEvent.AddListener(this.TriggerGuiUpdate);
            }
        }

        public override void OnInspectorGUI()
        {
            var t = (EditableStateBehaviour<StateType>)this.target;
			t.OnInspectorGUI();
        }

		private void TriggerGuiUpdate() {
			EditorUtility.SetDirty(this.target);
		}
    }
}         