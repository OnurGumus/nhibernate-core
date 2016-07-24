#if NET_4_5
using System;
using NUnit.Framework;
using NHibernate.Test.Immutable.EntityWithMutableCollection;
using System.Threading.Tasks;

namespace NHibernate.Test.Immutable.EntityWithMutableCollection.NonInverse
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class EntityWithNonInverseOneToManyUnidirTestAsync : AbstractEntityWithOneToManyTestAsync
	{
		protected override System.Collections.IList Mappings
		{
			get
			{
				return new string[]{"Immutable.EntityWithMutableCollection.NonInverse.ContractVariationUnidir.hbm.xml"};
			}
		}
	}
}
#endif
