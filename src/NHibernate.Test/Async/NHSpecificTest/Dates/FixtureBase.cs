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
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class FixtureBaseAsync : TestCaseAsync
	{
		protected abstract override IList Mappings
		{
			get;
		}

		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			var typeNames = (TypeNames)typeof (Dialect.Dialect).GetField("_typeNames", ReflectHelper.AnyVisibilityInstance).GetValue(Dialect);
			try
			{
				var value = AppliesTo();
				if (value == null)
					return true;
				typeNames.Get(value.Value);
			}
			catch (ArgumentException)
			{
				return false;
			}
			catch (Exception)
			{
				Assert.Fail("Probably a bug in the test case.");
			}

			return true;
		}

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

		protected virtual DbType? AppliesTo()
		{
			return null;
		}
	}
}
#endif
