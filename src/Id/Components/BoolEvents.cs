using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MicroState.Id.Components
{
	public class BoolEvents : MonoBehaviour
	{
		[Header("Attribute ID")]
		public string StateId;
		public string AttrId;

		[Header("Behaviour")]
		public bool InvokeWhenInactive = false;

		[System.Serializable]
		public class ValueTypeEvent : UnityEvent<bool> { }
		[Header("Bool Value")]
		public ValueTypeEvent ValueEvent;
		public UnityEvent TrueEvent;
		public UnityEvent FalseEvent;

		private AttrListener<bool> attrListener = null;
		private AttrListener<bool> AttrListener
		{
			get
			{
				if (this.attrListener == null) this.attrListener = new AttrListener<bool>(StateId, AttrId, this.gameObject);
				return this.attrListener;
			}
		}

		private void Start()
		{
			this.AttrListener.ValueEvent.AddListener(this.OnValue);
		}

		private void OnDestroy()
		{
			if (this.attrListener != null)
			{
				this.attrListener.Dispose();
				this.attrListener = null;
			}
		}

		private void OnValue(bool v)
		{
			if (!(this.InvokeWhenInactive || this.isActiveAndEnabled)) return;

			this.ValueEvent.Invoke(v);
			(v ? this.TrueEvent : this.FalseEvent).Invoke();
		}
	}
}
