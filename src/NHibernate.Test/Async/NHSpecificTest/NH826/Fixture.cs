#if NET_4_5
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH826
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task BugAsync()
		{
			ISession session = OpenSession();
			ITransaction transaction = session.BeginTransaction();
			Activity activity = new Activity();
			await (session.SaveAsync(activity));
			ActivitySet activitySet = new ActivitySet();
			activitySet.Activities.Add(activity);
			await (session.SaveAsync(activitySet));
			await (transaction.CommitAsync());
			session.Close();
			session = OpenSession();
			transaction = session.BeginTransaction();
			// This works:
			//IList<ActivitySet> list = session.CreateQuery("from ActivitySet a where a.Id = 1").List<ActivitySet>();
			//Console.WriteLine("Got it? {0}", list.Count == 1);
			//session.Flush();
			// This does not
			ActivitySet loadedActivitySet = (ActivitySet)await (session.CreateCriteria(typeof (ActivitySet)).Add(Expression.Eq("Id", activitySet.Id)).UniqueResultAsync());
			await (session.FlushAsync());
			foreach (object o in loadedActivitySet.Activities)
			{
				await (session.DeleteAsync(o));
			}

			await (session.DeleteAsync(loadedActivitySet));
			await (transaction.CommitAsync());
			session.Close();
		}
	}
}
#endif
