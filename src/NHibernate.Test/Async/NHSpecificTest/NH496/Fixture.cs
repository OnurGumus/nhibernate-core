#if NET_4_5
using System;
using NHibernate.Bytecode;
using NUnit.Framework;
using Environment = NHibernate.Cfg.Environment;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH496
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "NH496";
			}
		}

		[Test]
		public async Task CRUDAsync()
		{
			if (Environment.BytecodeProvider is NullBytecodeProvider)
			{
				Assert.Ignore("This test only runs with a non-null bytecode provider");
			}

			WronglyMappedClass obj = new WronglyMappedClass();
			obj.SomeInt = 10;
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.SaveAsync(obj));
					await (t.CommitAsync());
				}

			try
			{
				using (ISession s = OpenSession())
					using (ITransaction t = s.BeginTransaction())
					{
						Assert.ThrowsAsync<PropertyAccessException>(async () => await (s.GetAsync(typeof (WronglyMappedClass), obj.Id)));
						await (t.CommitAsync());
					}
			}
			finally
			{
				using (ISession s = OpenSession())
					using (ITransaction t = s.BeginTransaction())
					{
						await (s.DeleteAsync(obj));
						await (t.CommitAsync());
					}
			}
		}
	}
}
#endif
