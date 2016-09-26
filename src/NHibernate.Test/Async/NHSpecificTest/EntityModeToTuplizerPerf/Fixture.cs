#if NET_4_5
using System;
using System.Diagnostics;
using NHibernate.Tuple;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.EntityModeToTuplizerPerf
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync
	{
		private TargetClazz target;
		[SetUp]
		public void Setup()
		{
			target = new TargetClazz();
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private class TargetClazz : EntityModeToTuplizerMapping
		{
			public void Add(EntityMode eMode, ITuplizer tuplizer)
			{
				AddTuplizer(eMode, tuplizer);
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private partial class TuplizerStub : ITuplizer
		{
			public System.Type MappedClass
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public object[] GetPropertyValues(object entity)
			{
				throw new NotImplementedException();
			}

			public void SetPropertyValues(object entity, object[] values)
			{
				throw new NotImplementedException();
			}

			public object GetPropertyValue(object entity, int i)
			{
				throw new NotImplementedException();
			}

			public object Instantiate()
			{
				throw new NotImplementedException();
			}

			public bool IsInstance(object obj)
			{
				throw new NotImplementedException();
			}

			public Task<object> InstantiateAsync()
			{
				return TaskHelper.FromException<object>(new NotImplementedException());
			}
		}
	}

	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture
	{
		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private partial class TuplizerStub : ITuplizer
		{
			public Task<object> InstantiateAsync()
			{
				return TaskHelper.FromException<object>(new NotImplementedException());
			}
		}
	}
}
#endif
