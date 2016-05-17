#if NET_4_5
using System.Linq;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2772
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : TestCaseMappingByCode
	{
		[Test]
		public async Task Lazy_Collection_Is_Not_LoadedAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					var trip = await (s.GetAsync<Trip>(1));
					Assert.That(trip.Trackpoints.Count(), Is.EqualTo(3));
				}
		}
	}
}
#endif
