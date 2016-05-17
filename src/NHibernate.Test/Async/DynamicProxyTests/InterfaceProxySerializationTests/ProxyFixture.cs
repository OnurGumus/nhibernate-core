#if NET_4_5
using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.DynamicProxyTests.InterfaceProxySerializationTests
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ProxyFixture : TestCase
	{
		[Test]
		public async Task ExceptionStackTraceAsync()
		{
			ISession s = OpenSession();
			IMyProxy ap = new MyProxyImpl{Id = 1, Name = "first proxy"};
			await (s.SaveAsync(ap));
			await (s.FlushAsync());
			s.Close();
			s = OpenSession();
			ap = (IMyProxy)s.Load(typeof (MyProxyImpl), ap.Id);
			Assert.IsFalse(NHibernateUtil.IsInitialized(ap), "check we have a proxy");
			try
			{
				ap.ThrowDeepException();
				Assert.Fail("Exception not thrown");
			}
			catch (ArgumentException ae)
			{
				Assert.AreEqual("thrown from Level2", ae.Message);
				string[] stackTraceLines = ae.StackTrace.Split('\n');
				Assert.IsTrue(stackTraceLines[0].Contains("Level2"), "top of exception stack is Level2()");
				Assert.IsTrue(stackTraceLines[1].Contains("Level1"), "next on exception stack is Level1()");
			}
			finally
			{
				await (s.DeleteAsync(ap));
				await (s.FlushAsync());
				s.Close();
			}
		}

		[Test]
		public async Task ProxyAsync()
		{
			ISession s = OpenSession();
			IMyProxy ap = new MyProxyImpl{Id = 1, Name = "first proxy"};
			await (s.SaveAsync(ap));
			await (s.FlushAsync());
			s.Close();
			s = OpenSession();
			ap = (IMyProxy)s.Load(typeof (MyProxyImpl), ap.Id);
			Assert.IsFalse(NHibernateUtil.IsInitialized(ap));
			int id = ap.Id;
			Assert.IsFalse(NHibernateUtil.IsInitialized(ap), "get id should not have initialized it.");
			string name = ap.Name;
			Assert.IsTrue(NHibernateUtil.IsInitialized(ap), "get name should have initialized it.");
			await (s.DeleteAsync(ap));
			await (s.FlushAsync());
			s.Close();
		}

		[Test]
		public async Task ProxySerializeAsync()
		{
			ISession s = OpenSession();
			IMyProxy ap = new MyProxyImpl{Id = 1, Name = "first proxy"};
			await (s.SaveAsync(ap));
			await (s.FlushAsync());
			s.Close();
			s = OpenSession();
			ap = (IMyProxy)s.Load(typeof (MyProxyImpl), ap.Id);
			Assert.AreEqual(1, ap.Id);
			s.Disconnect();
			SerializeAndDeserialize(ref s);
			s.Reconnect();
			s.Disconnect();
			// serialize and then deserialize the session again - make sure Castle.DynamicProxy
			// has no problem with serializing two times - earlier versions of it did.
			SerializeAndDeserialize(ref s);
			s.Close();
			s = OpenSession();
			await (s.DeleteAsync(ap));
			await (s.FlushAsync());
			s.Close();
		}
	}
}
#endif
