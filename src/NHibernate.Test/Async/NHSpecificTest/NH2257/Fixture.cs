﻿#if NET_4_5
using System.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2257
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override bool AppliesTo(NHibernate.Dialect.Dialect dialect)
		{
			return (dialect is NHibernate.Dialect.InformixDialect1000);
		}

		[Test]
		public async Task InformixUsingDuplicateParametersAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (session.SaveAsync(new Foo()
					{Name = "aa"}));
					var list = await (session.CreateQuery("from Foo f where f.Name = :p1 and not f.Name <> :p1").SetParameter("p1", "aa").ListAsync<Foo>());
					Assert.That(list.Count, Is.EqualTo(1));
				}
		}
	}
}
#endif
