#if NET_4_5
using System;
using System.Collections.Generic;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1965
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ReattachWithCollectionTest : TestCaseMappingByCode
	{
		[Test]
		public async Task WhenReattachThenNotThrowsAsync()
		{
			var cat = new Cat();
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					await (session.SaveAsync(cat));
					await (session.Transaction.CommitAsync());
				}

			using (var session = OpenSession())
			{
				Assert.That(() => session.Lock(cat, LockMode.None), Throws.Nothing);
			}

			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					await (session.DeleteAsync(cat));
					await (session.Transaction.CommitAsync());
				}
		}
	}
}
#endif
