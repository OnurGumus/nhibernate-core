#if NET_4_5
using System;
using NUnit.Framework;
using NHibernate.Test.Immutable.EntityWithMutableCollection;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.Immutable.EntityWithMutableCollection.Inverse
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class VersionedEntityWithInverseOneToManyFailureExpectedTest : AbstractEntityWithOneToManyTest
	{
		[Test]
		[Ignore("known to fail with versioned entity with inverse collection")]
		public override Task AddExistingOneToManyElementToPersistentEntityAsync()
		{
			return TaskHelper.CompletedTask;
		}

		[Test]
		[Ignore("known to fail with versioned entity with inverse collection")]
		public override Task CreateWithEmptyOneToManyCollectionMergeWithExistingElementAsync()
		{
			return TaskHelper.CompletedTask;
		}

		[Test]
		[Ignore("known to fail with versioned entity with inverse collection")]
		public override Task CreateWithEmptyOneToManyCollectionUpdateWithExistingElementAsync()
		{
			return TaskHelper.CompletedTask;
		}

		[Test]
		[Ignore("known to fail with versioned entity with inverse collection")]
		public override Task RemoveOneToManyElementUsingUpdateAsync()
		{
			return TaskHelper.CompletedTask;
		}

		[Test]
		[Ignore("known to fail with versioned entity with inverse collection")]
		public override Task RemoveOneToManyElementUsingMergeAsync()
		{
			return TaskHelper.CompletedTask;
		}
	}
}
#endif
