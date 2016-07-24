#if NET_4_5
using System;
using System.Collections;
using System.Data.Common;
using NHibernate.DomainModel.NHSpecific;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class GetSetHelperFixtureAsync : TestCaseAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new string[]{"NHSpecific.GetSetHelper.hbm.xml"};
			}
		}

		[Test]
		public async Task TestDefaultValueAsync()
		{
			using (ISession s1 = OpenSession())
			{
				DbCommand cmd = s1.Connection.CreateCommand();
				cmd.CommandText = "insert into GetSetHelper(ID) values(1)";
				await (cmd.ExecuteNonQueryAsync());
			}

			try
			{
				// load the object and check default values
				using (ISession s2 = OpenSession())
				{
					GetSetHelper gs = (GetSetHelper)await (s2.LoadAsync(typeof (GetSetHelper), 1));
					Assert.AreEqual(new int (), gs.A);
					Assert.AreEqual(new TimeSpan(), gs.B);
					Assert.AreEqual(new bool (), gs.C);
					Assert.AreEqual(new DateTime(), gs.D);
					Assert.AreEqual(new short (), gs.E);
					Assert.AreEqual(new byte (), gs.F);
					Assert.AreEqual(new float (), gs.G);
					Assert.AreEqual(new double (), gs.H);
					Assert.AreEqual(new decimal (), gs.I);
					Assert.AreEqual(new GetSetHelper.TestEnum(), gs.L);
					Assert.IsNull(gs.M);
				}
			}
			finally
			{
				await (ExecuteStatementAsync("delete from GetSetHelper"));
			}
		}
	}
}
#endif
