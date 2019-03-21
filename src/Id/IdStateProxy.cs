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
    }
}
