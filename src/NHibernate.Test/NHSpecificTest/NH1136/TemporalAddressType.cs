using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.UserTypes;

namespace NHibernate.Test.NHSpecificTest.NH1136
{
	public partial class TemporalAddressesType : MilestoneCollectionType<DateTime, Address>
	{
	}
}
