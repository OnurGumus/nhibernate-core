#if NET_4_5
using NHibernate.Cfg;
using NHibernate.Event;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH1230
{
	[TestFixture, Ignore("TODO(Dario)This test demostrate the need of eliminate the 'bool' on pre-insert eventlisteners.")]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "NH1230";
			}
		}

		protected override Task ConfigureAsync(Configuration cfg)
		{
			try
			{
				cfg.SetListener(ListenerType.PreInsert, new PreSaveDoVeto());
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		/// <summary>
		/// Must be vetoed without thrown an <see cref = "AssertionFailure"/> exception.
		/// As no identifier generation has made, the id is null and is not posible create a EntityKey instance to agregate
		/// to the PersistenceContext (don't know why registrate a vetoed entity to PC, that's the reason of make the method: void
		/// </summary>
		[Test]
		public async Task NoExceptionMustBeThrown1Async()
		{
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					FooIdentity f = new FooIdentity();
					f.Description = "f1";
					f.Id = 1;
					await (s.SaveAsync(f));
					await (tx.CommitAsync());
				}
			}
		}

		/// <summary>
		/// Must be vetoed without thrown an <see cref = "NonUniqueObjectException"/> exception.
		/// The second time the entity must be vetoed again, but not registration on PersistenceContext must added.
		/// The identity map contains the first that has vetoed.
		/// </summary>
		[Test]
		public async Task NoExceptionMustBeThrown2Async()
		{
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					FooAssigned f = new FooAssigned();
					f.Description = "f1";
					f.Id = 1;
					await (s.SaveAsync(f));
					await (tx.CommitAsync());
				}

				using (ITransaction tx = s.BeginTransaction())
				{
					FooAssigned f = new FooAssigned();
					f.Description = "f2";
					f.Id = 1;
					await (s.SaveAsync(f));
					await (tx.CommitAsync());
				}
			}
		}
	}
}
#endif
