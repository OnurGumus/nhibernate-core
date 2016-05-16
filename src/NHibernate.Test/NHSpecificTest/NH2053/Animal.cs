using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernate.Test.NHSpecificTest.NH2053
{
    public partial class Animal
	{
        public virtual int AnimalId { get; set; }
		public virtual string Name { get; set; }
	}
}
