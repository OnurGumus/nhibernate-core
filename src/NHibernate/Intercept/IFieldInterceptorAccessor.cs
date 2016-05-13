namespace NHibernate.Intercept
{
	public partial interface IFieldInterceptorAccessor
	{
		IFieldInterceptor FieldInterceptor { get; set; }
	}
}