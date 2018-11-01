using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace MicroState.Id
{
	public class IdState<StateT>
	{
		private StateT Instance;
		private List<BaseAttrDef> AttrDefs = new List<BaseAttrDef>();
      
		public delegate void OnChangeDel(IdState<StateT> state);
		public event OnChangeDel OnChange;

		public IdState(StateT instance)
		{
			this.Instance = instance;
		}
      
		public void setDataInstance(StateT inst) { this.Instance = inst; }
        
		public StateT DataInstance { get { return Instance; }}
      
		protected void CreateAttr<ValT>(string id,
								 System.Func<StateT, ValT> getter,
								 System.Action<StateT, ValT> setter)
		{
			// wrap setter in a change notifier
			var notifySetter = (System.Action<StateT, ValT>)((state, val) => {
				// get current value
				var curval = getter.Invoke(state);
                // check for changes
				bool change = !AreEqual(curval, val);
                // apply original setter
				setter.Invoke(state, val);
                // notify if there were changes
				if (change && this.OnChange != null) this.OnChange(this);
		    });
         
			AttrDefs.Add(
				new AttrDef<StateT, ValT>(
					id,
					getter,
					notifySetter, // setter,
					() => this.GetAttr<ValT>(id)
				));
		}
      
		public Attr<StateT, ValT> GetAttr<ValT>(string id)
		{
			var def = AttrDefs.Find((attrdef) => attrdef.id.Equals(id));
			return def == null ? null : new Attr<StateT, ValT>(this.Instance, (AttrDef<StateT, ValT>)def);
		}

		public BaseAttr[] GetAttributes() {
			return (from it in this.AttrDefs select it.CreateAttr()).ToArray();
		}
      
		public static bool AreEqual<T>(T a, T b)
        {
            //if (Nullable.GetUnderlyingType(typeof(T)) != null && (a == null || b == null)) {
            //  // if a == null we cannot a a.Equals
            //  return a == null && b == null;
            //}

            //if (typeof(T) == typeof(string) && (a == null || b == null)) {
            //  return a == null && b == null;
            //}
         
            if (a == null || b == null)
            {
                return a == null && b == null;
            }
            return a.Equals(b);
        }
	}
}
