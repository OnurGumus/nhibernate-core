using System;
using System.Collections.Generic;

namespace NHibernate.Test.NHSpecificTest.NH2662
{
	public partial class Customer
	{
		public virtual Guid Id
		{
			get;
			protected set;
		}
		public virtual Order Order
		{
			get;
			set;
		}
	}

    public partial class Order
    {
        public virtual Guid Id
        {
            get;
            protected set;
        }

        public virtual DateTime OrderDate
        {
            get;
            set;
        }
    }

    public partial class PizzaOrder: Order
    {
        public virtual string PizzaName
        {
            get;
            set;
        }
    }
}