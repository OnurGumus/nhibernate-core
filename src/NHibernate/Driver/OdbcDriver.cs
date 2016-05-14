using System;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using NHibernate.SqlTypes;

namespace NHibernate.Driver
{
	/// <summary>
	/// A NHibernate Driver for using the Odbc DataProvider
	/// </summary>
	/// <remarks>
	/// Always look for a native .NET DataProvider before using the Odbc DataProvider.
	/// </remarks>
	public partial class OdbcDriver : DriverBase
	{
		public override DbConnection CreateConnection()
		{
			return new OdbcConnection();
		}

		public override DbCommand CreateCommand()
		{
			return new OdbcCommand();
		}

		public override bool UseNamedPrefixInSql
		{
			get { return false; }
		}

		public override bool UseNamedPrefixInParameter
		{
			get { return false; }
		}

		public override string NamedPrefix
		{
			get { return String.Empty; }
		}

		private static void SetVariableLengthParameterSize(DbParameter dbParam, SqlType sqlType)
		{
			// Override the defaults using data from SqlType.
			if (sqlType.LengthDefined)
			{
				dbParam.Size = sqlType.Length;
			}

			if (sqlType.PrecisionDefined)
			{
				((IDbDataParameter) dbParam).Precision = sqlType.Precision;
				((IDbDataParameter) dbParam).Scale = sqlType.Scale;
			}
		}

		protected override void InitializeParameter(DbParameter dbParam, string name, SqlType sqlType)
		{
			base.InitializeParameter(dbParam, name, sqlType);
			SetVariableLengthParameterSize(dbParam, sqlType);
		}
	}
}
