#if NET_4_5
using System;
using System.Collections;
using System.Data;
using NHibernate.Dialect;
using NHibernate.Util;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.Dates
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class FixtureBase : TestCase
	{
		protected async Task SavingAndRetrievingActionAsync(AllDates entity, Action<AllDates> action)
		{
			AllDates dates = entity;
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.SaveAsync(dates));
					await (tx.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					var datesRecovered = await (s.CreateQuery("from AllDates").UniqueResultAsync<AllDates>());
					action.Invoke(datesRecovered);
				}

			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					var datesRecovered = await (s.CreateQuery("from AllDates").UniqueResultAsync<AllDates>());
					await (s.DeleteAsync(datesRecovered));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
