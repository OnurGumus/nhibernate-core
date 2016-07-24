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
	public partial class TriggerGeneratedValuesWithCachingTestAsync : AbstractGeneratedPropertyTestAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new string[]{"GeneratedTest.GeneratedPropertyEntity.hbm.xml"};
			}
		}

		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect is MsSql2000Dialect || dialect is Oracle8iDialect;
		}
	}
}
#endif
