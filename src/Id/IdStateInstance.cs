using UnityEngine;

namespace MicroState.Id
{
	public class IdStateInstanceBase : MonoBehaviour {
		public string Id = "";

		public virtual IdStateBase GetState() {
			return null;
		}
	}

	public class IdStateInstance<DataT, IdStateT> : IdStateInstanceBase where IdStateT : IdState<DataT>, new() where DataT : new()
	{
		[SerializeField]
		protected DataT data;

		private IdStateT state_;
		private IdStateChangeChecker<DataT> changeChecker = null;

		public IdStateT State
		{
			get
			{
				if (state_ == null)
				{
					if (data == null)
						data = new DataT();

					this.state_ = CreateState(data);
					// The change checker makes sure the state's OnChange event gets invoked,
					// also when changes are made through the unity editor, directly into the
					// serializable this.data object;
					if (changeChecker != null) this.changeChecker.Dispose();
					changeChecker = new IdStateChangeChecker<DataT>(this.state_);
				}

				return state_;
			}
		}

		public override IdStateBase GetState()
		{
			return this.State;
		}

		virtual protected IdStateT CreateState(DataT data) {
			IdStateT state = new IdStateT();
			state.setDataInstance(data);
			return state;
		}

#if UNITY_EDITOR
        private void Update()
        {
			// we don't perform this operation inside the OnGUI method, because that causes a lot of
			// "Assertion failed on expression: 'IsActive() || GetRunInEditMode()'" log errors
            if (guiFlag)
            {
                State.setDataInstance(this.data);
                // if (this.state_ != null) this.state_.setDataInstance(this.data);
                this.changeChecker.Update();
            }
        }

        bool guiFlag = false;

        private void OnGUI()
        {
            guiFlag = true;
        }
#endif
	}
}
