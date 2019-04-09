using UnityEngine;

namespace MicroState.Id
{
	public class IdStateProxyInstance<DataT, IdStateT, OriginDataT, OriginStateT /*, OriginStateInstanceT*/> : IdStateInstance<DataT, IdStateT> 
		where DataT : new()
		where IdStateT : IdStateProxy<DataT, OriginDataT>, new()
		where OriginDataT : new()
		where OriginStateT : IdState<OriginDataT>, new()
		// where OriginStateInstanceT : IdStateInstance<OriginDataT, OriginStateT>
	{
		[Tooltip("Our proxy-origin; whenever this state instance's state invokes change notifications, our state will forward those notifications")]
		// public OriginStateInstanceT Origin;
		public IdStateInstanceBase Origin;

		override protected IdStateT CreateState(DataT data) {
			var state = base.CreateState(data);
			if (this.Origin != null) {
				state.SetOrigin(((IdStateInstance<OriginDataT, OriginStateT>)this.Origin).State);
			} else {
				Debug.Log("[IdStateProxyInstance.CreateState] NO ORIGIN");
			}
			return state;
		}
	}
}
