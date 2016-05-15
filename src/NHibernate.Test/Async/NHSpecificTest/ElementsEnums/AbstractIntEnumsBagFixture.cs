#if NET_4_5
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.ElementsEnums
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractIntEnumsBagFixture : TestCase
	{
		[Test]
		[Description("Should load the list of enums (NH-1772)")]
		public async Task LoadEnumsAsync()
		{
			object savedId;
			using (ISession s = OpenSession())
				using (s.BeginTransaction())
				{
					savedId = await (s.SaveAsync(new SimpleWithEnums{Things = new List<Something>{Something.B, Something.C, Something.D, Something.E}}));
					await (s.Transaction.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (s.BeginTransaction())
				{
					var swe = await (s.GetAsync<SimpleWithEnums>(savedId));
					Assert.That(swe.Things, Is.EqualTo(new[]{Something.B, Something.C, Something.D, Something.E}));
					await (s.Transaction.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (s.BeginTransaction())
				{
					await (s.DeleteAsync("from SimpleWithEnums"));
					await (s.Transaction.CommitAsync());
				}
		}
	}
}
#endif
