#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.Docs.Associations.BiM21
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : TestCaseAsync
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
				return new string[]{"NHSpecificTest.Docs.Associations.BiM21.Mappings.hbm.xml"};
			}
		}

		[Test]
		public async Task TestCorrectUseAsync()
		{
			ISession session = OpenSession();
			Person fred = new Person();
			Person wilma = new Person();
			Address flinstoneWay = new Address();
			fred.Address = flinstoneWay;
			wilma.Address = flinstoneWay;
			await (session.SaveAsync(flinstoneWay));
			await (session.SaveAsync(fred));
			await (session.SaveAsync(wilma));
			session.Close();
			// clean up
			session = OpenSession();
			await (session.DeleteAsync("from Person"));
			await (session.DeleteAsync("from Address"));
			await (session.FlushAsync());
			session.Close();
		}

		[Test]
		public async Task TestErrorUsageAsync()
		{
			using (ISession session = OpenSession())
			{
				Person fred = new Person();
				Person wilma = new Person();
				Address flinstoneWay = new Address();
				fred.Address = flinstoneWay;
				wilma.Address = flinstoneWay;
				Assert.ThrowsAsync<PropertyValueException>(async () => await (session.SaveAsync(fred)));
			}
		}
	}
}
#endif
