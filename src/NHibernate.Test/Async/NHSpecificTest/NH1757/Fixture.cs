#if NET_4_5
using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1757
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task MayBeABugAsync()
		{
			using (ISession s = OpenSession())
			{
				var query = s.CreateSQLQuery("SELECT SimpleEntity.*, 123 as field_not_in_entitytype FROM SimpleEntity").AddEntity(typeof (SimpleEntity)).AddScalar("field_not_in_entitytype", NHibernateUtil.Int64);
				IList<Object[]> result = await (query.ListAsync<Object[]>());
			}
		}
	}
}
#endif
