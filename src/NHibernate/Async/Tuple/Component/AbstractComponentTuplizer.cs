#if NET_4_5
using System;
using NHibernate.Engine;
using NHibernate.Properties;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Tuple.Component
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractComponentTuplizer : IComponentTuplizer
	{
		/// <summary> This method does not populate the component parent</summary>
		public virtual Task<object> InstantiateAsync()
		{
			try
			{
				return Task.FromResult<object>(Instantiate());
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}
#endif
