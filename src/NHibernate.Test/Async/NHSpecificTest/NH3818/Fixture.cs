#if NET_4_5
using System;
using System.Linq;
using System.Linq.Dynamic;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3818
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task SelectConditionalValuesTestAsync()
		{
			using (var spy = new SqlLogSpy())
				using (var session = OpenSession())
					using (session.BeginTransaction())
					{
						var days = 33;
						var cat = new MyLovelyCat{GUID = Guid.NewGuid(), Birthdate = DateTime.Now.AddDays(-days), Color = "Black", Name = "Kitty", Price = 0};
						await (session.SaveAsync(cat));
						await (session.FlushAsync());
						var catInfo = session.Query<MyLovelyCat>().Select(o => new
						{
						o.Color, AliveDays = (int)(DateTime.Now - o.Birthdate).TotalDays, o.Name, o.Price, }

						).Single();
						//Console.WriteLine(spy.ToString());
						Assert.That(catInfo.AliveDays == days);
						var catInfo2 = session.Query<MyLovelyCat>().Select(o => new
						{
						o.Color, AliveDays = o.Price > 0 ? (DateTime.Now - o.Birthdate).TotalDays : 0, o.Name, o.Price, }

						).Single();
						//Console.WriteLine(spy.ToString());
						Assert.That(catInfo2.AliveDays == 0);
					}
		}
	}
}
#endif
