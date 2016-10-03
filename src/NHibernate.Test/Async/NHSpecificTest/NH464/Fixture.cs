#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH464
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "NH464";
			}
		}

		/// <summary>
		/// Mapping files used in the TestCase
		/// </summary>
		protected override IList Mappings
		{
			get
			{
				return new string[]{"NHSpecificTest.NH464.Promotion.hbm.xml"};
			}
		}

		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (ISession session = OpenSession())
				using (ITransaction t = session.BeginTransaction())
				{
					await (session.DeleteAsync("from System.Object")); // clear everything from database
					await (t.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction t = session.BeginTransaction())
				{
					await (session.DeleteAsync("from System.Object")); // clear everything from database
					await (t.CommitAsync());
				}

			await (base.OnTearDownAsync());
		}

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
