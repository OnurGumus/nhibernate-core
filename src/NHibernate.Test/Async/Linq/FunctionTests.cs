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
		public void LikeFunction()
		{
			var query = (
				from e in db.Employees
				where NHibernate.Linq.SqlMethods.Like(e.FirstName, "Ma%et")select e).ToList();
			Assert.That(query.Count, Is.EqualTo(1));
			Assert.That(query[0].FirstName, Is.EqualTo("Margaret"));
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private static class SqlMethods
		{
			public static bool Like(string expression, string pattern)
			{
				throw new NotImplementedException();
			}
		}

		[Test]
		public void LikeFunctionUserDefined()
		{
			// Verify that any method named Like, in a class named SqlMethods, will be translated.
			var query = (
				from e in db.Employees
				where NHibernate.Test.Linq.FunctionTestsAsync.SqlMethods.Like(e.FirstName, "Ma%et")select e).ToList();
			Assert.That(query.Count, Is.EqualTo(1));
			Assert.That(query[0].FirstName, Is.EqualTo("Margaret"));
		}

		[Test]
		public void SubstringFunction2()
		{
			var query = (
				from e in db.Employees
				where e.FirstName.Substring(0, 2) == "An"
				select e).ToList();
			Assert.That(query.Count, Is.EqualTo(2));
		}

		[Test]
		public void SubstringFunction1()
		{
			var query = (
				from e in db.Employees
				where e.FirstName.Substring(3) == "rew"
				select e).ToList();
			Assert.That(query.Count, Is.EqualTo(1));
			Assert.That(query[0].FirstName, Is.EqualTo("Andrew"));
		}

		[Test]
		public void LeftFunction()
		{
			var query = (
				from e in db.Employees
				where e.FirstName.Substring(0, 2) == "An"
				select e.FirstName.Substring(3)).ToList();
			Assert.That(query.Count, Is.EqualTo(2));
			Assert.That(query[0], Is.EqualTo("rew")); //Andrew
			Assert.That(query[1], Is.EqualTo("e")); //Anne
		}

		[Test]
		public async Task ReplaceFunctionAsync()
		{
			var query =
				from e in db.Employees
				where e.FirstName.StartsWith("An")select new
				{
				Before = e.FirstName, AfterMethod = e.FirstName.Replace("An", "Zan"), AfterExtension = ExtensionMethods.Replace(e.FirstName, "An", "Zan"), AfterExtension2 = e.FirstName.ReplaceExtension("An", "Zan")}

			;
			var s = await (ObjectDumper.WriteAsync(query));
		}

		[Test]
		public async Task CharIndexFunctionAsync()
		{
			if (!TestDialect.SupportsLocate)
				Assert.Ignore("Locate function not supported.");
			var query =
				from e in db.Employees
				where e.FirstName.IndexOf('A') == 1
				select e.FirstName;
			await (ObjectDumper.WriteAsync(query));
		}

		[Test]
		public async Task IndexOfFunctionExpressionAsync()
		{
			if (!TestDialect.SupportsLocate)
				Assert.Ignore("Locate function not supported.");
			var query =
				from e in db.Employees
				where e.FirstName.IndexOf("An") == 1
				select e.FirstName;
			await (ObjectDumper.WriteAsync(query));
		}

		[Test]
		public async Task IndexOfFunctionProjectionAsync()
		{
			if (!TestDialect.SupportsLocate)
				Assert.Ignore("Locate function not supported.");
			var query =
				from e in db.Employees
				where e.FirstName.Contains("a")select e.FirstName.IndexOf('A', 3);
			await (ObjectDumper.WriteAsync(query));
		}

		[Test]
		public async Task TwoFunctionExpressionAsync()
		{
			if (!TestDialect.SupportsLocate)
				Assert.Ignore("Locate function not supported.");
			var query =
				from e in db.Employees
				where e.FirstName.IndexOf("A") == e.BirthDate.Value.Month
				select e.FirstName;
			await (ObjectDumper.WriteAsync(query));
		}

		[Test]
		public void ToStringFunction()
		{
			var query =
				from ol in db.OrderLines
				where ol.Quantity.ToString() == "4"
				select ol;
			Assert.AreEqual(55, query.Count());
		}

		[Test]
		public void ToStringWithContains()
		{
			var query =
				from ol in db.OrderLines
				where ol.Quantity.ToString().Contains("5")select ol;
			Assert.AreEqual(498, query.Count());
		}

		[Test]
		public void Coalesce()
		{
			Assert.AreEqual(2, session.Query<AnotherEntity>().Where(e => (e.Input ?? "hello") == "hello").Count());
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
				Assert.AreEqual(2, session.Query<AnotherEntity>().Where(e => e.Input.Trim() == "hi").Count());
				Assert.AreEqual(1, session.Query<AnotherEntity>().Where(e => e.Input.TrimEnd() == " hi").Count());
				// Emulated trim does not support multiple trim characters, but for many databases it should work fine anyways.
				Assert.AreEqual(1, session.Query<AnotherEntity>().Where(e => e.Input.Trim('h') == "e").Count());
				Assert.AreEqual(1, session.Query<AnotherEntity>().Where(e => e.Input.TrimStart('h') == "eh").Count());
				Assert.AreEqual(1, session.Query<AnotherEntity>().Where(e => e.Input.TrimEnd('h') == "he").Count());
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
				Assert.AreEqual(TestDialect.IgnoresTrailingWhitespace ? 2 : 1, session.Query<AnotherEntity>().Where(e => e.Input.TrimStart() == "hi ").Count());
			// Let it rollback to get rid of temporary changes.
			}
		}

		[Test]
		public async Task WhereStringEqualAsync()
		{
			var query = (
				from item in db.Users
				where item.Name.Equals("ayende")select item).ToList();
			await (ObjectDumper.WriteAsync(query));
		}

		[Test, Description("NH-3367")]
		public async Task WhereStaticStringEqualAsync()
		{
			var query = (
				from item in db.Users
				where string.Equals(item.Name, "ayende")select item).ToList();
			await (ObjectDumper.WriteAsync(query));
		}

		[Test]
		public async Task WhereIntEqualAsync()
		{
			var query = (
				from item in db.Users
				where item.Id.Equals(-1)select item).ToList();
			await (ObjectDumper.WriteAsync(query));
		}

		[Test]
		public async Task WhereShortEqualAsync()
		{
			var query =
				from item in session.Query<Foo>()where item.Short.Equals(-1)select item;
			await (ObjectDumper.WriteAsync(query));
		}

		[Test]
		public async Task WhereBoolConstantEqualAsync()
		{
			var query =
				from item in db.Role
				where item.IsActive.Equals(true)select item;
			await (ObjectDumper.WriteAsync(query));
		}

		[Test]
		public async Task WhereBoolParameterEqualAsync()
		{
			var query =
				from item in db.Role
				where item.IsActive.Equals(1 == 1)select item;
			await (ObjectDumper.WriteAsync(query));
		}

		[Test]
		public async Task WhereBoolFuncEqualAsync()
		{
			Func<bool> f = () => 1 == 1;
			var query =
				from item in db.Role
				where item.IsActive.Equals(f())select item;
			await (ObjectDumper.WriteAsync(query));
		}

		[Test]
		public async Task WhereLongEqualAsync()
		{
			var query =
				from item in db.PatientRecords
				where item.Id.Equals(-1)select item;
			await (ObjectDumper.WriteAsync(query));
		}

		[Test]
		public async Task WhereDateTimeEqualAsync()
		{
			var query =
				from item in db.Users
				where item.RegisteredAt.Equals(DateTime.Today)select item;
			await (ObjectDumper.WriteAsync(query));
		}

		[Test]
		public async Task WhereGuidEqualAsync()
		{
			var query =
				from item in db.Shippers
				where item.Reference.Equals(Guid.Empty)select item;
			await (ObjectDumper.WriteAsync(query));
		}

		[Test]
		public async Task WhereDoubleEqualAsync()
		{
			var query =
				from item in db.Animals
				where item.BodyWeight.Equals(-1)select item;
			await (ObjectDumper.WriteAsync(query));
		}

		[Test]
		public async Task WhereFloatEqualAsync()
		{
			var query =
				from item in session.Query<Foo>()where item.Float.Equals(-1)select item;
			await (ObjectDumper.WriteAsync(query));
		}

		[Test]
		public async Task WhereCharEqualAsync()
		{
			var query =
				from item in session.Query<Foo>()where item.Char.Equals('A')select item;
			await (ObjectDumper.WriteAsync(query));
		}

		[Test]
		public async Task WhereByteEqualAsync()
		{
			var query =
				from item in session.Query<Foo>()where item.Byte.Equals(1)select item;
			await (ObjectDumper.WriteAsync(query));
		}

		[Test]
		public async Task WhereDecimalEqualAsync()
		{
			var query =
				from item in db.OrderLines
				where item.Discount.Equals(-1)select item;
			await (ObjectDumper.WriteAsync(query));
		}
	}
}
#endif
