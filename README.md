# MicroState
state management for fast prototyping in unity

## When To Use
The aim of this library is to provide a quick 'n easy way of doing proper state-management without the verbosity of more mature and full-featured solutions.

These tools are specifically intended for developing and testing small components, NOT for managing full-blown complex application. For more complex situations I suggest to have a look at more elaborate libraries, like [UniRX](https://github.com/neuecc/UniRx).

## Idea
MicroState wants to help you manage the state of small components, specifically to create a clearly identifiable single source of truth and take away the temptation of using globals/singletons.

It provides the following classes;

##### MicroState.State
`State` is a simple C# class (NOT a MonoBehaviour) and serves as base class for the piece of state that should drive your component.

##### MicroState.StateBehaviour
`StateBehaviour` is a Unity 'frontend' for your state; it serves both as _editor_ for your state (giving you the Unity component editor to manipulate your State at runtime). as well as as a _handle_ to your state instance; all components that use the same State should point at the same StateBehaviour and use its public State attribute instead of relying on a global singleton instance.

#### MicroState.Attribute
`Attribute` is a helper class to wrap around a single attribute inside your State and helps the MicroState classes to automate a couple of processes and thus reduce the amount of boilerplate code in your application.

## Usage
Create your custom `State` class that contains all the data needed to control your component, and create a custom `StateBehaviour` that defines how to transfer ('push' and 'pull') data between the State instance and its public attributes
that can be accessed using the Unity component editor.

```C#
// Project/Assets/MyProject/MyComponent/State.cs

namespace MyProject.MyComponent {

  class State : MicroState.State {
    public MicroState.Attribute<string> TitleAttr { get; private set; };
    public MicroState.Attribute<bool> ShowHelpAttr { get; private set; };
    public MicroState.Attribute<float> ProgressAttr { get; private set; };

    public MyComponent() {
      TitleAttr = base.CreateAttribute<string>();
      ShowHelpAttr = base.CreateAttribute<bool>(false);
      ProgressAttr = base.CreateAttribute<float>(0.0f);
    }
  }


  class MyComponentState : MicroState.StateBehaviour<State> {
    // these public attributes are through the Unity component editor
    public string Title;
    public bool ShowHelp;
    public float Progress;

    // Override StateBehaviour's virtual Pull method with custom pull logic
    override protected void Pull(State state)
    {
      // "Pull" individual attribute values from the state
      this.Title = state.TitleAttr.Value;
      this.ShowHelp = state.ShowHelpAttr.Value;
      this.Progress = state.ProgressAttr.Value;
    }

    // Override StateBehaviour's virtual Push method with custom push logic
    override protected void Push(State state) {
      // trigger single notification for all changes
      state.BatchUpdate(() => {
        // "Push" individual attribute values into the state
        state.TitleAttr.Value = this.Title;
        state.ShowHelpAttr.Value = this.ShowHelp;
        state.ProgressAttr.Value = this.Progress;
      });
    }
  }
}
```

Now, if you add the MyComponentState script to an object, you have a managed State instance, as well
as an editor which shows the realtime values of the state and lets you change those values at runtime.

You can use this state in any custom script, by linking to your MyComponentState;

```C#
// Project/Assets/MyProject/MyComponent/Controller.cs

namespace MyProject.MyComponent {
  class Controller : MonoBehaviour {
    public MyComponentState ComponentState; // <-- configure this attribute to link to your MyComponentState in the editor

    void Start () {
      // register listener for whenever the State changes. The UpdateEvent provides
      // 2 arguments; the previous state and the new (current) state
      ComponentState.UpdateEvent.AddListener(this.OnStateUpdate);
    }

    private void OnStateUpdate(State previous, State current) {
      // use the data in current to update your components.
      transform.Find("Title").GetComponent<UnityEngine.UI.Text>().text = current.TitleAttr.Value;
      transform.Find("ProgressIndicator").transform.position = new Vector(current.ProgressAttr.Value, 0, 0);

      // Optionally, you can compare value in current with values in previous to see if something changes
      if (current.ShowHelpAttr.Value != previous.ShowHelpAttr.Value) {
        transform.Find("Help").SetActive(current.ShowHelpAttr.Value);
      }
    }

    // you can simply write to the Attributes in the State and it will Invoke the necessary Change/Update events
    // That will trigger update in your UI (assuming you've registered the proper event listeners with UI update logic)
    public void ToggleHelp() {
      ComponentState.State.ShowHelp.Value = !ComponentState.State.ShowHelp.Value;
    }
  }
}
```
