using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MicroState.Id
{
  public class AttrListener<ValueT> : AttrRef<ValueT>, System.IDisposable
  {
    public class ValueEventType : UnityEvent<ValueT> { }
    private ValueEventType valueEvent_ = null;

    private List<System.Action> disposeFuncs = new List<System.Action>();

    public ValueEventType ValueEvent
    {
      get
      {
        if (this.valueEvent_ == null)
        {
          this.valueEvent_ = new ValueEventType();
          // register attribute listener that trigger our value event
          var state = this.StateBase;
          if (state != null)
          {
            state.ChangeEvent += this.OnStateChange;

            // register cleanup func
            this.disposeFuncs.Add(() =>
            {
              this.StateBase.ChangeEvent -= this.OnStateChange;
            });
          }
        }

        return this.valueEvent_;
      }
    }

    public AttrListener(string stateid, string attrid, GameObject gameObject = null) : base(stateid, attrid, gameObject)
    {

    }

    public void Dispose()
    {
      foreach (var func in disposeFuncs) func.Invoke();
      this.disposeFuncs.Clear();
    }

    private void OnStateChange()
    {
      var attr = this.ValueAttr;
      if (attr != null) this.valueEvent_.Invoke(attr.Value);
    }
      
	public void InvokeValue() {
    	this.OnStateChange();
	}
  }
}
