#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1044
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task CrudAsync()
		{
			// Only as a quick check that is can work with the idbag inside the component
			var p = new Person{Name = "Fiamma", Delivery = new Delivery()};
			p.Delivery.Adresses.Add("via Parenzo 96");
			p.Delivery.Adresses.Add("viale Don Bosco 192");
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.SaveAsync(p));
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					var pp = await (s.GetAsync<Person>(p.Id));
					pp.Delivery.Adresses.RemoveAt(0);
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					var pp = await (s.GetAsync<Person>(p.Id));
					Assert.That(pp.Delivery.Adresses.Count, Is.EqualTo(1));
					await (s.DeleteAsync(pp));
					await (t.CommitAsync());
				}
		}
	}
}
#endif
