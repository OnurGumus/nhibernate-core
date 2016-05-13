using System.Collections.Generic;
namespace NHibernate.Mapping.ByCode
{
	public partial interface IGeneratorMapper
	{
		void Params(object generatorParameters);

		void Params(IDictionary<string, object> generatorParameters);
	}
}