#if NET_4_5
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.IdTest
{
	[TestFixture]
	[Ignore("Not supported yet")]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class MultipleHiLoPerTableGeneratorTestAsync : TestCaseAsync
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
				return new string[]{"IdTest.Car.hbm.xml", "IdTest.Plane.hbm.xml", "IdTest.Radio.hbm.xml"};
			}
		}

		public async Task DistinctIdAsync()
		{
			ISession s = OpenSession();
			ITransaction tx = s.BeginTransaction();
			const int testLength = 8;
			Car[] cars = new Car[testLength];
			Plane[] planes = new Plane[testLength];
			for (int i = 0; i < 8; i++)
			{
				cars[i] = new Car();
				cars[i].Color = "Color" + i;
				planes[i] = new Plane();
				planes[i].NbrOfSeats = i;
				await (s.PersistAsync(cars[i]));
			}

			await (tx.CommitAsync());
			s.Close();
			for (int i = 0; i < testLength; i++)
			{
				Assert.AreEqual(i + 1, cars[i].Id);
			}

			s = OpenSession();
			tx = s.BeginTransaction();
			await (s.DeleteAsync("from Car"));
			await (tx.CommitAsync());
			s.Close();
		}

		public async Task RollingBackAsync()
		{
			ISession s = OpenSession();
			ITransaction tx = s.BeginTransaction();
			const int testLength = 3;
			long lastId = 0;
			Car car;
			for (int i = 0; i < testLength; i++)
			{
				car = new Car();
				car.Color = "color " + i;
				await (s.SaveAsync(car));
				lastId = car.Id;
			}

			tx.Rollback();
			s.Close();
			s = OpenSession();
			tx = s.BeginTransaction();
			car = new Car();
			car.Color = "blue";
			await (s.SaveAsync(car));
			await (s.FlushAsync());
			await (tx.CommitAsync());
			s.Close();
			Assert.AreEqual(lastId + 1, car.Id, "id generation was rolled back");
			s = OpenSession();
			tx = s.BeginTransaction();
			await (s.DeleteAsync("from Car"));
			await (tx.CommitAsync());
			s.Close();
		}

		public async Task AllParamsAsync()
		{
			ISession s = OpenSession();
			ITransaction tx = s.BeginTransaction();
			Radio radio = new Radio();
			radio.Frequency = "32 MHz";
			await (s.PersistAsync(radio));
			Assert.AreEqual(1, radio.Id);
			radio = new Radio();
			radio.Frequency = "32 MHz";
			await (s.PersistAsync(radio));
			Assert.AreEqual(2, radio.Id);
			await (tx.CommitAsync());
			s.Close();
			s = OpenSession();
			tx = s.BeginTransaction();
			await (s.DeleteAsync("from Radio"));
			await (tx.CommitAsync());
			s.Close();
		}
	}
}
#endif
