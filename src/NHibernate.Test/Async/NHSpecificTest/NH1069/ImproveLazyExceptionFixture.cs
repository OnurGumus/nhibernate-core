#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1069
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ImproveLazyExceptionFixture : BugTestCase
	{
		[Test]
		public async Task LazyEntityAsync()
		{
			object savedId = 1;
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.SaveAsync(new LazyE(), 1));
					await (t.CommitAsync());
				}

			LazyE le;
			using (ISession s = OpenSession())
			{
				le = await (s.LoadAsync<LazyE>(savedId));
			}

			string n;
			var ex = Assert.Throws<LazyInitializationException>(() => n = le.Name);
			Assert.That(ex.EntityName, Is.EqualTo(typeof (LazyE).FullName));
			Assert.That(ex.EntityId, Is.EqualTo(1));
			Assert.That(ex.Message, Is.StringContaining(typeof (LazyE).FullName));
			Assert.That(ex.Message, Is.StringContaining("#1"));
			Console.WriteLine(ex.Message);
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.CreateQuery("delete from LazyE").ExecuteUpdateAsync());
					await (t.CommitAsync());
				}
		}

		[Test]
		public async Task LazyCollectionAsync()
		{
			object savedId = 1;
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.SaveAsync(new LazyE(), savedId));
					await (t.CommitAsync());
				}

			LazyE le;
			using (ISession s = OpenSession())
			{
				le = await (s.GetAsync<LazyE>(savedId));
			}

			var ex = Assert.Throws<LazyInitializationException>(() => le.LazyC.GetEnumerator());
			Assert.That(ex.EntityName, Is.EqualTo(typeof (LazyE).FullName));
			Assert.That(ex.EntityId, Is.EqualTo(1));
			Assert.That(ex.Message, Is.StringContaining(typeof (LazyE).FullName));
			Assert.That(ex.Message, Is.StringContaining("#1"));
			Assert.That(ex.Message, Is.StringContaining(typeof (LazyE).FullName + ".LazyC"));
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.CreateQuery("delete from LazyE").ExecuteUpdateAsync());
					await (t.CommitAsync());
				}
		}
	}
}
#endif
