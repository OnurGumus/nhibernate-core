#if NET_4_5
using System;
using System.Linq;
using NHibernate.Linq;
using NHibernate.Collection;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3480
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "NH3480";
			}
		}

		protected override async Task OnSetUpAsync()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction transaction = session.BeginTransaction())
				{
					var parent1 = new Entity{Id = new Entity.Key()
					{Id = Guid.NewGuid()}, Name = "Bob", OtherId = 20};
					await (session.SaveAsync(parent1));
					var child1 = new Child{Name = "Bob1", Parent = parent1};
					await (session.SaveAsync(child1));
					var child2 = new Child{Name = "Bob2", Parent = parent1};
					await (session.SaveAsync(child2));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
			}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from System.Object"));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
			}
		}

		[Test]
		public async Task TestAsync()
		{
			using (ISession session = OpenSession())
			{
				using (session.BeginTransaction())
				{
					var result =
						from e in session.Query<Entity>()where e.Name == "Bob"
						select e;
					var entity = result.Single();
					await (NHibernateUtil.InitializeAsync(entity.Children));
				}
			}
		}
	}
}
#endif
