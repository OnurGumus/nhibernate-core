#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH1907
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public Task CanSetParameterQueryByNameAsync()
		{
			try
			{
				using (ISession s = OpenSession())
				{
					var q = s.CreateQuery("from Something s where s.Relation = :aParam");
					Assert.DoesNotThrow(() => q.SetParameter("aParam", new MyType{ToPersist = 1}));
				}

				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public Task CanSetParameterQueryByPositionAsync()
		{
			try
			{
				using (ISession s = OpenSession())
				{
					var q = s.CreateQuery("from Something s where s.Relation = ?");
					q.SetParameter(0, new MyType{ToPersist = 1});
					Assert.DoesNotThrow(() => q.SetParameter(0, new MyType{ToPersist = 1}));
				}

				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}
#endif
