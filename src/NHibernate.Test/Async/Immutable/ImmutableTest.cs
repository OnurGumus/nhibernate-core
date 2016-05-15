#if NET_4_5
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;
using NHibernate.Proxy;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Immutable
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ImmutableTest : TestCase
	{
		[Test]
		public async Task ChangeImmutableEntityProxyToModifiableAsync()
		{
			Contract c = new Contract(null, "gavin", "phone");
			ContractVariation cv1 = new ContractVariation(1, c);
			cv1.Text = "expensive";
			ContractVariation cv2 = new ContractVariation(2, c);
			cv2.Text = "more expensive";
			ClearCounts();
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			Assert.That(s.IsReadOnly(c), Is.True);
			Assert.That(s.IsReadOnly(cv1), Is.True);
			Assert.That(s.IsReadOnly(cv2), Is.True);
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(3);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			Assert.That(s.IsReadOnly(c), Is.True);
			Assert.That(c.CustomerName, Is.EqualTo("gavin"));
			Assert.That(c.Variations.Count, Is.EqualTo(2));
			IEnumerator<ContractVariation> it = c.Variations.GetEnumerator();
			it.MoveNext();
			cv1 = it.Current;
			Assert.That(cv1.Text, Is.EqualTo("expensive"));
			it.MoveNext();
			cv2 = it.Current;
			Assert.That(cv2.Text, Is.EqualTo("more expensive"));
			Assert.That(s.IsReadOnly(cv1), Is.True);
			Assert.That(s.IsReadOnly(cv2), Is.True);
			try
			{
				Assert.That(c, Is.InstanceOf<INHibernateProxy>());
				await (s.SetReadOnlyAsync(c, false));
			}
			catch (System.InvalidOperationException)
			{
			// expected
			}
			finally
			{
				t.Rollback();
				s.Close();
			}

			s = OpenSession();
			t = s.BeginTransaction();
			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			Assert.That(await (s.CreateCriteria<ContractVariation>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(3);
		}

		[Test]
		public async Task ChangeImmutableEntityToModifiableAsync()
		{
			Contract c = new Contract(null, "gavin", "phone");
			ContractVariation cv1 = new ContractVariation(1, c);
			cv1.Text = "expensive";
			ContractVariation cv2 = new ContractVariation(2, c);
			cv2.Text = "more expensive";
			ClearCounts();
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			Assert.That(s.IsReadOnly(c), Is.True);
			Assert.That(s.IsReadOnly(cv1), Is.True);
			Assert.That(s.IsReadOnly(cv2), Is.True);
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(3);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			Assert.That(s.IsReadOnly(c), Is.True);
			Assert.That(c.CustomerName, Is.EqualTo("gavin"));
			Assert.That(c.Variations.Count, Is.EqualTo(2));
			IEnumerator<ContractVariation> it = c.Variations.GetEnumerator();
			it.MoveNext();
			cv1 = it.Current;
			Assert.That(cv1.Text, Is.EqualTo("expensive"));
			it.MoveNext();
			cv2 = it.Current;
			Assert.That(cv2.Text, Is.EqualTo("more expensive"));
			Assert.That(s.IsReadOnly(cv1), Is.True);
			Assert.That(s.IsReadOnly(cv2), Is.True);
			try
			{
				Assert.That(c, Is.InstanceOf<INHibernateProxy>());
				await (s.SetReadOnlyAsync(await (((INHibernateProxy)c).HibernateLazyInitializer.GetImplementationAsync()), false));
			}
			catch (System.InvalidOperationException)
			{
			// expected
			}
			finally
			{
				t.Rollback();
				s.Close();
			}

			s = OpenSession();
			t = s.BeginTransaction();
			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			Assert.That(await (s.CreateCriteria<ContractVariation>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(3);
		}

		[Test]
		public async Task PersistImmutableAsync()
		{
			Contract c = new Contract(null, "gavin", "phone");
			ContractVariation cv1 = new ContractVariation(1, c);
			cv1.Text = "expensive";
			ContractVariation cv2 = new ContractVariation(2, c);
			cv2.Text = "more expensive";
			ClearCounts();
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			Assert.That(s.IsReadOnly(c), Is.True);
			Assert.That(s.IsReadOnly(cv1), Is.True);
			Assert.That(s.IsReadOnly(cv2), Is.True);
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(3);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			Assert.That(s.IsReadOnly(c), Is.True);
			Assert.That(c.CustomerName, Is.EqualTo("gavin"));
			Assert.That(c.Variations.Count, Is.EqualTo(2));
			IEnumerator<ContractVariation> it = c.Variations.GetEnumerator();
			it.MoveNext();
			cv1 = it.Current;
			Assert.That(cv1.Text, Is.EqualTo("expensive"));
			it.MoveNext();
			cv2 = it.Current;
			Assert.That(cv2.Text, Is.EqualTo("more expensive"));
			Assert.That(s.IsReadOnly(cv1), Is.True);
			Assert.That(s.IsReadOnly(cv2), Is.True);
			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			Assert.That(await (s.CreateCriteria<ContractVariation>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(3);
		}

		[Test]
		public async Task PersistUpdateImmutableInSameTransactionAsync()
		{
			Contract c = new Contract(null, "gavin", "phone");
			ContractVariation cv1 = new ContractVariation(1, c);
			cv1.Text = "expensive";
			ContractVariation cv2 = new ContractVariation(2, c);
			cv2.Text = "more expensive";
			ClearCounts();
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			Assert.That(s.IsReadOnly(c), Is.True);
			Assert.That(s.IsReadOnly(cv1), Is.True);
			Assert.That(s.IsReadOnly(cv2), Is.True);
			c.CustomerName = "gail";
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(3);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			Assert.That(s.IsReadOnly(c), Is.True);
			Assert.That(c.CustomerName, Is.EqualTo("gavin"));
			Assert.That(c.Variations.Count, Is.EqualTo(2));
			IEnumerator<ContractVariation> it = c.Variations.GetEnumerator();
			it.MoveNext();
			cv1 = it.Current;
			Assert.That(cv1.Text, Is.EqualTo("expensive"));
			it.MoveNext();
			cv2 = it.Current;
			Assert.That(cv2.Text, Is.EqualTo("more expensive"));
			Assert.That(s.IsReadOnly(cv1), Is.True);
			Assert.That(s.IsReadOnly(cv2), Is.True);
			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			Assert.That(await (s.CreateCriteria<ContractVariation>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(3);
		}

		[Test]
		public async Task SaveImmutableAsync()
		{
			ClearCounts();
			Contract c = new Contract(null, "gavin", "phone");
			ContractVariation cv1 = new ContractVariation(1, c);
			cv1.Text = "expensive";
			ContractVariation cv2 = new ContractVariation(2, c);
			cv2.Text = "more expensive";
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.SaveAsync(c));
			Assert.That(s.IsReadOnly(c), Is.True);
			Assert.That(s.IsReadOnly(cv1), Is.True);
			Assert.That(s.IsReadOnly(cv2), Is.True);
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(3);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			Assert.That(s.IsReadOnly(c), Is.True);
			Assert.That(c.CustomerName, Is.EqualTo("gavin"));
			Assert.That(c.Variations.Count, Is.EqualTo(2));
			IEnumerator<ContractVariation> it = c.Variations.GetEnumerator();
			it.MoveNext();
			cv1 = it.Current;
			Assert.That(cv1.Text, Is.EqualTo("expensive"));
			it.MoveNext();
			cv2 = it.Current;
			Assert.That(cv2.Text, Is.EqualTo("more expensive"));
			Assert.That(s.IsReadOnly(cv1), Is.True);
			Assert.That(s.IsReadOnly(cv2), Is.True);
			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			Assert.That(await (s.CreateCriteria<ContractVariation>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(3);
		}

		[Test]
		public async Task SaveOrUpdateImmutableAsync()
		{
			ClearCounts();
			Contract c = new Contract(null, "gavin", "phone");
			ContractVariation cv1 = new ContractVariation(1, c);
			cv1.Text = "expensive";
			ContractVariation cv2 = new ContractVariation(2, c);
			cv2.Text = "more expensive";
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.SaveOrUpdateAsync(c));
			Assert.That(s.IsReadOnly(c), Is.True);
			Assert.That(s.IsReadOnly(cv1), Is.True);
			Assert.That(s.IsReadOnly(cv2), Is.True);
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(3);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			Assert.That(s.IsReadOnly(c), Is.True);
			Assert.That(c.CustomerName, Is.EqualTo("gavin"));
			Assert.That(c.Variations.Count, Is.EqualTo(2));
			IEnumerator<ContractVariation> it = c.Variations.GetEnumerator();
			it.MoveNext();
			cv1 = it.Current;
			Assert.That(cv1.Text, Is.EqualTo("expensive"));
			it.MoveNext();
			cv2 = it.Current;
			Assert.That(cv2.Text, Is.EqualTo("more expensive"));
			Assert.That(s.IsReadOnly(cv1), Is.True);
			Assert.That(s.IsReadOnly(cv2), Is.True);
			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			Assert.That(await (s.CreateCriteria<ContractVariation>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(3);
		}

		[Test]
		public async Task RefreshImmutableAsync()
		{
			ClearCounts();
			Contract c = new Contract(null, "gavin", "phone");
			ContractVariation cv1 = new ContractVariation(1, c);
			cv1.Text = "expensive";
			ContractVariation cv2 = new ContractVariation(2, c);
			cv2.Text = "more expensive";
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.SaveOrUpdateAsync(c));
			Assert.That(s.IsReadOnly(c), Is.True);
			Assert.That(s.IsReadOnly(cv1), Is.True);
			Assert.That(s.IsReadOnly(cv2), Is.True);
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(3);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			// refresh detached
			await (s.RefreshAsync(c));
			Assert.That(s.IsReadOnly(c), Is.True);
			Assert.That(c.CustomerName, Is.EqualTo("gavin"));
			Assert.That(c.Variations.Count, Is.EqualTo(2));
			IEnumerator<ContractVariation> it = c.Variations.GetEnumerator();
			it.MoveNext();
			cv1 = it.Current;
			Assert.That(cv1.Text, Is.EqualTo("expensive"));
			it.MoveNext();
			cv2 = it.Current;
			Assert.That(cv2.Text, Is.EqualTo("more expensive"));
			Assert.That(s.IsReadOnly(cv1), Is.True);
			Assert.That(s.IsReadOnly(cv2), Is.True);
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(0);
			AssertUpdateCount(0);
			ClearCounts();
			c.CustomerName = "joe";
			s = OpenSession();
			t = s.BeginTransaction();
			// refresh updated detached
			await (s.RefreshAsync(c));
			Assert.That(s.IsReadOnly(c), Is.True);
			Assert.That(c.CustomerName, Is.EqualTo("gavin"));
			Assert.That(c.Variations.Count, Is.EqualTo(2));
			it = c.Variations.GetEnumerator();
			it.MoveNext();
			cv1 = it.Current;
			Assert.That(cv1.Text, Is.EqualTo("expensive"));
			it.MoveNext();
			cv2 = it.Current;
			Assert.That(cv2.Text, Is.EqualTo("more expensive"));
			Assert.That(s.IsReadOnly(cv1), Is.True);
			Assert.That(s.IsReadOnly(cv2), Is.True);
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(0);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			Assert.That(await (s.CreateCriteria<ContractVariation>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(3);
		}

		[Test]
		public async Task ImmutableAsync()
		{
			ClearCounts();
			Contract c = new Contract(null, "gavin", "phone");
			ContractVariation cv1 = new ContractVariation(1, c);
			cv1.Text = "expensive";
			ContractVariation cv2 = new ContractVariation(2, c);
			cv2.Text = "more expensive";
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			Assert.That(s.IsReadOnly(c), Is.True);
			Assert.That(s.IsReadOnly(cv1), Is.True);
			Assert.That(s.IsReadOnly(cv2), Is.True);
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(3);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			Assert.That(s.IsReadOnly(c), Is.True);
			c.CustomerName = "foo bar";
			IEnumerator<ContractVariation> it = c.Variations.GetEnumerator();
			it.MoveNext();
			cv1 = it.Current;
			cv1.Text = "blah blah";
			Assert.That(s.IsReadOnly(cv1), Is.True);
			Assert.That(await (s.ContainsAsync(cv2)), Is.False);
			await (t.CommitAsync());
			Assert.That(s.IsReadOnly(c), Is.True);
			Assert.That(s.IsReadOnly(cv1), Is.True);
			Assert.That(await (s.ContainsAsync(cv2)), Is.False);
			s.Close();
			AssertInsertCount(0);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			Assert.That(s.IsReadOnly(c), Is.True);
			Assert.That(c.CustomerName, Is.EqualTo("gavin"));
			Assert.That(c.Variations.Count, Is.EqualTo(2));
			it = c.Variations.GetEnumerator();
			it.MoveNext();
			cv1 = it.Current;
			Assert.That(cv1.Text, Is.EqualTo("expensive"));
			it.MoveNext();
			cv2 = it.Current;
			Assert.That(cv2.Text, Is.EqualTo("more expensive"));
			Assert.That(s.IsReadOnly(cv1), Is.True);
			Assert.That(s.IsReadOnly(cv2), Is.True);
			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			Assert.That(await (s.CreateCriteria<ContractVariation>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(3);
		}

		[Test]
		public async Task PersistAndUpdateImmutableAsync()
		{
			ClearCounts();
			Contract c = new Contract(null, "gavin", "phone");
			ContractVariation cv1 = new ContractVariation(1, c);
			cv1.Text = "expensive";
			ContractVariation cv2 = new ContractVariation(2, c);
			cv2.Text = "more expensive";
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			Assert.That(s.IsReadOnly(c), Is.True);
			Assert.That(s.IsReadOnly(cv1), Is.True);
			Assert.That(s.IsReadOnly(cv2), Is.True);
			c.CustomerName = "Sherman";
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(3);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			Assert.That(s.IsReadOnly(c), Is.True);
			c.CustomerName = "foo bar";
			IEnumerator<ContractVariation> it = c.Variations.GetEnumerator();
			it.MoveNext();
			cv1 = it.Current;
			cv1.Text = "blah blah";
			Assert.That(s.IsReadOnly(cv1), Is.True);
			Assert.That(await (s.ContainsAsync(cv2)), Is.False);
			await (t.CommitAsync());
			Assert.That(s.IsReadOnly(c), Is.True);
			Assert.That(s.IsReadOnly(cv1), Is.True);
			Assert.That(await (s.ContainsAsync(cv2)), Is.False);
			s.Close();
			AssertInsertCount(0);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			Assert.That(s.IsReadOnly(c), Is.True);
			Assert.That(c.CustomerName, Is.EqualTo("gavin"));
			Assert.That(c.Variations.Count, Is.EqualTo(2));
			it = c.Variations.GetEnumerator();
			it.MoveNext();
			cv1 = it.Current;
			Assert.That(cv1.Text, Is.EqualTo("expensive"));
			it.MoveNext();
			cv2 = it.Current;
			Assert.That(cv2.Text, Is.EqualTo("more expensive"));
			Assert.That(s.IsReadOnly(cv1), Is.True);
			Assert.That(s.IsReadOnly(cv2), Is.True);
			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			Assert.That(await (s.CreateCriteria<ContractVariation>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(3);
		}

		[Test]
		public async Task UpdateAndDeleteManagedImmutableAsync()
		{
			ClearCounts();
			Contract c = new Contract(null, "gavin", "phone");
			ContractVariation cv1 = new ContractVariation(1, c);
			cv1.Text = "expensive";
			ContractVariation cv2 = new ContractVariation(2, c);
			cv2.Text = "more expensive";
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(3);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			Assert.That(s.IsReadOnly(c), Is.True);
			Assert.That(c.CustomerName, Is.EqualTo("gavin"));
			Assert.That(c.Variations.Count, Is.EqualTo(2));
			IEnumerator<ContractVariation> it = c.Variations.GetEnumerator();
			it.MoveNext();
			cv1 = it.Current;
			Assert.That(cv1.Text, Is.EqualTo("expensive"));
			it.MoveNext();
			cv2 = it.Current;
			Assert.That(cv2.Text, Is.EqualTo("more expensive"));
			Assert.That(s.IsReadOnly(cv1), Is.True);
			Assert.That(s.IsReadOnly(cv2), Is.True);
			c.CustomerName = "Sherman";
			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			Assert.That(await (s.CreateCriteria<ContractVariation>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(3);
		}

		[Test]
		public async Task GetAndDeleteManagedImmutableAsync()
		{
			ClearCounts();
			Contract c = new Contract(null, "gavin", "phone");
			ContractVariation cv1 = new ContractVariation(1, c);
			cv1.Text = "expensive";
			ContractVariation cv2 = new ContractVariation(2, c);
			cv2.Text = "more expensive";
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(3);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.GetAsync<Contract>(c.Id));
			Assert.That(s.IsReadOnly(c), Is.True);
			Assert.That(c.CustomerName, Is.EqualTo("gavin"));
			Assert.That(c.Variations.Count, Is.EqualTo(2));
			IEnumerator<ContractVariation> it = c.Variations.GetEnumerator();
			it.MoveNext();
			cv1 = it.Current;
			Assert.That(cv1.Text, Is.EqualTo("expensive"));
			it.MoveNext();
			cv2 = it.Current;
			Assert.That(cv2.Text, Is.EqualTo("more expensive"));
			Assert.That(s.IsReadOnly(cv1), Is.True);
			Assert.That(s.IsReadOnly(cv2), Is.True);
			c.CustomerName = "Sherman";
			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			Assert.That(await (s.CreateCriteria<ContractVariation>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(3);
		}

		[Test]
		public async Task DeleteDetachedImmutableAsync()
		{
			ClearCounts();
			Contract c = new Contract(null, "gavin", "phone");
			ContractVariation cv1 = new ContractVariation(1, c);
			cv1.Text = "expensive";
			ContractVariation cv2 = new ContractVariation(2, c);
			cv2.Text = "more expensive";
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(3);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			await (s.DeleteAsync(c));
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			Assert.That(c, Is.Null);
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(3);
		}

		[Test]
		public async Task DeleteDetachedModifiedImmutableAsync()
		{
			ClearCounts();
			Contract c = new Contract(null, "gavin", "phone");
			ContractVariation cv1 = new ContractVariation(1, c);
			cv1.Text = "expensive";
			ContractVariation cv2 = new ContractVariation(2, c);
			cv2.Text = "more expensive";
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(3);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c.CustomerName = "Sherman";
			await (s.DeleteAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(3);
		}

		[Test]
		public async Task ImmutableParentEntityWithUpdateAsync()
		{
			ClearCounts();
			Contract c = new Contract(null, "gavin", "phone");
			ContractVariation cv1 = new ContractVariation(1, c);
			cv1.Text = "expensive";
			ContractVariation cv2 = new ContractVariation(2, c);
			cv2.Text = "more expensive";
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(3);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c.CustomerName = "foo bar";
			await (s.UpdateAsync(c));
			Assert.That(s.IsReadOnly(c), Is.True);
			foreach (ContractVariation variation in c.Variations)
			{
				Assert.That(await (s.ContainsAsync(variation)), Is.True);
			}

			await (t.CommitAsync());
			Assert.That(s.IsReadOnly(c), Is.True);
			foreach (ContractVariation variation in c.Variations)
			{
				Assert.That(await (s.ContainsAsync(variation)), Is.True);
				Assert.That(s.IsReadOnly(variation), Is.True);
			}

			s.Close();
			AssertUpdateCount(0);
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			Assert.That(c.CustomerName, Is.EqualTo("gavin"));
			Assert.That(c.Variations.Count, Is.EqualTo(2));
			IEnumerator<ContractVariation> it = c.Variations.GetEnumerator();
			it.MoveNext();
			cv1 = it.Current;
			Assert.That(cv1.Text, Is.EqualTo("expensive"));
			it.MoveNext();
			cv2 = it.Current;
			Assert.That(cv2.Text, Is.EqualTo("more expensive"));
			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			Assert.That(await (s.CreateCriteria<ContractVariation>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(3);
		}

		[Test]
		public async Task ImmutableChildEntityWithUpdateAsync()
		{
			ClearCounts();
			Contract c = new Contract(null, "gavin", "phone");
			ContractVariation cv1 = new ContractVariation(1, c);
			cv1.Text = "expensive";
			ContractVariation cv2 = new ContractVariation(2, c);
			cv2.Text = "more expensive";
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(3);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			cv1 = c.Variations.First();
			cv1.Text = "blah blah";
			await (s.UpdateAsync(c));
			Assert.That(s.IsReadOnly(c), Is.True);
			Assert.That(await (s.ContainsAsync(cv1)), Is.True);
			Assert.That(await (s.ContainsAsync(cv2)), Is.True);
			await (t.CommitAsync());
			Assert.That(s.IsReadOnly(c), Is.True);
			Assert.That(s.IsReadOnly(cv1), Is.True);
			Assert.That(s.IsReadOnly(cv2), Is.True);
			s.Close();
			AssertUpdateCount(0);
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			Assert.That(c.CustomerName, Is.EqualTo("gavin"));
			Assert.That(c.Variations.Count, Is.EqualTo(2));
			IEnumerator<ContractVariation> it = c.Variations.GetEnumerator();
			it.MoveNext();
			cv1 = it.Current;
			Assert.That(cv1.Text, Is.EqualTo("expensive"));
			it.MoveNext();
			cv2 = it.Current;
			Assert.That(cv2.Text, Is.EqualTo("more expensive"));
			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			Assert.That(await (s.CreateCriteria<ContractVariation>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(3);
		}

		[Test]
		public async Task ImmutableCollectionWithUpdateAsync()
		{
			ClearCounts();
			Contract c = new Contract(null, "gavin", "phone");
			ContractVariation cv1 = new ContractVariation(1, c);
			cv1.Text = "expensive";
			ContractVariation cv2 = new ContractVariation(2, c);
			cv2.Text = "more expensive";
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(3);
			AssertUpdateCount(0);
			s = OpenSession();
			t = s.BeginTransaction();
			c.Variations.Add(new ContractVariation(3, c));
			await (s.UpdateAsync(c));
			try
			{
				await (t.CommitAsync());
				Assert.Fail("should have failed because reassociated object has a dirty collection");
			}
			catch (HibernateException)
			{
			// expected
			}
			finally
			{
				t.Rollback();
				s.Close();
			}

			AssertUpdateCount(0);
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			Assert.That(c.CustomerName, Is.EqualTo("gavin"));
			Assert.That(c.Variations.Count, Is.EqualTo(2));
			IEnumerator<ContractVariation> it = c.Variations.GetEnumerator();
			it.MoveNext();
			cv1 = it.Current;
			Assert.That(cv1.Text, Is.EqualTo("expensive"));
			it.MoveNext();
			cv2 = it.Current;
			Assert.That(cv2.Text, Is.EqualTo("more expensive"));
			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			Assert.That(await (s.CreateCriteria<ContractVariation>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(3);
		}

		[Test]
		public async Task UnmodifiedImmutableParentEntityWithMergeAsync()
		{
			ClearCounts();
			Contract c = new Contract(null, "gavin", "phone");
			ContractVariation cv1 = new ContractVariation(1, c);
			cv1.Text = "expensive";
			ContractVariation cv2 = new ContractVariation(2, c);
			cv2.Text = "more expensive";
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(3);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c = (Contract)await (s.MergeAsync(c));
			Assert.That(s.IsReadOnly(c), Is.True);
			Assert.That(NHibernateUtil.IsInitialized(c.Variations), Is.True);
			IEnumerator<ContractVariation> it = c.Variations.GetEnumerator();
			it.MoveNext();
			cv1 = it.Current;
			it.MoveNext();
			cv2 = it.Current;
			Assert.That(s.IsReadOnly(cv1), Is.True);
			Assert.That(s.IsReadOnly(cv2), Is.True);
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			Assert.That(c.CustomerName, Is.EqualTo("gavin"));
			Assert.That(c.Variations.Count, Is.EqualTo(2));
			it = c.Variations.GetEnumerator();
			it.MoveNext();
			cv1 = it.Current;
			Assert.That(cv1.Text, Is.EqualTo("expensive"));
			it.MoveNext();
			cv2 = it.Current;
			Assert.That(cv2.Text, Is.EqualTo("more expensive"));
			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			Assert.That(await (s.CreateCriteria<ContractVariation>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(3);
		}

		[Test]
		public async Task ImmutableParentEntityWithMergeAsync()
		{
			ClearCounts();
			Contract c = new Contract(null, "gavin", "phone");
			ContractVariation cv1 = new ContractVariation(1, c);
			cv1.Text = "expensive";
			ContractVariation cv2 = new ContractVariation(2, c);
			cv2.Text = "more expensive";
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(3);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c.CustomerName = "foo bar";
			c = (Contract)await (s.MergeAsync(c));
			Assert.That(s.IsReadOnly(c), Is.True);
			Assert.That(NHibernateUtil.IsInitialized(c.Variations), Is.True);
			IEnumerator<ContractVariation> it = c.Variations.GetEnumerator();
			it.MoveNext();
			cv1 = it.Current;
			it.MoveNext();
			cv2 = it.Current;
			Assert.That(s.IsReadOnly(c), Is.True);
			Assert.That(s.IsReadOnly(c), Is.True);
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			Assert.That(c.CustomerName, Is.EqualTo("gavin"));
			Assert.That(c.Variations.Count, Is.EqualTo(2));
			it = c.Variations.GetEnumerator();
			it.MoveNext();
			cv1 = it.Current;
			Assert.That(cv1.Text, Is.EqualTo("expensive"));
			it.MoveNext();
			cv2 = it.Current;
			Assert.That(cv2.Text, Is.EqualTo("more expensive"));
			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			Assert.That(await (s.CreateCriteria<ContractVariation>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(3);
		}

		[Test]
		public async Task ImmutableChildEntityWithMergeAsync()
		{
			ClearCounts();
			Contract c = new Contract(null, "gavin", "phone");
			ContractVariation cv1 = new ContractVariation(1, c);
			cv1.Text = "expensive";
			ContractVariation cv2 = new ContractVariation(2, c);
			cv2.Text = "more expensive";
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(3);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			cv1 = c.Variations.First();
			cv1.Text = "blah blah";
			c = (Contract)await (s.MergeAsync(c));
			Assert.That(s.IsReadOnly(c), Is.True);
			Assert.That(NHibernateUtil.IsInitialized(c.Variations), Is.True);
			IEnumerator<ContractVariation> it = c.Variations.GetEnumerator();
			it.MoveNext();
			cv1 = it.Current;
			it.MoveNext();
			cv2 = it.Current;
			Assert.That(s.IsReadOnly(c), Is.True);
			Assert.That(s.IsReadOnly(c), Is.True);
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			Assert.That(c.CustomerName, Is.EqualTo("gavin"));
			Assert.That(c.Variations.Count, Is.EqualTo(2));
			it = c.Variations.GetEnumerator();
			it.MoveNext();
			cv1 = it.Current;
			Assert.That(cv1.Text, Is.EqualTo("expensive"));
			it.MoveNext();
			cv2 = it.Current;
			Assert.That(cv2.Text, Is.EqualTo("more expensive"));
			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			Assert.That(await (s.CreateCriteria<ContractVariation>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(3);
		}

		[Test]
		public async Task ImmutableCollectionWithMergeAsync()
		{
			ClearCounts();
			Contract c = new Contract(null, "gavin", "phone");
			ContractVariation cv1 = new ContractVariation(1, c);
			cv1.Text = "expensive";
			ContractVariation cv2 = new ContractVariation(2, c);
			cv2.Text = "more expensive";
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(3);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c.Variations.Add(new ContractVariation(3, c));
			await (s.MergeAsync(c));
			try
			{
				await (t.CommitAsync());
				Assert.Fail("should have failed because an immutable collection was changed");
			}
			catch (HibernateException)
			{
				// expected
				t.Rollback();
			}
			finally
			{
				s.Close();
			}

			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			Assert.That(c.CustomerName, Is.EqualTo("gavin"));
			Assert.That(c.Variations.Count, Is.EqualTo(2));
			IEnumerator<ContractVariation> it = c.Variations.GetEnumerator();
			it.MoveNext();
			cv1 = it.Current;
			Assert.That(cv1.Text, Is.EqualTo("expensive"));
			it.MoveNext();
			cv2 = it.Current;
			Assert.That(cv2.Text, Is.EqualTo("more expensive"));
			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			Assert.That(await (s.CreateCriteria<ContractVariation>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(3);
		}

		[Test]
		public async Task NewEntityViaImmutableEntityWithImmutableCollectionUsingSaveOrUpdateAsync()
		{
			ClearCounts();
			Contract c = new Contract(null, "gavin", "phone");
			ContractVariation cv1 = new ContractVariation(1, c);
			cv1.Text = "expensive";
			ContractVariation cv2 = new ContractVariation(2, c);
			cv2.Text = "more expensive";
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(3);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			cv1.Infos.Add(new Info("cv1 info"));
			await (s.SaveOrUpdateAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(1);
			AssertUpdateCount(0);
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			Assert.That(c.CustomerName, Is.EqualTo("gavin"));
			Assert.That(c.Variations.Count, Is.EqualTo(2));
			IEnumerator<ContractVariation> it = c.Variations.GetEnumerator();
			it.MoveNext();
			cv1 = it.Current;
			Assert.That(cv1.Text, Is.EqualTo("expensive"));
			Assert.That(cv1.Infos.Count, Is.EqualTo(1));
			Assert.That(cv1.Infos.First().Text, Is.EqualTo("cv1 info"));
			it.MoveNext();
			cv2 = it.Current;
			Assert.That(cv2.Text, Is.EqualTo("more expensive"));
			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			Assert.That(await (s.CreateCriteria<ContractVariation>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(4);
		}

		[Test]
		public async Task NewEntityViaImmutableEntityWithImmutableCollectionUsingMergeAsync()
		{
			ClearCounts();
			Contract c = new Contract(null, "gavin", "phone");
			ContractVariation cv1 = new ContractVariation(1, c);
			cv1.Text = "expensive";
			ContractVariation cv2 = new ContractVariation(2, c);
			cv2.Text = "more expensive";
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(3);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			cv1.Infos.Add(new Info("cv1 info"));
			await (s.MergeAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(1);
			AssertUpdateCount(0);
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			Assert.That(c.CustomerName, Is.EqualTo("gavin"));
			Assert.That(c.Variations.Count, Is.EqualTo(2));
			IEnumerator<ContractVariation> it = c.Variations.GetEnumerator();
			it.MoveNext();
			cv1 = it.Current;
			Assert.That(cv1.Text, Is.EqualTo("expensive"));
			Assert.That(cv1.Infos.Count, Is.EqualTo(1));
			Assert.That(cv1.Infos.First().Text, Is.EqualTo("cv1 info"));
			it.MoveNext();
			cv2 = it.Current;
			Assert.That(cv2.Text, Is.EqualTo("more expensive"));
			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			Assert.That(await (s.CreateCriteria<ContractVariation>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(4);
		}

		[Test]
		public async Task UpdatedEntityViaImmutableEntityWithImmutableCollectionUsingSaveOrUpdateAsync()
		{
			ClearCounts();
			Contract c = new Contract(null, "gavin", "phone");
			ContractVariation cv1 = new ContractVariation(1, c);
			cv1.Text = "expensive";
			Info cv1Info = new Info("cv1 info");
			cv1.Infos.Add(cv1Info);
			ContractVariation cv2 = new ContractVariation(2, c);
			cv2.Text = "more expensive";
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(4);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			cv1Info.Text = "new cv1 info";
			await (s.SaveOrUpdateAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(0);
			AssertUpdateCount(1);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			Assert.That(c.CustomerName, Is.EqualTo("gavin"));
			Assert.That(c.Variations.Count, Is.EqualTo(2));
			IEnumerator<ContractVariation> it = c.Variations.GetEnumerator();
			it.MoveNext();
			cv1 = it.Current;
			Assert.That(cv1.Text, Is.EqualTo("expensive"));
			Assert.That(cv1.Infos.Count, Is.EqualTo(1));
			Assert.That(cv1.Infos.First().Text, Is.EqualTo("new cv1 info"));
			it.MoveNext();
			cv2 = it.Current;
			Assert.That(cv2.Text, Is.EqualTo("more expensive"));
			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			Assert.That(await (s.CreateCriteria<ContractVariation>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(4);
		}

		[Test]
		public async Task UpdatedEntityViaImmutableEntityWithImmutableCollectionUsingMergeAsync()
		{
			ClearCounts();
			Contract c = new Contract(null, "gavin", "phone");
			ContractVariation cv1 = new ContractVariation(1, c);
			cv1.Text = "expensive";
			Info cv1Info = new Info("cv1 info");
			cv1.Infos.Add(cv1Info);
			ContractVariation cv2 = new ContractVariation(2, c);
			cv2.Text = "more expensive";
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(4);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			cv1Info.Text = "new cv1 info";
			await (s.MergeAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(0);
			AssertUpdateCount(1);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			Assert.That(c.CustomerName, Is.EqualTo("gavin"));
			Assert.That(c.Variations.Count, Is.EqualTo(2));
			IEnumerator<ContractVariation> it = c.Variations.GetEnumerator();
			it.MoveNext();
			cv1 = it.Current;
			Assert.That(cv1.Text, Is.EqualTo("expensive"));
			Assert.That(cv1.Infos.Count, Is.EqualTo(1));
			Assert.That(cv1.Infos.First().Text, Is.EqualTo("new cv1 info"));
			it.MoveNext();
			cv2 = it.Current;
			Assert.That(cv2.Text, Is.EqualTo("more expensive"));
			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			Assert.That(await (s.CreateCriteria<ContractVariation>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(4);
		}

		[Test]
		public async Task ImmutableEntityAddImmutableToInverseMutableCollectionAsync()
		{
			ClearCounts();
			Contract c = new Contract(null, "gavin", "phone");
			ContractVariation cv1 = new ContractVariation(1, c);
			cv1.Text = "expensive";
			ContractVariation cv2 = new ContractVariation(2, c);
			cv2.Text = "more expensive";
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			Party party = new Party("a party");
			await (s.PersistAsync(party));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(4);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c.AddParty(new Party("a new party"));
			await (s.UpdateAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(1);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c.AddParty(party);
			await (s.UpdateAsync(c));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			Assert.That(c.CustomerName, Is.EqualTo("gavin"));
			Assert.That(c.Variations.Count, Is.EqualTo(2));
			IEnumerator<ContractVariation> it = c.Variations.GetEnumerator();
			it.MoveNext();
			cv1 = it.Current;
			Assert.That(cv1.Text, Is.EqualTo("expensive"));
			it.MoveNext();
			cv2 = it.Current;
			Assert.That(cv2.Text, Is.EqualTo("more expensive"));
			// Assert.That(c.Parties.Count, Is.EqualTo(2));
			await (s.DeleteAsync(c));
			await (s.DeleteAsync(party)); // NH-specific
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			Assert.That(await (s.CreateCriteria<ContractVariation>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(5); // NH-specific
		}

		[Test]
		public async Task ImmutableEntityRemoveImmutableFromInverseMutableCollectionAsync()
		{
			ClearCounts();
			Contract c = new Contract(null, "gavin", "phone");
			ContractVariation cv1 = new ContractVariation(1, c);
			cv1.Text = "expensive";
			ContractVariation cv2 = new ContractVariation(2, c);
			cv2.Text = "more expensive";
			Party party = new Party("party1");
			c.AddParty(party);
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(4);
			AssertUpdateCount(0);
			ClearCounts();
			party = c.Parties.First();
			c.RemoveParty(party);
			s = OpenSession();
			t = s.BeginTransaction();
			await (s.UpdateAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			Assert.That(c.CustomerName, Is.EqualTo("gavin"));
			Assert.That(c.Variations.Count, Is.EqualTo(2));
			IEnumerator<ContractVariation> it = c.Variations.GetEnumerator();
			it.MoveNext();
			cv1 = it.Current;
			Assert.That(cv1.Text, Is.EqualTo("expensive"));
			it.MoveNext();
			cv2 = it.Current;
			Assert.That(cv2.Text, Is.EqualTo("more expensive"));
			//Assert.That(c.Parties.Count, Is.EqualTo(0));
			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			Assert.That(await (s.CreateCriteria<ContractVariation>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(4);
		}

		[Test]
		public async Task ImmutableEntityRemoveImmutableFromInverseMutableCollectionByDeleteAsync()
		{
			ClearCounts();
			Contract c = new Contract(null, "gavin", "phone");
			ContractVariation cv1 = new ContractVariation(1, c);
			cv1.Text = "expensive";
			ContractVariation cv2 = new ContractVariation(2, c);
			cv2.Text = "more expensive";
			Party party = new Party("party1");
			c.AddParty(party);
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(4);
			AssertUpdateCount(0);
			ClearCounts();
			party = c.Parties.First();
			s = OpenSession();
			t = s.BeginTransaction();
			await (s.DeleteAsync(party));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(1);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			Assert.That(c.CustomerName, Is.EqualTo("gavin"));
			Assert.That(c.Variations.Count, Is.EqualTo(2));
			IEnumerator<ContractVariation> it = c.Variations.GetEnumerator();
			it.MoveNext();
			cv1 = it.Current;
			Assert.That(cv1.Text, Is.EqualTo("expensive"));
			it.MoveNext();
			cv2 = it.Current;
			Assert.That(cv2.Text, Is.EqualTo("more expensive"));
			Assert.That(c.Parties.Count, Is.EqualTo(0));
			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			Assert.That(await (s.CreateCriteria<ContractVariation>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(3);
		}

		[Test]
		public async Task ImmutableEntityRemoveImmutableFromInverseMutableCollectionByDerefAsync()
		{
			ClearCounts();
			Contract c = new Contract(null, "gavin", "phone");
			ContractVariation cv1 = new ContractVariation(1, c);
			cv1.Text = "expensive";
			ContractVariation cv2 = new ContractVariation(2, c);
			cv2.Text = "more expensive";
			Party party = new Party("party1");
			c.AddParty(party);
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(4);
			AssertUpdateCount(0);
			ClearCounts();
			party = c.Parties.First();
			party.Contract = null;
			s = OpenSession();
			t = s.BeginTransaction();
			await (s.UpdateAsync(party));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			party = await (s.GetAsync<Party>(party.Id));
			Assert.That(party.Contract, Is.Not.Null);
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			Assert.That(c.CustomerName, Is.EqualTo("gavin"));
			Assert.That(c.Variations.Count, Is.EqualTo(2));
			IEnumerator<ContractVariation> it = c.Variations.GetEnumerator();
			it.MoveNext();
			cv1 = it.Current;
			Assert.That(cv1.Text, Is.EqualTo("expensive"));
			it.MoveNext();
			cv2 = it.Current;
			Assert.That(cv2.Text, Is.EqualTo("more expensive"));
			Assert.That(c.Parties.Count, Is.EqualTo(1));
			party = c.Parties.First();
			Assert.That(party.Name, Is.EqualTo("party1"));
			Assert.That(party.Contract, Is.SameAs(c));
			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			Assert.That(await (s.CreateCriteria<ContractVariation>().SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(0));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(4);
		}
	}
}
#endif
