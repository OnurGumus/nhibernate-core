#if NET_4_5
using System;
using NHibernate.Proxy;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.ProxyValidator
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync
	{
		private readonly IProxyValidator pv = new DynProxyTypeValidator();
		private void Validate(System.Type type)
		{
			ICollection<string> errors = pv.ValidateType(type);
			if (errors != null)
			{
				throw new InvalidProxyTypeException(errors);
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class ValidClass
		{
			private int privateField;
			protected int protectedField;
			public virtual int SomeProperty
			{
				get
				{
					return privateField;
				}

				set
				{
					privateField = value;
				}
			}

			public virtual void SomeMethod(int arg1, object arg2)
			{
			}

#pragma warning disable 67
			public virtual event EventHandler VirtualEvent;
#pragma warning restore 67
			protected void NonVirtualProtectedMethod()
			{
			}

			protected int NonVirtualProtectedProperty
			{
				get
				{
					return 0;
				}

				set
				{
				}
			}

#pragma warning disable 67
			protected event EventHandler NonVirtualProtectedEvent;
#pragma warning restore 67
			protected void NonVirtualPrivateMethod()
			{
			}

			protected int NonVirtualPrivateProperty
			{
				get
				{
					return 0;
				}

				set
				{
				}
			}

#pragma warning disable 67
			protected event EventHandler NonVirtualPrivateEvent;
#pragma warning restore 67
		}

		[Test]
		public void ValidClassTest()
		{
			Validate(typeof (ValidClass));
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class InvalidPrivateConstructor : ValidClass
		{
			private InvalidPrivateConstructor()
			{
			}
		}

		[Test]
		public Task PrivateConstructorAsync()
		{
			try
			{
				Assert.Throws<InvalidProxyTypeException>(() => Validate(typeof (InvalidPrivateConstructor)));
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class InvalidNonVirtualProperty : ValidClass
		{
			public int NonVirtualProperty
			{
				get
				{
					return 1;
				}

				set
				{
				}
			}
		}

		[Test]
		public Task NonVirtualPropertyAsync()
		{
			try
			{
				Assert.Throws<InvalidProxyTypeException>(() => Validate(typeof (InvalidNonVirtualProperty)));
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class InvalidPublicField : ValidClass
		{
			public int publicField;
		}

		[Test]
		public Task PublicFieldAsync()
		{
			try
			{
				Assert.Throws<InvalidProxyTypeException>(() => Validate(typeof (InvalidPublicField)));
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class InvalidNonVirtualEvent : ValidClass
		{
#pragma warning disable 67
			public event EventHandler NonVirtualEvent;
#pragma warning restore 67
		}

		[Test]
		public Task NonVirtualEventAsync()
		{
			try
			{
				Assert.Throws<InvalidProxyTypeException>(() => Validate(typeof (InvalidNonVirtualEvent)));
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public interface ValidInterface
		{
		}

		[Test]
		public void Interface()
		{
			Validate(typeof (ValidInterface));
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class MultipleErrors
		{
			private MultipleErrors()
			{
			}

			public int publicField;
#pragma warning disable 67
			public event EventHandler NonVirtualEvent;
#pragma warning restore 67
			public int NonVirtualProperty
			{
				get
				{
					return 1;
				}

				set
				{
				}
			}
		}

		[Test]
		public void MultipleErrorsReported()
		{
			try
			{
				Validate(typeof (MultipleErrors));
				Assert.Fail("Should have failed validation");
			}
			catch (InvalidProxyTypeException e)
			{
				Assert.IsTrue(e.Errors.Count > 1);
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class InvalidNonVirtualInternalProperty : ValidClass
		{
			internal int NonVirtualProperty
			{
				get
				{
					return 1;
				}

				set
				{
				}
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class InvalidInternalField : ValidClass
		{
#pragma warning disable 649
			internal int internalField;
#pragma warning restore 649
		}

		[Test]
		public Task NonVirtualInternalAsync()
		{
			try
			{
				Assert.Throws<InvalidProxyTypeException>(() => Validate(typeof (InvalidNonVirtualInternalProperty)));
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public Task InternalFieldAsync()
		{
			try
			{
				Assert.Throws<InvalidProxyTypeException>(() => Validate(typeof (InvalidInternalField)));
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class InvalidNonVirtualProtectedProperty : ValidClass
		{
			protected int NonVirtualProperty
			{
				get
				{
					return 1;
				}

				set
				{
				}
			}
		}

		[Test]
		public void NonVirtualProtected()
		{
			Validate(typeof (InvalidNonVirtualProtectedProperty));
			Assert.IsTrue(true, "Always should pass, protected members do not need to be virtual.");
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class InvalidNonVirtualProtectedInternalProperty : ValidClass
		{
			protected internal int NonVirtualProperty
			{
				get
				{
					return 1;
				}

				set
				{
				}
			}
		}

		[Test]
		public Task NonVirtualProtectedInternalAsync()
		{
			try
			{
				Assert.Throws<InvalidProxyTypeException>(() => Validate(typeof (InvalidNonVirtualProtectedInternalProperty)));
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		interface INonVirtualPublicImplementsInterface
		{
			int NonVirtualMethodImplementsInterface
			{
				get;
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class NonVirtualPublicImplementsInterface : ValidClass, INonVirtualPublicImplementsInterface
		{
			public int NonVirtualMethodImplementsInterface
			{
				get
				{
					return 0;
				}
			}
		}

		[Test]
		public Task VirtualPublicImplementsInterfaceAsync()
		{
			try
			{
				Assert.Throws<InvalidProxyTypeException>(() => Validate(typeof (NonVirtualPublicImplementsInterface)));
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class InvalidVirtualPrivateAutoProperty : ValidClass
		{
			public virtual int NonVirtualSetterProperty
			{
				get;
				private set;
			}
		}

		[Test]
		public Task PrivateSetterOnVirtualPropertyShouldThrowsAsync()
		{
			try
			{
				Assert.Throws<InvalidProxyTypeException>(() => Validate(typeof (InvalidVirtualPrivateAutoProperty)));
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
