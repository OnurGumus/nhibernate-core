#if NET_4_5
using System;
using System.Linq;
using System.Collections.Generic;
using NHibernate.Dialect;
using NHibernate.DomainModel;
using NHibernate.DomainModel.Northwind.Entities;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.Linq
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FunctionTestsAsync : LinqTestCaseAsync
	{
		[Test]
		public async Task LikeFunctionAsync()
		{
			var query = await ((
				from e in db.Employees
				where NHibernate.Linq.SqlMethods.Like(e.FirstName, "Ma%et")select e).ToListAsync());
			Assert.That(query.Count, Is.EqualTo(1));
			Assert.That(query[0].FirstName, Is.EqualTo("Margaret"));
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private static partial class SqlMethods
		{
			public static bool Like(string expression, string pattern)
			{
				throw new NotImplementedException();
			}
		}

		[Test]
		public async Task LikeFunctionUserDefinedAsync()
		{
			// Verify that any method named Like, in a class named SqlMethods, will be translated.
			var query = await ((
				from e in db.Employees
				where NHibernate.Test.Linq.FunctionTestsAsync.SqlMethods.Like(e.FirstName, "Ma%et")select e).ToListAsync());
			Assert.That(query.Count, Is.EqualTo(1));
			Assert.That(query[0].FirstName, Is.EqualTo("Margaret"));
		}

		[Test]
		public async Task SubstringFunction2Async()
		{
			var query = await ((
				from e in db.Employees
				where e.FirstName.Substring(0, 2) == "An"
				select e).ToListAsync());
			Assert.That(query.Count, Is.EqualTo(2));
		}

		[Test]
		public async Task SubstringFunction1Async()
		{
			var query = await ((
				from e in db.Employees
				where e.FirstName.Substring(3) == "rew"
				select e).ToListAsync());
			Assert.That(query.Count, Is.EqualTo(1));
			Assert.That(query[0].FirstName, Is.EqualTo("Andrew"));
		}

		[Test]
		public async Task LeftFunctionAsync()
		{
			var query = await ((
				from e in db.Employees
				where e.FirstName.Substring(0, 2) == "An"
				select e.FirstName.Substring(3)).ToListAsync());
			Assert.That(query.Count, Is.EqualTo(2));
			Assert.That(query[0], Is.EqualTo("rew")); //Andrew
			Assert.That(query[1], Is.EqualTo("e")); //Anne
		}

		[Test]
		public async Task ToStringFunctionAsync()
		{
			var query =
				from ol in db.OrderLines
				where ol.Quantity.ToString() == "4"
				select ol;
			Assert.AreEqual(55, await (query.CountAsync()));
		}

		[Test]
		public async Task ToStringWithContainsAsync()
		{
			var query =
				from ol in db.OrderLines
				where ol.Quantity.ToString().Contains("5")select ol;
			Assert.AreEqual(498, await (query.CountAsync()));
		}

		[Test]
		public async Task CoalesceAsync()
		{
			Assert.AreEqual(2, await (session.Query<AnotherEntity>().Where(e => (e.Input ?? "hello") == "hello").CountAsync()));
		}

		[Test]
		public async Task TrimAsync()
		{
			using (session.BeginTransaction())
			{
				AnotherEntity ae1 = new AnotherEntity{Input = " hi "};
				AnotherEntity ae2 = new AnotherEntity{Input = "hi"};
				AnotherEntity ae3 = new AnotherEntity{Input = "heh"};
				await (session.SaveAsync(ae1));
				await (session.SaveAsync(ae2));
				await (session.SaveAsync(ae3));
				await (session.FlushAsync());
				Assert.AreEqual(2, await (session.Query<AnotherEntity>().Where(e => e.Input.Trim() == "hi").CountAsync()));
				Assert.AreEqual(1, await (session.Query<AnotherEntity>().Where(e => e.Input.TrimEnd() == " hi").CountAsync()));
				// Emulated trim does not support multiple trim characters, but for many databases it should work fine anyways.
				Assert.AreEqual(1, await (session.Query<AnotherEntity>().Where(e => e.Input.Trim('h') == "e").CountAsync()));
				Assert.AreEqual(1, await (session.Query<AnotherEntity>().Where(e => e.Input.TrimStart('h') == "eh").CountAsync()));
				Assert.AreEqual(1, await (session.Query<AnotherEntity>().Where(e => e.Input.TrimEnd('h') == "he").CountAsync()));
			// Let it rollback to get rid of temporary changes.
			}
		}

		[Test, Ignore("")]
		public async Task TrimTrailingWhitespaceAsync()
		{
			using (session.BeginTransaction())
			{
				await (session.SaveAsync(new AnotherEntity{Input = " hi "}));
				await (session.SaveAsync(new AnotherEntity{Input = "hi"}));
				await (session.SaveAsync(new AnotherEntity{Input = "heh"}));
				await (session.FlushAsync());
				Assert.AreEqual(TestDialect.IgnoresTrailingWhitespace ? 2 : 1, await (session.Query<AnotherEntity>().Where(e => e.Input.TrimStart() == "hi ").CountAsync()));
			// Let it rollback to get rid of temporary changes.
			}
		}

		[Test]
		public async Task WhereStringEqualAsync()
		{
			var query = await ((
				from item in db.Users
				where item.Name.Equals("ayende")select item).ToListAsync());
			ObjectDumper.Write(query);
		}

		[Test, Description("NH-3367")]
		public async Task WhereStaticStringEqualAsync()
		{
			var query = await ((
				from item in db.Users
				where string.Equals(item.Name, "ayende")select item).ToListAsync());
			ObjectDumper.Write(query);
		}

		[Test]
		public async Task WhereIntEqualAsync()
		{
			var query = await ((
				from item in db.Users
				where item.Id.Equals(-1)select item).ToListAsync());
			ObjectDumper.Write(query);
		}
	}
}
#endif
