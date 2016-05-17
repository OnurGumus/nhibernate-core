#if NET_4_5
using System;
using System.Diagnostics;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3141
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ProxyIdFixture : BugTestCase
	{
		[Test]
		public async Task ShouldThrowExceptionIfIdChangedOnUnloadEntityAsync()
		{
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var entity = s.Load<Entity>(id);
					entity.Id++;
					Assert.Throws<HibernateException>(async () => await tx.CommitAsync());
				}
		}

		[Test]
		public async Task ShouldThrowExceptionIfIdChangedOnLoadEntityAsync()
		{
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var entity = s.Load<Entity>(id);
					NHibernateUtil.Initialize(entity);
					entity.Id++;
					Assert.Throws<HibernateException>(async () => await tx.CommitAsync());
				}
		}
	}
}
#endif
