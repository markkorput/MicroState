using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

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
		public IEnumerator InvokesValueEventWhenStateValueChanges()
		{
			// create our state instance
			var parent = new GameObject();
            var stateinst = parent.AddComponent<TestClassIdStateInstance>();
			// our state's string-based ID
			stateinst.Id = "TheState";
			stateinst
				.State
				.GetAttr<float>("Score")
				.Value = 1f;

			// create our state-attribute
			var child = new GameObject();
			child.transform.SetParent(parent.transform);
			var floatattr = child.AddComponent<Components.FloatAttr>();
			// identify our state by its string-based ID
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
		public IEnumerator InvokeWhenInactive() {
			// setup state
			var parent = new GameObject();
            var stateinst = parent.AddComponent<TestClassIdStateInstance>();
            stateinst.Id = "TheState";
            stateinst
                .State
                .GetAttr<float>("Score")
                .Value = 1f;

			// setup attr
			var child = new GameObject();
            child.transform.SetParent(parent.transform);
            var floatattr = child.AddComponent<Components.FloatAttr>();
            floatattr.StateId = "TheState";
            floatattr.AttrId = "Score";

			// check default behaviour
			Assert.AreEqual(floatattr.InvokeWhenInactive, false); // default
			int counter = 0;
			floatattr.ValueEvent.AddListener((val) => counter += 1);
			yield return null;
			Assert.AreEqual(counter, 1);

			// inactive
			child.SetActive(false);
			Assert.AreEqual(counter, 1);
			stateinst.State.GetAttr<float>("Score").Value += 1;         
			yield return null;
			Assert.AreEqual(counter, 1);

			// active
			child.SetActive(true);
			Assert.AreEqual(counter, 1);
			stateinst.State.GetAttr<float>("Score").Value += 1;
			Assert.AreEqual(counter, 2);
			yield return null;
			Assert.AreEqual(counter, 2);

			// inactive, but invoking
			floatattr.InvokeWhenInactive = true;
			child.SetActive(false);
            Assert.AreEqual(counter, 2);
            stateinst.State.GetAttr<float>("Score").Value += 1;
			Assert.AreEqual(counter, 3);
            yield return null;
			Assert.AreEqual(counter, 3);
		}

		[UnityTest]
		public IEnumerator DirectInstanceAssignmentHasPriority () {

			// setup state
			var parent = new GameObject();
            var stateinst = parent.AddComponent<TestClassIdStateInstance>();
            stateinst.Id = "TheState";
            stateinst
                .State
                .GetAttr<float>("Score")
                .Value = 1f;
            var stateinst2 = parent.AddComponent<TestClassIdStateInstance>();
            stateinst2.Id = "TheState2";
            stateinst
                .State
                .GetAttr<float>("Score")
                .Value = 2f;

			// setup attr
			var child = new GameObject();
            child.transform.SetParent(parent.transform);
            var floatattr = child.AddComponent<Components.FloatAttr>();
			floatattr.StateInstance = stateinst; // #1, NOT #2
            floatattr.StateId = "TheState2"; // #2m NOT #1 (conflicting with) StateInstance
            floatattr.AttrId = "Score";

			Assert.AreEqual(floatattr.Get(), 2f);
			yield return null;

			List<float> vals = new List<float>();
			floatattr.ValueEvent.AddListener((val) => vals.Add(val));
			stateinst2.State.GetAttr<float>("Score").Value = 4f;
			Assert.AreEqual(vals.Count, 0);
			stateinst.State.GetAttr<float>("Score").Value = 5f;
			Assert.AreEqual(vals.Count, 1);
			Assert.AreEqual(vals[0], 5f);
		}
	}
}