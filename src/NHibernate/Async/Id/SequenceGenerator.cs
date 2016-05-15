#if NET_4_5
using System;
using System.Collections;
using System.Data;
using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Type;
using NHibernate.Util;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace NHibernate.Id
{
	/// <summary>
	/// An <see cref = "IIdentifierGenerator"/> that generates <c>Int64</c> values using an 
	/// oracle-style sequence. A higher performance algorithm is 
	/// <see cref = "SequenceHiLoGenerator"/>.
	/// </summary>
	/// <remarks>
	/// <p>
	///	This id generation strategy is specified in the mapping file as 
	///	<code>
	///	&lt;generator class="sequence"&gt;
	///		&lt;param name="sequence"&gt;uid_sequence&lt;/param&gt;
	///		&lt;param name="schema"&gt;db_schema&lt;/param&gt;
	///	&lt;/generator&gt;
	///	</code>
	/// </p>
	/// <p>
	/// The <c>sequence</c> parameter is required while the <c>schema</c> is optional.
	/// </p>
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SequenceGenerator : IPersistentIdentifierGenerator, IConfigurable
	{
		/// <summary>
		/// Generate an <see cref = "Int16"/>, <see cref = "Int32"/>, or <see cref = "Int64"/> 
		/// for the identifier by using a database sequence.
		/// </summary>
		/// <param name = "session">The <see cref = "ISessionImplementor"/> this id is being generated in.</param>
		/// <param name = "obj">The entity for which the id is being generated.</param>
		/// <returns>The new identifier as a <see cref = "Int16"/>, <see cref = "Int32"/>, or <see cref = "Int64"/>.</returns>
		public virtual async Task<object> GenerateAsync(ISessionImplementor session, object obj)
		{
			try
			{
				DbCommand cmd = await (session.Batcher.PrepareCommandAsync(CommandType.Text, sql, SqlTypeFactory.NoTypes));
				DbDataReader reader = null;
				try
				{
					reader = await (session.Batcher.ExecuteReaderAsync(cmd));
					try
					{
						await (reader.ReadAsync());
						object result = await (IdentifierGeneratorFactory.GetAsync(reader, identifierType, session));
						if (log.IsDebugEnabled)
						{
							log.Debug("Sequence identifier generated: " + result);
						}

						return result;
					}
					finally
					{
						reader.Close();
					}
				}
				finally
				{
					session.Batcher.CloseCommand(cmd, reader);
				}
			}
			catch (DbException sqle)
			{
				log.Error("error generating sequence", sqle);
				throw ADOExceptionHelper.Convert(session.Factory.SQLExceptionConverter, sqle, "could not get next sequence value");
			}
		}
	}
}
#endif
