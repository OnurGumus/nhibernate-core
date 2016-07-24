#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3453
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "NH3453";
			}
		}

		[Test]
		public async Task PropertyRefWithCompositeIdUpdateTestAsync()
		{
			using (var spy = new SqlLogSpy())
				using (var session = OpenSession())
					using (session.BeginTransaction())
					{
						var direction1 = new Direction{Id1 = 1, Id2 = 1, GUID = Guid.NewGuid()};
						await (session.SaveAsync(direction1));
						var direction2 = new Direction{Id1 = 2, Id2 = 2, GUID = Guid.NewGuid()};
						await (session.SaveAsync(direction2));
						await (session.FlushAsync());
						var directionReferrer = new DirectionReferrer{GUID = Guid.NewGuid(), Direction = direction1, };
						await (session.SaveAsync(directionReferrer));
						directionReferrer.Direction = direction2;
						await (session.UpdateAsync(directionReferrer));
						await (session.FlushAsync());
						Console.WriteLine(spy.ToString());
						Assert.That(true);
					}
		}
	}
}
#endif
