#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Unconstrained
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SimplyManyToOneIgnoreTestAsync : TestCaseAsync
	{
		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		protected override IList Mappings
		{
			get
			{
				return new string[]{"Unconstrained.Simply.hbm.xml"};
			}
		}

		[Test]
		public async Task UnconstrainedAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					SimplyB sb = new SimplyB(100);
					SimplyA sa = new SimplyA("ralph");
					sa.SimplyB = sb;
					await (s.SaveAsync(sb));
					await (s.SaveAsync(sa));
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
			{
				using (ITransaction t = s.BeginTransaction())
				{
					SimplyB sb = (SimplyB)await (s.GetAsync(typeof (SimplyB), 100));
					Assert.IsNotNull(sb);
					await (s.DeleteAsync(sb));
					await (t.CommitAsync());
				}

				// Have to do this in a separate transaction, otherwise ISession.Get retrieves
				// the cached version of SimplyA with its B being not null.
				using (ITransaction t = s.BeginTransaction())
				{
					SimplyA sa = (SimplyA)await (s.GetAsync(typeof (SimplyA), "ralph"));
					Assert.IsNull(sa.SimplyB);
					await (s.DeleteAsync(sa));
					await (t.CommitAsync());
				}
			}
		}
	}
}
#endif
