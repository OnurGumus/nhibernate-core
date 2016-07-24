#if NET_4_5
using System;
using System.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3221
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class WeirdBehaviourAsync : BugTestCaseAsync
	{
		private Guid nicePersonId;
		[Test]
		public async Task CanAddATodoAsync()
		{
			using (ISession session = OpenSession())
			{
				var person = new Person("myName");
				person.AddTodo(new Todo(person)
				{Name = "I need to get it"});
				await (session.SaveAsync(person));
				nicePersonId = person.Id;
				Assert.AreEqual(1, person.Todos.Count());
				await (session.FlushAsync());
			}

			using (ISession session = OpenSession())
			{
				var person = await (session.GetAsync<Person>(nicePersonId));
				Assert.AreEqual(1, person.Todos.Count());
				Assert.AreEqual(person.Todos.ToList()[0].Person.Id, person.Id);
			}
		}

		[Test]
		public async Task CanRemoveATodoAsync()
		{
			Todo myTodo;
			using (ISession session = OpenSession())
			{
				var person = new Person("myName2");
				myTodo = person.AddTodo(new Todo(person)
				{Name = "I need to get it"});
				await (session.SaveAsync(person));
				nicePersonId = person.Id;
				Assert.AreEqual(1, person.Todos.Count());
				await (session.FlushAsync());
			}

			using (ISession session = OpenSession())
			{
				var person = await (session.GetAsync<Person>(nicePersonId));
				Assert.AreEqual(1, person.Todos.Count());
				person.RemoveTodo(myTodo);
				Assert.AreEqual(0, person.Todos.Count());
				await (session.FlushAsync());
			}

			using (ISession session = OpenSession())
			{
				var person = await (session.GetAsync<Person>(nicePersonId));
				Assert.AreEqual(0, person.Todos.Count());
			}
		}

		[Test]
		public async Task CanAddStuffAsync()
		{
			using (ISession session = OpenSession())
			{
				var person = new Person("myName3");
				person.AddStuff(new Stuff(person)
				{Name = "this pen is mine"});
				await (session.SaveAsync(person));
				nicePersonId = person.Id;
				Assert.AreEqual(1, person.MyStuff.Count());
				await (session.FlushAsync());
			}

			using (ISession session = OpenSession())
			{
				var person = await (session.GetAsync<Person>(nicePersonId));
				Assert.AreEqual(1, person.MyStuff.Count());
				Assert.AreEqual(person.MyStuff.ToList()[0].Person.Id, person.Id);
			}
		}

		[Test]
		public async Task CanRemoveStuffAsync()
		{
			Stuff myStuff;
			using (ISession session = OpenSession())
			{
				var person = new Person("MyName4");
				myStuff = person.AddStuff(new Stuff(person)
				{Name = "BallPen"});
				await (session.SaveAsync(person));
				nicePersonId = person.Id;
				Assert.AreEqual(1, person.MyStuff.Count());
				await (session.FlushAsync());
			}

			using (ISession session = OpenSession())
			{
				var person = await (session.GetAsync<Person>(nicePersonId));
				Assert.AreEqual(1, person.MyStuff.Count());
				person.RemoveStuff(myStuff);
				Assert.AreEqual(0, person.MyStuff.Count());
				await (session.FlushAsync());
			}

			using (ISession session = OpenSession())
			{
				var person = await (session.GetAsync<Person>(nicePersonId));
				Assert.AreEqual(0, person.MyStuff.Count());
			}
		}

		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			Console.WriteLine("=====================BEGIN TEST");
		}

		protected override async Task OnTearDownAsync()
		{
			Console.WriteLine("=====================END TEST");
			await (base.OnTearDownAsync());
			using (ISession session = OpenSession())
			{
				const string hql = "from System.Object";
				await (session.DeleteAsync(hql));
				await (session.FlushAsync());
			}
		}
	}
}
#endif
