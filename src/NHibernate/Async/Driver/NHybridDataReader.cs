#if NET_4_5
using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Driver
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NHybridDataReader : DbDataReader
	{
		/// <summary>
		/// Initializes a new instance of the <see cref = "NHybridDataReader"/> class.
		/// </summary>
		/// <param name = "reader">The underlying DbDataReader to use.</param>
		public Task<NHybridDataReader> InitializeAsync(DbDataReader reader)
		{
			return InitializeAsync(reader, false);
		}

		/// <summary>
		/// Initializes a new instance of the NHybridDataReader class.
		/// </summary>
		/// <param name = "reader">The underlying DbDataReader to use.</param>
		/// <param name = "inMemory"><see langword = "true"/> if the contents of the DbDataReader should be read into memory right away.</param>
		public async Task<NHybridDataReader> InitializeAsync(DbDataReader reader, bool inMemory)
		{
			if (inMemory)
			{
				_reader = await (new NDataReader().InitializeAsync(reader, false));
			}
			else
			{
				_reader = reader;
			}

			return this;
		}

		/// <summary>
		/// Reads all of the contents into memory because another <see cref = "DbDataReader"/>
		/// needs to be opened.
		/// </summary>
		/// <remarks>
		/// This will result in a no op if the reader is closed or is already in memory.
		/// </remarks>
		public async Task ReadIntoMemoryAsync()
		{
			if (_reader.IsClosed == false && _reader.GetType() != typeof (NDataReader))
			{
				if (log.IsDebugEnabled)
				{
					log.Debug("Moving DbDataReader into an NDataReader.  It was converted in midstream " + _isMidstream.ToString());
				}

				_reader = await (new NDataReader().InitializeAsync(_reader, _isMidstream));
			}
		}
	}
}
#endif
