#if NET_4_5
using System.Data.Common;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Tool.hbm2ddl
{
	/// <summary>
	/// A <seealso cref = "IConnectionHelper"/> implementation based on an explicitly supplied
	/// connection.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SuppliedConnectionHelper : IConnectionHelper
	{
		public Task PrepareAsync()
		{
			return TaskHelper.CompletedTask;
		}
	}
}
#endif
