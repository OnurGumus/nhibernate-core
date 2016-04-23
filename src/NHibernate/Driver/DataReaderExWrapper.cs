using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using NHibernate.Linq;

namespace NHibernate.Driver
{
	public class DataReaderExWrapper : IDataReaderEx
	{
		private readonly DbDataReader reader;

		public DataReaderExWrapper(DbDataReader reader)
		{
			this.reader = reader;
		}

		#region IDisposable Members

		private bool disposed;

		~DataReaderExWrapper()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (disposed)
				return;

			if (disposing)
			{
				if (reader != null)
				{
					if (!reader.IsClosed) reader.Close();
					reader.Dispose();
				}
			}

			disposed = true;
		}

		#endregion

		public string GetName(int i)
		{
			return reader.GetName(i);
		}

		public string GetDataTypeName(int i)
		{
			return reader.GetDataTypeName(i);
		}

		public System.Type GetFieldType(int i)
		{
			return reader.GetFieldType(i);
		}

		public object GetValue(int i)
		{
			return reader.GetValue(i);
		}

		public int GetValues(object[] values)
		{
			return reader.GetValues(values);
		}

		public int GetOrdinal(string name)
		{
			return reader.GetOrdinal(name);
		}

		public bool GetBoolean(int i)
		{
			return reader.GetBoolean(i);
		}

		public byte GetByte(int i)
		{
			return reader.GetByte(i);
		}

		public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			return reader.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
		}

		public char GetChar(int i)
		{
			return reader.GetChar(i);
		}

		public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			return reader.GetChars(i, fieldoffset, buffer, bufferoffset, length);
		}

		public Guid GetGuid(int i)
		{
			return reader.GetGuid(i);
		}

		public short GetInt16(int i)
		{
			return reader.GetInt16(i);
		}

		public int GetInt32(int i)
		{
			return reader.GetInt32(i);
		}

		public long GetInt64(int i)
		{
			return reader.GetInt64(i);
		}

		public float GetFloat(int i)
		{
			return reader.GetFloat(i);
		}

		public double GetDouble(int i)
		{
			return reader.GetDouble(i);
		}

		public string GetString(int i)
		{
			return reader.GetString(i);
		}

		public decimal GetDecimal(int i)
		{
			return reader.GetDecimal(i);
		}

		public DateTime GetDateTime(int i)
		{
			return reader.GetDateTime(i);
		}

		public IDataReader GetData(int i)
		{
			return reader.GetData(i);
		}

		public bool IsDBNull(int i)
		{
			return reader.IsDBNull(i);
		}

		public int FieldCount { get { return reader.FieldCount; } }

		object IDataRecord.this[int i]
		{
			get { return reader[i]; }
		}

		object IDataRecord.this[string name]
		{
			get { return reader[name]; }
		}

		public void Close()
		{
			reader.Close();
		}

		public DataTable GetSchemaTable()
		{
			return reader.GetSchemaTable();
		}

		public bool NextResult()
		{
			return reader.NextResult();
		}

		public bool Read()
		{
			return reader.Read();
		}

		public int Depth { get { return reader.Depth; } }
		public bool IsClosed { get { return reader.IsClosed; } }
		public int RecordsAffected { get { return reader.RecordsAffected; } }

		public Task<bool> ReadAsync()
		{
			return reader.ReadAsync();
		}

		public Task<bool> NextResultAsync()
		{
			return reader.NextResultAsync();
		}
	}
}
