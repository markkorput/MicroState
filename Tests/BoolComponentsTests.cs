using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

namespace MicroState.Id
{
	public class BoolComponentsTests
	{

		private class TestClass
		{
			public bool Flag = false;
		}
      
		private class TestClassState : IdState<TestClass>
		{
			public TestClassState() : this(null) { }
         
			public TestClassState(TestClass instance) : base(instance)
			{
				base.CreateAttr<bool>("Flag",
                    (inst) =>
                    {
                        return inst.Flag;
                    },
                    (inst, val) =>
                    {
                        inst.Flag = val;
                    });
			}
		}
      
		private class TestClassIdStateInstance : IdStateInstance<TestClass, TestClassState>
		{
		}
      
		//[Test]
		//public void NewTestScriptSimplePasses() {
		//    // Use the Assert class to test conditions.
		//}

		// A UnityTest behaves like a coroutine in PlayMode
		// and allows you to yield null to skip a frame in EditMode
		[UnityTest]
		public IEnumerator TypicalUsage()
		{
			var parent = new GameObject();
            var stateinst = parent.AddComponent<TestClassIdStateInstance>();
			stateinst.Id = "TheState";
			stateinst
				.State
				.GetAttr<bool>("Flag")
				.Value = true;

			var child = new GameObject();
            child.transform.SetParent(parent.transform);
         
            var actions = child.AddComponent<BoolActions>();
            actions.StateId = "TheState";
            actions.AttrId = "Flag";

			var attrref = child.AddComponent<BoolEvents>();
			attrref.StateId = "TheState";
			attrref.AttrId = "Flag";

			// register listener
			int counter = 0;
			attrref.ValueEvent.AddListener((val) => counter += 1);

			Assert.AreEqual(counter, 0);
			yield return null; // This invoked the Start method on the attrref component         
			Assert.AreEqual(counter, 2); // ?? should be one?

			stateinst.State.GetAttr<bool>("Flag").Value = false;
			Assert.AreEqual(counter, 3);
			stateinst.State.GetAttr<bool>("Flag").Value = false;
			Assert.AreEqual(counter, 3);

			actions.Set(true);
			Assert.AreEqual(counter, 4);
			Assert.IsTrue(stateinst.State.DataInstance.Flag);
			actions.Set(false);
            Assert.AreEqual(counter, 5);
			Assert.IsFalse(stateinst.State.DataInstance.Flag);         
		}
	}
}