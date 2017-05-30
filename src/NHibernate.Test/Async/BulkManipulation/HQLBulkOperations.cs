﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using NUnit.Framework;

namespace NHibernate.Test.BulkManipulation
{
	using System.Threading.Tasks;
	using System.Threading;
	[TestFixture]
	public class HqlBulkOperationsAsync: BaseFixture
	{
		[Test]
		public async Task SimpleDeleteAsync()
		{
			using (var s = OpenSession())
			using (var tx = s.BeginTransaction())
			{
				await (s.SaveAsync(new SimpleClass {Description = "simple1"}, CancellationToken.None));
				await (s.SaveAsync(new SimpleClass {Description = "simple2"}, CancellationToken.None));
				await (tx.CommitAsync(CancellationToken.None));
			}

			using (var s = OpenSession())
			using (var tx = s.BeginTransaction())
			{
				Assert.That(await (s.CreateQuery("delete from SimpleClass where Description = 'simple2'").ExecuteUpdateAsync(CancellationToken.None)),
					Is.EqualTo(1));
				await (tx.CommitAsync(CancellationToken.None));
			}

			using (var s = OpenSession())
			using (var tx = s.BeginTransaction())
			{
				Assert.That(await (s.CreateQuery("delete from SimpleClass").ExecuteUpdateAsync(CancellationToken.None)),
					Is.EqualTo(1));
				await (tx.CommitAsync(CancellationToken.None));
			}
		}
	}
}