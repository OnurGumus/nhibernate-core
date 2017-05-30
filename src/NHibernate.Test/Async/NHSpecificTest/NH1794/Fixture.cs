﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System.Collections.Generic;
using NHibernate.Criterion;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH1794
{
	using System.Threading.Tasks;
	using System.Threading;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		[Test]
		public async Task CanQueryOnCollectionThatAppearsOnlyInTheMappingAsync()
		{
			using (ISession session = OpenSession())
			{
				await (session
					.CreateQuery("select p.Name, c.Name from Person p join p.Children c")
					.ListAsync(CancellationToken.None));
			}
		}

		[Test]
		public async Task CanQueryOnPropertyThatOnlyShowsUpInMapping_AsAccessNoneAsync()
		{
			using (ISession session = OpenSession())
			{
				await (session
					.CreateQuery("from Person p where p.UpdatedAt is null")
					.ListAsync(CancellationToken.None));
			}
		}
	}
}