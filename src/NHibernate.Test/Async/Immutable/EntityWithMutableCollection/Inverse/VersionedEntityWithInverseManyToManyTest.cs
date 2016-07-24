#if NET_4_5
using System;
using NUnit.Framework;
using NHibernate.Test.Immutable.EntityWithMutableCollection;
using System.Threading.Tasks;

namespace NHibernate.Test.Immutable.EntityWithMutableCollection.Inverse
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class VersionedEntityWithInverseManyToManyTestAsync : AbstractEntityWithManyToManyTestAsync
	{
		protected override System.Collections.IList Mappings
		{
			get
			{
				return new string[]{"Immutable.EntityWithMutableCollection.Inverse.ContractVariationVersioned.hbm.xml"};
			}
		}
	}
}
#endif
