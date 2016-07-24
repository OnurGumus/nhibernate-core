#if NET_4_5
using System;
using NUnit.Framework;
using NHibernate.Test.Immutable.EntityWithMutableCollection;
using System.Threading.Tasks;

namespace NHibernate.Test.Immutable.EntityWithMutableCollection.Inverse
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class EntityWithInverseManyToManyTestAsync : AbstractEntityWithManyToManyTestAsync
	{
		protected override System.Collections.IList Mappings
		{
			get
			{
				return new string[]{"Immutable.EntityWithMutableCollection.Inverse.ContractVariation.hbm.xml"};
			}
		}
	}
}
#endif
