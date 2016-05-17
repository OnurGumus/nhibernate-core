#if NET_4_5
using System;
using NHibernate.Driver;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1941
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task SaveCanOverrideStringEnumGetValueAsync()
		{
			var paramPrefix = ((DriverBase)Sfi.ConnectionProvider.Driver).NamedPrefix;
			using (var ls = new SqlLogSpy())
			{
				using (var s = OpenSession())
					using (var t = s.BeginTransaction())
					{
						var person = new Person{Sex = Sex.Male};
						await (s.SaveAsync(person));
						await (t.CommitAsync());
					}

				var log = ls.GetWholeLog();
				Assert.That(log.Contains(paramPrefix + "p0 = 'M'"), Is.True);
			}
		}
	}
}
#endif
