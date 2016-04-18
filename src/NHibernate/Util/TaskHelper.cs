using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.Util
{
	/// <summary>
	/// Task methods and fields from .NET 4.6
	/// </summary>
	public static class TaskHelper
	{
		public static readonly Task CompletedTask = Task.FromResult(true);

		public static Task<TResult> FromException<TResult>(Exception exc)
		{
			var tcs = new TaskCompletionSource<TResult>();
			tcs.SetException(exc);
			return tcs.Task;
		}
	}
}
