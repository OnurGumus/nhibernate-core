#if NET_4_5
using System.Collections;
using NHibernate.Cfg.MappingSchema;
using System.Threading.Tasks;

namespace NHibernate.Test
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class TestCaseMappingByCodeAsync : TestCaseAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new string[0];
			}
		}

		protected override string MappingsAssembly
		{
			get
			{
				return null;
			}
		}

		protected override void AddMappings(Cfg.Configuration configuration)
		{
			configuration.AddDeserializedMapping(GetMappings(), "TestDomain");
		}

		protected abstract HbmMapping GetMappings();
	}
}
#endif
