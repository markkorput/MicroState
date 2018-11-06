using System;
using System.Collections.Generic;

namespace MicroState.Id
{
	public class IdStateChangeChecker<DataT> : IDisposable
    {
        private IdState<DataT> state;
		private BaseAttrValue[] lastSnapshot;

        private class BaseAttrValue
        {
			public virtual bool IsEqual(BaseAttrValue other) {
				return false;
			}
        }
      
		private class AttrValue<T> : BaseAttrValue
        {
            public T val;
            public AttrValue(T v) { this.val = v; }

			public override bool IsEqual(BaseAttrValue Other)
			{
				var other = (AttrValue<T>)Other;
				if (this.val == null || other.val == null) return this.val == null && other.val == null;
				return this.val.Equals(other.val);            
			}
        }
      
        public IdStateChangeChecker(IdState<DataT> state)
        {
            this.state = state;

			this.state.OnChange += this.OnChange;

			this.lastSnapshot = CreateSnapShot(state);
        }
      
		public void Dispose() {
			this.state.OnChange -= this.OnChange;
		}

		public void OnChange(IdState<DataT> state) {
            // to avoid double-notiying about the same changes;
            // when an (also when triggered externally), we update
            // our lastSnapshot
            lastSnapshot = CreateSnapShot(state);
		}

        public void Update()
        {
			var snapshot = CreateSnapShot(state);

			if (!AreEqual(snapshot, lastSnapshot)) {
				this.state.NotifyChange();
			}         

			lastSnapshot = snapshot; // this happens in the OnChange callback, see constructor
        }

        private static BaseAttrValue[] CreateSnapShot(IdState<DataT> state)
        {
			List<BaseAttrValue> attrvalues = new List<BaseAttrValue>();

			BaseAttr[] attrs = state.GetAttributes();

			for (int i = 0; i < attrs.Length; i++)
            {
				var attr = attrs[i];
				if (attr.ValueType.Equals(((int)0).GetType()))
				{               
					attrvalues.Add(new AttrValue<int>(((Attr<DataT, int>)attr).Value));
				}     

                 // TODO support other types
            }

			return attrvalues.ToArray();
        }

		private static bool AreEqual(BaseAttrValue[] snapshotA, BaseAttrValue[] snapshotB)
		{
			if (snapshotA.Length != snapshotB.Length) return true;
         
			for (int i = 0; i < snapshotA.Length; i++)
			{
				if (!snapshotA[i].IsEqual(snapshotB[i])) return false;
			}

			return true;
		}
    }
}

