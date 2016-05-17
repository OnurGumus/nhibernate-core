#if NET_4_5
using System;
using System.Collections;
using NHibernate.DomainModel.NHSpecific;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class BasicSerializableFixture : TestCase
	{
		/// <summary>
		/// This contains portions of FumTest.CompositeIDs that deal with <c>type="Serializable"</c>
		/// and replacements Foo.NullBlob, and Foo.Blob.
		/// </summary>
		[Test]
		public async Task TestCRUDAsync()
		{
			ISession s = OpenSession();
			BasicSerializable ser = new BasicSerializable();
			SerializableClass serClass = ser.SerializableProperty;
			await (s.SaveAsync(ser));
			await (s.FlushAsync());
			s.Close();
			s = OpenSession();
			ser = (BasicSerializable)s.Load(typeof (BasicSerializable), ser.Id);
			Assert.IsNull(ser.Serial, "should have saved as null");
			ser.Serial = ser.SerializableProperty;
			await (s.FlushAsync());
			s.Close();
			s = OpenSession();
			ser = (BasicSerializable)s.Load(typeof (BasicSerializable), ser.Id);
			Assert.IsTrue(ser.Serial is SerializableClass, "should have been a SerializableClass");
			Assert.AreEqual(ser.SerializableProperty, ser.Serial, "SerializablePorperty and Serial should both be 5 and 'serialize me'");
			IDictionary props = new Hashtable();
			props["foo"] = "bar";
			props["bar"] = "foo";
			ser.Serial = props;
			await (s.FlushAsync());
			props["x"] = "y";
			await (s.FlushAsync());
			s.Close();
			s = OpenSession();
			ser = (BasicSerializable)s.Load(typeof (BasicSerializable), ser.Id);
			props = (IDictionary)ser.Serial;
			Assert.AreEqual("bar", props["foo"]);
			Assert.AreEqual("y", props["x"]);
			Assert.AreEqual(serClass, ser.SerializableProperty);
			ser.SerializableProperty._classString = "modify me";
			await (s.FlushAsync());
			s.Close();
			s = OpenSession();
			ser = (BasicSerializable)s.Load(typeof (BasicSerializable), ser.Id);
			Assert.AreEqual("modify me", ser.SerializableProperty._classString);
			Assert.AreEqual("bar", props["foo"]);
			Assert.AreEqual("y", props["x"]);
			await (s.DeleteAsync(ser));
			await (s.FlushAsync());
			s.Close();
		}
	}
}
#endif
