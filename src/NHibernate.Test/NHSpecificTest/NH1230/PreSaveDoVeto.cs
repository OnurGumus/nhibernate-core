using System.Threading.Tasks;
using log4net;
using NHibernate.Event;

namespace NHibernate.Test.NHSpecificTest.NH1230
{
	public class PreSaveDoVeto : IPreInsertEventListener
	{
		private ILog log = LogManager.GetLogger(typeof(PreSaveDoVeto));

		#region IPreInsertEventListener Members

		/// <summary> Return true if the operation should be vetoed</summary>
		/// <param name="event"></param>
		public Task<bool> OnPreInsert(PreInsertEvent @event)
		{
			log.Debug("OnPreInsert: The entity will be vetoed.");
			
			return Task.FromResult(true);
		}

		#endregion
	}
}