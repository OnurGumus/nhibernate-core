#if NET_4_5
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.BulkManipulation
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NativeSQLBulkOperationsAsync : TestCaseAsync
	{
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
				return new string[]{"BulkManipulation.Vehicle.hbm.xml"};
			}
		}

		[Test]
		public async Task SimpleNativeSQLInsertAsync()
		{
			await (PrepareDataAsync());
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			IList l = await (s.CreateQuery("from Vehicle").ListAsync());
			Assert.AreEqual(4, l.Count);
			string ssql = string.Format("insert into VEHICLE (id, TofC, Vin, Owner) select {0}, 22, Vin, Owner from VEHICLE where TofC = 10", GetNewId());
			await (s.CreateSQLQuery(ssql).ExecuteUpdateAsync());
			l = await (s.CreateQuery("from Vehicle").ListAsync());
			Assert.AreEqual(5, l.Count);
			await (t.CommitAsync());
			t = s.BeginTransaction();
			await (s.CreateSQLQuery("delete from VEHICLE where TofC = 20").ExecuteUpdateAsync());
			l = await (s.CreateQuery("from Vehicle").ListAsync());
			Assert.AreEqual(4, l.Count);
			Car c = await (s.CreateQuery("from Car c where c.Owner = 'Kirsten'").UniqueResultAsync<Car>());
			c.Owner = "NotKirsten";
			IQuery sql = s.GetNamedQuery("native-delete-car").SetString(0, "Kirsten");
			Assert.AreEqual(0, await (sql.ExecuteUpdateAsync()));
			sql = s.GetNamedQuery("native-delete-car").SetString(0, "NotKirsten");
			Assert.AreEqual(1, await (sql.ExecuteUpdateAsync()));
			sql = s.CreateSQLQuery("delete from VEHICLE where (TofC = 21) and (Owner = :owner)").SetString("owner", "NotThere");
			Assert.AreEqual(0, await (sql.ExecuteUpdateAsync()));
			sql = s.CreateSQLQuery("delete from VEHICLE where (TofC = 21) and (Owner = :owner)").SetString("owner", "Joe");
			Assert.AreEqual(1, await (sql.ExecuteUpdateAsync()));
			await (s.CreateSQLQuery("delete from VEHICLE where (TofC = 22)").ExecuteUpdateAsync());
			l = await (s.CreateQuery("from Vehicle").ListAsync());
			Assert.AreEqual(0, l.Count);
			await (t.CommitAsync());
			s.Close();
			await (CleanupDataAsync());
		}

		private int customId = 0;
		private int GetNewId()
		{
			return ++customId;
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
