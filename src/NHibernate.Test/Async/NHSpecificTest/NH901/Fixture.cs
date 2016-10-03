#if NET_4_5
using System.Collections;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using NHibernate.Type;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH901
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class FixtureBaseAsync : TestCaseAsync
	{
		private new ISession OpenSession(IInterceptor interceptor)
		{
			lastOpenedSession = sessions.OpenSession(interceptor);
			return lastOpenedSession;
		}

		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from Person"));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task EmptyValueTypeComponentAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					Person p = new Person("Jimmy Hendrix");
					await (s.SaveAsync(p));
					await (tx.CommitAsync());
				}

			InterceptorStub interceptor = new InterceptorStub();
			using (ISession s = OpenSession(interceptor))
				using (ITransaction tx = s.BeginTransaction())
				{
					Person jimmy = await (s.GetAsync<Person>("Jimmy Hendrix"));
					interceptor.entityToCheck = jimmy;
					await (tx.CommitAsync());
				}

			Assert.IsFalse(interceptor.entityWasDeemedDirty);
			InterceptorStub interceptor2 = new InterceptorStub();
			using (ISession s = OpenSession(interceptor2))
				using (ITransaction tx = s.BeginTransaction())
				{
					Person jimmy = await (s.GetAsync<Person>("Jimmy Hendrix"));
					jimmy.Address = new Address();
					interceptor.entityToCheck = jimmy;
					await (tx.CommitAsync());
				}

			Assert.IsFalse(interceptor2.entityWasDeemedDirty);
		}

		[Test]
		public async Task ReplaceValueTypeComponentWithSameValueDoesNotMakeItDirtyAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					Person p = new Person("Jimmy Hendrix");
					Address a = new Address();
					a.Street = "Some Street";
					a.City = "Some City";
					p.Address = a;
					await (s.SaveAsync(p));
					await (tx.CommitAsync());
				}

			InterceptorStub interceptor = new InterceptorStub();
			using (ISession s = OpenSession(interceptor))
				using (ITransaction tx = s.BeginTransaction())
				{
					Person jimmy = await (s.GetAsync<Person>("Jimmy Hendrix"));
					interceptor.entityToCheck = jimmy;
					Address a = new Address();
					a.Street = "Some Street";
					a.City = "Some City";
					jimmy.Address = a;
					Assert.AreNotSame(jimmy.Address, a);
					await (tx.CommitAsync());
				}

			Assert.IsFalse(interceptor.entityWasDeemedDirty);
		}
	}
}
#endif
