#if NET_4_5
using System;
using System.Data;
using System.Data.Common;
using NHibernate.Criterion;
using NHibernate.Type;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.TypesTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class GuidTypeFixtureAsync : TypeFixtureBaseAsync
	{
		protected override string TypeName
		{
			get
			{
				return "Guid";
			}
		}

		[Test]
		public async Task ReadWriteAsync()
		{
			Guid val = new Guid("{01234567-abcd-abcd-abcd-0123456789ab}");
			GuidClass basic = new GuidClass();
			basic.Id = 1;
			basic.GuidValue = val;
			ISession s = OpenSession();
			await (s.SaveAsync(basic));
			await (s.FlushAsync());
			s.Close();
			s = OpenSession();
			basic = (GuidClass)await (s.LoadAsync(typeof (GuidClass), 1));
			Assert.AreEqual(val, basic.GuidValue);
			await (s.DeleteAsync(basic));
			await (s.FlushAsync());
			s.Close();
		}

		[Test]
		public async Task GuidInWhereClauseAsync()
		{
			Guid val = new Guid("{01234567-abcd-abcd-abcd-0123456789ab}");
			GuidClass basic = new GuidClass();
			using (ISession s = OpenSession())
			{
				basic.Id = 1;
				basic.GuidValue = val;
				await (s.SaveAsync(basic));
				await (s.FlushAsync());
			}

			using (ISession s = OpenSession())
			{
				basic = (GuidClass)await (s.CreateCriteria(typeof (GuidClass)).Add(Expression.Eq("GuidValue", val)).UniqueResultAsync());
				Assert.IsNotNull(basic);
				Assert.AreEqual(1, basic.Id);
				Assert.AreEqual(val, basic.GuidValue);
				await (s.DeleteAsync(basic));
				await (s.FlushAsync());
			}
		}

		[Test]
		public async Task GetGuidWorksWhenUnderlyingTypeIsRepresentedByStringAsync()
		{
			GuidType type = (GuidType)NHibernateUtil.Guid;
			Guid value = Guid.NewGuid();
			DataTable data = new DataTable("test");
			data.Columns.Add("guid", typeof (Guid));
			data.Columns.Add("varchar", typeof (string));
			DataRow row = data.NewRow();
			row["guid"] = value;
			row["varchar"] = value.ToString();
			data.Rows.Add(row);
			DbDataReader reader = data.CreateDataReader();
			await (reader.ReadAsync());
			Assert.AreEqual(value, type.Get(reader, "guid"));
			Assert.AreEqual(value, type.Get(reader, 0));
			Assert.AreEqual(value, type.Get(reader, "varchar"));
			Assert.AreEqual(value, type.Get(reader, 1));
		}
	}
}
#endif
