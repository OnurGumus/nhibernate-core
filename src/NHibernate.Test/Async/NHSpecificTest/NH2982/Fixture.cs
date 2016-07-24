#if NET_4_5
using System;
using NUnit.Framework;
using NHibernate.Criterion;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2982
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					var e1 = new Entity{Id = 1, Name = "A"};
					await (session.SaveAsync(e1));
					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from System.Object"));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}

			await (base.OnTearDownAsync());
		}

		[Test]
		public async Task SimpleExpressionWithProxyAsync()
		{
			using (ISession session = OpenSession())
				using (session.BeginTransaction())
				{
					var a = await (session.LoadAsync<Entity>(1));
					var restriction = Restrictions.Eq("A", a);
					Assert.AreEqual("A = Entity#1", restriction.ToString());
				}
		}

		[Test]
		public void SimpleExpressionWithNewInstance()
		{
			var a = new Entity()
			{Id = 2, Name = "2"};
			var restriction = Restrictions.Eq("A", a);
			Assert.AreEqual(@"A = Entity@" + a.GetHashCode() + "(hash)", restriction.ToString());
		}

		[Test]
		public void SimpleExpressionWithNull()
		{
			var restriction = Restrictions.Eq("A", null);
			Assert.AreEqual("A = null", restriction.ToString());
		}

		[Test]
		public void SimpleExpressionWithPrimitive()
		{
			var restriction = Restrictions.Eq("A", 5);
			Assert.AreEqual("A = 5", restriction.ToString());
		}

		[Test]
		public void SimpleExpressionWithNullablePrimitive()
		{
			int ? value = null;
			value = 5;
			var restriction = Restrictions.Eq("A", value);
			Assert.AreEqual("A = 5", restriction.ToString());
		}

		[Test]
		public void SimpleExpressionWithString()
		{
			var restriction = Restrictions.Like("A", "Test");
			Assert.AreEqual("A like Test", restriction.ToString());
		}

		[Test]
		public void SimpleExpressionWithNullableDate()
		{
			DateTime? date = new DateTime(2012, 1, 1);
			var restriction = Restrictions.Eq("A", date);
			Assert.AreEqual("A = " + date, restriction.ToString());
		}
	}
}
#endif
