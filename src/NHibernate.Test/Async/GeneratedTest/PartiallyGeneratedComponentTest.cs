#if NET_4_5
using System;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.GeneratedTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class PartiallyGeneratedComponentTestAsync : TestCaseAsync
	{
		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		protected override System.Collections.IList Mappings
		{
			get
			{
				return new string[]{"GeneratedTest.ComponentOwner.hbm.xml"};
			}
		}

		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect is MsSql2000Dialect || dialect is FirebirdDialect || dialect is Oracle8iDialect;
		}

		[Test]
		public async Task PartialComponentGenerationAsync()
		{
			ComponentOwner owner = new ComponentOwner("initial");
			ISession s = OpenSession();
			s.BeginTransaction();
			await (s.SaveAsync(owner));
			await (s.Transaction.CommitAsync());
			s.Close();
			Assert.IsNotNull(owner.Component, "expecting insert value generation");
			int previousValue = owner.Component.Generated;
			Assert.AreNotEqual(0, previousValue, "expecting insert value generation");
			s = OpenSession();
			s.BeginTransaction();
			owner = (ComponentOwner)await (s.GetAsync(typeof (ComponentOwner), owner.Id));
			Assert.AreEqual(previousValue, owner.Component.Generated, "expecting insert value generation");
			owner.Name = "subsequent";
			await (s.Transaction.CommitAsync());
			s.Close();
			Assert.IsNotNull(owner.Component);
			previousValue = owner.Component.Generated;
			s = OpenSession();
			s.BeginTransaction();
			owner = (ComponentOwner)await (s.GetAsync(typeof (ComponentOwner), owner.Id));
			Assert.AreEqual(previousValue, owner.Component.Generated, "expecting update value generation");
			await (s.DeleteAsync(owner));
			await (s.Transaction.CommitAsync());
			s.Close();
		}
	}
}
#endif
