using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MicroState.Id.Components
{
  public class StringArrayValue : MonoBehaviour
  {
    [Header("Attribute ID")]
    public string StateId;
    public string AttrId;

    [Header("Behaviour")]
    public bool InvokeWhenInactive = false;

    [System.Serializable]
    public class ValueTypeEvent : UnityEvent<string[]> { }
    public ValueTypeEvent ChangeEvent = new ValueTypeEvent();

    private MicroState.Id.AttrListener<string[]> attrListener = null;
    private MicroState.Id.AttrListener<string[]> AttrListener
    {
      get
      {
        if (this.attrListener == null) this.attrListener = new MicroState.Id.AttrListener<string[]>(StateId, AttrId, this.gameObject);
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

    private void OnValue(string[] v)
    {
      if (!(this.InvokeWhenInactive || this.isActiveAndEnabled)) return;
      this.ChangeEvent.Invoke(v);
    }

    #region Public Action Methods
      public void Set(string[] v) { this.AttrListener.Set(v); }
    #endregion      
  }

#if UNITY_EDITOR
  [CustomEditor(typeof(StringArrayValue))]
  [CanEditMultipleObjects]
  public class StringArrayValueEditor : AttrRefEditor<string[], StringArrayValue>
  {

  }
#endif
}