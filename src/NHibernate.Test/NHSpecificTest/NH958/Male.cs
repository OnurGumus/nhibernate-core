using System;

namespace NHibernate.Test.NHSpecificTest.NH958
{
    public partial class Male : Person
    {
        public Male()
        {
        }

        public Male(string name)
            : base(name) {}
    }
}
