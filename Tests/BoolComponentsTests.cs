using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

namespace MicroState.Id
{
	public class BoolAttrTests
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
                    (inst) => inst.Flag,
                    (inst, val) => inst.Flag = val);
			}
		}
      
		private class TestClassIdStateInstance : IdStateInstance<TestClass, TestClassState>
		{
		}

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
			child.SetActive(false);

            var bval = child.AddComponent<Components.BoolAttr>();
			child.SetActive(true);
			bval.StateInstance = stateinst;
			bval.AttrId = "Flag";

			// register listener
			int counter = 0;
			bval.ChangeEvent.AddListener((val) => counter += 1);

			Assert.AreEqual(counter, 0);
			yield return null; // This invoked the Start method on the attrref component         
			Assert.AreEqual(counter, 1);

			stateinst.State.GetAttr<bool>("Flag").Value = false;
			Assert.AreEqual(counter, 2);
			stateinst.State.GetAttr<bool>("Flag").Value = false;
			Assert.AreEqual(counter, 2);

			bval.Set(true);
			Assert.AreEqual(counter, 3);
			Assert.IsTrue(stateinst.State.DataInstance.Flag);
			bval.Set(false);
            Assert.AreEqual(counter, 4);
			Assert.IsFalse(stateinst.State.DataInstance.Flag);
		}
	}
}