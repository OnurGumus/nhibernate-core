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
using NHibernate.Dialect;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH1788
{
	using System.Threading.Tasks;
	using System.Threading;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		protected override bool AppliesTo(NHibernate.Dialect.Dialect dialect)
		{
			return dialect is MsSql2000Dialect;
		}

		[Test]
		public async Task CanUseSqlTimestampWithDynamicInsertAsync()
		{
			using (var session = OpenSession())
			using (var tx = session.BeginTransaction())
			{
				await (session.SaveAsync(new Person
				{
					Name = "hi"
				}, CancellationToken.None));
				await (tx.CommitAsync(CancellationToken.None));
			}


			using (var session = OpenSession())
			using (var tx = session.BeginTransaction())
			{
				var person = await (session.GetAsync<Person>(1, CancellationToken.None));
				person.Name = "other";
				await (tx.CommitAsync(CancellationToken.None));
			} 


			using (ISession session = OpenSession())
			using (var tx = session.BeginTransaction())
			{
				await (session.DeleteAsync(await (session.GetAsync<Person>(1, CancellationToken.None)), CancellationToken.None));
				await (tx.CommitAsync(CancellationToken.None));
			}
			
		}
	}
}