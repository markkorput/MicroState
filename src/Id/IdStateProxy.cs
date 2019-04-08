using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace MicroState.Id
{
    public class IdStateProxy<DataType, OriginType> : IdState<DataType>, System.IDisposable {

        protected IdState<OriginType> origin;
        private bool isOriginProxyRegistered = false;

        public IdStateProxy(DataType inst) : base(inst) { }
        public IdStateProxy(DataType inst, IdState<OriginType> origin) : this(inst) {
            this.SetOrigin(origin);
        }

        public void Dispose() {
            this.SetOrigin(null);
        }

        public IdState<OriginType> Origin { get {
            return this.origin;
        } }

        public void SetOrigin(IdState<OriginType> origin) {
            if (this.isOriginProxyRegistered) {
                this.origin.OnChange -= this.OnOriginChange;
                this.isOriginProxyRegistered = false;
            }

            this.origin = origin;

            if (this.origin != null) {
                this.origin.OnChange += this.OnOriginChange;
                this.isOriginProxyRegistered = true;
            }
        }

        private void OnOriginChange(IdState<OriginType> origin) {
            this.NotifyChange();
        }

        protected void CreateAttr<ValT>(string id,
            System.Func<DataType, OriginType, ValT> getter,
            System.Action<DataType, OriginType, ValT> setter = null) {

            base.CreateAttr(id,
                (data) => {
                    if (this.origin == null) return default(ValT);
                    return getter.Invoke(data, this.origin.DataInstance);
                },
                (data,val) => {
                    if (this.origin != null && setter != null) setter.Invoke(data, this.origin.DataInstance, val);
                });
        }

        protected void CreateStateAttr<ValT>(string id,
            System.Func<IdState<DataType>, IdState<OriginType>, ValT> getter,
            System.Action<IdState<DataType>, IdState<OriginType>, ValT> setter = null) {

            base.CreateAttr(id,
                (data) => {
                    if (this.origin == null) return default(ValT);
                    return getter.Invoke(this, this.origin);
                },
                (data,val) => {
                    if (this.origin != null && setter != null) setter.Invoke(this, this.origin, val);
                });
        }
    }
}
