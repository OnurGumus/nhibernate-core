#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Join
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class JoinedFilters : TestCase
	{
		[Test]
		public async Task FilterOnPrimaryTableAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					s.EnableFilter("NameFilter").SetParameter("name", "Nadal");
					await (CreatePlayersAsync(s));
					IList<TennisPlayer> people = s.CreateCriteria<TennisPlayer>().List<TennisPlayer>();
					Assert.AreEqual(1, people.Count);
					Assert.AreEqual("Nadal", people[0].Name);
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task FilterOnJoinedTableAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					s.EnableFilter("MakeFilter").SetParameter("make", "Babolat");
					await (CreatePlayersAsync(s));
					IList<TennisPlayer> people = s.CreateCriteria<TennisPlayer>().List<TennisPlayer>();
					Assert.AreEqual(1, people.Count);
					Assert.AreEqual("Babolat", people[0].RacquetMake);
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task FilterOnJoinedTableWithRepeatedColumnAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					s.EnableFilter("ModelFilter").SetParameter("model", "AeroPro Drive");
					await (CreatePlayersAsync(s));
					IList<TennisPlayer> people = s.CreateCriteria<TennisPlayer>().List<TennisPlayer>();
					Assert.AreEqual(1, people.Count);
					Assert.AreEqual("AeroPro Drive", people[0].RacquetModel);
					await (tx.CommitAsync());
				}
		}

		private static async Task CreatePlayersAsync(ISession s)
		{
			await (CreateAndSavePlayerAsync(s, "Nadal", "Babolat", "AeroPro Drive"));
			await (CreateAndSavePlayerAsync(s, "Federer", "Wilson", "Six.One Tour BLX"));
			await (s.FlushAsync());
		}

		private static async Task CreateAndSavePlayerAsync(ISession session, string name, string make, string model)
		{
			var s = new TennisPlayer()
			{Name = name, RacquetMake = make, RacquetModel = model};
			await (session.SaveAsync(s));
		}
	}
}
#endif
