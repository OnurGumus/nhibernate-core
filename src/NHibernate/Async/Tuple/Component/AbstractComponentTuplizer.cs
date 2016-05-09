using System;
using NHibernate.Engine;
using NHibernate.Properties;
using System.Threading.Tasks;

namespace NHibernate.Tuple.Component
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractComponentTuplizer : IComponentTuplizer
	{
		/// <summary> This method does not populate the component parent</summary>
		public virtual async Task<object> InstantiateAsync()
		{
			return instantiator.Instantiate();
		}
	}
}