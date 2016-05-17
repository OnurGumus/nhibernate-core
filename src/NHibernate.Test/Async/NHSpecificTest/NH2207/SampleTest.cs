#if NET_4_5
using System;
using System.Data;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2207
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SampleTest : BugTestCase
	{
		[Test]
		public async Task Dates_Before_1753_Should_Not_Insert_NullAsync()
		{
			object savedId;
			var expectedStoredValue = DateTime.MinValue.Date.AddDays(1).Date;
			using (ISession session = OpenSession())
				using (var tx = session.BeginTransaction())
				{
					var concrete = new DomainClass{Date = expectedStoredValue.AddMinutes(90)};
					savedId = await (session.SaveAsync(concrete));
					await (tx.CommitAsync());
				}

			using (ISession session = OpenSession())
				using (var tx = session.BeginTransaction())
				{
					var savedObj = await (session.GetAsync<DomainClass>(savedId));
					Assert.That(savedObj.Date, Is.EqualTo(expectedStoredValue));
					await (session.DeleteAsync(savedObj));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
