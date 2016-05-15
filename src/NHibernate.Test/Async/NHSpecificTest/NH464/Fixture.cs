#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH464
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task CompositeElementAsync()
		{
			Promotion promo = new Promotion();
			promo.Description = "test promo";
			promo.Window = new PromotionWindow();
			promo.Window.Dates.Add(new DateRange(DateTime.Today, DateTime.Today.AddDays(20)));
			int id = 0;
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					id = (int)await (session.SaveAsync(promo));
					await (tx.CommitAsync());
				}

			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					promo = (Promotion)await (session.LoadAsync(typeof (Promotion), id));
					Assert.AreEqual(1, promo.Window.Dates.Count);
					Assert.AreEqual(DateTime.Today, ((DateRange)promo.Window.Dates[0]).Start);
					Assert.AreEqual(DateTime.Today.AddDays(20), ((DateRange)promo.Window.Dates[0]).End);
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
