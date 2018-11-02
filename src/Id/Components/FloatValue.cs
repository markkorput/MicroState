using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace MicroState.Id.Components
{
	public class FloatValue : MonoBehaviour
	{
		[System.Serializable]
		public class FloatEvent : UnityEvent<float> {}
      
		public string StateId;
		public string AttrId;
		public FloatEvent ValueEvent = new FloatEvent();
      
		private IdStateBase stateBase = null;
		private ValueAttr<float> valueAttr = null;
		private bool isFirstValue = true;
		private float lastValue;

		private void Start()
		{
			this.Register();
		}
      
		//private void Update()
		//{
		//	this.Register();         
		//}

		private void Register() {
			if (this.stateBase == null)
            {
                var stateinst = FindStateInstance(this.StateId);
				if (stateinst != null) {
					this.stateBase = stateinst.GetState();
					this.stateBase.ChangeEvent += this.OnStateChange;
					this.ProcessAttr(this.stateBase);
				}
            }
		}
      
		private void OnStateChange()
		{
			this.ProcessAttr(this.stateBase);
		}

		private void ProcessAttr(IdStateBase state) {
			if (this.valueAttr == null)
			{
				this.valueAttr = this.stateBase.GetAttr<float>(AttrId);
				if (this.valueAttr == null) return;
			}
         
			var val = this.valueAttr.Value;
			if (this.isFirstValue || !lastValue.Equals(val))
			{
				this.ValueEvent.Invoke(val);
				this.lastValue = val;
				this.isFirstValue = false;
			}
		}
      
		protected IdStateInstanceBase FindStateInstance(string id) {
			return new List<IdStateInstanceBase>(
				this.GetComponentsInParent<IdStateInstanceBase>())
			    .Find((stateinstance) => stateinstance.Id.Equals(id));
		}
	}
}