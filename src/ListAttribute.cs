using System;

namespace MicroState
{
	public class ListAttribute<T> : Attribute<T[]> where T : State
    {
		public ListAttribute(UnityEngine.Events.UnityAction changeFunc) : base(new T[0], changeFunc)
		{
        }

		public ListAttribute(T[] content, UnityEngine.Events.UnityAction changeFunc) : base(content, changeFunc)      
        {
        }

		public void Add(T item) {
			T[] newlist = new T[Value.Length + 1];
			for (int i = 0; i < Value.Length; i++) newlist[i] = Value[i];
			newlist[Value.Length] = item;
			this.Value = newlist;
         
            // whenever the new item invokes its change event, we'll notify our owner
			item.ChangeEvent.AddListener(this.NotifyChange);
		}

		public void Assign(int idx, T item) {
			if (this.Value[idx] != item) {
				// remove existing
				if (this.Value[idx] != null) this.Value[idx].ChangeEvent.RemoveListener(this.NotifyChange);
                // insert new
				this.Value[idx] = item;
                // register to its change event
				this.ChangeEvent.Invoke();
			}
		}
      
		public void Remove(T item)
		{
			for (int i = this.Value.Length - 1; i >= 0; i--) {
				if (this.Value[i] == item)
				{
					Remove(item);
				}
			}
		}

		public void Remove(int idx) {
			// create copy of array without the specified item
			T[] newlist = new T[this.Value.Length - 1];
			for (int i = 0; i < idx; i++) newlist[i] = this.Value[i];
			for (int i = idx + 1; i < this.Value.Length; i++) newlist[i-1] = this.Value[i];
            // make new copy our value
			this.Value = newlist;
		}
    }
}
