#if NET_4_5
using System;
using System.Collections;
using System.Threading;
using NHibernate.Criterion;
using NHibernate.Engine;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Stateless
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class StatelessSessionFixture : TestCase
	{
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
					ss.Insert(doc);
					Assert.IsNotNull(doc.LastModified);
					initVersion = doc.LastModified;
					Assert.IsTrue(initVersion.HasValue);
					await (tx.CommitAsync());
				}

				Thread.Sleep(1100); // Ensure version increment (some dialects lack fractional seconds).
				using (tx = ss.BeginTransaction())
				{
					doc.Text = "blah blah blah .... blah";
					ss.Update(doc);
					Assert.IsTrue(doc.LastModified.HasValue);
					Assert.AreNotEqual(initVersion, doc.LastModified);
					await (tx.CommitAsync());
				}

				using (tx = ss.BeginTransaction())
				{
					doc.Text = "blah blah blah .... blah blay";
					ss.Update(doc);
					await (tx.CommitAsync());
				}

				var doc2 = ss.Get<Document>("Blahs");
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
					ss.Delete(doc);
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
			ss.Insert(doc);
			var paper = new Paper{Color = "White"};
			ss.Insert(paper);
			await (tx.CommitAsync());
			tx = ss.BeginTransaction();
			int count = ss.CreateQuery("update Document set Name = :newName where Name = :oldName").SetString("newName", "Foos").SetString("oldName", "Blahs").ExecuteUpdate();
			Assert.AreEqual(1, count, "hql-delete on stateless session");
			count = ss.CreateQuery("update Paper set Color = :newColor").SetString("newColor", "Goldenrod").ExecuteUpdate();
			Assert.AreEqual(1, count, "hql-delete on stateless session");
			await (tx.CommitAsync());
			tx = ss.BeginTransaction();
			count = ss.CreateQuery("delete Document").ExecuteUpdate();
			Assert.AreEqual(1, count, "hql-delete on stateless session");
			count = ss.CreateQuery("delete Paper").ExecuteUpdate();
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
					ss.Insert(paper);
					Assert.IsTrue(paper.Id != 0);
					await (tx.CommitAsync());
				}

				using (tx = ss.BeginTransaction())
				{
					ss.Delete(ss.Get<Paper>(paper.Id));
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
					ss.Insert(paper);
					await (tx.CommitAsync());
				}
			}

			using (IStatelessSession ss = sessions.OpenStatelessSession())
			{
				using (ITransaction tx = ss.BeginTransaction())
				{
					var p2 = ss.Get<Paper>(paper.Id);
					p2.Color = "White";
					ss.Update(p2);
					await (tx.CommitAsync());
				}
			}

			using (IStatelessSession ss = sessions.OpenStatelessSession())
			{
				using (ITransaction tx = ss.BeginTransaction())
				{
					Assert.AreEqual("whtie", paper.Color);
					ss.Refresh(paper);
					Assert.AreEqual("White", paper.Color);
					ss.Delete(paper);
					await (tx.CommitAsync());
				}
			}
		}
	}
}
#endif
