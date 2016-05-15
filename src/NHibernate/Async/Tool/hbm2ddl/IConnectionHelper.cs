#if NET_4_5
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Tool.hbm2ddl
{
	/// <summary>
	/// Contract for delegates responsible for managing connection used by the hbm2ddl tools.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IConnectionHelper
	{
		/// <summary>
		///  Prepare the helper for use.
		/// </summary>
		Task PrepareAsync();
	}
}
#endif
