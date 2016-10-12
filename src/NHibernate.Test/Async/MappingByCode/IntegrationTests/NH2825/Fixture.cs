#if NET_4_5
using System.Collections;
using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.MappingByCode.IntegrationTests.NH2825
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : TestCaseAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new[]{"MappingByCode.IntegrationTests.NH2825.Mappings.hbm.xml"};
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
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					var child1 = new Child{Name = "Child 1"};
					var child2 = new Child{Name = "Child 2"};
					var child3 = new Child{Name = "Child 3"};
					var parent1 = new Parent{Name = "Parent 1", ParentCode = 10};
					var parent2 = new Parent{Name = "Parent 2", ParentCode = 20};
					await (session.SaveAsync(parent1));
					await (session.SaveAsync(parent2));
					parent1.AddChild(child1);
					parent1.AddChild(child2);
					parent2.AddChild(child3);
					await (session.SaveAsync(child1));
					await (session.SaveAsync(child2));
					await (session.SaveAsync(child3));
					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Child"));
					await (session.DeleteAsync("from Parent"));
					await (transaction.CommitAsync());
				}
		}

		[Test]
		public async Task VerifyOneEndOfManyToOneMappingUsingPropertyRefAsync()
		{
			using (ISession session = OpenSession())
				using (session.BeginTransaction())
				{
					var result = await (session.Query<Child>().ToListAsync());
					Assert.That(result.Count, Is.EqualTo(3));
					Assert.That(result.Where(c => c.Name == "Child 1").First().Parent.ParentCode, Is.EqualTo(10));
				}
		}

		[Test]
		public async Task VerifyManyEndOfManyToOneMappingUsingPropertyRefAsync()
		{
			using (ISession session = OpenSession())
				using (session.BeginTransaction())
				{
					var result = await (session.Query<Parent>().ToListAsync());
					Assert.That(result.Count(), Is.EqualTo(2));
					Assert.That(result.First(p => p.ParentCode == 10).Children.Count(), Is.EqualTo(2));
				}
		}
	}
}
#endif
