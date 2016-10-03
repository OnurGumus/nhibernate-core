#if NET_4_5
using System;
using NHibernate.Cfg;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH642
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync
	{
		private async Task DoTestAsync(string name)
		{
			try
			{
				ISessionFactory factory = new Configuration().AddResource("NHibernate.Test.NHSpecificTest.NH642." + name + ".hbm.xml", typeof (FixtureAsync).Assembly).BuildSessionFactory();
				await (factory.CloseAsync());
			}
			catch (MappingException me)
			{
				PropertyNotFoundException found = null;
				Exception find = me;
				while (find != null)
				{
					found = find as PropertyNotFoundException;
					find = find.InnerException;
				}

				Assert.IsNotNull(found, "The PropertyNotFoundException is not present in the Exception tree.");
			}
		}

		[Test]
		public async Task MissingGetterAsync()
		{
			await (DoTestAsync("MissingGetter"));
		}

		[Test]
		public async Task MissingSetterAsync()
		{
			await (DoTestAsync("MissingSetter"));
		}
	}
}
#endif
