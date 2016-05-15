#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.TypesTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class EnumStringTypeFixture : TypeFixtureBase
	{
		[Test]
		public async Task ReadFromLoadAsync()
		{
			ISession s = OpenSession();
			EnumStringClass basic = (EnumStringClass)await (s.LoadAsync(typeof (EnumStringClass), 1));
			Assert.AreEqual(SampleEnum.Dimmed, basic.EnumValue);
			EnumStringClass basic2 = (EnumStringClass)await (s.LoadAsync(typeof (EnumStringClass), 2));
			Assert.AreEqual(SampleEnum.On, basic2.EnumValue);
			s.Close();
		}

		[Test]
		public async Task ReadFromQueryAsync()
		{
			ISession s = OpenSession();
			IQuery q = s.CreateQuery("from EnumStringClass as esc where esc.EnumValue=:enumValue");
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
			q = s.CreateQuery("from EnumStringClass as esc where esc.EnumValue=:stringValue");
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
