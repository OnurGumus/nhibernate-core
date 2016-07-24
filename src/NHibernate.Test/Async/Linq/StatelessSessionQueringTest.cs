#if NET_4_5
using System.Linq;
using System.Text;
using NHibernate.DomainModel.Northwind.Entities;
using NUnit.Framework;
using NHibernate.Linq;
using System.Threading.Tasks;

namespace NHibernate.Test.Linq
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class StatelessSessionQueringTestAsync : LinqTestCaseAsync
	{
		[Test]
		public void WhenQueryThroughStatelessSessionThenDoesNotThrows()
		{
			using (var statelessSession = Sfi.OpenStatelessSession())
			{
				var query = statelessSession.Query<Customer>();
				Assert.That(() => query.ToList(), Throws.Nothing);
			}
		}

		[Test]
		public void AggregateWithStartsWith()
		{
			using (IStatelessSession statelessSession = Sfi.OpenStatelessSession())
			{
				StringBuilder query = (
					from c in statelessSession.Query<Customer>()where c.CustomerId.StartsWith("A")select c.CustomerId).Aggregate(new StringBuilder(), (sb, id) => sb.Append(id).Append(","));
				Assert.That(query.ToString(), Is.EqualTo("ALFKI,ANATR,ANTON,AROUT,"));
			}
		}
	}
}
#endif
