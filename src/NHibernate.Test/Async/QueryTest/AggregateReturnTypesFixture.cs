#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.QueryTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class AggregateReturnTypesFixtureAsync : TestCaseAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new string[]{"QueryTest.Aggregated.hbm.xml"};
			}
		}

		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		protected override async Task OnSetUpAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					Aggregated agg = new Aggregated();
					agg.AByte = 10;
					agg.AShort = 20;
					agg.AnInt = 30;
					agg.ALong = 40;
					agg.AFloat = 50.5f;
					agg.ADouble = 60.6;
					agg.ADecimal = 70.707m;
					await (s.SaveAsync(agg));
					await (tx.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from Aggregated"));
					await (tx.CommitAsync());
				}
		}

		private async Task<System.Type> AggregateTypeAsync(string expr)
		{
			using (ISession s = OpenSession())
			{
				return (await (s.CreateQuery("select " + expr + " from Aggregated a").UniqueResultAsync())).GetType();
			}
		}

		private async Task CheckTypeAsync(string expr, System.Type type)
		{
			Assert.AreSame(type, await (AggregateTypeAsync(expr)));
		}

		[Test]
		public async Task SumAsync()
		{
			await (CheckTypeAsync("sum(a.AByte)", typeof (UInt64)));
			await (CheckTypeAsync("sum(a.AShort)", typeof (Int64)));
			await (CheckTypeAsync("sum(a.AnInt)", typeof (Int64)));
			await (CheckTypeAsync("sum(a.ALong)", typeof (Int64)));
			await (CheckTypeAsync("sum(a.AFloat)", typeof (Double)));
			await (CheckTypeAsync("sum(a.ADouble)", typeof (Double)));
			await (CheckTypeAsync("sum(a.ADecimal)", typeof (Decimal)));
		}

		[Test]
		public async Task AvgAsync()
		{
			await (CheckTypeAsync("avg(a.AByte)", typeof (Double)));
			await (CheckTypeAsync("avg(a.AShort)", typeof (Double)));
			await (CheckTypeAsync("avg(a.AnInt)", typeof (Double)));
			await (CheckTypeAsync("avg(a.ALong)", typeof (Double)));
			await (CheckTypeAsync("avg(a.AFloat)", typeof (Double)));
			await (CheckTypeAsync("avg(a.ADouble)", typeof (Double)));
			await (CheckTypeAsync("avg(a.ADecimal)", typeof (Double)));
		}

		[Test]
		public async Task MinAsync()
		{
			await (CheckTypeAsync("min(a.AByte)", typeof (Byte)));
			await (CheckTypeAsync("min(a.AShort)", typeof (Int16)));
			await (CheckTypeAsync("min(a.AnInt)", typeof (Int32)));
			await (CheckTypeAsync("min(a.ALong)", typeof (Int64)));
			await (CheckTypeAsync("min(a.AFloat)", typeof (Single)));
			await (CheckTypeAsync("min(a.ADouble)", typeof (Double)));
			await (CheckTypeAsync("min(a.ADecimal)", typeof (Decimal)));
		}

		[Test]
		public async Task MaxAsync()
		{
			await (CheckTypeAsync("max(a.AByte)", typeof (Byte)));
			await (CheckTypeAsync("max(a.AShort)", typeof (Int16)));
			await (CheckTypeAsync("max(a.AnInt)", typeof (Int32)));
			await (CheckTypeAsync("max(a.ALong)", typeof (Int64)));
			await (CheckTypeAsync("max(a.AFloat)", typeof (Single)));
			await (CheckTypeAsync("max(a.ADouble)", typeof (Double)));
			await (CheckTypeAsync("max(a.ADecimal)", typeof (Decimal)));
		}
	}
}
#endif
