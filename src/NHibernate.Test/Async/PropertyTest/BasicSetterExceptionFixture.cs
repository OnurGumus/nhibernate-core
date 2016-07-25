#if NET_4_5
using System;
using NHibernate.Properties;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.PropertyTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class BasicSetterExceptionFixtureAsync
	{
		protected IPropertyAccessor _accessor;
		protected ISetter _setter;
		[SetUp]
		public void SetUp()
		{
			_accessor = PropertyAccessorFactory.GetPropertyAccessor("property");
			_setter = _accessor.GetSetter(typeof (A), "Id");
		}


		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class A
		{
			private int _id = 0;
			public int Id
			{
				get
				{
					return _id;
				}

				set
				{
					if (value == 5)
					{
						throw new ArgumentException("can't be 5 for testing purposes");
					}

					_id = value;
				}
			}
		}
	}
}
#endif
