using UnityEngine;
using UnityEngine.Events;

namespace MicroState
{
    public class StateInstance<StateType> : MonoBehaviour where StateType : MicroState.State, new()
    {
        #region Private attributes      
        private StateType state_ = new StateType();
        private MicroState.StateHandler<StateType> stateHandler_ = null;
        #endregion

        public StateType State { get { return state_; } }

        public StateHandler<StateType> StateHandler
        {
            get
            {
                if (stateHandler_ == null) stateHandler_ = new StateHandler<StateType>(state_);
                return stateHandler_;
            }
        }

        #region Configurable Attributes      
        [Header("State Behaviour")]
        public bool PullChanges = false;
        public bool PushChanges = false;
        #endregion

		public static StateInstance<StateType> For(GameObject go) {
			return go.GetComponentInParent<StateInstance<StateType>>();
		}
    }
}