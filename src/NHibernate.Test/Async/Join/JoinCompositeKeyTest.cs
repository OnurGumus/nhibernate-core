#if NET_4_5
using log4net;
using NUnit.Framework;
using System;
using System.Collections;
using System.Data;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.Join
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class JoinCompositeKeyTestAsync : TestCaseAsync
	{
		private static ILog log = LogManager.GetLogger(typeof (JoinCompositeKeyTestAsync));
		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		protected override IList Mappings
		{
			get
			{
				return new string[]{"Join.CompositeKey.hbm.xml"};
			}
		}

		ISession s;
		protected override Task OnSetUpAsync()
		{
			try
			{
				s = OpenSession();
				objectsNeedDeleting.Clear();
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		protected override async Task OnTearDownAsync()
		{
			await (s.FlushAsync());
			s.Clear();
			try
			{
				// Delete the objects in reverse order because it is
				// less likely to violate foreign key constraints.
				for (int i = objectsNeedDeleting.Count - 1; i >= 0; i--)
				{
					await (s.DeleteAsync(objectsNeedDeleting[i]));
				}

				await (s.FlushAsync());
			}
			finally
			{
				//t.Commit();
				s.Close();
			}

			s = null;
		}

		private IList objectsNeedDeleting = new ArrayList();
		[Test]
		public async Task SimpleSaveAndRetrieveAsync()
		{
			EmployeeWithCompositeKey emp = new EmployeeWithCompositeKey(1, 100);
			emp.StartDate = DateTime.Today;
			emp.FirstName = "Karl";
			emp.Surname = "Chu";
			emp.OtherNames = "The Yellow Dart";
			emp.Title = "Rock Star";
			objectsNeedDeleting.Add(emp);
			await (s.SaveAsync(emp));
			await (s.FlushAsync());
			s.Clear();
			EmployeePk pk = new EmployeePk(1, 100);
			EmployeeWithCompositeKey retrieved = await (s.GetAsync<EmployeeWithCompositeKey>(pk));
			Assert.IsNotNull(retrieved);
			Assert.AreEqual(emp.StartDate, retrieved.StartDate);
			Assert.AreEqual(emp.FirstName, retrieved.FirstName);
			Assert.AreEqual(emp.Surname, retrieved.Surname);
			Assert.AreEqual(emp.OtherNames, retrieved.OtherNames);
			Assert.AreEqual(emp.Title, retrieved.Title);
		}
	}
}
#endif
