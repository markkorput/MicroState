using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MicroState.Id
{
	public class AttrRef<ValueT>
	{
		public string StateId { get; private set; }
		public string AttrId { get; private set; }

		private GameObject gameObject = null;

		private IdStateBase stateBase_ = null;
		private ValueAttr<ValueT> valueAttr_ = null;
      
		public AttrRef(string stateid, string attrid, GameObject gameObject = null) {
			this.StateId = stateid;
			this.AttrId = attrid;
			this.gameObject = gameObject;
		}

        private IdStateInstanceBase FindStateInstance(string id)
        {
            return new List<IdStateInstanceBase>(
				this.gameObject == null
    				? GameObject.FindObjectsOfType<IdStateInstanceBase>()
    				: this.gameObject.GetComponentsInParent<IdStateInstanceBase>())
                        .Find((stateinstance) => stateinstance.Id.Equals(id));
        }
      
		public IdStateInstanceBase StateInstanceBase {
			get {
				return this.FindStateInstance(this.StateId);
			}
		}
      
        public IdStateBase StateBase
        {
            get
            {
                if (this.stateBase_ != null) return this.stateBase_;
                var stateinst = FindStateInstance(this.StateId);
                if (stateinst != null) this.stateBase_ = stateinst.GetState();
                return this.stateBase_;
            }
        }
      
        public ValueAttr<ValueT> ValueAttr
        {
            get
            {
                if (this.valueAttr_ != null) return this.valueAttr_;
                var statebase = this.StateBase;
                if (statebase == null) return null;
                this.valueAttr_ = statebase.GetAttr<ValueT>(this.AttrId);
                return this.valueAttr_;
            }
        }
      
        public void Set(ValueT val)
        {
            var attr = this.ValueAttr;
            if (attr == null) return;
            attr.Value = val;
        }

        public ValueT Value {
            get
            {
                var attr = this.ValueAttr;
                if (attr == null) return default(ValueT);
                return attr.Value;
            }

            set {
                this.Set(value);
            }
        }
	}
}
