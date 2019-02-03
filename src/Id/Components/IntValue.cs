using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MicroState.Id.Components
{
    public class IntValue : MonoBehaviour
    {
        [Header("Attribute ID")]
        public string StateId;
        public string AttrId;

        [Header("Behaviour")]
        public bool InvokeWhenInactive = false;

        [System.Serializable]
        public class ValueTypeEvent : UnityEvent<int> { }
        public ValueTypeEvent ChangeEvent = new ValueTypeEvent();
        
        private MicroState.Id.AttrListener<int> attrListener = null;
        private MicroState.Id.AttrListener<int> AttrListener
        {
            get
            {
                if (this.attrListener == null) this.attrListener = new MicroState.Id.AttrListener<int>(StateId, AttrId, this.gameObject);
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

        private void OnValue(int v)
        {
            if (!(this.InvokeWhenInactive || this.isActiveAndEnabled)) return;
            this.ChangeEvent.Invoke(v);
        }

		#region Public Action Methods
        public void Set(int v) { this.AttrListener.Set(v); }

		public void InvokeValue(bool alsoWhenInactive) {
			if (this.isActiveAndEnabled || alsoWhenInactive)
				this.ChangeEvent.Invoke(this.AttrListener.Value);
		}
        #endregion
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(IntValue))]
    [CanEditMultipleObjects]
    public class IntValueEditor : AttrRefEditor<int, IntValue>
    {

    }
#endif
}