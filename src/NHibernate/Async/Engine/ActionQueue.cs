#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Action;
using NHibernate.Cache;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Engine
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ActionQueue
	{
		private async Task ExecuteActionsAsync(IList list)
		{
			int size = list.Count;
			for (int i = 0; i < size; i++)
				await (ExecuteAsync((IExecutable)list[i]));
			list.Clear();
			session.Batcher.ExecuteBatch();
		}

		public async Task ExecuteAsync(IExecutable executable)
		{
			try
			{
				await (executable.ExecuteAsync());
			}
			finally
			{
				RegisterCleanupActions(executable);
			}
		}

		/// <summary> 
		/// Perform all currently queued entity-insertion actions.
		/// </summary>
		public Task ExecuteInsertsAsync()
		{
			return ExecuteActionsAsync(insertions);
		}

		/// <summary> 
		/// Perform all currently queued actions. 
		/// </summary>
		public async Task ExecuteActionsAsync()
		{
			await (ExecuteActionsAsync(insertions));
			await (ExecuteActionsAsync(updates));
			await (ExecuteActionsAsync(collectionRemovals));
			await (ExecuteActionsAsync(collectionUpdates));
			await (ExecuteActionsAsync(collectionCreations));
			await (ExecuteActionsAsync(deletions));
		}
	}
}
#endif
