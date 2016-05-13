using NHibernate.Mapping.ByCode.Impl;

namespace NHibernate.Mapping.ByCode
{
	public partial interface IConformistHoldersProvider
	{
		ICustomizersHolder CustomizersHolder { get; }
		IModelExplicitDeclarationsHolder ExplicitDeclarationsHolder { get; }
	}
}