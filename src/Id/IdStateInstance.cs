using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MicroState.Id
{
	public class IdStateInstance<DataT, IdStateT> : MonoBehaviour where IdStateT : IdState<DataT>, new()
	{
		[SerializeField]
		protected DataT data;
		private IdStateT state_;

		public IdStateT State
		{
			get
			{
				if (state_ == null)
				{
					state_ = new IdStateT();
					state_.setDataInstance(data);
				}

				return state_;
			}
		}

		private void OnGUI()
		{
			
		}
	}
}