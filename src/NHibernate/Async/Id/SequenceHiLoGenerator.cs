#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NHibernate.Engine;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Id
{
	/// <summary>
	/// An <see cref = "IIdentifierGenerator"/> that combines a hi/lo algorithm with an underlying
	/// oracle-style sequence that generates hi values.
	/// </summary>
	/// <remarks>
	/// <p>
	///	This id generation strategy is specified in the mapping file as 
	///	<code>
	///	&lt;generator class="seqhilo"&gt;
	///		&lt;param name="sequence"&gt;uid_sequence&lt;/param&gt;
	///		&lt;param name="max_lo"&gt;max_lo_value&lt;/param&gt;
	///		&lt;param name="schema"&gt;db_schema&lt;/param&gt;
	///	&lt;/generator&gt;
	///	</code>
	/// </p>
	/// <p>
	/// The <c>sequence</c> parameter is required, the <c>max_lo</c> and <c>schema</c> are optional.
	/// </p>
	/// <p>
	/// The user may specify a <c>max_lo</c> value to determine how often new hi values are
	/// fetched. If sequences are not avaliable, <c>TableHiLoGenerator</c> might be an
	/// alternative.
	/// </p>
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SequenceHiLoGenerator : SequenceGenerator
	{
		private readonly AsyncLock _lock = new AsyncLock();
		/// <summary>
		/// Generate an <see cref = "Int16"/>, <see cref = "Int32"/>, or <see cref = "Int64"/> 
		/// for the identifier by using a database sequence.
		/// </summary>
		/// <param name = "session">The <see cref = "ISessionImplementor"/> this id is being generated in.</param>
		/// <param name = "obj">The entity for which the id is being generated.</param>
		/// <returns>The new identifier as a <see cref = "Int16"/>, <see cref = "Int32"/>, or <see cref = "Int64"/>.</returns>
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
					lo = 1;
					hi = hival * (maxLo + 1);
					if (log.IsDebugEnabled)
						log.Debug("new hi value: " + hival);
				}

				return IdentifierGeneratorFactory.CreateNumber(hi + lo++, returnClass);
			}
		}
	}
}
#endif
