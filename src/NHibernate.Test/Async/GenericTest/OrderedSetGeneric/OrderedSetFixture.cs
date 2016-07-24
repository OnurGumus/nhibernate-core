#if NET_4_5
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.GenericTest.OrderedSetGeneric
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class OrderedSetFixtureAsync : TestCaseAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new[]{"GenericTest.OrderedSetGeneric.OrderedSetFixture.hbm.xml"};
			}
		}

		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from B"));
					await (s.DeleteAsync("from A"));
					await (tx.CommitAsync());
				}
			}
		}

		[Test]
		public async Task OrderedSetIsInOrderAsync()
		{
			var names = new[]{"First B", "Second B"};
			const int TheId = 100;
			var a = new A{Name = "First", Id = TheId};
			var b = new B{Name = names[1], OrderBy = 3, AId = TheId};
			a.Items.Add(b);
			var b2 = new B{Name = names[0], OrderBy = 1, AId = TheId};
			a.Items.Add(b2);
			ISession s = OpenSession();
			await (s.SaveAsync(a));
			await (s.FlushAsync());
			s.Close();
			s = OpenSession();
			var newA = await (s.GetAsync<A>(a.Id));
			Assert.AreEqual(2, newA.Items.Count);
			int counter = 0;
			foreach (B item in newA.Items)
			{
				Assert.AreEqual(names[counter], item.Name);
				counter++;
			}

			s.Close();
		}
	}
}
#endif
