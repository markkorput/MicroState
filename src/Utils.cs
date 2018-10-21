using System;
using UnityEngine;
using UnityEngine.Events;

namespace MicroState {
	public class Subscription : System.IDisposable
    {
        private System.Action disposeFunc;

        public Subscription(System.Action func)
        {
            this.disposeFunc = func;
        }

        public void Dispose()
        {
            this.disposeFunc.Invoke();
        }
    }

	public class Utils
	{
		public static bool With<StateType>(StateInstance<StateType> instance, GameObject go, System.Action<StateType> func) where StateType : State, new()
        {
			// find default instance if instance == null
            if (instance == null) instance = StateInstance<StateType>.For(go);         
			if (instance == null) return false;
         
            // invoke callback now
            func.Invoke(instance.State);
			return true;
		}

		public static Subscription Subscribe<StateType>(StateInstance<StateType> instance, GameObject go, System.Action<StateType> func) where StateType : State, new() {
            // find default instance if instance == null
			if (instance == null) instance = StateInstance<StateType>.For(go);

			// convert generic C# action into UnityAction
			var action = new UnityEngine.Events.UnityAction(() => func.Invoke(instance.State));

            // subscribe
			instance.State.ChangeEvent.AddListener(action);
         
            // invoke callback now
			func.Invoke(instance.State);

			// return subscrition with unsubscribe logic
			return new Subscription(() => instance.State.ChangeEvent.RemoveListener(action));
		}

		public static Subscription Subscribe<StateType>(StateInstance<StateType> instance, GameObject go, UnityAction<StateType, StateType> func) where StateType : State, new()
        {
            // find default instance if instance == null
            if (instance == null) instance = StateInstance<StateType>.For(go);

			// subscribe
			instance.StateHandler.Add(func);
         
			// return subscrition with unsubscribe logic
			return new Subscription(() => instance.StateHandler.Remove(func));
        }
    }
}