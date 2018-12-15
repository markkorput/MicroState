using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace MicroState.Id
{
	public class IdStateBase {
		public delegate void ChangeEventDel();
		public event ChangeEventDel ChangeEvent;
      
		public void NotifyChange() {
			if (this.IsBatching) {
				this.batchChanges += 1;
				return;
			}

			if (this.ChangeEvent != null) this.ChangeEvent();
		}     

		virtual public ValueAttr<ValT> GetAttr<ValT>(string id) {
			return null;
		}

		virtual public string[] GetAttributeIds() {
			return new string[0];
		}

        // Batching Stuff
		private int batchCount = 0;
        private int batchChanges = 0;
      
        public bool IsBatching { get { return this.batchCount > 0; } }
      
        public void BatchUpdate(System.Action func)
        {
            batchCount += 1;
            func.Invoke();
            batchCount -= 1;
         
            if (batchCount == 0 && batchChanges > 0)
            {
                this.NotifyChange();
                batchChanges = 0;
            }
        }      
	}

	public class IdState<StateT> : IdStateBase
	{
		private StateT Instance;
		private List<BaseAttrDef> AttrDefs = new List<BaseAttrDef>();

		public delegate void OnChangeDel(IdState<StateT> state);
        public event OnChangeDel OnChange;      

		public IdState(StateT instance)
		{
			this.Instance = instance;
		}

		public void setDataInstance(StateT inst)
		{
			bool change = (this.Instance == null && inst != null) || !this.Instance.Equals(inst);
			this.Instance = inst;
            this.NotifyChange();
		}
        
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
				if (change) this.NotifyChange();
		    });
         
			AttrDefs.Add(
				new AttrDef<StateT, ValT>(
					id,
					getter,
					notifySetter, // setter,
					() => this.GetAttr<ValT>(id)
				));
		}

		public override ValueAttr<ValT> GetAttr<ValT>(string id)
		{
			// if (this.Instance == null) return null;
			var def = AttrDefs.Find((attrdef) => attrdef.id.Equals(id));
			return def == null ? null : new Attr<StateT, ValT>(this.Instance, (AttrDef<StateT, ValT>)def);
		}
      
		public override string[] GetAttributeIds()
        {
			return (from def in this.AttrDefs select def.id).ToArray();
        }

		public BaseAttr[] GetAttributes() {
			return (from it in this.AttrDefs select it.CreateAttr()).ToArray();
		}

		public BaseAttr[] GetAttributes(StateT data)
		{
			var original = this.Instance;
			this.Instance = data;
			var attrs = this.GetAttributes();
			this.Instance = original;
			return attrs;
		}
      
		public static bool AreEqual<T>(T a, T b)
        {
			if (a == null || b == null) return a == null && b == null;
            return a.Equals(b);
        }
      
		public bool AreEqual(StateT a, StateT b) {
			var attrsA = this.GetAttributes(a);
			var attrsB = this.GetAttributes(b);
         
			for (int i = 0; i < attrsA.Length; i++) {
				if (!attrsA[i].IsEqual(attrsB[i])) return false;
			}
         
			return true;
		}
      
		public new void NotifyChange() {
			if (this.OnChange != null) this.OnChange(this);
			base.NotifyChange();
		}
	}
}
