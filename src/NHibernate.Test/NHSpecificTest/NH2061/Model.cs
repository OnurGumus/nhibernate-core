using System;
using System.Collections.Generic;

namespace NHibernate.Test.NHSpecificTest.NH2061
{
	public partial class Order
	{
		public virtual Guid Id { get; set; }
		public virtual GroupComponent GroupComponent { get; set; }
	}
	
	public partial class GroupComponent
	{
		public virtual IList<Country> Countries { get; set; }
	}

	public partial class Country
	{
		public virtual string CountryCode { get; set; }
	} 
}