#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.TypesTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class GenericEnumStringTypeFixtureAsync : TypeFixtureBaseAsync
	{
		protected override string TypeName
		{
			get
			{
				return "GenericEnumString";
			}
		}

		protected override async Task OnSetUpAsync()
		{
			GenericEnumStringClass basic = new GenericEnumStringClass();
			basic.Id = 1;
			basic.EnumValue = SampleEnum.Dimmed;
			GenericEnumStringClass basic2 = new GenericEnumStringClass();
			basic2.Id = 2;
			basic2.EnumValue = SampleEnum.On;
			ISession s = OpenSession();
			await (s.SaveAsync(basic));
			await (s.SaveAsync(basic2));
			await (s.FlushAsync());
			s.Close();
		}

		protected override async Task OnTearDownAsync()
		{
			ISession s = OpenSession();
			await (s.DeleteAsync("from GenericEnumStringClass"));
			await (s.FlushAsync());
			s.Close();
		}

		[Test]
		public async Task ReadFromLoadAsync()
		{
			ISession s = OpenSession();
			GenericEnumStringClass basic = (GenericEnumStringClass)await (s.LoadAsync(typeof (GenericEnumStringClass), 1));
			Assert.AreEqual(SampleEnum.Dimmed, basic.EnumValue);
			GenericEnumStringClass basic2 = (GenericEnumStringClass)await (s.LoadAsync(typeof (GenericEnumStringClass), 2));
			Assert.AreEqual(SampleEnum.On, basic2.EnumValue);
			s.Close();
		}

		[Test]
		public async Task ReadFromQueryAsync()
		{
			ISession s = OpenSession();
			IQuery q = s.CreateQuery("from GenericEnumStringClass as esc where esc.EnumValue=:enumValue");
			q.SetParameter("enumValue", SampleEnum.On, new SampleEnumType());
			IList results = await (q.ListAsync());
			Assert.AreEqual(1, results.Count, "only 1 was 'On'");
			q.SetParameter("enumValue", SampleEnum.Off, new SampleEnumType());
			results = await (q.ListAsync());
			Assert.AreEqual(0, results.Count, "should not be any in the 'Off' status");
			s.Close();
			// it will also be possible to query based on a string value
			// since that is what is in the db
			s = OpenSession();
			q = s.CreateQuery("from GenericEnumStringClass as esc where esc.EnumValue=:stringValue");
			q.SetString("stringValue", "On");
			results = await (q.ListAsync());
			Assert.AreEqual(1, results.Count, "only 1 was 'On' string");
			q.SetString("stringValue", "Off");
			results = await (q.ListAsync());
			Assert.AreEqual(0, results.Count, "should not be any in the 'Off' string");
			s.Close();
		}
	}
}
#endif
