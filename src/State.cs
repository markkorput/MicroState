using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MicroState
{   
    public class State
	{
        public UnityEvent ChangeEvent = new UnityEvent();

		private int postPonersCount = 0;
		private bool bHasPostponedNotifications = false;
		private List<ICopyableAttribute> copyableAttributes = new List<ICopyableAttribute>();

		public void BatchUpdate(System.Action func) {
			postPonersCount += 1;
			func.Invoke();         
			postPonersCount -= 1;
			if (bHasPostponedNotifications) this.NotifyChange();
		}

		protected void NotifyChange() {
			if (postPonersCount > 0) {
				bHasPostponedNotifications = true;
				return;
			}
         
			ChangeEvent.Invoke();
		}
      
		protected  Attribute<T> CreateAttribute<T>() {
			var attr = new Attribute<T>(this.NotifyChange);
			this.copyableAttributes.Add(attr);
			return attr;
		}
      
		protected Attribute<T> CreateAttribute<T>(T val)
        {
            var attr = new Attribute<T>(val, this.NotifyChange);
            this.copyableAttributes.Add(attr);
            return attr;
        }

		protected ListAttribute<T> CreateListAttribute<T>(T[] content = null)
        {
			var attr = content != null ? new ListAttribute<T>(content, this.NotifyChange) : new ListAttribute<T>(this.NotifyChange);
            this.copyableAttributes.Add(attr);
            return attr;
        }

        /// <summary>
		/// Default copy behaviour; have each instance in our copyableAttributes
		/// list copy from each instance in the other state's copyableAttributes
        /// </summary>
        /// <param name="state">State.</param>
		public virtual void TakeContentFrom(State state){
			if (state.copyableAttributes.Count != this.copyableAttributes.Count) {
				Debug.Log("[MicroState.State.TakeContentFrom] source state has different number o copyableAttributes");
				return;
			}
         
			this.BatchUpdate(() =>
			{
				for (int i = 0; i < copyableAttributes.Count; i += 1)
				{
					var source = state.copyableAttributes[i];
					var dest = this.copyableAttributes[i];
					dest.CopyFrom(source);
				}
			});
		}
    }
}
