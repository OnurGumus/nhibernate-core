#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH440
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "NH440";
			}
		}

		protected override IList Mappings
		{
			get
			{
				return new string[]{"NHSpecificTest.NH440.Fruit.hbm.xml", "NHSpecificTest.NH440.Apple.hbm.xml"};
			}
		}

		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (ISession session = OpenSession())
				using (ITransaction t = session.BeginTransaction())
				{
					// pump up the ids for one of the classes to avoid the tests passing coincidentally
					for (int i = 0; i < 10; i++)
					{
						await (session.SaveAsync(new Fruit()));
					}

					await (session.DeleteAsync("from System.Object")); // clear everything from database
					await (t.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction t = session.BeginTransaction())
				{
					await (session.DeleteAsync("from System.Object")); // clear everything from database
					await (t.CommitAsync());
				}

			await (base.OnTearDownAsync());
		}

		[Test]
		public async Task StoreAndLookupAsync()
		{
			Apple apple = new Apple();
			Fruit fruit = new Fruit();
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.SaveAsync(apple));
					await (session.SaveAsync(fruit));
					Assert.IsNotNull(await (session.GetAsync(typeof (Apple), apple.Id)));
					Assert.IsNotNull(await (session.GetAsync(typeof (Fruit), fruit.Id)));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task StoreWithLinksAndLookupAsync()
		{
			Apple apple = new Apple();
			Fruit fruit = new Fruit();
			apple.TheFruit = fruit;
			fruit.TheApple = apple;
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.SaveAsync(apple));
					await (session.SaveAsync(fruit));
					await (tx.CommitAsync());
				}

			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					Apple apple2 = (Apple)await (session.GetAsync(typeof (Apple), apple.Id));
					Fruit fruit2 = (Fruit)await (session.GetAsync(typeof (Fruit), fruit.Id));
					Assert.IsNotNull(apple2);
					Assert.IsNotNull(fruit2);
					Assert.AreSame(apple2, fruit2.TheApple);
					Assert.AreSame(fruit2, apple2.TheFruit);
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task StoreWithLinksAndLookupWithQueryFromFruitAsync()
		{
			Apple apple = new Apple();
			Fruit fruit = new Fruit();
			apple.TheFruit = fruit;
			fruit.TheApple = apple;
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.SaveAsync(apple));
					await (session.SaveAsync(fruit));
					await (tx.CommitAsync());
				}

			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					Fruit fruit2 = (Fruit)await (session.GetAsync(typeof (Fruit), fruit.Id));
					Assert.IsNotNull(fruit2);
					IList results = await (session.CreateQuery("from Apple a where a.TheFruit = ?").SetParameter(0, fruit2).ListAsync());
					Assert.AreEqual(1, results.Count);
					Apple apple2 = (Apple)results[0];
					Assert.IsNotNull(apple2);
					Assert.AreSame(apple2, fruit2.TheApple);
					Assert.AreSame(fruit2, apple2.TheFruit);
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
