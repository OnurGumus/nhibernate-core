#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Id
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class UUIDHexGenerator : IIdentifierGenerator, IConfigurable
	{
		/// <summary>
		/// Generate a new <see cref = "string "/> for the identifier using the "uuid.hex" algorithm.
		/// </summary>
		/// <param name = "session">The <see cref = "ISessionImplementor"/> this id is being generated in.</param>
		/// <param name = "obj">The entity for which the id is being generated.</param>
		/// <returns>The new identifier as a <see cref = "string "/>.</returns>
		public virtual Task<object> GenerateAsync(ISessionImplementor session, object obj)
		{
			try
			{
				return Task.FromResult<object>(Generate(session, obj));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}
#endif
