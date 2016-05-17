#if NET_4_5
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using NHibernate.Dialect;
using NHibernate.Type;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.TypesTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class BinaryTypeFixture : TypeFixtureBase
	{
		/// <summary>
		/// Certain drivers (ie - Oracle) don't handle writing and reading null byte[] 
		/// to and from the db consistently.  Verify if this driver does.
		/// </summary>
		[Test]
		public async Task InsertNullAsync()
		{
			BinaryClass bcBinary = new BinaryClass();
			bcBinary.Id = 1;
			bcBinary.DefaultSize = null;
			bcBinary.WithSize = null;
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.SaveAsync(bcBinary));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			BinaryClass bcBinaryLoaded = (BinaryClass)s.Load(typeof (BinaryClass), 1);
			Assert.IsNotNull(bcBinaryLoaded);
			Assert.AreEqual(null, bcBinaryLoaded.DefaultSize, "A property mapped as type=\"Byte[]\" with a null byte[] value was not saved & loaded as null");
			Assert.AreEqual(null, bcBinaryLoaded.WithSize, "A property mapped as type=\"Byte[](length)\" with null byte[] value was not saved & loaded as null");
			await (s.DeleteAsync(bcBinaryLoaded));
			await (t.CommitAsync());
			s.Close();
		}

		/// <summary>
		/// Certain drivers (ie - Oracle) don't handle writing and reading byte[0] 
		/// to and from the db consistently.  Verify if this driver does.
		/// </summary>
		[Test]
		public async Task InsertZeroLengthAsync()
		{
			if (Dialect is Oracle8iDialect)
			{
				Assert.Ignore("Certain drivers (ie - Oralce) don't handle writing and reading byte[0]");
			}

			BinaryClass bcBinary = new BinaryClass();
			bcBinary.Id = 1;
			bcBinary.DefaultSize = new byte[0];
			bcBinary.WithSize = new byte[0];
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.SaveAsync(bcBinary));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			BinaryClass bcBinaryLoaded = (BinaryClass)s.Load(typeof (BinaryClass), 1);
			Assert.IsNotNull(bcBinaryLoaded);
			Assert.AreEqual(0, bcBinaryLoaded.DefaultSize.Length, "A property mapped as type=\"Byte[]\" with a byte[0] value was not saved & loaded as byte[0]");
			Assert.AreEqual(0, bcBinaryLoaded.WithSize.Length, "A property mapped as type=\"Byte[](length)\" with a byte[0] value was not saved & loaded as byte[0]");
			await (s.DeleteAsync(bcBinaryLoaded));
			await (t.CommitAsync());
			s.Close();
		}

		/// <summary>
		/// Test the setting of values in Parameters and the reading of the 
		/// values out of the DbDataReader.
		/// </summary>
		[Test]
		public async Task ReadWriteAsync()
		{
			BinaryClass bcBinary = Create(1);
			BinaryClass expected = Create(1);
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.SaveAsync(bcBinary));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			bcBinary = (BinaryClass)s.Load(typeof (BinaryClass), 1);
			// make sure what was saved was expected
			ObjectAssert.AreEqual(expected.DefaultSize, bcBinary.DefaultSize);
			ObjectAssert.AreEqual(expected.WithSize, bcBinary.WithSize);
			Assert.IsFalse(s.IsDirty(), "The session is dirty: an Update will be raised on commit, See NH-1246");
			await (s.DeleteAsync(bcBinary));
			await (t.CommitAsync());
			s.Close();
		}
	}
}
#endif
