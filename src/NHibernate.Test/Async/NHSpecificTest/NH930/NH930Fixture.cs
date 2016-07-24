#if NET_4_5
using System;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH930
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NH930FixtureAsync
	{
		[Test]
		public void DuplicateConstraints()
		{
			Configuration cfg = new Configuration();
			cfg.AddResource(GetType().Namespace + ".Mappings.hbm.xml", GetType().Assembly);
			string[] script = cfg.GenerateSchemaCreationScript(new MsSql2000Dialect());
			int constraintCount = 0;
			foreach (string str in script)
			{
				if (str.IndexOf("foreign key (DependentVariableId) references NVariable") >= 0)
				{
					constraintCount++;
				}
			}

			Assert.AreEqual(1, constraintCount);
		}
	}
}
#endif
