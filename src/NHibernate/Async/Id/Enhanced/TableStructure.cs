#if NET_4_5
using System;
using System.Data;
using System.Data.Common;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.AdoNet.Util;
using System.Threading.Tasks;
using NHibernate.Util;

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
			public virtual Task<long> GetNextValueAsync()
			{
				try
				{
					return Task.FromResult<long>(GetNextValue());
				}
				catch (Exception ex)
				{
					return TaskHelper.FromException<long>(ex);
				}
			}
		}
	}
}
#endif
