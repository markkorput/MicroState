using UnityEngine;
using UnityEngine.Events;

namespace MicroState
{
	// NOT YET, only way to have a pre-initialized state [System.Obsolete("Use MicroState.StateInstance and MicroState.Editor Instead")]
	public class StateBehaviour<StateType> : StateInstance<StateType> where StateType : MicroState.State, new()
	{
     
#if UNITY_EDITOR
		private bool mightHaveChanges = false;
		protected bool isPushing = false;
		protected bool isPulling = false;
#endif
      
		void Awake()
        {
            // base.Awake();
			this.State.ChangeEvent.AddListener(this.OnStateChange);
         
			if (PushChanges) {
				StateType editorState = this.GetEditorState();
				this.State.TakeContentFrom(editorState);
			}

			if (PullChanges) Pull(this.State);
        }
      
		private void OnStateChange()
		{
			if (PullChanges && !isPushing)
			{
				isPulling = true;
				Pull(this.State);
				isPulling = false;
			}
		}

#if UNITY_EDITOR
        private void Update()
        {
            if (PushChanges && mightHaveChanges)
            {
				isPushing = true;
				mightHaveChanges = false;
				// get a state instance with the value in our editorput our editor attributes into a state instance
				StateType editorState = this.GetEditorState();
				State.TakeContentFrom(editorState);
				isPushing = false;
            }
        }

		void OnValidate()
        {
			this.SetDirty();

        }
#endif
      
		protected void SetDirty() {
			this.mightHaveChanges = true;
		}
      
		/// <summary>
        /// Method should pull content from our state_ into our public properties
        /// </summary>
		protected virtual void Pull(StateType state) {
			// virtual
		}

		/// <summary>
        /// Method should push content from our public properties into our state_
        /// </summary>      
		protected virtual void Push(StateType state) {
            // virtual
		}

		protected StateType GetEditorState() {
			var state = new StateType();
			this.Push(state);
			return state;
		}      
	}
}