#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.QueryTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class PositionalParametersFixtureAsync : TestCaseAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new string[]{"Simple.hbm.xml"};
			}
		}

		[Test]
		public async Task TestMissingHQLParametersAsync()
		{
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			try
			{
				IQuery q = s.CreateQuery("from s in class Simple where s.Name=? and s.Count=?");
				// Set the first property, but not the second
				q.SetParameter(0, "Fred");
				// Try to execute it
				Assert.ThrowsAsync<QueryException>(async () => await (q.ListAsync()));
			}
			finally
			{
				t.Rollback();
				s.Close();
			}
		}

		[Test]
		public async Task TestMissingHQLParameters2Async()
		{
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			try
			{
				IQuery q = s.CreateQuery("from s in class Simple where s.Name=? and s.Count=?");
				// Set the second property, but not the first - should give a nice not found at position xxx error
				q.SetParameter(1, "Fred");
				// Try to execute it
				Assert.ThrowsAsync<QueryException>(async () => await (q.ListAsync()));
			}
			finally
			{
				t.Rollback();
				s.Close();
			}
		}

		[Test]
		public Task TestPositionOutOfBoundsAsync()
		{
			try
			{
				ISession s = OpenSession();
				ITransaction t = s.BeginTransaction();
				try
				{
					IQuery q = s.CreateQuery("from s in class Simple where s.Name=? and s.Count=?");
					// Try to set the third positional parameter
					Assert.Throws<ArgumentException>(() => q.SetParameter(3, "Fred"));
				}
				finally
				{
					t.Rollback();
					s.Close();
				}

				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public Task TestNoPositionalParametersAsync()
		{
			try
			{
				ISession s = OpenSession();
				ITransaction t = s.BeginTransaction();
				try
				{
					IQuery q = s.CreateQuery("from s in class Simple where s.Name=:Name and s.Count=:Count");
					// Try to set the first property
					Assert.Throws<ArgumentException>(() => q.SetParameter(0, "Fred"));
				}
				finally
				{
					t.Rollback();
					s.Close();
				}

				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		/// <summary>
		/// Verifying that a <see langword = "null"/> value passed into SetParameter(index, val) throws
		/// an exception
		/// </summary>
		[Test]
		public Task TestNullIndexedParameterAsync()
		{
			try
			{
				ISession s = OpenSession();
				try
				{
					IQuery q = s.CreateQuery("from Simple as s where s.Name=?");
					Assert.Throws<ArgumentNullException>(() => q.SetParameter(0, null));
				}
				finally
				{
					s.Close();
				}

				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}
#endif
