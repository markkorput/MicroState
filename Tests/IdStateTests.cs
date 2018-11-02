using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using MicroState.Id;

namespace MicroState.Id
{
	public class IdStateTests
	{
		private class RandomClass
		{
			public int Number = 0;
			public string Name = "";
		}

		private class RandomClassState : IdState<RandomClass>
		{
			public RandomClassState() : this(null) { }

			public RandomClassState(RandomClass instance) : base(instance)
			{
				base.CreateAttr<int>("Number",
					(Instance) => Instance.Number,
					(Instance, val) => Instance.Number = val);

				base.CreateAttr<string>("Name",
					(Instance) => Instance.Name,
					(Instance, val) => Instance.Name = val);
			}
		}

		[Test]
		public void IdStateGeneralUsage()
		{
			// general purpose class
			var cl = new RandomClass();
			// IdState
			var state = new RandomClassState(cl);

			Assert.AreEqual(state.GetAttr<int>("Number").Value, 0);

			// Values Set on the State Attribute are persisted to the original instance
			state.GetAttr<int>("Number").Value = 101;
			state.GetAttr<string>("Name").Value = "Jane Doe";
			Assert.AreEqual(cl.Number, 101);
			Assert.AreEqual(cl.Name, "Jane Doe");

			// And vice-verse
			cl.Number = 203;
			cl.Name = "John Doe";
			Assert.AreEqual(state.GetAttr<int>("Number").Value, 203);
			Assert.AreEqual(state.GetAttr<string>("Name").Value, "John Doe");
		}

		[Test]
		public void AttrsHaveValueTypeIdsForTypeChecking()
		{
			// general purpose class
			var cl = new RandomClass();
			// IdState
			var state = new RandomClassState(cl);

			Assert.AreEqual(state.GetAttr<int>("Number").ValueType, ((int)0).GetType());
			Assert.AreEqual(state.GetAttr<string>("Name").ValueType, ((string)"").GetType());
		}

		[Test]
		public void OnChange_notification_for_changes_to_Attrs()
		{
			// general purpose class
			var cl = new RandomClass();
			// IdState
			var state = new RandomClassState(cl);

			int counter = 0;
			state.OnChange += (s) => { counter += s.GetAttr<int>("Number").Value; };

			state.GetAttr<int>("Number").Value = 1;
			Assert.AreEqual(counter, 1);
			state.GetAttr<string>("Name").Value = "a";
			Assert.AreEqual(counter, 2);
			state.GetAttr<string>("Name").Value = "b";
			Assert.AreEqual(counter, 3);
			state.GetAttr<int>("Number").Value = 10;
			Assert.AreEqual(counter, 13);
			state.GetAttr<int>("Number").Value = 10; // no change
			Assert.AreEqual(counter, 13);
		}

		[Test]
		public void OnChange_notification_for_changes_to_original_object()
		{
			// general purpose class
			var cl = new RandomClass();
			// IdState
			var state = new RandomClassState(cl);

			int counter = 0;
			state.OnChange += (s) => { counter += s.GetAttr<int>("Number").Value; };

			cl.Name = "Bob";
			cl.Number = 1;
			Assert.AreEqual(counter, 0); // OnChange callback NOT invoked

			var checker = new MicroState.Id.IdStateChangeChecker<RandomClass>(state);
			Assert.AreEqual(counter, 0); // OnChange callback NOT invoked
			cl.Number = 2;
			Assert.AreEqual(counter, 0); // OnChange callback NOT invoked
			checker.Update();
			Assert.AreEqual(counter, 2); // Invoked
			cl.Number = 8;
			Assert.AreEqual(counter, 2); // OnChange callback NOT invoked
			cl.Number = 4;
			Assert.AreEqual(counter, 2); // OnChange callback NOT invoked
			checker.Update();
			Assert.AreEqual(counter, 6); // Invoked
			cl.Number = 3;
			state.GetAttr<int>("Number").Value = 1; // no change
			Assert.AreEqual(counter, 7); // Invoked
			checker.Update();
			Assert.AreEqual(counter, 7); // OnChange callback NOT invoked
		}

		[Test]
		public void AreEqual()
		{
			// general purpose class
			var cl1 = new RandomClass();
			var cl2 = new RandomClass();
			// IdState
			var state = new RandomClassState(cl1);

			Assert.AreEqual(state.AreEqual(cl1, cl2), true);
			cl1.Number = 5;
			Assert.AreEqual(state.AreEqual(cl1, cl2), false);
			cl2.Number = 5;
			Assert.AreEqual(state.AreEqual(cl1, cl2), true);
			cl2.Name = "foo";
			Assert.AreEqual(state.AreEqual(cl1, cl2), false);
			cl1.Name = "foo";
			Assert.AreEqual(state.AreEqual(cl1, cl2), true);
		}      
	}
}