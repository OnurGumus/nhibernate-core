﻿#if NET_4_5
using NHibernate.Criterion;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH247
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "NH247";
			}
		}

		private void AssertDialect()
		{
			if (!(Dialect is FirebirdDialect))
				Assert.Ignore("This test is specific for FirebirdDialect");
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession s = OpenSession())
			{
				await (s.DeleteAsync("from LiteralDescription"));
				await (s.FlushAsync());
			}
		}

		private async Task FillDBAsync()
		{
			LiteralDescription ld1 = new LiteralDescription("DescriptioN 1");
			LiteralDescription ld2 = new LiteralDescription(" Description 2");
			LiteralDescription ld3 = new LiteralDescription(" Description ");
			LiteralDescription ld4 = new LiteralDescription("1234567890");
			LiteralDescription ld5 = new LiteralDescription("    1234567890    ");
			LiteralDescription ld6 = new LiteralDescription("DescRiptioN TheEnd");
			using (ISession s = OpenSession())
			{
				await (s.SaveAsync(ld1));
				await (s.SaveAsync(ld2));
				await (s.SaveAsync(ld3));
				await (s.SaveAsync(ld4));
				await (s.SaveAsync(ld5));
				await (s.SaveAsync(ld6));
				await (s.FlushAsync());
			}
		}

		[Test]
		public async Task CommonLiteralFunctionsAsync()
		{
			AssertDialect();
			await (FillDBAsync());
			using (ISession s = OpenSession())
			{
				IQuery q;
				q = s.CreateQuery("from ld in class LiteralDescription where upper(ld.Description) = 'DESCRIPTION 1'");
				Assert.AreEqual(1, (await (q.ListAsync())).Count);
				q = s.CreateQuery("from ld in class LiteralDescription where lower(ld.Description) = 'description 1'");
				Assert.AreEqual(1, (await (q.ListAsync())).Count);
				q = s.CreateQuery("from ld in class LiteralDescription where trim(ld.Description) = 'Description 2'");
				Assert.AreEqual(1, (await (q.ListAsync())).Count);
				q = s.CreateQuery("from ld in class LiteralDescription where trim(ld.Description) = 'Description'");
				Assert.AreEqual(1, (await (q.ListAsync())).Count);
				q = s.CreateQuery("from ld in class LiteralDescription where trim(upper(ld.Description)) = 'DESCRIPTION'");
				Assert.AreEqual(1, (await (q.ListAsync())).Count);
				q = s.CreateQuery("from ld in class LiteralDescription where lower(trim(ld.Description)) = 'description'");
				Assert.AreEqual(1, (await (q.ListAsync())).Count);
				q = s.CreateQuery("from ld in class LiteralDescription where upper(ld.Description) like 'DESCRIPTION%'");
				Assert.AreEqual(2, (await (q.ListAsync())).Count);
				q = s.CreateQuery("from ld in class LiteralDescription where lower(ld.Description) like 'description%'");
				Assert.AreEqual(2, (await (q.ListAsync())).Count);
			}
		}

		[Test]
		public async Task FirebirdLiteralFunctionsAsync()
		{
			AssertDialect();
			await (FillDBAsync());
			using (ISession s = OpenSession())
			{
				IQuery q;
				q = s.CreateQuery("from ld in class LiteralDescription where char_length(ld.Description) = 10");
				Assert.AreEqual(1, (await (q.ListAsync())).Count);
				q = s.CreateQuery("from ld in class LiteralDescription where char_length(trim(ld.Description)) = 10");
				Assert.AreEqual(2, (await (q.ListAsync())).Count);
			}
		}

		[Test]
		public async Task InsensitiveLikeCriteriaAsync()
		{
			await (FillDBAsync());
			using (ISession s = OpenSession())
			{
				ICriteria c;
				c = s.CreateCriteria(typeof (LiteralDescription));
				c.Add(new InsensitiveLikeExpression("Description", "DeScripTion%"));
				Assert.AreEqual(2, (await (c.ListAsync())).Count);
				c = s.CreateCriteria(typeof (LiteralDescription));
				c.Add(Expression.InsensitiveLike("Description", "DeScripTion", MatchMode.Start));
				Assert.AreEqual(2, (await (c.ListAsync())).Count);
				c = s.CreateCriteria(typeof (LiteralDescription));
				c.Add(Expression.InsensitiveLike("Description", "DeScripTion", MatchMode.Anywhere));
				Assert.AreEqual(4, (await (c.ListAsync())).Count);
				c = s.CreateCriteria(typeof (LiteralDescription));
				c.Add(Expression.InsensitiveLike("Description", "tHeeND", MatchMode.End));
				Assert.AreEqual(1, (await (c.ListAsync())).Count);
				c = s.CreateCriteria(typeof (LiteralDescription));
				c.Add(Expression.InsensitiveLike("Description", "DescRiptioN TheEnd", MatchMode.Exact));
				Assert.AreEqual(1, (await (c.ListAsync())).Count);
			}
		}
	}
}
#endif
