using System;
using NHibernate.Exceptions;

namespace NHibernate.Test.NHSpecificTest.SqlConverterAndMultiQuery
{
	public partial class SqlConverter : ISQLExceptionConverter
	{
		public Exception Convert(AdoExceptionContextInfo adoExceptionContextInfo)
		{
			return new UnitTestException();
		}
	}

	public partial class UnitTestException : Exception{}
}
