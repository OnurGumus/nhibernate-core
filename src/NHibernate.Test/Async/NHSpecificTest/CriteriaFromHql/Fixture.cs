#if NET_4_5
using System.Diagnostics;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.CriteriaFromHql
{
	using System.Collections;

	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : TestCaseAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new string[]{"NHSpecificTest.CriteriaFromHql.Mappings.hbm.xml"};
			}
		}

		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		[Test]
		public async Task UsingCriteriaAndHqlAsync()
		{
			await (CreateDataAsync());
			using (SqlLogSpy spy = new SqlLogSpy())
				using (ISession session = sessions.OpenSession())
					using (ITransaction tx = session.BeginTransaction())
					{
						Person result = await (session.CreateQuery(@"
select p from Person p 
join fetch p.Children c 
join fetch c.Children gc
where p.Parent is null").UniqueResultAsync<Person>());
						string hqlQuery = spy.Appender.GetEvents()[0].MessageObject.ToString();
						Debug.WriteLine("HQL: " + hqlQuery);
						Assertions(result);
					}

			using (SqlLogSpy spy = new SqlLogSpy())
				using (ISession session = sessions.OpenSession())
					using (ITransaction tx = session.BeginTransaction())
					{
						Person result = await (session.CreateCriteria(typeof (Person)).Add(Restrictions.IsNull("Parent")).SetFetchMode("Children", FetchMode.Join).SetFetchMode("Children.Children", FetchMode.Join).UniqueResultAsync<Person>());
						string criteriaQuery = spy.Appender.GetEvents()[0].MessageObject.ToString();
						Debug.WriteLine("Criteria: " + criteriaQuery);
						Assertions(result);
					}

			await (DeleteDataAsync());
		}

		private async Task DeleteDataAsync()
		{
			using (ISession session = sessions.OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Person"));
					await (tx.CommitAsync());
				}
		}

		private async Task CreateDataAsync()
		{
			using (ISession session = sessions.OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					Person root = new Person();
					await (session.SaveAsync(root));
					for (int i = 0; i < 2; i++)
					{
						Person child = new Person();
						root.Children.Add(child);
						child.Parent = root;
						await (session.SaveAsync(child));
						for (int j = 0; j < 3; j++)
						{
							Person child2 = new Person();
							child2.Parent = child;
							child.Children.Add(child2);
							await (session.SaveAsync(child2));
						}
					}

					await (tx.CommitAsync());
				}
		}

		private static void Assertions(Person p)
		{
			Assert.IsTrue(NHibernateUtil.IsInitialized(p.Children));
			Assert.AreEqual(2, p.Children.Count);
			foreach (Person child in p.Children)
			{
				Assert.IsTrue(NHibernateUtil.IsInitialized(child));
				Assert.IsTrue(NHibernateUtil.IsInitialized(child.Children));
				Assert.AreEqual(3, child.Children.Count);
			}
		}
	}
}
#endif
