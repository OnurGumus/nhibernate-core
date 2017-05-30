﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH2234
{
  using System.Threading.Tasks;

	[TestFixture]
	public class FixtureAsync: BugTestCase
	{
	  [Test]
	  public async Task CanQueryViaLinqAsync()
	  {
		using (var s = OpenSession())
		{
		var qry = from item in s.Query<SomethingLinq>() where item.Relation == MyUserTypes.Value1 select item;

			await (qry.ToListAsync());
			Assert.That(() => qry.ToList(), Throws.Nothing);
		}
	  }
	}
}
