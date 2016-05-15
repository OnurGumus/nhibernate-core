#if NET_4_5
using System;
using System.Collections;
using NHibernate.DomainModel.NHSpecific;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class BasicObjectFixture : TestCase
	{
		/// <summary>
		/// This is the replacement for ParentChildTest.ObjectType() and FooBarTest.ObjectType()
		/// </summary>
		[Test]
		public async Task TestCRUDAsync()
		{
			ISession s = OpenSession();
			BasicObjectRef any = new BasicObjectRef();
			any.Name = "the any";
			IBasicObjectProxy anyProxy = new BasicObjectProxy();
			anyProxy.Name = "proxied object";
			BasicObject bo = new BasicObject();
			bo.Name = "the object";
			bo.Any = any;
			bo.AnyWithProxy = anyProxy;
			await (s.SaveAsync(any));
			await (s.SaveAsync(anyProxy));
			await (s.SaveAsync(bo));
			await (s.FlushAsync());
			s.Close();
			s = OpenSession();
			bo = (BasicObject)await (s.LoadAsync(typeof (BasicObject), bo.Id));
			Assert.IsNotNull(bo.AnyWithProxy, "AnyWithProxy should not be null");
			Assert.IsTrue(bo.AnyWithProxy is IBasicObjectProxy, "AnyWithProxy should have been a IBasicObjectProxy instance");
			Assert.AreEqual(anyProxy.Id, ((IBasicObjectProxy)bo.AnyWithProxy).Id);
			Assert.IsNotNull(bo.Any, "any should not be null");
			Assert.IsTrue(bo.Any is BasicObjectRef, "any should have been a BasicObjectRef instance");
			Assert.AreEqual(any.Id, ((BasicObjectRef)bo.Any).Id);
			any = (BasicObjectRef)await (s.LoadAsync(typeof (BasicObjectRef), any.Id));
			Assert.AreSame(any, bo.Any, "any loaded and ref by BasicObject should be the same");
			await (s.DeleteAsync(bo.Any));
			await (s.DeleteAsync(bo.AnyWithProxy));
			await (s.DeleteAsync(bo));
			await (s.FlushAsync());
			s.Close();
		}
	}
}
#endif
