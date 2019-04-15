using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MicroState.Id.Components
{
    [System.Obsolete("Use FloatAttr")]
    public class FloatValue : MonoBehaviour
    {
        [Header("Attribute ID")]
        public string StateId;
        public string AttrId;

        [Header("Behaviour")]
        public bool InvokeWhenInactive = false;

        [System.Serializable]
        public class ValueTypeEvent : UnityEvent<float> { }
        public ValueTypeEvent ChangeEvent = new ValueTypeEvent();

        private MicroState.Id.AttrListener<float> attrListener = null;
        private MicroState.Id.AttrListener<float> AttrListener
        {
            get
            {
                if (this.attrListener == null) this.attrListener = new MicroState.Id.AttrListener<float>(StateId, AttrId, this.gameObject);
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

        private void OnValue(float v)
        {
            if (!(this.InvokeWhenInactive || this.isActiveAndEnabled)) return;
            this.ChangeEvent.Invoke(v);
        }

		#region Public Action Methods
        public void Set(float v) { this.AttrListener.Value = v; }
        #endregion
    }
   
#if UNITY_EDITOR
    [CustomEditor(typeof(FloatValue))]
    [CanEditMultipleObjects]
    public class FloatValueEditor : AttrRefEditor<float, FloatValue>
    {

    }
#endif
}