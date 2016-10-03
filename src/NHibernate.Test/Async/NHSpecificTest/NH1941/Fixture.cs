#if NET_4_5
using System;
using NHibernate.Driver;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1941
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnTearDownAsync()
		{
			using (var s = OpenSession())
				using (var t = s.BeginTransaction())
				{
					await (s.DeleteAsync("from Person"));
					await (t.CommitAsync());
				}
		}

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

		[Test]
		public async Task ReadCanOverrideStringEnumGetValueAsync()
		{
			var paramPrefix = ((DriverBase)Sfi.ConnectionProvider.Driver).NamedPrefix;
			using (var ls = new SqlLogSpy())
			{
				using (var s = OpenSession())
					using (s.BeginTransaction())
					{
						var person = await (s.CreateQuery("from Person p where p.Sex = :personSex").SetParameter("personSex", Sex.Female).UniqueResultAsync<Person>());
						Assert.That(person, Is.Null);
					}

				string log = ls.GetWholeLog();
				Assert.IsTrue(log.Contains(paramPrefix + "p0 = 'F'"));
			}
		}
	}
}
#endif
