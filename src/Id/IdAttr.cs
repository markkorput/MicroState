using System;
using UnityEngine;

namespace MicroState.Id
{
	public class BaseAttrDef
    {
        public string id;
        public System.Type ValueType;
      
		public System.Func<BaseAttr> attrCreator;
      
		public BaseAttr CreateAttr()
        {
			return this.attrCreator == null ? null : this.attrCreator.Invoke();
        }
    }
   
    public class AttrDef<StateT, ValT> : BaseAttrDef
    {
        public System.Func<StateT, ValT> getter;
        public System.Action<StateT, ValT> setter;
      
        public AttrDef(string i, System.Func<StateT, ValT> g, System.Action<StateT, ValT> s)
        {
            id = i; getter = g; setter = s;
            this.ValueType = getter.Method.ReturnType;
        }
      
		public AttrDef(string i, System.Func<StateT, ValT> g, System.Action<StateT, ValT> s, System.Func<BaseAttr> attrCreator)
			: this(i, g, s) {
			this.attrCreator = attrCreator;         
		}
    }
   
	public class BaseAttr
	{
		protected BaseAttrDef attrDef;
		public string Id { get { return attrDef.id; } }
		public System.Type ValueType { get { return attrDef.ValueType; } }
      
		public virtual bool IsEqual(BaseAttr other)
		{
			return false;
		}
	}
   
	public class ValueAttr<ValT> : BaseAttr {

		private System.Func<ValT> getter;
		private System.Action<ValT> setter;

		public ValT Value
        {
			get { return getter.Invoke(); }
			set { setter.Invoke(value);  }
        }

		protected ValueAttr() {}

		public ValueAttr(System.Func<ValT> g, System.Action<ValT> s) {
			this.getter = g;
			this.setter = s;
		}

		protected void SetAccessors(System.Func<ValT> g, System.Action<ValT> s)
        {
            this.getter = g;
            this.setter = s;
        }

		public override bool IsEqual(BaseAttr other)
        {
            return this.IsEqual((ValueAttr<ValT>)other);
        }
      
        public bool IsEqual(ValueAttr<ValT> other)
        {
            if (this.Value == null || other.Value == null) return this.Value == null && other.Value == null;
            return this.Value.Equals(other.Value);
        }
	}
   
    /// <summary>
	/// Attr binds an data-object (instance) to a ValueAttr, making the instance
	/// the source for ValueAttr's getter and the destination for ValueAttr's setter
    /// </summary>
    public class Attr<StateT, ValT> : ValueAttr<ValT>
    {
        private StateT instance;

		public Attr(StateT inst, AttrDef<StateT, ValT> attrDef)
        {
			this.instance = inst;
            this.attrDef = attrDef;

			if (inst == null) Debug.Log("NULL INST");
         
			base.SetAccessors(
                () => ((AttrDef<StateT, ValT>)attrDef).getter.Invoke(this.instance),
				(val) => ((AttrDef<StateT, ValT>)attrDef).setter.Invoke(this.instance, val));
        }
    }
}
