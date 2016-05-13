namespace NHibernate.Mapping.ByCode
{
	public partial interface INaturalIdAttributesMapper
	{
		void Mutable(bool isMutable);
	}

	public partial interface INaturalIdMapper : INaturalIdAttributesMapper, IBasePlainPropertyContainerMapper {}
}