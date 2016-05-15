#if NET_4_5
using log4net;
using NHibernate.Event;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH1230
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class PreSaveDoVeto : IPreInsertEventListener
	{
		/// <summary> Return true if the operation should be vetoed</summary>
		/// <param name = "event"></param>
		public Task<bool> OnPreInsertAsync(PreInsertEvent @event)
		{
			try
			{
				return Task.FromResult<bool>(OnPreInsert(event));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<bool>(ex);
			}
		}
	}
}
#endif
