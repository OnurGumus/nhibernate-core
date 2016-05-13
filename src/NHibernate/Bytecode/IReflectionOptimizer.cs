using System;

namespace NHibernate.Bytecode
{
	/// <summary>
	/// Represents reflection optimization for a particular class.
	/// </summary>
	public partial interface IReflectionOptimizer
	{
		IAccessOptimizer AccessOptimizer { get; }
		IInstantiationOptimizer InstantiationOptimizer { get; }
	}
}