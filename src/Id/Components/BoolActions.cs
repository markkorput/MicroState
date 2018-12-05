using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MicroState.Id.Components
{
	public class BoolActions : MonoBehaviour
	{
		[Header("Attribute ID")]
		public string StateId;
		public string AttrId;

		private AttrRef<bool> attrRef = null;

		private AttrRef<bool> AttrRef
		{
			get
			{
				if (this.attrRef == null) this.attrRef = new AttrRef<bool>(StateId, AttrId, this.gameObject);
				return this.attrRef;
			}
		}
      
		#region Public Action Methods
		public void Set(bool v) { this.AttrRef.Set(v); }
		#endregion
	}
}