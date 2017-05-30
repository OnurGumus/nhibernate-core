﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH479
{
	using System.Threading.Tasks;
	using System.Threading;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		public override string BugNumber
		{
			get { return "NH479"; }
		}

		[Test]
		public async Task MergeTestAsync()
		{
			Main main = new Main();
			Aggregate aggregate = new Aggregate();

			main.Aggregate = aggregate;
			aggregate.Main = main;

			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				await (s.SaveAsync(main, CancellationToken.None));
				await (s.SaveAsync(aggregate, CancellationToken.None));
				await (t.CommitAsync(CancellationToken.None));
			}

			using (ISession s = OpenSession())
			{
				using (ITransaction t = s.BeginTransaction())
				{
					s.Merge(main);
					s.Merge(aggregate);
					await (t.CommitAsync(CancellationToken.None));
				}

				using (ITransaction t = s.BeginTransaction())
				{
					await (s.DeleteAsync("from Aggregate", CancellationToken.None));
					await (s.DeleteAsync("from Main", CancellationToken.None));
					await (t.CommitAsync(CancellationToken.None));
				}
			}
		}
	}
}