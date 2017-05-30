﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System.Collections.Generic;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.BagWithLazyExtraAndFilter
{
	using System.Threading.Tasks;
	using System.Threading;
	[TestFixture]
	public class FixtureAsync: BugTestCase
	{
		[Test]
		public async Task CanUseFilterForLazyExtraAsync()
		{
			using (var s = OpenSession())
			{
				s.BeginTransaction();
				var machineRequest = new MachineRequest { EnvId = 1L, Id = 2L };
				await (s.SaveAsync(new Env
				{
					Id = 1L,
					RequestsFailed = new List<MachineRequest>
					{
						machineRequest
					}
				}, CancellationToken.None));
				await (s.SaveAsync(machineRequest, CancellationToken.None));
				await (s.Transaction.CommitAsync(CancellationToken.None));
			}

			using (var s = OpenSession())
			{
				var env = await (s.LoadAsync<Env>(1L, CancellationToken.None));
				Assert.AreEqual(1, env.RequestsFailed.Count);
			}

			using (var s = OpenSession())
			{
				s.EnableFilter("CurrentOnly");
				var env = await (s.LoadAsync<Env>(1L, CancellationToken.None));
				Assert.AreEqual(0, env.RequestsFailed.Count);
			}

			using (var s = OpenSession())
			{
				s.BeginTransaction();
				await (s.DeleteAsync(await (s.LoadAsync<MachineRequest>(2L, CancellationToken.None)), CancellationToken.None));
				await (s.DeleteAsync(await (s.LoadAsync<Env>(1L, CancellationToken.None)), CancellationToken.None));
				await (s.Transaction.CommitAsync(CancellationToken.None));
			}
		}
	}
}
