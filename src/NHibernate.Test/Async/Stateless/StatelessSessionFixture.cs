#if NET_4_5
using System;
using System.Collections;
using System.Threading;
using NHibernate.Criterion;
using NHibernate.Engine;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.Stateless
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class StatelessSessionFixtureAsync : TestCaseAsync
	{
		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		protected override IList Mappings
		{
			get
			{
				return new[]{"Stateless.Document.hbm.xml"};
			}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession s = OpenSession())
			{
				await (s.DeleteAsync("from Document"));
				await (s.DeleteAsync("from Paper"));
			}
		}

		[Test]
		public async Task CreateUpdateReadDeleteAsync()
		{
			Document doc;
			DateTime? initVersion;
			using (IStatelessSession ss = sessions.OpenStatelessSession())
			{
				ITransaction tx;
				using (tx = ss.BeginTransaction())
				{
					doc = new Document("blah blah blah", "Blahs");
					await (ss.InsertAsync(doc));
					Assert.IsNotNull(doc.LastModified);
					initVersion = doc.LastModified;
					Assert.IsTrue(initVersion.HasValue);
					await (tx.CommitAsync());
				}

				Thread.Sleep(1100); // Ensure version increment (some dialects lack fractional seconds).
				using (tx = ss.BeginTransaction())
				{
					doc.Text = "blah blah blah .... blah";
					await (ss.UpdateAsync(doc));
					Assert.IsTrue(doc.LastModified.HasValue);
					Assert.AreNotEqual(initVersion, doc.LastModified);
					await (tx.CommitAsync());
				}

				using (tx = ss.BeginTransaction())
				{
					doc.Text = "blah blah blah .... blah blay";
					await (ss.UpdateAsync(doc));
					await (tx.CommitAsync());
				}

				var doc2 = await (ss.GetAsync<Document>("Blahs"));
				Assert.AreEqual("Blahs", doc2.Name);
				Assert.AreEqual(doc.Text, doc2.Text);
				doc2 = (Document)await (ss.CreateQuery("from Document where text is not null").UniqueResultAsync());
				Assert.AreEqual("Blahs", doc2.Name);
				Assert.AreEqual(doc.Text, doc2.Text);
				doc2 = (Document)await (ss.CreateSQLQuery("select * from Document").AddEntity(typeof (Document)).UniqueResultAsync());
				Assert.AreEqual("Blahs", doc2.Name);
				Assert.AreEqual(doc.Text, doc2.Text);
				doc2 = await (ss.CreateCriteria<Document>().UniqueResultAsync<Document>());
				Assert.AreEqual("Blahs", doc2.Name);
				Assert.AreEqual(doc.Text, doc2.Text);
				doc2 = (Document)await (ss.CreateCriteria(typeof (Document)).UniqueResultAsync());
				Assert.AreEqual("Blahs", doc2.Name);
				Assert.AreEqual(doc.Text, doc2.Text);
				using (tx = ss.BeginTransaction())
				{
					await (ss.DeleteAsync(doc));
					await (tx.CommitAsync());
				}
			}
		}

		[Test]
		public async Task HqlBulkAsync()
		{
			IStatelessSession ss = sessions.OpenStatelessSession();
			ITransaction tx = ss.BeginTransaction();
			var doc = new Document("blah blah blah", "Blahs");
			await (ss.InsertAsync(doc));
			var paper = new Paper{Color = "White"};
			await (ss.InsertAsync(paper));
			await (tx.CommitAsync());
			tx = ss.BeginTransaction();
			int count = await (ss.CreateQuery("update Document set Name = :newName where Name = :oldName").SetString("newName", "Foos").SetString("oldName", "Blahs").ExecuteUpdateAsync());
			Assert.AreEqual(1, count, "hql-delete on stateless session");
			count = await (ss.CreateQuery("update Paper set Color = :newColor").SetString("newColor", "Goldenrod").ExecuteUpdateAsync());
			Assert.AreEqual(1, count, "hql-delete on stateless session");
			await (tx.CommitAsync());
			tx = ss.BeginTransaction();
			count = await (ss.CreateQuery("delete Document").ExecuteUpdateAsync());
			Assert.AreEqual(1, count, "hql-delete on stateless session");
			count = await (ss.CreateQuery("delete Paper").ExecuteUpdateAsync());
			Assert.AreEqual(1, count, "hql-delete on stateless session");
			await (tx.CommitAsync());
			ss.Close();
		}

		[Test]
		public async Task InitIdAsync()
		{
			Paper paper;
			using (IStatelessSession ss = sessions.OpenStatelessSession())
			{
				ITransaction tx;
				using (tx = ss.BeginTransaction())
				{
					paper = new Paper{Color = "White"};
					await (ss.InsertAsync(paper));
					Assert.IsTrue(paper.Id != 0);
					await (tx.CommitAsync());
				}

				using (tx = ss.BeginTransaction())
				{
					await (ss.DeleteAsync(await (ss.GetAsync<Paper>(paper.Id))));
					await (tx.CommitAsync());
				}
			}
		}

		[Test]
		public async Task RefreshAsync()
		{
			Paper paper;
			using (IStatelessSession ss = sessions.OpenStatelessSession())
			{
				using (ITransaction tx = ss.BeginTransaction())
				{
					paper = new Paper{Color = "whtie"};
					await (ss.InsertAsync(paper));
					await (tx.CommitAsync());
				}
			}

			using (IStatelessSession ss = sessions.OpenStatelessSession())
			{
				using (ITransaction tx = ss.BeginTransaction())
				{
					var p2 = await (ss.GetAsync<Paper>(paper.Id));
					p2.Color = "White";
					await (ss.UpdateAsync(p2));
					await (tx.CommitAsync());
				}
			}

			using (IStatelessSession ss = sessions.OpenStatelessSession())
			{
				using (ITransaction tx = ss.BeginTransaction())
				{
					Assert.AreEqual("whtie", paper.Color);
					await (ss.RefreshAsync(paper));
					Assert.AreEqual("White", paper.Color);
					await (ss.DeleteAsync(paper));
					await (tx.CommitAsync());
				}
			}
		}

		[Test]
		public async Task HavingDetachedCriteriaThenCanGetExecutableCriteriaFromStatelessSessionAsync()
		{
			var dc = DetachedCriteria.For<Paper>();
			using (IStatelessSession ss = sessions.OpenStatelessSession())
			{
				ICriteria criteria = null;
				Assert.That(() => criteria = dc.GetExecutableCriteria(ss), Throws.Nothing);
				Assert.That(async () => await (criteria.ListAsync()), Throws.Nothing);
			}
		}
	}
}
#endif
