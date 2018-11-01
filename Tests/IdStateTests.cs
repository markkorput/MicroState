using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using MicroState.Id;

public class IdStateTests
{
	private class RandomClass
    {
        public int Number = 0;
        public string Name = "";
    }

	private class RandomClassState : IdState<RandomClass>
    {
		public RandomClassState() : this(null) {}
      
		public RandomClassState(RandomClass instance) : base(instance) {
            base.CreateAttr<int>("Number",
                (Instance) => Instance.Number,
                (Instance, val) => Instance.Number = val);
         
			base.CreateAttr<string>("Name",
                (Instance) => Instance.Name,
                (Instance, val) => Instance.Name= val);         
        }
    }
   
    [Test]
    public void IdStateGeneralUsage() {
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
	public void AttrsHaveValueTypeIdsForTypeChecking() {
		// general purpose class
        var cl = new RandomClass();
        // IdState
        var state = new RandomClassState(cl);

		Assert.AreEqual(state.GetAttr<int>("Number").ValueType, ((int)0).GetType());
		Assert.AreEqual(state.GetAttr<string>("Name").ValueType, ((string)"").GetType());
	}


    //// A UnityTest behaves like a coroutine in PlayMode
    //// and allows you to yield null to skip a frame in EditMode
    //[UnityTest]
    //public IEnumerator IdStateTestsWithEnumeratorPasses() {
    //    // Use the Assert class to test conditions.
    //    // yield to skip a frame
    //    yield return null;
    //}
}
