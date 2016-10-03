#if NET_4_5
using System.Linq;
using NHibernate.Driver;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH2852
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override bool AppliesTo(Engine.ISessionFactoryImplementor factory)
		{
			return !(factory.ConnectionProvider.Driver is OracleManagedDataClientDriver);
		}

		protected override async Task OnSetUpAsync()
		{
			using (var session = OpenSession())
				using (var transction = session.BeginTransaction())
				{
					var city = new City{Name = "London"};
					await (session.SaveAsync(city));
					var address = new Address{City = city, Name = "Tower"};
					await (session.SaveAsync(address));
					var person = new Person{Address = address, Name = "Bill"};
					await (session.SaveAsync(person));
					var child = new Person{Parent = person};
					await (session.SaveAsync(child));
					var grandChild = new Person{Parent = child};
					await (session.SaveAsync(grandChild));
					await (transction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = OpenSession())
				using (var transction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from System.Object"));
					await (transction.CommitAsync());
				}
		}
	}
}
#endif
