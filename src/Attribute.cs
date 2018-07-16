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
    public class Attribute<T> : ICopyableAttribute
    {
        private T value_;
        private System.Action changeFunc;

        public T Value
        {
            get { return value_; }
            set
            {
                bool change = (!value_.Equals(value));
                value_ = value;
                if (changeFunc != null) changeFunc.Invoke();
            }
        }

        public Attribute(System.Action changeFunc)
        {
            this.changeFunc = changeFunc;
        }

        public Attribute(T val, System.Action changeFunc)
        {
            this.value_ = val;
            this.changeFunc = changeFunc;
        }

        public void CopyFrom(ICopyableAttribute other)
        {
            this.Value = (other as Attribute<T>).Value;
        }
    }
   
}
