#if NET_4_5
using System;
using System.Collections;
using NHibernate.Cfg;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH606
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync
	{
		[Test]
		public Task InvalidGenericMappingAsync()
		{
			try
			{
				Assert.Throws<MappingException>(() => new Configuration().AddResource(typeof (FixtureAsync).Namespace + ".Mapping.hbm.xml", typeof (FixtureAsync).Assembly).BuildSessionFactory());
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
