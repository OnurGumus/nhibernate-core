#if NET_4_5
using System;
using NHibernate.Type;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.TypesTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ByteTypeFixtureAsync : TypeFixtureBaseAsync
	{
		protected override string TypeName
		{
			get
			{
				return "Byte";
			}
		}

		[Test]
		public async Task ReadWriteAsync()
		{
			ByteClass basic = new ByteClass();
			basic.Id = 1;
			basic.ByteValue = (byte)43;
			ISession s = OpenSession();
			await (s.SaveAsync(basic));
			await (s.FlushAsync());
			s.Close();
			s = OpenSession();
			basic = (ByteClass)await (s.LoadAsync(typeof (ByteClass), 1));
			Assert.AreEqual((byte)43, basic.ByteValue);
			await (s.DeleteAsync(basic));
			await (s.FlushAsync());
			s.Close();
		}
	}
}
#endif
