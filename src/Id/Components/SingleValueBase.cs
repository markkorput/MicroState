using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace MicroState.Id.Components
{
	public class SingleValueBase<ValueT> : MonoBehaviour
	{      
		public string StateId;
		public string AttrId;

		[System.Serializable]
        public class ValueTypeEvent : UnityEvent<float> {}
		public ValueTypeEvent ValueEvent; // = new ValueTypeEvent();      

		private IdStateBase stateBase = null;
		private ValueAttr<ValueT> valueAttr = null;
		private bool isFirstValue = true;
		private ValueT lastValue;

		private void Start()
		{
			this.Register();
		}

		//private void Update()
		//{
		//	this.Register();         
		//}

		private void Register()
		{
			if (this.stateBase == null)
			{
				var stateinst = FindStateInstance(this.StateId);
				if (stateinst != null)
				{
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

		private void ProcessAttr(IdStateBase state)
		{
			if (this.valueAttr == null)
			{
				this.valueAttr = this.stateBase.GetAttr<ValueT>(AttrId);
				if (this.valueAttr == null) return;
			}

			var val = this.valueAttr.Value;

			if (this.isFirstValue || !lastValue.Equals(val))
			{
				//this.ValueEvent.Invoke(val);
				this.lastValue = val;
				this.isFirstValue = false;
			}
		}

		protected IdStateInstanceBase FindStateInstance(string id)
		{
			return new List<IdStateInstanceBase>(
				this.GetComponentsInParent<IdStateInstanceBase>())
				.Find((stateinstance) => stateinstance.Id.Equals(id));
		}

		#region Public Methods
		public void Set(ValueT val) {
			if (this.valueAttr == null)
            {
                this.valueAttr = this.stateBase.GetAttr<ValueT>(AttrId);
                if (this.valueAttr == null) return;
            }

			this.valueAttr.Value = val;         
		}
		#endregion
	}
}