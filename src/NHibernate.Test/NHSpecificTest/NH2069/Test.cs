using System;

namespace NHibernate.Test.NHSpecificTest.NH2069
{
    public partial class Test : TestBase, ITest
    {
        public Test() { }
    
        public string Description { get; set; }
        
        public ITest2 Category { get; set; }        
    }
}
