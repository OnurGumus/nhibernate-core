#if NET_4_5
using System.Linq;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2923
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ExtraLazyFixture : TestCaseMappingByCode
	{
		[Test]
		public async Task ShouldNotThrowExceptionAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var bob = await (session.GetAsync<Parent>(bobId));
					int ? count = null;
					Assert.DoesNotThrow(() =>
					{
						count = bob.Children.Count;
					}

					);
					Assert.NotNull(count);
					Assert.That(count.Value, Is.EqualTo(0));
				}
		}
	}
}
#endif
