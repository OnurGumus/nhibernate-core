#if NET_4_5
using System.Collections;
using System.Xml;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.EntityModeTest.Xml.Many2One
{
	[TestFixture, Ignore("Not supported yet.")]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class XmlManyToOneFixtureAsync : TestCaseAsync
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
				return new[]{"EntityModeTest.Xml.Many2One.Car.hbm.xml"};
			}
		}

		[Test]
		public async Task XmlManyToOneAsync()
		{
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			var carType = new CarType{TypeName = "Type 1"};
			await (s.SaveAsync(carType));
			var car1 = new Car{CarType = carType, Model = "Model 1"};
			await (s.SaveAsync(car1));
			var car2 = new Car{CarType = carType, Model = "Model 2"};
			await (s.SaveAsync(car2));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			ISession xmlSession = s.GetSession(EntityMode.Xml);
			t = s.BeginTransaction();
			IList list = await (xmlSession.CreateQuery("from Car c join fetch c.carType order by c.model asc").ListAsync());
			var expectedResults = new[]{"<car id=\"" + car1.Id + "\"><model>Model 1</model><carType id=\"" + carType.Id + "\"><typeName>Type 1</typeName></carType></car>", "<car id=\"" + car2.Id + "\"><model>Model 2</model><carType id=\"" + carType.Id + "\"><typeName>Type 1</typeName></carType></car>"};
			for (int i = 0; i < list.Count; i++)
			{
				var element = (XmlElement)list[i];
				//print(element);
				Assert.That(element.InnerXml.Equals(expectedResults[i]));
			}

			await (s.DeleteAsync("from CarType"));
			await (s.DeleteAsync("from Car"));
			await (t.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task XmlOneToManyAsync()
		{
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			var carType = new CarType{TypeName = "Type 1"};
			await (s.SaveAsync(carType));
			var car = new Car{CarType = carType, Model = "Model 1"};
			await (s.SaveAsync(car));
			var carPart1 = new CarPart{PartName = "chassis"};
			car.CarParts.Add(carPart1);
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			ISession xmlSession = s.GetSession(EntityMode.Xml);
			t = s.BeginTransaction();
			var element = (XmlElement)await (xmlSession.CreateQuery("from Car c join fetch c.carParts").UniqueResultAsync());
			string expectedResult = "<car id=\"" + car.Id + "\"><carPart>" + carPart1.Id + "</carPart><model>Model 1</model><carType id=\"" + carType.Id + "\"><typeName>Type 1</typeName></carType></car>";
			//print(element);
			Assert.That(element.InnerXml.Equals(expectedResult));
			await (s.DeleteAsync("from CarPart"));
			await (s.DeleteAsync("from CarType"));
			await (s.DeleteAsync("from Car"));
			await (t.CommitAsync());
			s.Close();
		}
	}
}
#endif
