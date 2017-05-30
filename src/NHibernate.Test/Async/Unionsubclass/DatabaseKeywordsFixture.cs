﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using NHibernate.Cfg;
using NUnit.Framework;
using System.Collections;

namespace NHibernate.Test.Unionsubclass
{
	using System.Threading.Tasks;
	using System.Threading;
	[TestFixture]
	public class DatabaseKeywordsFixtureAsync : TestCase
	{
		protected override string MappingsAssembly
		{
			get { return "NHibernate.Test"; }
		}

		protected override IList Mappings
		{
			get { return new string[] { "Unionsubclass.DatabaseKeyword.hbm.xml" }; }
		}

		protected override void Configure(Configuration configuration)
		{
			base.Configure(configuration);

			configuration.SetProperty(Environment.Hbm2ddlKeyWords, "auto-quote");
		}

		[Test]
		public async Task UnionSubClassQuotesReservedColumnNamesAsync()
		{
			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				await (s.SaveAsync(new DatabaseKeyword() { User = "user", View = "view", Table = "table", Create = "create" }, CancellationToken.None));

				await (t.CommitAsync(CancellationToken.None));
			}

			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				await (s.DeleteAsync("from DatabaseKeywordBase", CancellationToken.None));

				await (t.CommitAsync(CancellationToken.None));
			}
		}
	}
}
