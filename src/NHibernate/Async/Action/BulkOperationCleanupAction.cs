#if NET_4_5
using System;
using System.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Metadata;
using NHibernate.Persister.Entity;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Action
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class BulkOperationCleanupAction : IExecutable
	{
		public Task ExecuteAsync()
		{
			return TaskHelper.CompletedTask;
		// nothing to do
		}
	}
}
#endif
