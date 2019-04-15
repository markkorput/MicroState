using System;
using UnityEngine;

namespace MicroState.Id
{
	/// Typeless base for each Attr with method/attributes common to all attr, regardless of value-type
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

		public ValueAttr(System.Func<ValT> g, System.Action<ValT> s, BaseAttrDef attrDef) {
			base.attrDef = attrDef;
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
            return AreEqual(this, (ValueAttr<ValT>)other);
        }

        public static bool AreEqual(ValueAttr<ValT> a, ValueAttr<ValT> b)
        {
            if (a.Value == null || b.Value == null) return a.Value == null && b.Value == null;
            return a.Value.Equals(b.Value);
        }
	}

    // /// <summary>
	// /// Attr binds an data-object (instance) to a ValueAttr, making the instance
	// /// the source for ValueAttr's getter and the destination for ValueAttr's setter
    // /// </summary>
    // public class Attr<StateT, ValT> : ValueAttr<ValT>
    // {
    //     private StateT instance;

	// 	public Attr(StateT inst, AttrDef<StateT, ValT> attrDef)
    //     {
	// 		this.instance = inst;
    //         this.attrDef = attrDef;

	// 		if (inst == null) Debug.Log("NULL INST");
         
	// 		base.SetAccessors(
    //             () => ((AttrDef<StateT, ValT>)attrDef).getter.Invoke(this.instance),
	// 			(val) => ((AttrDef<StateT, ValT>)attrDef).setter.Invoke(this.instance, val));
    //     }
    // }
}
