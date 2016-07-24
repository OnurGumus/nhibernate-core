#if NET_4_5
using System;
using System.Collections;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.GeneratedTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class TimestampGeneratedValuesWithCachingTestAsync : AbstractGeneratedPropertyTestAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new string[]{"GeneratedTest.MSSQLGeneratedPropertyEntity.hbm.xml"};
			}
		}

		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			// this test is specific to SQL Server as it is testing support
			// for its TIMESTAMP datatype...
			return dialect is MsSql2000Dialect;
		}
	}
}
#endif
