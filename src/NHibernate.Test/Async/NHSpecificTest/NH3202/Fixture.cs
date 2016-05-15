#if NET_4_5
using System.Data.Common;
using System.Text.RegularExpressions;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3202
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task OffsetNotStartingAtOneSetsParameterToSkipValueAsync()
		{
			OffsetStartsAtOneTestDialect.ForceOffsetStartsAtOne = false;
			using (var session = OpenSession())
			{
				var item2 = await (session.QueryOver<SequencedItem>().OrderBy(i => i.I).Asc.Take(1).Skip(2).SingleOrDefaultAsync<SequencedItem>());
				Assert.That(CustomDriver.OffsetParameterValueFromCommand, Is.EqualTo(2));
			}
		}

		[Test]
		public async Task OffsetStartingAtOneSetsParameterToSkipValuePlusOneAsync()
		{
			OffsetStartsAtOneTestDialect.ForceOffsetStartsAtOne = true;
			using (var session = OpenSession())
			{
				var item2 = await (session.QueryOver<SequencedItem>().OrderBy(i => i.I).Asc.Take(1).Skip(2).SingleOrDefaultAsync<SequencedItem>());
				Assert.That(CustomDriver.OffsetParameterValueFromCommand, Is.EqualTo(3));
			}
		}
	}
}
#endif
