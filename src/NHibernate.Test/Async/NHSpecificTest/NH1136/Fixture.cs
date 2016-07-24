#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1136
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "NH1136";
			}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession s = OpenSession())
			{
				await (s.DeleteAsync("from Address"));
				await (s.DeleteAsync("from Person"));
				await (s.FlushAsync());
			}

			await (base.OnTearDownAsync());
		}

		[Test]
		public async Task TestAsync()
		{
			int id = -1;
			using (ISession s = OpenSession())
			{
				var address1 = new Address("60", "EH3 8BE");
				var address2 = new Address("2", "EH6 6JA");
				await (s.SaveAsync(address1));
				await (s.SaveAsync(address2));
				var person1 = new Person("'lil old me");
				person1.AddPercentageToFeeMatrix(0, .20m);
				person1.AddPercentageToFeeMatrix(50, .15m);
				person1.AddPercentageToFeeMatrix(100, .1m);
				person1.RegisterChangeOfAddress(new DateTime(2005, 4, 15), address1);
				person1.RegisterChangeOfAddress(new DateTime(2007, 5, 29), address2);
				await (s.SaveAsync(person1));
				await (s.FlushAsync());
				id = person1.Id;
			}

			using (ISession s = OpenSession())
			{
				var person1 = await (s.LoadAsync<Person>(id));
				person1.RegisterChangeOfAddress(new DateTime(2008, 3, 23), new Address("8", "SS7 1TT"));
				await (s.SaveAsync(person1));
				await (s.FlushAsync());
			}
		}
	}
}
#endif
