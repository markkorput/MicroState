using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MicroState.Id {
    public class ValueChangeChecker<ValueT>
    {
        private ValueT lastValue;
		private bool isFirstValue = true;

        public bool Check(ValueT val) {
            bool change = false;

            if (this.isFirstValue 
                || (this.lastValue == null && val != null)
                || (this.lastValue != null && !this.lastValue.Equals(val)))
            {
                change = true;
            }

            this.isFirstValue = false;
            this.lastValue = val;
            return change;
        }
    }
}