using System;
using System.Collections.Generic;

namespace MicroState
{
	public class ListAttribute<T> : Attribute<T[]> where T : State, new()
    {
		public ListAttribute(UnityEngine.Events.UnityAction changeFunc) : base(new T[0], changeFunc)
		{
        }

		public ListAttribute(T[] content, UnityEngine.Events.UnityAction changeFunc) : base(content, changeFunc)      
        {
        }
      
		override public T[] Value {
			get { return base.Value;  }
			set {
				this.BatchUpdate(() =>
				{
					foreach (var it in base.Value) if (it != null) it.ChangeEvent.RemoveListener(this.LocalNotify);               
					base.Value = value;
					foreach (var it in base.Value) if (it != null) it.ChangeEvent.AddListener(this.LocalNotify);
				});
			}
		}

		private void LocalNotify() {
			this.NotifyChange();         
		}

		public int Length { get { return this.Value == null ? 0 : this.Value.Length; }}
      
		public void Add(T item) {
			T[] newlist = new T[Value.Length + 1];
			for (int i = 0; i < Value.Length; i++) newlist[i] = Value[i];
			newlist[Value.Length] = item;
			// whenever the new item invokes its change event, we'll notify our owner
			if (item != null) item.ChangeEvent.AddListener(this.LocalNotify);
			this.Value = newlist;
		}

		public void Assign(int idx, T item) {
			if (this.Value[idx] != item) {
				// remove existing
				if (this.Value[idx] != null) this.Value[idx].ChangeEvent.RemoveListener(this.LocalNotify);
                // insert new
				this.Value[idx] = item;
				// register to its change event
				if (item != null) item.ChangeEvent.AddListener(this.LocalNotify);
				this.LocalNotify();
			}
		}
      
		public void Remove(T item)
		{
			for (int i = this.Value.Length - 1; i >= 0; i--) {
				if (this.Value[i] == item)
				{
					Remove(i);
				}
			}
		}

		public void Remove(int idx) {
			T item = this.Value[idx];
			if (item != null) item.ChangeEvent.RemoveListener(this.LocalNotify);

			// create copy of array without the specified item
			T[] newlist = new T[this.Value.Length - 1];
			for (int i = 0; i < idx; i++) newlist[i] = this.Value[i];
			for (int i = idx + 1; i < this.Value.Length; i++) newlist[i-1] = this.Value[i];
            // make new copy our value
			this.Value = newlist;
		}

		public void Clear() {
			this.BatchUpdate(() =>
			{
				while (this.Value.Length > 0) this.Remove(0);
			});
		}
      
		override public void CopyFrom(ICopyableAttribute other)
		{
			this.BatchUpdate(() =>
			{            
			    List<T> list = new List<T>();
				foreach (var it in (other as ListAttribute<T>).Value) {
					list.Add(it == null ? null : (T)it.Clone());
				}
            
				this.Value = list.ToArray();
			});
		}         
    }
}
