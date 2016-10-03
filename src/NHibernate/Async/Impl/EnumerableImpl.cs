#if NET_4_5
using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Exceptions;
using NHibernate.Hql;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Impl
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class EnumerableImpl : IEnumerable, IEnumerator, IDisposable
	{
		private async Task PostNextAsync()
		{
			log.Debug("attempting to retrieve next results");
			bool readResult;
			try
			{
				readResult = await (_reader.ReadAsync());
				if (!readResult)
				{
					log.Debug("exhausted results");
					_currentResult = null;
					_session.Batcher.CloseCommand(_cmd, _reader);
				}
				else
					log.Debug("retrieved next results");
			}
			catch (DbException e)
			{
				throw ADOExceptionHelper.Convert(_session.Factory.SQLExceptionConverter, e, "Error executing Enumerable() query", new SqlString(_cmd.CommandText));
			}
		}

		private async Task PostMoveNextAsync(bool hasNext)
		{
			_startedReading = true;
			_hasNext = hasNext;
			_currentRow++;
			if (_selection != null && _selection.MaxRows != RowSelection.NoValue)
			{
				_hasNext = _hasNext && (_currentRow < _selection.MaxRows);
			}

			bool sessionDefaultReadOnlyOrig = _session.DefaultReadOnly;
			_session.DefaultReadOnly = _readOnly;
			try
			{
				if (!_hasNext)
				{
					// there are no more records in the DataReader so clean up
					log.Debug("exhausted results");
					_currentResult = null;
					_session.Batcher.CloseCommand(_cmd, _reader);
				}
				else
				{
					log.Debug("retrieving next results");
					bool isHolder = _holderInstantiator.IsRequired;
					if (_single && !isHolder)
					{
						_currentResult = await (_types[0].NullSafeGetAsync(_reader, _names[0], _session, null));
					}
					else
					{
						object[] currentResults = new object[_types.Length];
						// move through each of the ITypes contained in the DbDataReader and convert them
						// to their objects.  
						for (int i = 0; i < _types.Length; i++)
						{
							// The IType knows how to extract its value out of the DbDataReader.  If the IType
							// is a value type then the value will simply be pulled out of the DbDataReader.  If
							// the IType is an Entity type then the IType will extract the id from the DbDataReader
							// and use the ISession to load an instance of the object.
							currentResults[i] = await (_types[i].NullSafeGetAsync(_reader, _names[i], _session, null));
						}

						if (isHolder)
						{
							_currentResult = _holderInstantiator.Instantiate(currentResults);
						}
						else
						{
							_currentResult = currentResults;
						}
					}
				}
			}
			finally
			{
				_session.DefaultReadOnly = sessionDefaultReadOnlyOrig;
			}
		}
	}
}
#endif
