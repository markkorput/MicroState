using System;
namespace MicroState.Id
{
	public class BaseAttrDef
    {
        public string id;
        public System.Type ValueType;
    }

    public class AttrDef<StateT, ValT> : BaseAttrDef
    {
        public System.Func<StateT, ValT> getter;
        public System.Action<StateT, ValT> setter;

        public AttrDef(string i, System.Func<StateT, ValT> g, System.Action<StateT, ValT> s)
        {
            id = i; getter = g; setter = s;
            this.ValueType = getter.Method.ReturnType;
        }
    }

    public class BaseAttr
    {
        protected BaseAttrDef attrDef;
        public System.Type ValueType { get { return attrDef.ValueType; } }      
    }

    public class Attr<StateT, ValT> : BaseAttr
    {
        private StateT instance;
      
        public ValT Value
        {
            get { return ((AttrDef<StateT, ValT>)this.attrDef).getter.Invoke(instance); }
            set { ((AttrDef<StateT, ValT>)this.attrDef).setter.Invoke(instance, value); }
        }

        public Attr(StateT instance, AttrDef<StateT, ValT> attrDef)
        {
            this.instance = instance;
            this.attrDef = attrDef;
        }
    }
}
