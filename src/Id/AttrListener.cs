using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MicroState.Id
{
	public class AttrListener<ValueT> : AttrRef<ValueT>, System.IDisposable
	{
		public class ValueEventType : UnityEvent<ValueT> { }
		private ValueEventType valueEvent_ = null;
		private ValueEventType changeEvent_ = null;

		private List<System.Action> disposeFuncs = new List<System.Action>();
		private ValueT lastValue;
		private bool isFirstValue = true;

		public ValueEventType ValueEvent
		{
			get
			{
				if (this.valueEvent_ == null) this.StartListening();
				return this.valueEvent_;
			}
		}
		public ValueEventType ChangeEvent
		{
			get
			{
				if (this.changeEvent_ == null) this.StartListening();
				return this.changeEvent_;
			}
		}

		public AttrListener(string stateid, string attrid, GameObject gameObject = null) : base(stateid, attrid, gameObject)
		{
		}

		public void Dispose()
		{
			foreach (var func in disposeFuncs) func.Invoke();
			this.disposeFuncs.Clear();
		}

		public void InvokeValue()
		{
			if (this.valueEvent_ == null) this.StartListening();
			this.OnStateChange();
		}

		private void OnStateChange()
		{
			var attr = this.ValueAttr;
			if (attr != null)
			{
				var val = attr.Value;
				if (this.isFirstValue 
					|| (this.lastValue == null && val != null)
					|| (this.lastValue != null && !this.lastValue.Equals(val)))
				{
					this.changeEvent_.Invoke(val);
				}

				this.valueEvent_.Invoke(val);
				this.isFirstValue = false;
				this.lastValue = val;
			}
		}

		private void StartListening()
		{
			this.valueEvent_ = new ValueEventType();
			this.changeEvent_ = new ValueEventType();
			// register attribute listener that trigger our value event
			var state = this.StateBase;
			if (state != null)
			{
				state.ChangeEvent += this.OnStateChange;

				// register cleanup func
				this.disposeFuncs.Add(() =>
					{
						this.StateBase.ChangeEvent -= this.OnStateChange;
					});
			}
		}
	}
}
