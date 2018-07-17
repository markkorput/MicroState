using System;
using UnityEngine.Events;

namespace MicroState
{
	public class MutationNotifier
	{
		public UnityEvent ChangeEvent = new UnityEvent();
      
		private int postPonersCount = 0;
        private bool bHasPostponedNotifications = false;

        public void BatchUpdate(System.Action func)
        {
            postPonersCount += 1;
            func.Invoke();
            postPonersCount -= 1;
            if (bHasPostponedNotifications) this.NotifyChange();
        }
      
		protected void NotifyChange()
        {
            if (postPonersCount > 0)
            {
                bHasPostponedNotifications = true;
                return;
            }

            ChangeEvent.Invoke();
        }
    }
}
