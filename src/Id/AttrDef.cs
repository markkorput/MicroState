using System;
using UnityEngine;

namespace MicroState.Id
{
    // AttrDef is a ValueAttr producing 
	public class AttrDef
    {
        public string id;
      
		public System.Func<BaseAttr> attrCreator;
      
		public BaseAttr CreateAttr()
        {
			return this.attrCreator == null ? null : this.attrCreator.Invoke();
        }

        public AttrDef (string id, System.Func<BaseAttr> attrCreator) {
            this.id = id; 
            this.attrCreator = attrCreator;
        }
    }

    // public class AttrDef<DataT, ValT> : BaseAttrDef
    // {
    //     private System.Func<DataT, ValT> getter;
    //     private System.Action<DataT, ValT> setter;

    //     // public AttrDef(string i, System.Func<DataT, ValT> g, System.Action<DataT, ValT> s)
    //     // {
    //     //     id = i; 
    //     //     getter = g;
    //     //     setter = s;
    //     // }
      
	// 	// public AttrDef(string i, System.Func<DataT, ValT> g, System.Action<DataT, ValT> s, System.Func<BaseAttr> attrCreator) {
    //     //     base.id = i; 
    //     //     base.attrCreator = attrCreator;
    //     //     getter = g;
    //     //     setter = s;
	// 	// }
    //     public AttrDef (string id, System.Func<BaseAttr> attrCreator) {
    //         base.id = id; 
    //         base.attrCreator = attrCreator;
    //     }
    // }
}
