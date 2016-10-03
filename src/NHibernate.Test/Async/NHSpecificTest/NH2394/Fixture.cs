#if NET_4_5
using System;
using System.Collections.Generic;
using NHibernate.Criterion;
using NUnit.Framework;
using NHibernate.Linq;
using System.Linq;
using NHibernate.Linq.Functions;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2394
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnTearDownAsync()
		{
			using (ISession s = sessions.OpenSession())
			{
				await (s.DeleteAsync("from A"));
				await (s.FlushAsync());
			}
		}

		[Test]
		public async Task LinqUserTypeEqualityAsync()
		{
			ISession s = OpenSession();
			try
			{
				await (s.SaveAsync(new A{Type = TypeOfA.Awesome, Phone = new PhoneNumber(1, "555-1111")}));
				await (s.SaveAsync(new A{Type = TypeOfA.Boring, NullableType = TypeOfA.Awesome, Phone = new PhoneNumber(1, "555-2222")}));
				await (s.SaveAsync(new A{Type = TypeOfA.Cool, Phone = new PhoneNumber(1, "555-3333")}));
				await (s.FlushAsync());
			}
			finally
			{
				s.Close();
			}

			s = OpenSession();
			try
			{
				A item;
				Assert.AreEqual(3, (await (s.CreateQuery("from A a where a.IsNice = ?").SetParameter(0, false).ListAsync())).Count);
				Assert.AreEqual(3, s.Query<A>().Count(a => a.IsNice == false));
				item = await (s.CreateQuery("from A a where a.Type = ?").SetParameter(0, TypeOfA.Awesome).UniqueResultAsync<A>());
				Assert.AreEqual(TypeOfA.Awesome, item.Type);
				Assert.AreEqual("555-1111", item.Phone.Number);
				item = s.Query<A>().Where(a => a.Type == TypeOfA.Awesome).Single();
				Assert.AreEqual(TypeOfA.Awesome, item.Type);
				Assert.AreEqual("555-1111", item.Phone.Number);
				item = s.Query<A>().Where(a => TypeOfA.Awesome == a.Type).Single();
				Assert.AreEqual(TypeOfA.Awesome, item.Type);
				Assert.AreEqual("555-1111", item.Phone.Number);
				IA interfaceItem = s.Query<IA>().Where(a => a.Type == TypeOfA.Awesome).Single();
				Assert.AreEqual(TypeOfA.Awesome, interfaceItem.Type);
				Assert.AreEqual("555-1111", interfaceItem.Phone.Number);
				item = await (s.CreateQuery("from A a where a.NullableType = ?").SetParameter(0, TypeOfA.Awesome).UniqueResultAsync<A>());
				Assert.AreEqual(TypeOfA.Boring, item.Type);
				Assert.AreEqual("555-2222", item.Phone.Number);
				Assert.AreEqual(TypeOfA.Awesome, item.NullableType);
				item = s.Query<A>().Where(a => a.NullableType == TypeOfA.Awesome).Single();
				Assert.AreEqual(TypeOfA.Boring, item.Type);
				Assert.AreEqual("555-2222", item.Phone.Number);
				Assert.AreEqual(TypeOfA.Awesome, item.NullableType);
				Assert.AreEqual(2, s.Query<A>().Count(a => a.NullableType == null));
				item = await (s.CreateQuery("from A a where a.Phone = ?").SetParameter(0, new PhoneNumber(1, "555-2222")).UniqueResultAsync<A>());
				Assert.AreEqual(TypeOfA.Boring, item.Type);
				Assert.AreEqual("555-2222", item.Phone.Number);
				item = s.Query<A>().Where(a => a.Phone == new PhoneNumber(1, "555-2222")).Single();
				Assert.AreEqual(TypeOfA.Boring, item.Type);
				Assert.AreEqual("555-2222", item.Phone.Number);
			}
			finally
			{
				s.Close();
			}
		}
	}
}
#endif
