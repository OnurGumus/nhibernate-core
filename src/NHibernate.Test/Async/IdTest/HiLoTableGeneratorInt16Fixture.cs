#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.IdTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class HiLoTableGeneratorInt16Fixture : IdFixtureBase
	{
		[Test]
		public async Task ReadWriteAsync()
		{
			Int16 id;
			ISession s = OpenSession();
			HiLoInt16Class b = new HiLoInt16Class();
			await (s.SaveAsync(b));
			await (s.FlushAsync());
			id = b.Id;
			s.Close();
			s = OpenSession();
			b = (HiLoInt16Class)await (s.LoadAsync(typeof (HiLoInt16Class), b.Id));
			Assert.AreEqual(id, b.Id);
			await (s.DeleteAsync(b));
			await (s.FlushAsync());
			s.Close();
		}
	}
}
#endif
