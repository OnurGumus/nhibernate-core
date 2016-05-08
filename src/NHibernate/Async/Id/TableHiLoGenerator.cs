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
	/// <summary>
	/// An <see cref = "IIdentifierGenerator"/> that returns an <c>Int64</c>, constructed using
	/// a hi/lo algorithm.
	/// </summary>
	/// <remarks>
	/// <p>
	///	This id generation strategy is specified in the mapping file as 
	///	<code>
	///	&lt;generator class="hilo"&gt;
	///		&lt;param name="table"&gt;table&lt;/param&gt;
	///		&lt;param name="column"&gt;id_column&lt;/param&gt;
	///		&lt;param name="max_lo"&gt;max_lo_value&lt;/param&gt;
	///		&lt;param name="schema"&gt;db_schema&lt;/param&gt;
	///	&lt;/generator&gt;
	///	</code>
	/// </p>
	/// <p>
	/// The <c>table</c> and <c>column</c> parameters are required, the <c>max_lo</c> and 
	/// <c>schema</c> are optional.
	/// </p>
	/// <p>
	/// The hi value MUST be fecthed in a separate transaction to the <c>ISession</c>
	/// transaction so the generator must be able to obtain a new connection and 
	/// commit it. Hence this implementation may not be used when the user is supplying
	/// connections.  In that case a <see cref = "SequenceHiLoGenerator"/> would be a 
	/// better choice (where supported).
	/// </p>
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class TableHiLoGenerator : TableGenerator
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		public override async Task<object> GenerateAsync(ISessionImplementor session, object obj)
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