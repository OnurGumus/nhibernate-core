using System.Collections.Generic;

namespace NHibernate.Test.NHSpecificTest.NH2603
{
    public partial class Parent
    {
        public virtual int Id
        {
            get;
            set;
        }

        public virtual IList<Child> ListChildren
        {
            get;
            set;
        }

        public virtual IDictionary<int, Child> MapChildren
        {
            get;
            set;
        }
    }

    public partial class Child
    {
        public virtual int Id
        {
            get;
            set;
        }
    }
}
