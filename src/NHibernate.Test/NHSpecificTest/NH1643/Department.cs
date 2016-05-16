using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernate.Test.NHSpecificTest.NH1643
{
    public partial class Department
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }
    }
}
