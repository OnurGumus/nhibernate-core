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

namespace NHibernate.Test.IdGen.NativeGuid
{
	using System.Threading.Tasks;
	using System.Threading;
	[TestFixture]
	public class NativeGuidFixtureAsync : TestCase
	{
		protected override string MappingsAssembly
		{
			get { return "NHibernate.Test"; }
		}

		protected override IList Mappings
		{
			get { return new[] {"IdGen.NativeGuid.NativeGuidPoid.hbm.xml"}; }
		}

		[Test]
		public async Task CrdAsync()
		{
			object savedId;
			using (var s = OpenSession())
			using (var tx = s.BeginTransaction())
			{
				var nativeGuidPoid = new NativeGuidPoid();
				savedId = await (s.SaveAsync(nativeGuidPoid, CancellationToken.None));
				await (tx.CommitAsync(CancellationToken.None));
				Assert.That(savedId, Is.Not.Null);
				Assert.That(savedId, Is.EqualTo(nativeGuidPoid.Id));
			}

			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				var nativeGuidPoid = await (s.GetAsync<NativeGuidPoid>(savedId, CancellationToken.None));
				Assert.That(nativeGuidPoid, Is.Not.Null);
				await (s.DeleteAsync(nativeGuidPoid, CancellationToken.None));
				await (tx.CommitAsync(CancellationToken.None));
			}
		}
	}
}