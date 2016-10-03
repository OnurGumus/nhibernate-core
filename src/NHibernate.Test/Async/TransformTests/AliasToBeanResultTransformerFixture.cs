#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Transform;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.TransformTests
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class AliasToBeanResultTransformerFixtureAsync : TestCaseAsync
	{
		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class WithOutPublicParameterLessCtor
		{
			private string something;
			protected WithOutPublicParameterLessCtor()
			{
			}

			public WithOutPublicParameterLessCtor(string something)
			{
				this.something = something;
			}

			public string Something
			{
				get
				{
					return something;
				}
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class PublicParameterLessCtor
		{
			private string something;
			public string Something
			{
				get
				{
					return something;
				}

				set
				{
					something = value;
				}
			}
		}

		public struct TestStruct
		{
			public string Something
			{
				get;
				set;
			}
		}

		protected override IList Mappings
		{
			get
			{
				return new[]{"TransformTests.Simple.hbm.xml"};
			}
		}

		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		[Test]
		public async Task WorkWithOutPublicParameterLessCtorAsync()
		{
			try
			{
				await (SetupAsync());
				using (ISession s = OpenSession())
				{
					IList<WithOutPublicParameterLessCtor> l = await (s.CreateSQLQuery("select s.Name as something from Simple s").SetResultTransformer(Transformers.AliasToBean<WithOutPublicParameterLessCtor>()).ListAsync<WithOutPublicParameterLessCtor>());
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
				await (AssertAreWorkingAsync(queryString)); // working for field access
				queryString = "select s.Name as Something from Simple s";
				await (AssertAreWorkingAsync(queryString)); // working for property access
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
					result = await (s.CreateSQLQuery("select s.Name as something from Simple s").SetResultTransformer(Transformers.AliasToBean<TestStruct>()).ListAsync<TestStruct>());
				}

				Assert.AreEqual(2, result.Count);
			}
			finally
			{
				await (CleanupAsync());
			}
		}

		private async Task AssertAreWorkingAsync(string queryString)
		{
			using (ISession s = OpenSession())
			{
				IList<PublicParameterLessCtor> l = await (s.CreateSQLQuery(queryString).SetResultTransformer(Transformers.AliasToBean<PublicParameterLessCtor>()).ListAsync<PublicParameterLessCtor>());
				Assert.That(l.Count, Is.EqualTo(2));
				Assert.That(l, Has.All.Not.Null);
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
