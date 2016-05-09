using System;
using System.Data;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.AdoNet.Util;
using System.Threading.Tasks;

namespace NHibernate.Id.Enhanced
{
	/// <summary>
	/// Describes a table used to mimic sequence behavior
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class TableStructure : TransactionHelper, IDatabaseStructure
	{
		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private partial class TableAccessCallback : IAccessCallback
		{
			public virtual async Task<long> GetNextValueAsync()
			{
				return Convert.ToInt64(_owner.DoWorkInNewTransaction(_session));
			}
		}
	}
}