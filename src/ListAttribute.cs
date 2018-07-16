using System;

namespace MicroState
{
	public class ListAttribute<T> : Attribute<T[]>
    {
		public ListAttribute(System.Action changeFunc) : base(new T[0], changeFunc)
		{
        }

		public ListAttribute(T[] content, System.Action changeFunc) : base(content, changeFunc)      
        {
        }
            
		public void Add(T item) {
			T[] newlist = new T[Value.Length + 1];
			for (int i = 0; i < Value.Length; i++) newlist[i] = Value[i];
			newlist[Value.Length] = item;
			this.Value = newlist;
		}
    }
}
