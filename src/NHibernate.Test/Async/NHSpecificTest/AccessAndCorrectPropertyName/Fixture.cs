#if NET_4_5
using System.Reflection;
using NHibernate.Cfg;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.AccessAndCorrectPropertyName
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync
	{
		private const string ns = "NHibernate.Test.NHSpecificTest.AccessAndCorrectPropertyName.";
		[Test]
		public Task WrongPropertyNameForCamelcaseUnderscoreShouldThrowAsync()
		{
			try
			{
				//default-access="field.camelcase-underscore" on entity
				var cfg = new Configuration();
				Assert.Throws<MappingException>(() => cfg.AddResource(ns + "PersonMapping.hbm.xml", Assembly.GetExecutingAssembly()));
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public Task WrongPropertyNameForCamelcaseShouldThrowAsync()
		{
			try
			{
				//default-access="field.camelcase" on property
				var cfg = new Configuration();
				Assert.Throws<MappingException>(() => cfg.AddResource(ns + "DogMapping.hbm.xml", Assembly.GetExecutingAssembly()));
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
