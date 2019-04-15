using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MicroState.Id.Components
{
	public class BaseAttrBase : MonoBehaviour {
		[Header("State Attribute")]
		[Tooltip("Optional; when specified, will use this state instacne's State")]
		public IdStateInstanceBase StateInstance;
		[Tooltip("Optional; when specified, will try to find the IdStateInstance with this ID and use that state instance's State")]
		public string StateId;
		[Tooltip("Required; the name of the virtual attribute in our State")]
		public string AttrId;

		[Header("Behaviour")]
		public bool InvokeWhenInactive = false;
		public bool InvokeStartValue = false;
		public bool InvokeOnEnable = true;
	}

	public class BaseAttr<T> : BaseAttrBase {
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
				if (this.attrListener == null) {
					this.attrListener =
						this.StateInstance != null
							? new MicroState.Id.AttrListener<T>(this.StateInstance.GetState(), this.AttrId)
							: new MicroState.Id.AttrListener<T>(this.StateId, this.AttrId, this.gameObject);
				}
				return this.attrListener;
			}
		}

		public T Value { get { return this.Get(); } set { this.Set(value); }}
		private bool bRegistered = false;

		private void Start()
		{
			if (!bRegistered) {
				this.AttrListener.ChangeEvent.AddListener(this.OnValueIf);
				this.bRegistered = true;
			}
			if (this.InvokeStartValue && !this.InvokeOnEnable) this.OnValue(this.AttrListener.Value);
		}

		private void OnEnable() {
			if (!bRegistered) {
				this.AttrListener.ChangeEvent.AddListener(this.OnValueIf);
				this.bRegistered = true;
			}
			if (this.InvokeOnEnable) this.OnValue(this.AttrListener.Value);
		}

		private void OnDestroy()
		{
			if (this.attrListener != null)
			{
				this.attrListener.Dispose();
				this.attrListener = null;
			}
		}

		/// only invoked when active of inactive invoked are allowed
		protected virtual void OnValue(T v)
		{
			// if (!(this.InvokeWhenInactive || this.isActiveAndEnabled)) return;
			// this.ChangeEvent.Invoke(v);
			// var evt = (v ? this.TrueEvent : this.FalseEvent);
			// if (evt != null) evt.Invoke(); // In unity 2018.3 this started giving NullReferenceExceptions in PlayMode tests...
		}

		protected void OnValueIf(T v) {
			if (this.InvokeWhenInactive || this.isActiveAndEnabled) {
				this.OnValue(v);
			}
		}

		#region Public Action Methods
			public void InvokeValue() { this.OnValueIf(this.AttrListener.Value); }
			public void Set(T v) { this.AttrListener.Value = v; }
			public T Get() { return this.AttrListener.Value; }
		#endregion
	}
}