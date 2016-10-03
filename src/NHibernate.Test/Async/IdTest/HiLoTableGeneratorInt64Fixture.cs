#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.IdTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class HiLoTableGeneratorInt64FixtureAsync : IdFixtureBaseAsync
	{
		protected override string TypeName
		{
			get
			{
				return "HiLoInt64";
			}
		}

		[Test]
		public async Task ReadWriteAsync()
		{
			Int64 id;
			ISession s = OpenSession();
			HiLoInt64Class b = new HiLoInt64Class();
			await (s.SaveAsync(b));
			await (s.FlushAsync());
			id = b.Id;
			s.Close();
			s = OpenSession();
			b = (HiLoInt64Class)await (s.LoadAsync(typeof (HiLoInt64Class), b.Id));
			Assert.AreEqual(id, b.Id);
			await (s.DeleteAsync(b));
			await (s.FlushAsync());
			s.Close();
		}
	}
}
#endif
