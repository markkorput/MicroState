// This example shows a custom inspector for an
// object "MyPlayer", which has a variable speed.
using UnityEngine;
using UnityEditor;
using System.Collections;

namespace MicroState.Id
{
	public class IdStateInstanceEditor<DataT, IdStateT> : Editor where IdStateT : IdState<DataT>, new()
	{
		public override void OnInspectorGUI()
		{
			var stateInstance = (Id.IdStateInstance<DataT, IdStateT>)target;
         
			var attrs = stateInstance.State.GetAttributes();
         
			foreach(var generic_attr in attrs) {            

				// int?
				if( generic_attr.ValueType.Equals(((int)0).GetType()) ) {               
					var attr = (Id.Attr<DataT, int>)generic_attr;
					attr.Value = EditorGUILayout.IntField(attr.Id, attr.Value);
				}

				// string?
                if( generic_attr.ValueType.Equals(((string)"").GetType()) ) {               
                    var attr = (Id.Attr<DataT, string>)generic_attr;
					attr.Value = EditorGUILayout.TextField(attr.Id, attr.Value);
                }

				// float?
                if (generic_attr.ValueType.Equals(((float)0.0f).GetType()))
                {
                    var attr = (Id.Attr<DataT, float>)generic_attr;
					attr.Value = EditorGUILayout.FloatField(attr.Id, attr.Value);
                }
            
				// bool?
                if (generic_attr.ValueType.Equals(((bool)true).GetType()))
                {
                    var attr = (Id.Attr<DataT, bool>)generic_attr;
                    attr.Value = EditorGUILayout.Toggle(attr.Id, attr.Value);
                }
                
				// double?
                if (generic_attr.ValueType.Equals(((double)0.0).GetType()))
                {
                    var attr = (Id.Attr<DataT, double>)generic_attr;
					attr.Value = EditorGUILayout.DoubleField(attr.Id, attr.Value);
                }

				// long?
                if (generic_attr.ValueType.Equals(((long)0).GetType()))
                {
                    var attr = (Id.Attr<DataT, long>)generic_attr;
					attr.Value = EditorGUILayout.LongField(attr.Id, attr.Value);
                }
			}

			// Show default inspector property editor
			DrawDefaultInspector();
		}
	}
   
	// [CustomEditor(typeof(IdStateInstance<InstanceT>))]
}