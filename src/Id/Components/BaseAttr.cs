using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MicroState.Id.Components
{
	public class BaseAttr<T> : MonoBehaviour {
		[Header("Attribute ID")]
		public string StateId;
		public string AttrId;

		[Header("Behaviour")]
		public bool InvokeWhenInactive = false;

		// [System.Serializable]
		// public class ValueTypeEvent : UnityEvent<T> { }
		// public ValueTypeEvent ChangeEvent = new ValueTypeEvent();
		// public UnityEvent TrueEvent;
		// public UnityEvent FalseEvent;

		protected MicroState.Id.AttrListener<T> attrListener = null;
		protected MicroState.Id.AttrListener<T> AttrListener
		{
			get
			{
				if (this.attrListener == null) this.attrListener = new MicroState.Id.AttrListener<T>(StateId, AttrId, this.gameObject);
				return this.attrListener;
			}
		}

		private void Start()
		{
			this.AttrListener.ChangeEvent.AddListener(this.OnValue);
		}

		private void OnDestroy()
		{
			if (this.attrListener != null)
			{
				this.attrListener.Dispose();
				this.attrListener = null;
			}
		}

		protected virtual void OnValue(T v)
		{
			// if (!(this.InvokeWhenInactive || this.isActiveAndEnabled)) return;
			// this.ChangeEvent.Invoke(v);
			// var evt = (v ? this.TrueEvent : this.FalseEvent);
			// if (evt != null) evt.Invoke(); // In unity 2018.3 this started giving NullReferenceExceptions in PlayMode tests...
		}

		#region Public Action Methods
			public void InvokeValue() { this.OnValue(this.AttrListener.Value); }
			public void Set(T v) { this.AttrListener.Set(v); }
			public T Get() { return this.AttrListener.Value; }
		#endregion
	}
}