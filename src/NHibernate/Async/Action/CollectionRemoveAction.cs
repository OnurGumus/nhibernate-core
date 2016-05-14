#if NET_4_5
using System;
using System.Diagnostics;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Persister.Collection;
using System.Threading.Tasks;

namespace NHibernate.Action
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public sealed partial class CollectionRemoveAction : CollectionAction
	{
		public override async Task ExecuteAsync()
		{
			bool statsEnabled = Session.Factory.Statistics.IsStatisticsEnabled;
			Stopwatch stopwatch = null;
			if (statsEnabled)
			{
				stopwatch = Stopwatch.StartNew();
			}

			PreRemove();
			if (!emptySnapshot)
			{
				await (Persister.RemoveAsync(Key, Session));
			}

			IPersistentCollection collection = Collection;
			if (collection != null)
			{
				await (Session.PersistenceContext.GetCollectionEntry(collection).AfterActionAsync(collection));
			}

			Evict();
			PostRemove();
			if (statsEnabled)
			{
				stopwatch.Stop();
				Session.Factory.StatisticsImplementor.RemoveCollection(Persister.Role, stopwatch.Elapsed);
			}
		}
	}
}
#endif
