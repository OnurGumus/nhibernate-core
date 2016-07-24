#if NET_4_5
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH3408
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public Task ProjectAnonymousTypeWithArrayPropertyAsync()
		{
			try
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
						Assert.DoesNotThrow(() =>
						{
							query.ToList();
						}

						);
					}

				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public Task ProjectAnonymousTypeWithArrayPropertyWhenByteArrayContainsAsync()
		{
			try
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
						Assert.DoesNotThrow(() =>
						{
							query.ToList();
						}

						);
					}

				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public Task SelectBytePropertyWithArrayPropertyWhenByteArrayContainsAsync()
		{
			try
			{
				using (var session = OpenSession())
					using (session.BeginTransaction())
					{
						var pictures = new List<byte[]>();
						var query =
							from c in session.Query<Country>()where pictures.Contains(c.Picture)select c.Picture;
						Assert.DoesNotThrow(() =>
						{
							query.ToList();
						}

						);
					}

				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}
#endif
