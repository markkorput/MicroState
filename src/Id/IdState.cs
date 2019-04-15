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

        protected bool SupportsReusableAttrListeners = true;

        // public virtual AttrListener<ValueT> GetResuableAttrListener<ValueT>(string id) {
        //     return null;
        // }
    }

    public class IdState<StateT> : IdStateBase
    {
        private StateT Instance;
        private List<AttrDef> AttrDefs = new List<AttrDef>();

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
                bool change = !BaseAttr.AreEqual(curval, val);
                // apply original setter
                if (setter != null) setter.Invoke(state, val);
                // notify if there were changes
                if (change) this.NotifyChange();
            });

            AttrDefs.Add(
                new AttrDef(
                    id,
                    () => {
                        return new ValueAttr<ValT>(
                            () => getter.Invoke(this.Instance),
                            (val) => notifySetter.Invoke(this.Instance, val),
                            id);
                    }));
        }

        protected void CreateStateAttr<ValT>(string id,
            System.Func<IdState<StateT>, ValT> getter,
            System.Action<IdState<StateT>, ValT> setter = null) {

            this.CreateAttr<ValT>(id,
                (data) => getter.Invoke(this),
                (data,val) => {
                    if (setter != null) setter.Invoke(this, val);
                });
        }

        // Creates Read-Only Virtual Attribute
        protected void CreateAttr<ValT>(string id, System.Func<StateT, ValT> getter) {
            this.CreateAttr(id, getter, null);
        }

        protected Dictionary<string, System.Object> ResuableAttrListeners = null;

        // public override AttrListener<ValueT> GetResuableAttrListener<ValueT>(string id) {
        //     if (!this.SupportsReusableAttrListeners) return null;
            
        //     // lazy-init
        //     if (this.ResuableAttrListeners == null) {
        //         this.ResuableAttrListeners = new Dictionary<string, System.Object>();
        //     }

        //     // find existing
        //     System.Object listener = null;
        //     if (this.ResuableAttrListeners.TryGetValue(id, out listener)) {
        //         return (AttrListener<ValueT>)listener;
        //     }

        //     // create reusable AttrListener
        //     var valAttrRef = GetAttr<ValueT>(id);
        //     var attrRef = new AttrRef<ValueT>(valAttrRef);
        //     var attrListener = new AttrListener<ValueT>(attrRef);
        //     // save for later reference
        //     this.ResuableAttrListeners.Add(id, (System.Object)listener);

        //     return attrListener;
        // }

        public override ValueAttr<ValT> GetAttr<ValT>(string id)
        {
            var def = AttrDefs.Find((attrdef) => attrdef.id.Equals(id));
            return def == null ? null : (ValueAttr<ValT>)def.CreateAttr();
        }

        /// Used by editor class to fetch all available attribute IDs
        public override string[] GetAttributeIds()
        {
            return (from def in this.AttrDefs select def.id).ToArray();
        }

        public BaseAttr[] GetAttributes() {
            return (from it in this.AttrDefs select it.CreateAttr()).ToArray();
        }

        public new void NotifyChange() {
            if (this.OnChange != null) this.OnChange(this);
            base.NotifyChange();
        }

        // /// HACKY :/
        // public BaseAttr[] GetAttributes(StateT data)
        // {
        //     var original = this.Instance;
        //     this.Instance = data;
        //     var attrs = this.GetAttributes();
        //     this.Instance = original;
        //     return attrs;
        // }


        // public bool AreEqual(StateT a, StateT b) {
        //     var attrsA = this.GetAttributes(a);
        //     var attrsB = this.GetAttributes(b);

        //     for (int i = 0; i < attrsA.Length; i++) {
        //         if (!attrsA[i].IsEqual(attrsB[i])) return false;
        //     }

        //     return true;
        // }
    }
}
