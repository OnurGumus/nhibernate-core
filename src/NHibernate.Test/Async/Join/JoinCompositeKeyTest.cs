#if NET_4_5
using log4net;
using NUnit.Framework;
using System;
using System.Collections;
using System.Data;
using System.Threading.Tasks;

namespace NHibernate.Test.Join
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class JoinCompositeKeyTest : TestCase
	{
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
