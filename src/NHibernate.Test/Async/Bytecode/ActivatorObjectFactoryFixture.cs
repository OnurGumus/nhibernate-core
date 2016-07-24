#if NET_4_5
using System;
using NHibernate.Bytecode;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.Bytecode
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ActivatorObjectFactoryFixtureAsync
	{
		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class WithOutPublicParameterLessCtor
		{
			public string Something
			{
				get;
				set;
			}

			protected WithOutPublicParameterLessCtor()
			{
			}

			public WithOutPublicParameterLessCtor(string something)
			{
				Something = something;
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class PublicParameterLessCtor
		{
		}

		public struct ValueType
		{
			public string Something
			{
				get;
				set;
			}
		}

		protected virtual IObjectsFactory GetObjectsFactory()
		{
			return new ActivatorObjectsFactory();
		}

		[Test]
		public Task CreateInstanceDefCtorAsync()
		{
			try
			{
				IObjectsFactory of = GetObjectsFactory();
				Assert.Throws<ArgumentNullException>(() => of.CreateInstance(null));
				Assert.Throws<MissingMethodException>(() => of.CreateInstance(typeof (WithOutPublicParameterLessCtor)));
				var instance = of.CreateInstance(typeof (PublicParameterLessCtor));
				Assert.That(instance, Is.Not.Null);
				Assert.That(instance, Is.InstanceOf<PublicParameterLessCtor>());
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public Task CreateInstanceWithNoPublicCtorAsync()
		{
			try
			{
				IObjectsFactory of = GetObjectsFactory();
				Assert.Throws<ArgumentNullException>(() => of.CreateInstance(null, false));
				var instance = of.CreateInstance(typeof (WithOutPublicParameterLessCtor), true);
				Assert.That(instance, Is.Not.Null);
				Assert.That(instance, Is.InstanceOf<WithOutPublicParameterLessCtor>());
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public void CreateInstanceOfValueType()
		{
			IObjectsFactory of = GetObjectsFactory();
			var instance = of.CreateInstance(typeof (ValueType), true);
			Assert.That(instance, Is.Not.Null);
			Assert.That(instance, Is.InstanceOf<ValueType>());
		}

		[Test]
		public Task CreateInstanceWithArgumentsAsync()
		{
			try
			{
				IObjectsFactory of = GetObjectsFactory();
				Assert.Throws<ArgumentNullException>(() => of.CreateInstance(null, new[]{1}));
				var value = "a value";
				var instance = of.CreateInstance(typeof (WithOutPublicParameterLessCtor), new[]{value});
				Assert.That(instance, Is.Not.Null);
				Assert.That(instance, Is.InstanceOf<WithOutPublicParameterLessCtor>());
				Assert.That(((WithOutPublicParameterLessCtor)instance).Something, Is.EqualTo(value));
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}
#endif
