﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System.Collections;
using NUnit.Framework;

namespace NHibernate.Test.Generatedkeys.ByTrigger
{
	using System.Threading.Tasks;
	using System.Threading;
	[TestFixture]
	public class GeneratedIdentityFixtureAsync : TestCase
	{
		protected override IList Mappings
		{
			get { return new[] { "Generatedkeys.ByTrigger.MyEntity.hbm.xml" }; }
		}

		protected override string MappingsAssembly
		{
			get { return "NHibernate.Test"; }
		}

		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect is Dialect.Oracle8iDialect;
		}

		[Test]
		public async Task GetGeneratedKeysSupportAsync()
		{
			ISession session = OpenSession();
			session.BeginTransaction();

			var e = new MyEntity { Name = "entity-1" };
			await (session.SaveAsync(e, CancellationToken.None));

			// this insert should happen immediately!
			Assert.AreEqual(1, e.Id, "id not generated through forced insertion");

			await (session.DeleteAsync(e, CancellationToken.None));
			await (session.Transaction.CommitAsync(CancellationToken.None));
			session.Close();
		}
	}
}