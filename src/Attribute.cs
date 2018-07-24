using System;

namespace MicroState
{
	public interface ICopyableAttribute
    {
        void CopyFrom(ICopyableAttribute other);
    }
   
    /// <summary>
    /// The Attribute class wraps around a single value of any given type and
	/// invokes the changeFunc provided at instantiation whenever its value changes.
	/// 
	/// Mostly used inthe MicroState.State class.
    /// </summary>
    public class Attribute<T> : MutationNotifier, ICopyableAttribute
    {
        private T value_;

        public T Value
        {
            get { return value_; }
            set
            {
				bool change = ((value_ == null && value != null) || !value_.Equals(value));
                value_ = value;
				if (change) this.NotifyChange();
            }
        }

		public Attribute(UnityEngine.Events.UnityAction changeFunc)
        {
			this.ChangeEvent.AddListener(changeFunc);
        }

		public Attribute(T val, UnityEngine.Events.UnityAction changeFunc)
        {
            this.value_ = val;
			this.ChangeEvent.AddListener(changeFunc);
        }

        public void CopyFrom(ICopyableAttribute other)
        {
            this.Value = (other as Attribute<T>).Value;
        }
    }
}
