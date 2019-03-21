using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using MicroState.Id;
using System.Collections;
using System.Collections.Generic;

namespace MicroState.Id
{
	public class IdStateProxyTests
	{
		// Our custom DATA classes
		private class MasterData
		{
			public string[] names = new string[] {
				"Abe", "Bob", "Cal"
			};
		}

		private class PersonLinkData
		{
			public int Index = 0;
		}

		// Our custom STATE classes, which is an interface to an instance of our DATA class
		private class MasterState : IdState<MasterData>
		{
			public MasterState() : this(null) { }

			public MasterState(MasterData instance) : base(instance) {}
		}

		private class PersonLinkState : IdStateProxy<PersonLinkData, MasterData>
		{
			public PersonLinkState() : this(new PersonLinkData()) { }
			public PersonLinkState(PersonLinkData _inst) : base(_inst) { 
				base.CreateAttr<string>("name",
					(inst) => this.origin.DataInstance.names[inst.Index]);
			}
		}

		private class MasterStateInstance : IdStateInstance<MasterData, MasterState> {}
		private class PersonLinkStateInstance : IdStateProxyInstance<PersonLinkData, PersonLinkState, MasterData, MasterState>{}


		[Test]
		public void IdStateProxyForwardOriginChangeNotifications()
		{
			var master = new MasterState(new MasterData());
			var person = new PersonLinkState(new PersonLinkData());
			person.SetOrigin(master);

			Assert.AreEqual(person.GetAttr<string>("name").Value, "Abe");

			List<string> values = new List<string>();
			person.OnChange += (state) => values.Add(state.GetAttr<string>("name").Value);

			person.DataInstance.Index = 2;
			Assert.AreEqual(string.Join(", ", values), "");
			person.NotifyChange();
			Assert.AreEqual(string.Join(", ", values), "Cal");

			master.DataInstance.names = new string[] {
				"Abe", "Bob", "Cat"
			};
			// master's change notification
			master.NotifyChange(); 
			// verify will person change notification
			Assert.AreEqual(string.Join(", ", values), "Cal, Cat");
		}



		[UnityTest]
		public IEnumerator IdStateProxyInstance()
		{
			var entity = new GameObject("IdstateProxyInstance");			
			var masterStateInstance = entity.AddComponent<MasterStateInstance>();
			var personStateInstance = entity.AddComponent<PersonLinkStateInstance>();
			personStateInstance.Origin = masterStateInstance;
			yield return null;

			List<string> values = new List<string>();			
			personStateInstance.State.OnChange += (state) => values.Add(state.GetAttr<string>("name").Value);
			Assert.AreEqual(string.Join(", ", values), "");
			masterStateInstance.State.DataInstance.names = new string[] { "Abi", "Bob", "Cat" };
			masterStateInstance.State.NotifyChange();
			Assert.AreEqual(string.Join(", ", values), "Abi");
		}
	}
}