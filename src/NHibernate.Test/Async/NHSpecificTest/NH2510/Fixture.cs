#if NET_4_5
using System;
using NHibernate.Cache;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2510
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : TestCaseMappingByCode
	{
		[Test]
		public async Task WhenReadFromCacheThenDoesNotThrowAsync()
		{
			using (new Scenario(Sfi))
			{
				using (ISession s = OpenSession())
				{
					var book = await (s.GetAsync<Image>(1));
				}

				using (ISession s = OpenSession())
				{
					var book = await (s.GetAsync<Image>(1));
				}
			}
		}
	}
}
#endif
