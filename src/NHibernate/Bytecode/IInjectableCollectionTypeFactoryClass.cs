namespace NHibernate.Bytecode
{
	public partial interface IInjectableCollectionTypeFactoryClass
	{
		void SetCollectionTypeFactoryClass(string typeAssemblyQualifiedName);
		void SetCollectionTypeFactoryClass(System.Type type);
	}
}