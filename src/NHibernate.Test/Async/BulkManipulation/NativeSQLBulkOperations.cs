#if NET_4_5
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.BulkManipulation
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NativeSQLBulkOperations : TestCase
	{
		[Test]
		public async Task SimpleNativeSQLInsertAsync()
		{
			await (PrepareDataAsync());
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			IList l = s.CreateQuery("from Vehicle").List();
			Assert.AreEqual(4, l.Count);
			string ssql = string.Format("insert into VEHICLE (id, TofC, Vin, Owner) select {0}, 22, Vin, Owner from VEHICLE where TofC = 10", GetNewId());
			s.CreateSQLQuery(ssql).ExecuteUpdate();
			l = s.CreateQuery("from Vehicle").List();
			Assert.AreEqual(5, l.Count);
			await (t.CommitAsync());
			t = s.BeginTransaction();
			s.CreateSQLQuery("delete from VEHICLE where TofC = 20").ExecuteUpdate();
			l = s.CreateQuery("from Vehicle").List();
			Assert.AreEqual(4, l.Count);
			Car c = s.CreateQuery("from Car c where c.Owner = 'Kirsten'").UniqueResult<Car>();
			c.Owner = "NotKirsten";
			IQuery sql = s.GetNamedQuery("native-delete-car").SetString(0, "Kirsten");
			Assert.AreEqual(0, sql.ExecuteUpdate());
			sql = s.GetNamedQuery("native-delete-car").SetString(0, "NotKirsten");
			Assert.AreEqual(1, sql.ExecuteUpdate());
			sql = s.CreateSQLQuery("delete from VEHICLE where (TofC = 21) and (Owner = :owner)").SetString("owner", "NotThere");
			Assert.AreEqual(0, sql.ExecuteUpdate());
			sql = s.CreateSQLQuery("delete from VEHICLE where (TofC = 21) and (Owner = :owner)").SetString("owner", "Joe");
			Assert.AreEqual(1, sql.ExecuteUpdate());
			s.CreateSQLQuery("delete from VEHICLE where (TofC = 22)").ExecuteUpdate();
			l = s.CreateQuery("from Vehicle").List();
			Assert.AreEqual(0, l.Count);
			await (t.CommitAsync());
			s.Close();
			await (CleanupDataAsync());
		}

		public async Task PrepareDataAsync()
		{
			ISession s = OpenSession();
			ITransaction txn = s.BeginTransaction();
			Car car = new Car();
			car.Id = GetNewId();
			car.Vin = "123c";
			car.Owner = "Kirsten";
			await (s.SaveAsync(car));
			Truck truck = new Truck();
			truck.Id = GetNewId();
			truck.Vin = "123t";
			truck.Owner = "Steve";
			await (s.SaveAsync(truck));
			SUV suv = new SUV();
			suv.Id = GetNewId();
			suv.Vin = "123s";
			suv.Owner = "Joe";
			await (s.SaveAsync(suv));
			Pickup pickup = new Pickup();
			pickup.Id = GetNewId();
			pickup.Vin = "123p";
			pickup.Owner = "Cecelia";
			await (s.SaveAsync(pickup));
			await (txn.CommitAsync());
			s.Close();
		}

		public async Task CleanupDataAsync()
		{
			ISession s = OpenSession();
			ITransaction txn = s.BeginTransaction();
			await (s.DeleteAsync("from Vehicle"));
			await (txn.CommitAsync());
			s.Close();
		}
	}
}
#endif
