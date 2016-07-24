#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH318
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
				return new string[]{"NHSpecificTest.NH318.Mappings.hbm.xml"};
			}
		}

		[Test]
		public async Task DeleteWithNotNullPropertySetToNullAsync()
		{
			ISession session = OpenSession();
			NotNullPropertyHolder a = new NotNullPropertyHolder();
			a.NotNullProperty = "Value";
			await (session.SaveAsync(a));
			await (session.FlushAsync());
			a.NotNullProperty = null;
			await (session.DeleteAsync(a));
			await (session.FlushAsync());
			session.Close();
		}
	}
}
#endif
