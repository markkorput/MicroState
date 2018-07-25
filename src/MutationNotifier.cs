using System;
using UnityEngine.Events;

namespace MicroState
{
	public class MutationNotifier
	{
		public UnityEvent ChangeEvent = new UnityEvent();
      
		private int postPonersCount = 0;
        private bool bHasNotifications = false;

        public void BatchUpdate(System.Action func)
        {
            postPonersCount += 1;
            func.Invoke();
            postPonersCount -= 1;
            if (bHasNotifications) this.NotifyChange();
        }
      
		protected void NotifyChange()
        {
			bHasNotifications = true;
			if (postPonersCount > 0) return;

			while (bHasNotifications)
			{
				// clear has notifications flag
				bHasNotifications = false;
                // avoid ChangeEvent being triggered while executing ChangeEvent callbacks
				postPonersCount += 1;
				// run change event listeners; the might trigger new changes and set bHasNotications back to true
				ChangeEvent.Invoke();
                // stop our change event block
				postPonersCount -= 1;
			}
        }
    }
}
