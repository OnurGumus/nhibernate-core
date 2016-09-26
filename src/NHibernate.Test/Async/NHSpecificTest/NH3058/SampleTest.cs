#if NET_4_5
using NHibernate.Context;
using NHibernate.Engine;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3058
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SampleTestAsync : BugTestCaseAsync
	{
		public static ISessionFactoryImplementor AmbientSfi
		{
			get;
			private set;
		}

		protected override void BuildSessionFactory()
		{
			base.BuildSessionFactory();
			AmbientSfi = Sfi;
		}

		protected override void Configure(Cfg.Configuration configuration)
		{
			base.Configure(configuration);
			configuration.Properties.Add("current_session_context_class", "thread_static");
		}

		protected override ISession OpenSession()
		{
			var session = base.OpenSession();
			CurrentSessionContext.Bind(session);
			return session;
		}

		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var book = new DomainClass{Name = "Some name", ALotOfText = "Some text", Id = 1};
					await (s.PersistAsync(book));
					await (tx.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					Assert.That(await (s.CreateSQLQuery("delete from DomainClass").ExecuteUpdateAsync()), Is.EqualTo(1));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task MethodShouldLoadLazyPropertyAsync()
		{
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var book = await (s.LoadAsync<DomainClass>(1));
					Assert.False(await (NHibernateUtil.IsPropertyInitializedAsync(book, "ALotOfText")));
					string value = book.LoadLazyProperty();
					Assert.That(value, Is.EqualTo("Some text"));
					Assert.That(await (NHibernateUtil.IsPropertyInitializedAsync(book, "ALotOfText")), Is.True);
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
