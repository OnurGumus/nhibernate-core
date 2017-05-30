﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System.Collections;
using NHibernate.Cfg;
using NUnit.Framework;

namespace NHibernate.Test.IdTest
{
	using System.Threading.Tasks;
	using System.Threading;
	[TestFixture]
	[Ignore("Not supported yet")]
	public class UseIdentifierRollbackTestAsync: TestCase
	{
		protected override string MappingsAssembly
		{
			get { return "NHibernate.Test"; }
		}

		protected override IList Mappings
		{
			get { return new string[] { "IdTest.Product.hbm.xml" }; }
		}

		protected override void Configure(Configuration configuration)
		{
			cfg.SetProperty(Environment.UseIdentifierRollBack, "true");
			base.Configure(configuration);
		}

		public async Task SimpleRollbackAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			ISession session = OpenSession();
			ITransaction t = session.BeginTransaction();
			Product prod = new Product();
			Assert.IsNull(prod.Name);
			await (session.PersistAsync(prod, cancellationToken));
			await (session.FlushAsync(cancellationToken));
			Assert.IsNotNull(prod.Name);
			t.Rollback();
			session.Close();
		}
	}
}
