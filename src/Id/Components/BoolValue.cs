using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MicroState.Id.Components
{
	public class BoolValue : MonoBehaviour
	{
		[Header("Attribute ID")]
		public string StateId;
		public string AttrId;

		[Header("Behaviour")]
		public bool InvokeWhenInactive = false;

		[System.Serializable]
		public class ValueTypeEvent : UnityEvent<bool> { }
		public ValueTypeEvent ChangeEvent = new ValueTypeEvent();
		public UnityEvent TrueEvent;
		public UnityEvent FalseEvent;

		private MicroState.Id.AttrListener<bool> attrListener = null;
		private MicroState.Id.AttrListener<bool> AttrListener
		{
			get
			{
				if (this.attrListener == null) this.attrListener = new MicroState.Id.AttrListener<bool>(StateId, AttrId, this.gameObject);
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
      
		private void OnValue(bool v)
		{
			if (!(this.InvokeWhenInactive || this.isActiveAndEnabled)) return;
			this.ChangeEvent.Invoke(v);

			(v ? this.TrueEvent : this.FalseEvent).Invoke();
		}

		#region Public Action Methods
			public void Set(bool v) { this.AttrListener.Set(v); }
			public void Toggle() { this.AttrListener.Value = !this.AttrListener.Value; }
		#endregion
	}

#if UNITY_EDITOR
    [CustomEditor(typeof(BoolValue))]
    [CanEditMultipleObjects]
    public class BoolValueEditor : AttrRefEditor<bool, BoolValue>
    {

    }
#endif
}