#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace MicroState.Id
{
	[CustomEditor(typeof(IdStateInstanceBase), true)]
    [CanEditMultipleObjects]
	public class IdStateInstanceBaseEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            this.DrawDefaultInspector();
			EditorGUILayout.Separator();

			this.DrawAvailableAttributesInfo();         
        }
      
        protected void DrawAvailableAttributesInfo() {
			var inst = (IdStateInstanceBase)this.target;
			var state = inst.GetState();
			var attrIds = state == null ? new string[0] : state.GetAttributeIds();
			EditorGUILayout.HelpBox("Attribute IDs:\n\n"+string.Join("\n", attrIds), MessageType.Info);
        }
    }
}
#endif