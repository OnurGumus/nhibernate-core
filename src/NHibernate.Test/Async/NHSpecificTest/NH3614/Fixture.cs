﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Linq;

namespace NHibernate.Test.NHSpecificTest.NH3614
{
	using System.Threading.Tasks;
	using System.Threading;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		[Test]
		public async Task CanProjectListOfStringsAsync()
		{
			Guid id;
			using (var s = OpenSession())
			using (var tx = s.BeginTransaction())
			{
				var testEntity = new Entity
				{
					SomeStrings = new List<string> { "Hello", "World" }
				};
				await (s.SaveAsync(testEntity, CancellationToken.None));

				await (tx.CommitAsync(CancellationToken.None));

				id = testEntity.Id;
			}

			using (var s = OpenSession())
			{
				var result = await (s.Query<Entity>()
					.Where(x => x.Id == id)
					.Select(x => x.SomeStrings)
					.ToListAsync(CancellationToken.None));

				Assert.AreEqual(1, result.Count);

				Assert.AreEqual(2, result.Single().Count);
			}

			using (var s = OpenSession())
			using (var tx = s.BeginTransaction())
			{
				await (s.DeleteAsync("from Entity", CancellationToken.None));
				await (tx.CommitAsync(CancellationToken.None));
			}
		}
	}
}
