using System;
using System.Collections.Generic;

namespace MicroState.Id
{
	public class IdState<StateT>
	{
		private StateT Instance;
		private List<BaseAttrDef> AttrDefs = new List<BaseAttrDef>();
      
		public IdState(StateT instance)
		{
			this.Instance = instance;
		}

		protected void CreateAttr<ValT>(string id,
								 System.Func<StateT, ValT> getter,
								 System.Action<StateT, ValT> setter)
		{
			AttrDefs.Add(new AttrDef<StateT, ValT>(id, getter, setter));
		}

		public Attr<StateT, ValT> GetAttr<ValT>(string id)
		{
			var def = AttrDefs.Find((attrdef) => attrdef.id.Equals(id));
			return def == null ? null : new Attr<StateT, ValT>(this.Instance, (AttrDef<StateT, ValT>)def);
		}
	}
}
