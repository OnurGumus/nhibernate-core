#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2976
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task ShouldRemoveItemFromUninitializedGenericDictionaryAsync()
		{
			using (ISession session = OpenSession())
				using (session.BeginTransaction())
				{
					var employer = await (session.GetAsync<Employer>(_employerId));
					Assert.IsFalse(NHibernateUtil.IsInitialized(employer.Employees1)); // Just make sure the dictionary really is not initialized
					// First call to PersistentGenericMap.Remove will initialize the dictionary and then enqueue a delayed operation.
					// The item will not be removed from the dictionary. Enqueued operation will never be executed since
					// AbstractPersistentCollection.PerformQueuedOperations is executed on AfterInitialize - and dictionary was already
					// initialized before the operation was enqueued
					employer.Employees1.Remove(_employeeId1);
					employer.Employees1.Remove(_employeeId2); // The item will be removed as normal
					Assert.That(employer.Employees1.Values.Count, Is.EqualTo(0));
				}
		}
	}
}
#endif
