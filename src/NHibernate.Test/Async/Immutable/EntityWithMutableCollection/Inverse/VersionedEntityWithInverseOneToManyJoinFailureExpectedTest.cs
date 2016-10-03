#if NET_4_5
using System;
using NUnit.Framework;
using NHibernate.Test.Immutable.EntityWithMutableCollection;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.Immutable.EntityWithMutableCollection.Inverse
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class VersionedEntityWithInverseOneToManyJoinFailureExpectedTestAsync : AbstractEntityWithOneToManyTestAsync
	{
		protected override System.Collections.IList Mappings
		{
			get
			{
				return new string[]{"Immutable.EntityWithMutableCollection.Inverse.ContractVariationVersionedOneToManyJoin.hbm.xml"};
			}
		}

		[Test]
		[Ignore("known to fail with inverse collection")]
		// Also [Ignore("Fails. Passes in Hibernate because nullability check on Contract.Party (with JOIN mapping) is skipped due to 'check_nullability' setting not implemented by NH.")]
		public override Task AddExistingOneToManyElementToPersistentEntityAsync()
		{
			return TaskHelper.CompletedTask;
		}

		[Test]
		[Ignore("known to fail with inverse collection")]
		// Also [Ignore("Fails. Passes in Hibernate because nullability check on Contract.Party (with JOIN mapping) is skipped due to 'check_nullability' setting not implemented by NH.")]
		public override Task CreateWithEmptyOneToManyCollectionUpdateWithExistingElementAsync()
		{
			return TaskHelper.CompletedTask;
		}

		[Test]
		[Ignore("known to fail with inverse collection")]
		// Also [Ignore("Fails. Passes in Hibernate because nullability check on Contract.Party (with JOIN mapping) is skipped due to 'check_nullability' setting not implemented by NH.")]
		public override Task CreateWithEmptyOneToManyCollectionMergeWithExistingElementAsync()
		{
			return TaskHelper.CompletedTask;
		}

		[Test]
		[Ignore("known to fail with inverse collection")]
		public override Task RemoveOneToManyElementUsingUpdateAsync()
		{
			return TaskHelper.CompletedTask;
		}

		[Test]
		[Ignore("known to fail with inverse collection")]
		public override Task RemoveOneToManyElementUsingMergeAsync()
		{
			return TaskHelper.CompletedTask;
		}

		[Test]
		[Ignore("Fails. Passes in Hibernate because nullability check on Contract.Party (with JOIN mapping) is skipped due to 'check_nullability' setting not implemented by NH.")]
		public override Task CreateWithNonEmptyOneToManyCollectionOfExistingAsync()
		{
			return TaskHelper.CompletedTask;
		}

		[Test]
		[Ignore("Fails. Passes in Hibernate because nullability check on Contract.Party (with JOIN mapping) is skipped due to 'check_nullability' setting not implemented by NH.")]
		public override Task DeleteOneToManyElementAsync()
		{
			return TaskHelper.CompletedTask;
		}

		[Test]
		[Ignore("Fails. Passes in Hibernate because nullability check on Contract.Party (with JOIN mapping) is skipped due to 'check_nullability' setting not implemented by NH.")]
		public override Task RemoveOneToManyElementByDeleteAsync()
		{
			return TaskHelper.CompletedTask;
		}
	}
}
#endif
