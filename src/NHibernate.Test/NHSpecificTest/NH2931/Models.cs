using System;

namespace NHibernate.Test.NHSpecificTest.NH2931
{
    public abstract partial class Entity
    {
        public Guid Id { get; private set; }
    }
    public partial class BaseClass : Entity
    {
        public string BaseProperty { get; set; }
    }
    public partial class DerivedClass : BaseClass
    {
        public string DerivedProperty { get; set; }
    }
}
