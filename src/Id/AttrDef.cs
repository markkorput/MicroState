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

    public class AttrDef<DataT, ValT> : BaseAttrDef
    {
        public System.Func<DataT, ValT> getter;
        public System.Action<DataT, ValT> setter;
      
        public AttrDef(string i, System.Func<DataT, ValT> g, System.Action<DataT, ValT> s)
        {
            id = i; getter = g; setter = s;
            this.ValueType = getter.Method.ReturnType;
        }
      
		public AttrDef(string i, System.Func<DataT, ValT> g, System.Action<DataT, ValT> s, System.Func<BaseAttr> attrCreator)
			: this(i, g, s) {
			this.attrCreator = attrCreator;         
		}
    }
}
