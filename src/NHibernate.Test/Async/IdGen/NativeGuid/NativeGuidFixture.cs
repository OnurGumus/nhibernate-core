#if NET_4_5
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.IdGen.NativeGuid
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NativeGuidFixtureAsync : TestCaseAsync
	{
		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		protected override IList Mappings
		{
			get
			{
				return new[]{"IdGen.NativeGuid.NativeGuidPoid.hbm.xml"};
			}
		}

		[Test]
		public async Task CrdAsync()
		{
			object savedId;
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var nativeGuidPoid = new NativeGuidPoid();
					savedId = await (s.SaveAsync(nativeGuidPoid));
					await (tx.CommitAsync());
					Assert.That(savedId, Is.Not.Null);
					Assert.That(savedId, Is.EqualTo(nativeGuidPoid.Id));
				}

			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					var nativeGuidPoid = await (s.GetAsync<NativeGuidPoid>(savedId));
					Assert.That(nativeGuidPoid, Is.Not.Null);
					await (s.DeleteAsync(nativeGuidPoid));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
