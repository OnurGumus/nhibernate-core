#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH623
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "NH623";
			}
		}

		private ISession session;
		private ITransaction tran;
		protected override async Task OnSetUpAsync()
		{
			session = OpenSession();
			tran = session.BeginTransaction();
			// save some data
			Document doc = new Document(1, "test doc");
			Image img = new Image(1, doc, "c:\a.jpg");
			Paragraph para = new Paragraph(2, doc, "Arial");
			Page p1 = new Page(1, doc);
			p1.IsActive = true;
			Page p2 = new Page(2, doc);
			p2.IsActive = false;
			Review r = new Review(10, doc, "this is a good document"); // this id is 10 on purpose (to be != docId)
			await (session.SaveAsync(doc));
			await (session.SaveAsync(img));
			await (session.SaveAsync(para));
			await (session.SaveAsync(p1));
			await (session.SaveAsync(p2));
			await (session.SaveAsync(r));
			await (session.FlushAsync());
			session.Clear();
		}

		protected override Task OnTearDownAsync()
		{
			try
			{
				if (session != null)
				{
					tran.Rollback();
					session.Dispose();
				}

				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public async Task WhereAttributesOnBagsAsync()
		{
			IList result;
			Document d;
			result = await (session.CreateCriteria(typeof (Document)).ListAsync());
			d = result[0] as Document;
			// collection is lazy loaded an so it is also filtered so we will get here one element
			Assert.AreEqual(1, d.Pages.Count);
			session.Clear();
			result = await (session.CreateCriteria(typeof (Document)).SetFetchMode("Pages", FetchMode.Join).ListAsync());
			d = result[0] as Document;
			// this assertion fails because if the collection is eager fetched it will contain all elements and will ignore the where clause.
			Assert.AreEqual(1, d.Pages.Count);
		}
	}
}
#endif
