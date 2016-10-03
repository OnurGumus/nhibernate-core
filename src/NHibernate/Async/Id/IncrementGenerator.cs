﻿#if NET_4_5
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Text;
using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Id
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class IncrementGenerator : IIdentifierGenerator, IConfigurable
	{
		private readonly AsyncLock _lock = new AsyncLock();
		/// <summary>
		///
		/// </summary>
		/// <param name = "session"></param>
		/// <param name = "obj"></param>
		/// <returns></returns>
		public async Task<object> GenerateAsync(ISessionImplementor session, object obj)
		{
			using (var releaser = await _lock.LockAsync())
			{
				if (_sql != null)
				{
					await (GetNextAsync(session));
				}

				return IdentifierGeneratorFactory.CreateNumber(_next++, _returnClass);
			}
		}

		private async Task GetNextAsync(ISessionImplementor session)
		{
			Logger.Debug("fetching initial value: " + _sql);
			try
			{
				var cmd = await (session.Batcher.PrepareCommandAsync(CommandType.Text, _sql, SqlTypeFactory.NoTypes));
				DbDataReader reader = null;
				try
				{
					reader = await (session.Batcher.ExecuteReaderAsync(cmd));
					try
					{
						if (await (reader.ReadAsync()))
						{
							_next = !await (reader.IsDBNullAsync(0)) ? Convert.ToInt64(reader.GetValue(0)) + 1 : 1L;
						}
						else
						{
							_next = 1L;
						}

						_sql = null;
						Logger.Debug("first free id: " + _next);
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
				Logger.Error("could not get increment value", sqle);
				throw ADOExceptionHelper.Convert(session.Factory.SQLExceptionConverter, sqle, "could not fetch initial value for increment generator");
			}
		}
	}
}
#endif
