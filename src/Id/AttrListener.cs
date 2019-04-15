using System.Collections;
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
		private ValueChangeChecker<ValueT> changeChecker = new ValueChangeChecker<ValueT>();

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

		public AttrListener(IdStateBase state, string attrid) : this(new AttrRef<ValueT>(state, attrid)) {
		}

		public AttrListener(string stateid, string attrid, GameObject gameObject) : this (new AttrRef<ValueT>(stateid, attrid, gameObject)) {
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

		// public void InvokeValue()
		// {
		// 	if (this.valueEvent_ == null) this.StartListening();
		// 	this.OnStateChange();
		// }

		private void OnStateChange()
		{
			var attr = this.attrRef.ValueAttr;
			if (attr == null) return;

			var val = attr.Value;
			if (changeChecker.Check(val))
				this.changeEvent_.Invoke(val);

			this.valueEvent_.Invoke(val);
		}

		private void StartListening()
		{
			this.valueEvent_ = new ValueEventType();
			this.changeEvent_ = new ValueEventType();
			// register attribute listener that trigger our value event
			var state = this.attrRef.StateBase;
			if (state != null)
			{
				// state.GetResuableAttrListener<ValueT>()

				state.ChangeEvent += this.OnStateChange;

				// register cleanup func
				this.disposeFuncs.Add(() =>
					{
						this.attrRef.StateBase.ChangeEvent -= this.OnStateChange;
					});
			}
		}

		#region AttrRef Proxies
		public ValueT Value {
				get { return this.attrRef.ValueAttr.Value; }
				set { this.attrRef.ValueAttr.Value = value; }
		}
		#endregion
	}
}
