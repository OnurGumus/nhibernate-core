#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Dialect;
using NHibernate.DomainModel;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Legacy
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class MultiTableTest : TestCase
	{
		[Test]
		public async Task SubclassCollectionAsync()
		{
			ISession s = OpenSession();
			SubMulti sm = new SubMulti();
			SubMulti sm1 = new SubMulti();
			SubMulti sm2 = new SubMulti();
			sm.Children = new List<SubMulti>{sm1, sm2};
			sm.MoreChildren = new List<SubMulti>{sm1, sm2};
			sm.ExtraProp = "foo";
			sm1.Parent = sm;
			sm2.Parent = sm;
			object id = await (s.SaveAsync(sm));
			await (s.SaveAsync(sm1));
			await (s.SaveAsync(sm2));
			await (s.FlushAsync());
			s.Close();
			sessions.Evict(typeof (SubMulti));
			s = OpenSession();
			// TODO: I don't understand why h2.0.3/h2.1 issues a select statement here
			Assert.AreEqual(2, s.CreateQuery("select s from SubMulti as sm join sm.Children as s where s.Amount>-1 and s.Name is null").List().Count);
			Assert.AreEqual(2, s.CreateQuery("select elements(sm.Children) from SubMulti as sm").List().Count);
			Assert.AreEqual(1, s.CreateQuery("select distinct sm from SubMulti as sm join sm.Children as s where s.Amount>-1 and s.Name is null").List().Count);
			sm = (SubMulti)s.Load(typeof (SubMulti), id);
			Assert.AreEqual(2, sm.Children.Count);
			ICollection filterColl = s.CreateFilter(sm.MoreChildren, "select count(*) where this.Amount>-1 and this.Name is null").List();
			foreach (object obj in filterColl)
			{
				Assert.AreEqual(2, obj);
				// only want the first one
				break;
			}

			Assert.AreEqual("FOO", sm.Derived, "should have uppercased the column in a formula");
			IEnumerator enumer = s.CreateQuery("select distinct s from s in class SubMulti where s.MoreChildren[1].Amount < 1.0").Enumerable().GetEnumerator();
			Assert.IsTrue(enumer.MoveNext());
			Assert.AreSame(sm, enumer.Current);
			Assert.AreEqual(2, sm.MoreChildren.Count);
			await (s.DeleteAsync(sm));
			foreach (object obj in sm.Children)
			{
				await (s.DeleteAsync(obj));
			}

			await (s.FlushAsync());
			s.Close();
		}

		[Test]
		public async Task CollectionOnlyAsync()
		{
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			NotMono m = new NotMono();
			long id = (long)await (s.SaveAsync(m));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			s.Update(m, id);
			await (s.FlushAsync());
			m.Address = "foo bar";
			await (s.FlushAsync());
			await (s.DeleteAsync(m));
			await (t.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task QueriesAsync()
		{
			ISession s = OpenSession();
			long id = 1L;
			if (Dialect is MsSql2000Dialect)
			{
				id = (long)await (s.SaveAsync(new TrivialClass()));
			}
			else
			{
				s.Save(new TrivialClass(), id);
			}

			await (s.FlushAsync());
			s.Close();
			s = OpenSession();
			TrivialClass tc = (TrivialClass)s.Load(typeof (TrivialClass), id);
			s.CreateQuery("from s in class TrivialClass where s.id = 2").List();
			s.CreateQuery("select s.Count from s in class Top").List();
			s.CreateQuery("from s in class Lower where s.Another.Name='name'").List();
			s.CreateQuery("from s in class Lower where s.YetAnother.Name='name'").List();
			s.CreateQuery("from s in class Lower where s.YetAnother.Name='name' and s.YetAnother.Foo is null").List();
			s.CreateQuery("from s in class Top where s.Count=1").List();
			s.CreateQuery("select s.Count from s in class Top, ls in class Lower where ls.Another=s").List();
			s.CreateQuery("select elements(ls.Bag), elements(ls.Set) from ls in class Lower").List();
			s.CreateQuery("from s in class Lower").Enumerable();
			s.CreateQuery("from s in class Top").Enumerable();
			await (s.DeleteAsync(tc));
			await (s.FlushAsync());
			s.Close();
		}

		[Test]
		public async Task ConstraintsAsync()
		{
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			SubMulti sm = new SubMulti();
			sm.Amount = 66.5f;
			if (Dialect is MsSql2000Dialect)
			{
				await (s.SaveAsync(sm));
			}
			else
			{
				s.Save(sm, (long)2);
			}

			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			await (s.DeleteAsync("from sm in class SubMulti"));
			t = s.BeginTransaction();
			await (t.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task MultiTableAsync()
		{
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			Multi multi = new Multi();
			multi.ExtraProp = "extra";
			multi.Name = "name";
			Top simp = new Top();
			simp.Date = DateTime.Now;
			simp.Name = "simp";
			object mid;
			object sid;
			if (Dialect is MsSql2000Dialect)
			{
				mid = await (s.SaveAsync(multi));
				sid = await (s.SaveAsync(simp));
			}
			else
			{
				mid = 123L;
				sid = 1234L;
				s.Save(multi, mid);
				s.Save(simp, sid);
			}

			SubMulti sm = new SubMulti();
			sm.Amount = 66.5f;
			object smid;
			if (Dialect is MsSql2000Dialect)
			{
				smid = await (s.SaveAsync(sm));
			}
			else
			{
				smid = 2L;
				s.Save(sm, smid);
			}

			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			multi.ExtraProp = multi.ExtraProp + "2";
			multi.Name = "new name";
			s.Update(multi, mid);
			simp.Name = "new name";
			s.Update(simp, sid);
			sm.Amount = 456.7f;
			s.Update(sm, smid);
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			multi = (Multi)s.Load(typeof (Multi), mid);
			Assert.AreEqual("extra2", multi.ExtraProp);
			multi.ExtraProp = multi.ExtraProp + "3";
			Assert.AreEqual("new name", multi.Name);
			multi.Name = "newer name";
			sm = (SubMulti)s.Load(typeof (SubMulti), smid);
			Assert.AreEqual(456.7f, sm.Amount);
			sm.Amount = 23423f;
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			multi = (Multi)s.Load(typeof (Top), mid);
			simp = (Top)s.Load(typeof (Top), sid);
			Assert.IsFalse(simp is Multi);
			Assert.AreEqual("extra23", multi.ExtraProp);
			Assert.AreEqual("newer name", multi.Name);
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			IEnumerator enumer = s.CreateQuery("select\n\ns from s in class Top where s.Count>0").Enumerable().GetEnumerator();
			bool foundSimp = false;
			bool foundMulti = false;
			bool foundSubMulti = false;
			while (enumer.MoveNext())
			{
				object o = enumer.Current;
				if ((o is Top) && !(o is Multi))
					foundSimp = true;
				if ((o is Multi) && !(o is SubMulti))
					foundMulti = true;
				if (o is SubMulti)
					foundSubMulti = true;
			}

			Assert.IsTrue(foundSimp);
			Assert.IsTrue(foundMulti);
			Assert.IsTrue(foundSubMulti);
			s.CreateQuery("from m in class Multi where m.Count>0 and m.ExtraProp is not null").List();
			s.CreateQuery("from m in class Top where m.Count>0 and m.Name is not null").List();
			s.CreateQuery("from m in class Lower where m.Other is not null").List();
			s.CreateQuery("from m in class Multi where m.Other.id = 1").List();
			s.CreateQuery("from m in class SubMulti where m.Amount > 0.0").List();
			Assert.AreEqual(2, s.CreateQuery("from m in class Multi").List().Count);
			//if( !(dialect is Dialect.HSQLDialect) ) 
			//{
			Assert.AreEqual(1, s.CreateQuery("from m in class Multi where m.class = SubMulti").List().Count);
			Assert.AreEqual(1, s.CreateQuery("from m in class Top where m.class = Multi").List().Count);
			//}
			Assert.AreEqual(3, s.CreateQuery("from s in class Top").List().Count);
			Assert.AreEqual(0, s.CreateQuery("from ls in class Lower").List().Count);
			Assert.AreEqual(1, s.CreateQuery("from sm in class SubMulti").List().Count);
			s.CreateQuery("from ls in class Lower, s in elements(ls.Bag) where s.id is not null").List();
			s.CreateQuery("from ls in class Lower, s in elements(ls.Set) where s.id is not null").List();
			s.CreateQuery("from sm in class SubMulti where exists elements(sm.Children)").List();
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			if (TestDialect.SupportsSelectForUpdateOnOuterJoin)
				multi = (Multi)s.Load(typeof (Top), mid, LockMode.Upgrade);
			simp = (Top)s.Load(typeof (Top), sid);
			s.Lock(simp, LockMode.UpgradeNoWait);
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			s.Update(multi, mid);
			await (s.DeleteAsync(multi));
			Assert.AreEqual(2, await (s.DeleteAsync("from s in class Top")));
			await (t.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task MultiTableGeneratedIdAsync()
		{
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			Multi multi = new Multi();
			multi.ExtraProp = "extra";
			multi.Name = "name";
			Top simp = new Top();
			simp.Date = DateTime.Now;
			simp.Name = "simp";
			object multiId = await (s.SaveAsync(multi));
			object simpId = await (s.SaveAsync(simp));
			SubMulti sm = new SubMulti();
			sm.Amount = 66.5f;
			object smId = await (s.SaveAsync(sm));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			multi.ExtraProp += "2";
			multi.Name = "new name";
			s.Update(multi, multiId);
			simp.Name = "new name";
			s.Update(simp, simpId);
			sm.Amount = 456.7f;
			s.Update(sm, smId);
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			multi = (Multi)s.Load(typeof (Multi), multiId);
			Assert.AreEqual("extra2", multi.ExtraProp);
			multi.ExtraProp += "3";
			Assert.AreEqual("new name", multi.Name);
			multi.Name = "newer name";
			sm = (SubMulti)s.Load(typeof (SubMulti), smId);
			Assert.AreEqual(456.7f, sm.Amount);
			sm.Amount = 23423f;
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			multi = (Multi)s.Load(typeof (Top), multiId);
			simp = (Top)s.Load(typeof (Top), simpId);
			Assert.IsFalse(simp is Multi);
			// Can't see the point of this test since the variable is declared as Multi!
			//Assert.IsTrue( multi is Multi );
			Assert.AreEqual("extra23", multi.ExtraProp);
			Assert.AreEqual("newer name", multi.Name);
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			IEnumerable enumer = s.CreateQuery("select\n\ns from s in class Top where s.Count>0").Enumerable();
			bool foundSimp = false;
			bool foundMulti = false;
			bool foundSubMulti = false;
			foreach (object obj in enumer)
			{
				if ((obj is Top) && !(obj is Multi))
					foundSimp = true;
				if ((obj is Multi) && !(obj is SubMulti))
					foundMulti = true;
				if (obj is SubMulti)
					foundSubMulti = true;
			}

			Assert.IsTrue(foundSimp);
			Assert.IsTrue(foundMulti);
			Assert.IsTrue(foundSubMulti);
			s.CreateQuery("from m in class Multi where m.Count>0 and m.ExtraProp is not null").List();
			s.CreateQuery("from m in class Top where m.Count>0 and m.Name is not null").List();
			s.CreateQuery("from m in class Lower where m.Other is not null").List();
			s.CreateQuery("from m in class Multi where m.Other.id = 1").List();
			s.CreateQuery("from m in class SubMulti where m.Amount > 0.0").List();
			Assert.AreEqual(2, s.CreateQuery("from m in class Multi").List().Count);
			Assert.AreEqual(3, s.CreateQuery("from s in class Top").List().Count);
			Assert.AreEqual(0, s.CreateQuery("from s in class Lower").List().Count);
			Assert.AreEqual(1, s.CreateQuery("from sm in class SubMulti").List().Count);
			s.CreateQuery("from ls in class Lower, s in elements(ls.Bag) where s.id is not null").List();
			s.CreateQuery("from sm in class SubMulti where exists elements(sm.Children)").List();
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			if (TestDialect.SupportsSelectForUpdateOnOuterJoin)
				multi = (Multi)s.Load(typeof (Top), multiId, LockMode.Upgrade);
			simp = (Top)s.Load(typeof (Top), simpId);
			s.Lock(simp, LockMode.UpgradeNoWait);
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			s.Update(multi, multiId);
			await (s.DeleteAsync(multi));
			Assert.AreEqual(2, await (s.DeleteAsync("from s in class Top")));
			await (t.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task MultiTableCollectionsAsync()
		{
			if (Dialect is MySQLDialect)
			{
				return;
			}

			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			Assert.AreEqual(0, s.CreateQuery("from s in class Top").List().Count);
			Multi multi = new Multi();
			multi.ExtraProp = "extra";
			multi.Name = "name";
			Top simp = new Top();
			simp.Date = DateTime.Now;
			simp.Name = "simp";
			object mid;
			object sid;
			if (Dialect is MsSql2000Dialect)
			{
				mid = await (s.SaveAsync(multi));
				sid = await (s.SaveAsync(simp));
			}
			else
			{
				mid = 123L;
				sid = 1234L;
				s.Save(multi, mid);
				s.Save(simp, sid);
			}

			Lower ls = new Lower();
			ls.Other = ls;
			ls.Another = ls;
			ls.YetAnother = ls;
			ls.Name = "Less Simple";
			ls.Set = new HashSet<Top>{multi, simp};
			object id;
			if (Dialect is MsSql2000Dialect)
			{
				id = await (s.SaveAsync(ls));
			}
			else
			{
				id = 2L;
				s.Save(ls, id);
			}

			await (t.CommitAsync());
			s.Close();
			Assert.AreSame(ls, ls.Other);
			Assert.AreSame(ls, ls.Another);
			Assert.AreSame(ls, ls.YetAnother);
			s = OpenSession();
			t = s.BeginTransaction();
			ls = (Lower)s.Load(typeof (Lower), id);
			Assert.AreSame(ls, ls.Other);
			Assert.AreSame(ls, ls.Another);
			Assert.AreSame(ls, ls.YetAnother);
			Assert.AreEqual(2, ls.Set.Count);
			int foundMulti = 0;
			int foundSimple = 0;
			foreach (object obj in ls.Set)
			{
				if (obj is Top)
					foundSimple++;
				if (obj is Multi)
					foundMulti++;
			}

			Assert.AreEqual(2, foundSimple);
			Assert.AreEqual(1, foundMulti);
			Assert.AreEqual(3, await (s.DeleteAsync("from s in class Top")));
			await (t.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task MultiTableManyToOneAsync()
		{
			if (Dialect is MySQLDialect)
			{
				return;
			}

			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			Assert.AreEqual(0, s.CreateQuery("from s in class Top").List().Count);
			Multi multi = new Multi();
			multi.ExtraProp = "extra";
			multi.Name = "name";
			Top simp = new Top();
			simp.Date = DateTime.Now;
			simp.Name = "simp";
			object mid;
			if (Dialect is MsSql2000Dialect)
			{
				mid = await (s.SaveAsync(multi));
			}
			else
			{
				mid = 123L;
				s.Save(multi, mid);
			}

			Lower ls = new Lower();
			ls.Other = ls;
			ls.Another = multi;
			ls.YetAnother = ls;
			ls.Name = "Less Simple";
			object id;
			if (Dialect is MsSql2000Dialect)
			{
				id = await (s.SaveAsync(ls));
			}
			else
			{
				id = 2L;
				s.Save(ls, id);
			}

			await (t.CommitAsync());
			s.Close();
			Assert.AreSame(ls, ls.Other);
			Assert.AreSame(multi, ls.Another);
			Assert.AreSame(ls, ls.YetAnother);
			s = OpenSession();
			t = s.BeginTransaction();
			ls = (Lower)s.Load(typeof (Lower), id);
			Assert.AreSame(ls, ls.Other);
			Assert.AreSame(ls, ls.YetAnother);
			Assert.AreEqual("name", ls.Another.Name);
			Assert.IsTrue(ls.Another is Multi);
			await (s.DeleteAsync(ls));
			await (s.DeleteAsync(ls.Another));
			await (t.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task MultiTableNativeIdAsync()
		{
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			Multi multi = new Multi();
			multi.ExtraProp = "extra";
			object id = await (s.SaveAsync(multi));
			Assert.IsNotNull(id);
			await (s.DeleteAsync(multi));
			await (t.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task CollectionAsync()
		{
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			Multi multi1 = new Multi();
			multi1.ExtraProp = "extra1";
			Multi multi2 = new Multi();
			multi2.ExtraProp = "extra2";
			Po po = new Po();
			multi1.Po = po;
			multi2.Po = po;
			po.Set = new HashSet<Multi>{multi1, multi2};
			po.List = new List<SubMulti>{new SubMulti()};
			object id = await (s.SaveAsync(po));
			Assert.IsNotNull(id);
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			po = (Po)s.Load(typeof (Po), id);
			Assert.AreEqual(2, po.Set.Count);
			Assert.AreEqual(1, po.List.Count);
			await (s.DeleteAsync(po));
			Assert.AreEqual(0, s.CreateQuery("from s in class Top").List().Count);
			await (t.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task OneToOneAsync()
		{
			ISession s = OpenSession();
			Lower ls = new Lower();
			object id = await (s.SaveAsync(ls));
			await (s.FlushAsync());
			s.Close();
			s = OpenSession();
			s.Load(typeof (Lower), id);
			s.Close();
			s = OpenSession();
			await (s.DeleteAsync(s.Load(typeof (Lower), id)));
			await (s.FlushAsync());
			s.Close();
		}

		[Test]
		public async Task CollectionPointerAsync()
		{
			ISession s = OpenSession();
			Lower ls = new Lower();
			IList<Top> list = new List<Top>();
			ls.Bag = list;
			Top simple = new Top();
			object id = await (s.SaveAsync(ls));
			await (s.SaveAsync(simple));
			await (s.FlushAsync());
			list.Add(simple);
			await (s.FlushAsync());
			s.Close();
			s = OpenSession();
			ls = (Lower)s.Load(typeof (Lower), id);
			Assert.AreEqual(1, ls.Bag.Count);
			await (s.DeleteAsync("from o in class System.Object"));
			await (s.FlushAsync());
			s.Close();
		}

		[Test]
		public async Task DynamicUpdateAsync()
		{
			object id;
			Top simple = new Top();
			simple.Name = "saved";
			using (ISession s = OpenSession())
			{
				id = await (s.SaveAsync(simple));
				await (s.FlushAsync());
				simple.Name = "updated";
				await (s.FlushAsync());
			}

			using (ISession s = OpenSession())
			{
				simple = (Top)s.Load(typeof (Top), id);
				Assert.AreEqual("updated", simple.Name, "name should have been updated");
			}

			using (ISession s = OpenSession())
			{
				await (s.DeleteAsync("from Top"));
				await (s.FlushAsync());
			}
		}
	}
}
#endif
