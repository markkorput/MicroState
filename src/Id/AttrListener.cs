﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MicroState.Id
{
	public class AttrListener<ValueT> : System.IDisposable
	{
		private AttrRef<ValueT> attrRef = null;

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

		public AttrListener(IdStateBase state, string attrid)
		{
			this.attrRef = new AttrRef<ValueT>(state, attrid);
		}

		public AttrListener(string stateid, string attrid, GameObject gameObject = null)
		{
			this.attrRef = new AttrRef<ValueT>(stateid, attrid, gameObject);
		}

		public AttrListener(AttrRef<ValueT> attrRef)
		{
			this.attrRef = attrRef;
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
				// state.GetResuableAttrListener<ValueT>()

				state.ChangeEvent += this.OnStateChange;

				// register cleanup func
				this.disposeFuncs.Add(() =>
					{
						this.StateBase.ChangeEvent -= this.OnStateChange;
					});
			}
		}

		#region AttrRef Proxies
		public ValueAttr<ValueT> ValueAttr { get { 
			return this.attrRef.ValueAttr;
		}}

		public IdStateBase StateBase { get { return this.attrRef.StateBase; }}

		public ValueT Get() { return this.attrRef.Get(); }
		public void Set(ValueT v) { this.attrRef.Set(v); }


		public ValueT Value {
				get { return this.Get(); }
				set { this.Set(value); }
		}
		#endregion
	}
}
