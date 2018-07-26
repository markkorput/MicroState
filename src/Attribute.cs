using System;
using UnityEngine;
namespace MicroState
{
	public interface ICopyableAttribute
    {
        void CopyFrom(ICopyableAttribute other);
    }

    public interface IValueTypeProvider
	{
		Type GetValueType();
	}
   
    /// <summary>
    /// The Attribute class wraps around a single value of any given type and
	/// invokes the changeFunc provided at instantiation whenever its value changes.
	/// 
	/// Mostly used inthe MicroState.State class.
    /// </summary>
	public class Attribute<T> : MutationNotifier, ICopyableAttribute, IValueTypeProvider
    {
        private T value_;
      
        public T Value
        {
            get { return value_; }
            set
            {
				bool change = !this.AreEqual(value_, value);
                value_ = value;
				if (change) this.NotifyChange();
            }
        }

		public Attribute(UnityEngine.Events.UnityAction changeFunc)
        {
			this.ChangeEvent.AddListener(changeFunc);
        }

		public Attribute(T val, UnityEngine.Events.UnityAction changeFunc) : this(changeFunc)
        {
            this.value_ = val;
        }

        public void CopyFrom(ICopyableAttribute other)
        {
            this.Value = (other as Attribute<T>).Value;
        }
      
		public Type GetValueType() {
			return typeof(T);
		}

		private bool AreEqual(T a, T b) {
			if (Nullable.GetUnderlyingType(typeof(T)) != null && (a == null || b == null)) {
				// if a == null we cannot a a.Equals
				return a == null && b == null;
			}

			if (typeof(T) == typeof(string) && (a == null || b == null)) {
				return a == null && b == null;
			}

			return a.Equals(b);
		}
    }
}
