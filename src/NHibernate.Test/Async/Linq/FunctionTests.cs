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
		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private static partial class SqlMethods
		{
			public static bool Like(string expression, string pattern)
			{
				throw new NotImplementedException();
			}
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
	}
}
#endif
