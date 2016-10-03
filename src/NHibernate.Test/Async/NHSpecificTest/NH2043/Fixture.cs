#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2043
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class Namer : EmptyInterceptor
		{
			public override string GetEntityName(object entity)
			{
				if (entity.GetType().Name.EndsWith("Impl"))
					return entity.GetType().BaseType.FullName;
				return base.GetEntityName(entity);
			}
		}

		protected override void BuildSessionFactory()
		{
			cfg.SetInterceptor(new Namer());
			base.BuildSessionFactory();
		}

		[Test]
		public async Task TestAsync()
		{
			try
			{
				using (ISession s = OpenSession())
				{
					var a = new AImpl{Id = 1, Name = "A1"};
					var b = new BImpl{Id = 1, Name = "B1", A = a};
					a.B = b;
					await (s.SaveAsync(a));
					await (s.SaveAsync(b));
					await (s.FlushAsync());
				}
			}
			finally
			{
				using (ISession s = OpenSession())
				{
					await (s.DeleteAsync("from B"));
					await (s.DeleteAsync("from A"));
					await (s.FlushAsync());
				}
			}
		}
	}
}
#endif
