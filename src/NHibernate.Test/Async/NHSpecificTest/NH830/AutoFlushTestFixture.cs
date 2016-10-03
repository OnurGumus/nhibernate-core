#if NET_4_5
using System.Collections;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH830
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class AutoFlushTestFixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task AutoFlushTestAsync()
		{
			ISession sess = OpenSession();
			ITransaction t = sess.BeginTransaction();
			//Setup the test data
			Cat mum = new Cat();
			Cat son = new Cat();
			await (sess.SaveAsync(mum));
			await (sess.SaveAsync(son));
			await (sess.FlushAsync());
			//reload the data and then setup the many-to-many association
			mum = (Cat)await (sess.GetAsync(typeof (Cat), mum.Id));
			son = (Cat)await (sess.GetAsync(typeof (Cat), son.Id));
			mum.Children.Add(son);
			son.Parents.Add(mum);
			//Use criteria API to search first 
			IList result = await (sess.CreateCriteria(typeof (Cat)).CreateAlias("Children", "child").Add(Expression.Eq("child.Id", son.Id)).ListAsync());
			//the criteria failed to find the mum cat with the child
			Assert.AreEqual(1, result.Count);
			await (sess.DeleteAsync(mum));
			await (sess.DeleteAsync(son));
			await (t.CommitAsync());
			sess.Close();
		}
	}
}
#endif
