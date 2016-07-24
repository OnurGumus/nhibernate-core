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

		[Test]
		public void VerifyEntityModeNotFound()
		{
			Assert.IsNull(target.GetTuplizerOrNull(EntityMode.Xml));
		}

		[Test]
		public void VerifyEntityModeFound()
		{
			ITuplizer tuplizer = new TuplizerStub();
			target.Add(EntityMode.Map, tuplizer);
			Assert.AreSame(tuplizer, target.GetTuplizerOrNull(EntityMode.Map));
		}

		[Test, Explicit("To the commiter - run before and after")]
		public void RemoveThisTest_JustToShowPerfDifference()
		{
			const int loop = 1000000;
			target.Add(EntityMode.Map, new TuplizerStub());
			target.Add(EntityMode.Poco, new TuplizerStub());
			target.Add(EntityMode.Xml, new TuplizerStub());
			var watch = new Stopwatch();
			watch.Start();
			for (int i = 0; i < loop; i++)
			{
				target.GetTuplizerOrNull(EntityMode.Map);
				target.GetTuplizerOrNull(EntityMode.Poco);
			}

			watch.Stop();
			Console.WriteLine(watch.ElapsedMilliseconds);
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
