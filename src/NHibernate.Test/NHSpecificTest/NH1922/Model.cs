using System;

namespace NHibernate.Test.NHSpecificTest.NH1922
{
    public partial class Customer 
    {
			public virtual int ID { get; protected set; }
        public virtual DateTime ValidUntil { get; set; }
    }
}
