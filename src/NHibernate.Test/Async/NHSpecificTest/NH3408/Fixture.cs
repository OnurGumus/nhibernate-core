#if NET_4_5
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3408
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task ProjectAnonymousTypeWithArrayPropertyAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var query =
						from c in session.Query<Country>()select new
						{
						c.Picture, c.NationalHolidays
						}

					;
					Assert.DoesNotThrowAsync(async () =>
					{
						await (query.ToListAsync());
					}

					);
				}
		}

		[Test]
		public async Task ProjectAnonymousTypeWithArrayPropertyWhenByteArrayContainsAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var pictures = new List<byte[]>();
					var query =
						from c in session.Query<Country>()where pictures.Contains(c.Picture)select new
						{
						c.Picture, c.NationalHolidays
						}

					;
					Assert.DoesNotThrowAsync(async () =>
					{
						await (query.ToListAsync());
					}

					);
				}
		}

		[Test]
		public async Task SelectBytePropertyWithArrayPropertyWhenByteArrayContainsAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var pictures = new List<byte[]>();
					var query =
						from c in session.Query<Country>()where pictures.Contains(c.Picture)select c.Picture;
					Assert.DoesNotThrowAsync(async () =>
					{
						await (query.ToListAsync());
					}

					);
				}
		}
	}
}
#endif
