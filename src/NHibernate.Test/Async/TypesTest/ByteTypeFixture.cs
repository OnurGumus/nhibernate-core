#if NET_4_5
using System;
using NHibernate.Type;
using NUnit.Framework;
using System.Threading.Tasks;

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

		/// <summary>
		/// Verify Equals will correctly determine when the property
		/// is dirty.
		/// </summary>
		[Test]
		public void Equals()
		{
			ByteType type = (ByteType)NHibernateUtil.Byte;
			Assert.IsTrue(type.IsEqual((byte)5, (byte)5));
			Assert.IsFalse(type.IsEqual((byte)5, (byte)6));
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
