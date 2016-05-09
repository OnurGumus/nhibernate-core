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
	public sealed partial class CollectionRecreateAction : CollectionAction
	{
		public override async Task ExecuteAsync()
		{
			bool statsEnabled = Session.Factory.Statistics.IsStatisticsEnabled;
			Stopwatch stopwatch = null;
			if (statsEnabled)
			{
				stopwatch = Stopwatch.StartNew();
			}

			IPersistentCollection collection = Collection;
			PreRecreate();
			await (Persister.RecreateAsync(collection, Key, Session));
			await (Session.PersistenceContext.GetCollectionEntry(collection).AfterActionAsync(collection));
			Evict();
			PostRecreate();
			if (statsEnabled)
			{
				stopwatch.Stop();
				Session.Factory.StatisticsImplementor.RecreateCollection(Persister.Role, stopwatch.Elapsed);
			}
		}
	}
}