using System;
using System.Collections.Generic;
using System.Data;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Type
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DateType : PrimitiveType, IIdentifierType, ILiteralType, IParameterizedType
	{
		public override Task<int> GetHashCodeAsync(object x, EntityMode entityMode)
		{
			try
			{
				return Task.FromResult<int>(GetHashCode(x, entityMode));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<int>(ex);
			}
		}
	}
}