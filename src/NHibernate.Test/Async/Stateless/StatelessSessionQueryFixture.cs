#if NET_4_5
using System.Collections;
using NHibernate.Cfg;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Stateless
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class StatelessSessionQueryFixtureAsync : TestCaseAsync
	{
		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		protected override IList Mappings
		{
			get
			{
				return new[]{"Stateless.Contact.hbm.xml"};
			}
		}

		protected override void Configure(Configuration configuration)
		{
			base.Configure(configuration);
			cfg.SetProperty(Environment.MaxFetchDepth, 1.ToString());
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private partial class TestData
		{
			internal readonly IList list = new ArrayList();
			private readonly ISessionFactory sessions;
			public TestData(ISessionFactory sessions)
			{
				this.sessions = sessions;
			}

			public virtual async Task createDataAsync()
			{
				using (ISession session = sessions.OpenSession())
				{
					using (ITransaction tx = session.BeginTransaction())
					{
						var usa = new Country();
						await (session.SaveAsync(usa));
						list.Add(usa);
						var disney = new Org();
						disney.Country = usa;
						await (session.SaveAsync(disney));
						list.Add(disney);
						var waltDisney = new Contact();
						waltDisney.Org = disney;
						await (session.SaveAsync(waltDisney));
						list.Add(waltDisney);
						await (tx.CommitAsync());
					}
				}
			}

			public virtual async Task cleanDataAsync()
			{
				using (ISession session = sessions.OpenSession())
				{
					using (ITransaction tx = session.BeginTransaction())
					{
						foreach (object obj in list)
						{
							await (session.DeleteAsync(obj));
						}

						await (tx.CommitAsync());
					}
				}
			}
		}

		[Test]
		public async Task CriteriaAsync()
		{
			var testData = new TestData(sessions);
			await (testData.createDataAsync());
			using (IStatelessSession s = sessions.OpenStatelessSession())
			{
				Assert.AreEqual(1, (await (s.CreateCriteria<Contact>().ListAsync())).Count);
			}

			await (testData.cleanDataAsync());
		}

		[Test]
		public async Task CriteriaWithSelectFetchModeAsync()
		{
			var testData = new TestData(sessions);
			await (testData.createDataAsync());
			using (IStatelessSession s = sessions.OpenStatelessSession())
			{
				Assert.AreEqual(1, (await (s.CreateCriteria<Contact>().SetFetchMode("Org", FetchMode.Select).ListAsync())).Count);
			}

			await (testData.cleanDataAsync());
		}

		[Test]
		public async Task HqlAsync()
		{
			var testData = new TestData(sessions);
			await (testData.createDataAsync());
			using (IStatelessSession s = sessions.OpenStatelessSession())
			{
				Assert.AreEqual(1, (await (s.CreateQuery("from Contact c join fetch c.Org join fetch c.Org.Country").ListAsync<Contact>())).Count);
			}

			await (testData.cleanDataAsync());
		}
	}
}
#endif
