#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.DomainModel;
using NHibernate.DomainModel.NHSpecific;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class OptimisticConcurrencyFixture : TestCase
	{
		// NH-768
		[Test]
		public async Task DeleteOptimisticAsync()
		{
			using (ISession s = OpenSession())
			{
				Optimistic op = new Optimistic();
				op.Bag = new List<string>{"xyz"};
				await (s.SaveAsync(op));
			}

			using (ISession s = OpenSession())
			{
				await (s.DeleteAsync("from Optimistic"));
				await (s.FlushAsync());
			}
		}

		[Test]
		public async Task StaleObjectStateCheckWithNormalizedEntityPersisterAsync()
		{
			Top top = new Top();
			top.Name = "original name";
			try
			{
				using (ISession session = OpenSession())
				{
					await (session.SaveAsync(top));
					await (session.FlushAsync());
					using (ISession concurrentSession = OpenSession())
					{
						Top sameTop = (Top)await (concurrentSession.GetAsync(typeof (Top), top.Id));
						sameTop.Name = "another name";
						await (concurrentSession.FlushAsync());
					}

					top.Name = "new name";
					Assert.Throws<StaleObjectStateException>(() => session.Flush());
				}
			}
			finally
			{
				using (ISession session = OpenSession())
				{
					await (session.DeleteAsync("from Top"));
					await (session.FlushAsync());
				}
			}
		}

		[Test]
		public async Task StaleObjectStateCheckWithEntityPersisterAndOptimisticLockAsync()
		{
			Optimistic optimistic = new Optimistic();
			optimistic.String = "original string";
			try
			{
				using (ISession session = OpenSession())
				{
					await (session.SaveAsync(optimistic));
					await (session.FlushAsync());
					using (ISession concurrentSession = OpenSession())
					{
						Optimistic sameOptimistic = (Optimistic)await (concurrentSession.GetAsync(typeof (Optimistic), optimistic.Id));
						sameOptimistic.String = "another string";
						await (concurrentSession.FlushAsync());
					}

					optimistic.String = "new string";
					Assert.Throws<StaleObjectStateException>(() => session.Flush());
				}
			}
			finally
			{
				using (ISession session = OpenSession())
				{
					await (session.DeleteAsync("from Optimistic"));
					await (session.FlushAsync());
				}
			}
		}
	}
}
#endif
