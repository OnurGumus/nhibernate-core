#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2976
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		private readonly Guid _employerId = Guid.NewGuid();
		private Guid _employeeId1;
		private Guid _employeeId2;
		protected override async Task OnSetUpAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					var employer = new Employer{Id = _employerId};
					var employee1 = new Employee("Carl", employer);
					var employee2 = new Employee("Philip", employer);
					employer.AddEmployee1(employee1);
					employer.AddEmployee1(employee2);
					_employeeId1 = employee1.Id;
					_employeeId2 = employee2.Id;
					await (session.SaveAsync(employer));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from System.Object"));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}

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
