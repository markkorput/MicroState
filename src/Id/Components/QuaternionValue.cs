﻿using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MicroState.Id.Components
{
  [System.Obsolete("Use QuaternionAttr")]
  public class QuaternionValue : MonoBehaviour
  {
    [Header("Attribute ID")]
    public string StateId;
    public string AttrId;

    [Header("Behaviour")]
    public bool InvokeWhenInactive = false;

    [System.Serializable]
    public class ValueTypeEvent : UnityEvent<Quaternion> { }
    public ValueTypeEvent ChangeEvent = new ValueTypeEvent();

    private MicroState.Id.AttrListener<Quaternion> attrListener = null;
    private MicroState.Id.AttrListener<Quaternion> AttrListener
    {
      get
      {
        if (this.attrListener == null) this.attrListener = new MicroState.Id.AttrListener<Quaternion>(StateId, AttrId, this.gameObject);
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

    private void OnValue(Quaternion v)
    {
      if (!(this.InvokeWhenInactive || this.isActiveAndEnabled)) return;
      this.ChangeEvent.Invoke(v);
    }

    #region Public Action Methods
    public void Set(Quaternion v) { this.AttrListener.Set(v); }
    public void Invoke(bool alsoWhenInactive=true) {
      if (this.isActiveAndEnabled || alsoWhenInactive)
        this.ChangeEvent.Invoke(this.AttrListener.Value);
    }
    #endregion      
  }

#if UNITY_EDITOR
  [CustomEditor(typeof(QuaternionValue))]
  [CanEditMultipleObjects]
  public class QuaternionValueEditor : AttrRefEditor<Quaternion, QuaternionValue>
  {

  }
#endif
}