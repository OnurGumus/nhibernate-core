#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Transform;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.TransformTests
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class AliasToBeanResultTransformerFixture : TestCase
	{
		[Test]
		public async Task WorkWithOutPublicParameterLessCtorAsync()
		{
			try
			{
				await (SetupAsync());
				using (ISession s = OpenSession())
				{
					IList<WithOutPublicParameterLessCtor> l = s.CreateSQLQuery("select s.Name as something from Simple s").SetResultTransformer(Transformers.AliasToBean<WithOutPublicParameterLessCtor>()).List<WithOutPublicParameterLessCtor>();
					Assert.That(l.Count, Is.EqualTo(2));
					Assert.That(l, Has.All.Not.Null);
				}
			}
			finally
			{
				await (CleanupAsync());
			}
		}

		[Test]
		public async Task WorkWithPublicParameterLessCtorAsync()
		{
			try
			{
				await (SetupAsync());
				var queryString = "select s.Name as something from Simple s";
				AssertAreWorking(queryString); // working for field access
				queryString = "select s.Name as Something from Simple s";
				AssertAreWorking(queryString); // working for property access
			}
			finally
			{
				await (CleanupAsync());
			}
		}

		[Test]
		public async Task WorksWithStructAsync()
		{
			try
			{
				await (SetupAsync());
				IList<TestStruct> result;
				using (ISession s = OpenSession())
				{
					result = s.CreateSQLQuery("select s.Name as something from Simple s").SetResultTransformer(Transformers.AliasToBean<TestStruct>()).List<TestStruct>();
				}

				Assert.AreEqual(2, result.Count);
			}
			finally
			{
				await (CleanupAsync());
			}
		}

		private async Task CleanupAsync()
		{
			using (ISession s = OpenSession())
			{
				using (s.BeginTransaction())
				{
					await (s.DeleteAsync("from Simple"));
					await (s.Transaction.CommitAsync());
				}
			}
		}

		private async Task SetupAsync()
		{
			using (ISession s = OpenSession())
			{
				using (s.BeginTransaction())
				{
					await (s.SaveAsync(new Simple{Name = "Name1"}));
					await (s.SaveAsync(new Simple{Name = "Name2"}));
					await (s.Transaction.CommitAsync());
				}
			}
		}
	}
}
#endif
