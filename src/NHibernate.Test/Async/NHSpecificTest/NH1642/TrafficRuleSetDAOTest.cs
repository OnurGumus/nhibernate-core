#if NET_4_5
using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1642
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class TrafficRuleSetDAOTest : BugTestCase
	{
		[Test]
		public async Task AddRuleSetAsync()
		{
			using (var scenario = new Scenario(Sfi))
				using (var session = OpenSession())
					using (session.BeginTransaction())
					{
						var fromDb = await (session.GetAsync<TrafficRuleSet>(scenario.RuleSetId));
						Assert.IsNotNull(fromDb);
						Assert.AreEqual(fromDb.name, scenario.RuleSetName);
						Assert.AreEqual(fromDb.rules[0].name, scenario.RuleSetName + "-a");
					}
		}
	}
}
#endif
