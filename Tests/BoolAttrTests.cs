using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

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
			var stateobj = new GameObject();
            var stateinst = stateobj.AddComponent<TestClassIdStateInstance>();
			stateinst
				.State
				.GetAttr<bool>("Flag")
				.Value = true;

			var attrobj = new GameObject();
            attrobj.transform.SetParent(stateobj.transform);
			attrobj.SetActive(false);

            var bval = attrobj.AddComponent<Components.BoolAttr>();
			bval.StateInstance = stateinst;
			bval.AttrId = "Flag";

			// register listener
			int counter = 0;
			bval.ChangeEvent.AddListener((val) => counter += 1);
			Assert.AreEqual(counter, 0);

			attrobj.SetActive(true);
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
			
			GameObject.Destroy(stateobj);
			GameObject.Destroy(attrobj);
		}

		[UnityTest]
		public IEnumerator InvokeStartValueTest() {
			var stateobj = new GameObject();
            var stateinst = stateobj.AddComponent<TestClassIdStateInstance>();
			stateinst.Id = "TheState";
			// stateinst.State.GetAttr<bool>("Flag").Value = true;
			
			System.Func<Components.BoolAttr> createAttr = () => {
				var attrobj = new GameObject();
				attrobj.SetActive(false);
				var attr = attrobj.AddComponent<Components.BoolAttr>();
				attr.StateInstance = stateinst;
				attr.InvokeStartValue = true;
				attr.AttrId = "Flag";
				return attr;
			};

			List<bool> values = new List<bool>();
			var bval = createAttr();
			// Assert.AreEqual(bval.Value, false);
			bval.ChangeEvent.AddListener((v) => values.Add(v));
			bval.gameObject.SetActive(true);
			yield return null;

			Assert.AreEqual(values.Count, 1);
			Assert.AreEqual(values[0], false);

			bval.Value = true; // change value
			Assert.AreEqual(stateinst.State.DataInstance.Flag, true);
			GameObject.Destroy(bval.gameObject);
			values.Clear();

			bval = createAttr();
			// Assert.AreEqual(bval.Value, true);
			bval.ChangeEvent.AddListener((v) => values.Add(v));
			bval.gameObject.SetActive(true);
			yield return null;
			Assert.AreEqual(values.Count, 1);
			Assert.AreEqual(values[0], true);

			GameObject.Destroy(bval.gameObject);

			GameObject.Destroy(stateobj);
		}

		[UnityTest]
		public IEnumerator InvokeOnEnableTest() {
			var stateobj = new GameObject();
            var stateinst = stateobj.AddComponent<TestClassIdStateInstance>();
			stateinst.Id = "TheState";
			stateinst.State.GetAttr<bool>("Flag").Value = true;

			System.Func<Components.BoolAttr> createAttr = () => {
				var attrobj = new GameObject();
				attrobj.SetActive(false);
				var attr = attrobj.AddComponent<Components.BoolAttr>();
				attr.StateInstance = stateinst;
				attr.InvokeOnEnable = true;
				attr.AttrId = "Flag";
				return attr;
			};

			List<bool> values = new List<bool>();
			var bval = createAttr();
			// Assert.AreEqual(bval.Value, false);
			bval.ChangeEvent.AddListener((v) => values.Add(v));
			bval.gameObject.SetActive(true);
			yield return null;

			Assert.AreEqual(values.Count, 1);
			Assert.AreEqual(values[values.Count-1], true); // invoked initial value

			stateinst.State.GetAttr<bool>("Flag").Value = false;
			Assert.AreEqual(values.Count, 2); // new value invoked
			Assert.AreEqual(values[values.Count-1], false);

			stateinst.State.GetAttr<bool>("Flag").Value = false;
			Assert.AreEqual(values.Count, 2); // no change

			bval.gameObject.SetActive(false);
			stateinst.State.GetAttr<bool>("Flag").Value = true;
			Assert.AreEqual(values.Count, 2); // new true value not invoked

			bval.gameObject.SetActive(true);
			Assert.AreEqual(values.Count, 3); // new true value  invoked
			Assert.AreEqual(values[values.Count-1], true);

			bval.gameObject.SetActive(false);
			stateinst.State.GetAttr<bool>("Flag").Value = false;
			stateinst.State.GetAttr<bool>("Flag").Value = true;
			bval.gameObject.SetActive(true);
			Assert.AreEqual(values.Count, 4); // value invoked, even when not changed
			Assert.AreEqual(values[values.Count-1], true);

			GameObject.Destroy(bval.gameObject);
			GameObject.Destroy(stateobj);
		}
	}
}