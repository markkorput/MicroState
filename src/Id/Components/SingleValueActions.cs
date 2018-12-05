using UnityEngine;
using System.Collections.Generic;

namespace MicroState.Id.Components
{
	public class SingleValueActions<ValueT> : MonoBehaviour
	{
		[Header("Attribute ID")]
		public string StateId;
		public string AttrId;
      
		private IdStateBase stateBase = null;
		private ValueAttr<ValueT> valueAttr = null;      

		protected IdStateInstanceBase FindStateInstance(string id)
		{
			return new List<IdStateInstanceBase>(
				this.GetComponentsInParent<IdStateInstanceBase>())
				.Find((stateinstance) => stateinstance.Id.Equals(id));
		}

		protected IdStateBase ResolvedStateBase { 
			get {
				if (this.stateBase != null) return this.stateBase;
				var stateinst = FindStateInstance(this.StateId);
                if (stateinst != null) this.stateBase = stateinst.GetState();
				return this.stateBase;            
			}
		}
      
		protected ValueAttr<ValueT> ResolvedValueAttr {
			get {
				if (this.valueAttr != null) return this.valueAttr;
				var statebase = this.ResolvedStateBase;
				if (statebase == null) return null;
				this.valueAttr = statebase.GetAttr<ValueT>(this.AttrId);
				return this.valueAttr;
			}
		}

		#region Public Methods
		public void Set(ValueT val) {
			var attr = this.ResolvedValueAttr;
			if (attr == null) return;
			attr.Value = val;
		}
		#endregion
	}
}