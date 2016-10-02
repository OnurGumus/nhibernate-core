#if NET_4_5
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using NHibernate.Engine;
using NHibernate.Type;
using NHibernate.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NHibernate.Id
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class TableHiLoGenerator : TableGenerator
	{
		private readonly AsyncLock _lock = new AsyncLock();
		/// <summary>
		/// Generate a <see cref = "Int64"/> for the identifier by selecting and updating a value in a table.
		/// </summary>
		/// <param name = "session">The <see cref = "ISessionImplementor"/> this id is being generated in.</param>
		/// <param name = "obj">The entity for which the id is being generated.</param>
		/// <returns>The new identifier as a <see cref = "Int64"/>.</returns>
		public override async Task<object> GenerateAsync(ISessionImplementor session, object obj)
		{
			using (var releaser = await _lock.LockAsync())
			{
				if (maxLo < 1)
				{
					//keep the behavior consistent even for boundary usages
					long val = Convert.ToInt64(await (base.GenerateAsync(session, obj)));
					if (val == 0)
						val = Convert.ToInt64(await (base.GenerateAsync(session, obj)));
					return IdentifierGeneratorFactory.CreateNumber(val, returnClass);
				}

				if (lo > maxLo)
				{
					long hival = Convert.ToInt64(await (base.GenerateAsync(session, obj)));
					lo = (hival == 0) ? 1 : 0;
					hi = hival * (maxLo + 1);
					log.Debug("New high value: " + hival);
				}

				return IdentifierGeneratorFactory.CreateNumber(hi + lo++, returnClass);
			}
		}
	}
}
#endif
