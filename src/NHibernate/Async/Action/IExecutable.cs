#if NET_4_5
using NHibernate.Engine;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Action
{
	/// <summary>
	/// An operation which may be scheduled for later execution.
	/// Usually, the operation is a database insert/update/delete,
	/// together with required second-level cache management.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IExecutable
	{
		/// <summary> Execute this action</summary>
		Task ExecuteAsync();
	}
}
#endif
