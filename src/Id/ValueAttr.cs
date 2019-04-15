using System;
using UnityEngine;

namespace MicroState.Id
{
	/// Typeless base for ValueAttr with method/attributes
	// common to all ValueAttrs, regardless of value-type
	public class BaseAttr
	{
		protected string id;
		protected System.Type valueType;

		public string Id { get { return id; } }
		public virtual System.Type ValueType { get { return valueType; } }

		public virtual bool IsEqual(BaseAttr other)
		{
			return false;
		}

		// Compair two values of any type
		public static bool AreEqual<ValT>(ValT a, ValT b)
        {
            if (a == null || b == null) return a == null && b == null;
            return a.Equals(b);
        }
	}

	/// A ValueAttrs is bundles logic to fetch (getter) and write (setter)
	/// the value of an attribute of any type, and compair its value with another attr
	public class ValueAttr<ValT> : BaseAttr {

		private System.Func<ValT> getter;
		private System.Action<ValT> setter;

		public ValT Value
        {
			get { return getter.Invoke(); }
			set { setter.Invoke(value);  }
        }

		protected ValueAttr() {}

		public ValueAttr(System.Func<ValT> g, System.Action<ValT> s, string id) {
			base.id = id;
			base.valueType = g.Method.ReturnType;

			this.getter = g;
			this.setter = s;
		}

		public override bool IsEqual(BaseAttr other)
        {
            return BaseAttr.AreEqual(this.Value, ((ValueAttr<ValT>)other).Value);
        }
	}
}
