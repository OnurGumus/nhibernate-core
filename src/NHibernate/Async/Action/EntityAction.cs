#if NET_4_5
using System;
using System.IO;
using System.Runtime.Serialization;
using NHibernate.Engine;
using NHibernate.Persister.Entity;
using NHibernate.Util;
using NHibernate.Impl;
using System.Threading.Tasks;

namespace NHibernate.Action
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class EntityAction : IExecutable, IComparable<EntityAction>, IDeserializationCallback
	{
		public abstract Task ExecuteAsync();
		protected virtual Task AfterTransactionCompletionProcessImplAsync(bool success)
		{
			return TaskHelper.CompletedTask;
		}
	}
}
#endif
