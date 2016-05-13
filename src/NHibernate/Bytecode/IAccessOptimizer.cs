namespace NHibernate.Bytecode
{
	/// <summary>
	/// Represents optimized entity property access.
	/// </summary>
	public partial interface IAccessOptimizer
	{
		object[] GetPropertyValues(object target);
		void SetPropertyValues(object target, object[] values);
	}
}