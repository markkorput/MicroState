using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

namespace MicroState.Id
{
	public class FloatAttrTests
	{

		private class TestClass
		{
			public float Score = 0;
		}
      
		private class TestClassState : IdState<TestClass>
		{
			public TestClassState() : this(null) { }
         
			public TestClassState(TestClass instance) : base(instance)
			{
				base.CreateAttr<float>("Score",
                    (inst) =>
                    {
                        return inst.Score;
                    },
                    (inst, val) =>
                    {
                        inst.Score = val;
                    });
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
				.GetAttr<float>("Score")
				.Value = 1f;

			var child = new GameObject();
			child.transform.SetParent(parent.transform);
			var floatattr = child.AddComponent<Components.FloatAttr>();
			floatattr.StateId = "TheState";
			floatattr.AttrId = "Score";         

            // register listener
			float valsum = 0.0f;
			floatattr.ValueEvent.AddListener((val) => valsum += val);

			Assert.AreEqual(valsum, 0.0f);
			yield return null; // This invoked the Start method on the floatattr component         
			Assert.AreEqual(valsum, 1f);         
         
			stateinst.State.GetAttr<float>("Score").Value = 2f;
			Assert.AreEqual(valsum, 3f);
			stateinst.State.GetAttr<float>("Score").Value = 4f;
			Assert.AreEqual(valsum, 7f);         
		}

        [UnityTest]
		public IEnumerator EventListenerManagement() {
			var parent = new GameObject();
            var stateinst = parent.AddComponent<TestClassIdStateInstance>();
            stateinst.Id = "TheState";
            stateinst
                .State
                .GetAttr<float>("Score")
                .Value = 1f;

			var child = new GameObject();
            child.transform.SetParent(parent.transform);
            var floatattr = child.AddComponent<Components.FloatAttr>();
            floatattr.StateId = "TheState";
            floatattr.AttrId = "Score";
         
			int counter = 0;
			floatattr.ValueEvent.AddListener((val) => counter += 1);
			yield return null;
			Assert.AreEqual(counter, 1);
			Assert.AreEqual(floatattr.InvokeWhenInactive, false);
			child.SetActive(false);
			Assert.AreEqual(counter, 1);
			stateinst.State.GetAttr<float>("Score").Value += 1;         
			yield return null;
			Assert.AreEqual(counter, 1);
			child.SetActive(true);
			Assert.AreEqual(counter, 1);
			stateinst.State.GetAttr<float>("Score").Value += 1;
			Assert.AreEqual(counter, 2);
			yield return null;
			Assert.AreEqual(counter, 2);
         
			floatattr.InvokeWhenInactive = true;
			child.SetActive(false);
            Assert.AreEqual(counter, 2);
            stateinst.State.GetAttr<float>("Score").Value += 1;
			Assert.AreEqual(counter, 3);
            yield return null;
			Assert.AreEqual(counter, 3);
		}
	}
}