#if NET_4_5
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH2931
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class MappingByCodeTestAsync : BugTestCaseAsync
	{
		//no xml mappings here, since we use MappingByCode
		protected override System.Collections.IList Mappings
		{
			get
			{
				return new string[0];
			}
		}

		[Test]
		public Task CompiledMappings_ShouldNotDependOnAddedOrdering_AddedBy_AddMappingAsync()
		{
			try
			{
				var mapper = new ModelMapper();
				mapper.AddMapping<EntityMapping>();
				mapper.AddMapping<DerivedClassMapping>();
				mapper.AddMapping<BaseClassMapping>();
				var config = TestConfigurationHelper.GetDefaultConfiguration();
				Assert.DoesNotThrow(() => config.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities()));
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public Task CompiledMappings_ShouldNotDependOnAddedOrdering_AddedBy_AddMappingsAsync()
		{
			try
			{
				var mapper = new ModelMapper();
				mapper.AddMappings(typeof (EntityMapping).Assembly.GetExportedTypes()//only add our test entities/mappings
				.Where(t => t.Namespace == typeof (MappingByCodeTestAsync).Namespace));
				var config = TestConfigurationHelper.GetDefaultConfiguration();
				Assert.DoesNotThrow(() => config.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities()));
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
