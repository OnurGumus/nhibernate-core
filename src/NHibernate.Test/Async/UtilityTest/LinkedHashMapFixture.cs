#if NET_4_5
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NHibernate.Util;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.UtilityTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class LinkedHashMapFixtureAsync
	{
		private static readonly Player[] players = {new Player("12341", "Boeta Dippenaar"), new Player("23432", "Gary Kirsten"), new Player("23411", "Graeme Smith"), new Player("55221", "Jonty Rhodes"), new Player("61234", "Monde Zondeki"), new Player("23415", "Paul Adams")};
		private static void Fill(IDictionary<string, Player> lhm)
		{
			foreach (Player player in players)
				lhm.Add(player.Id, player);
		}

		[Test]
		public void Add()
		{
			IDictionary<string, Player> lhm = new LinkedHashMap<string, Player>();
			Fill(lhm);
			lhm.Add("55555", new Player("55555", "Monde Zondeki"));
			Assert.AreEqual(7, lhm.Count);
		}

		[Test]
		public void LastKeyLastValue()
		{
			LinkedHashMap<string, Player> lhm = new LinkedHashMap<string, Player>();
			Fill(lhm);
			Assert.AreEqual(players[players.Length - 1].Id, lhm.LastKey);
			Assert.AreEqual(players[players.Length - 1], lhm.LastValue);
			// override
			Player antWithSameId = new Player("12341", "Another");
			lhm[antWithSameId.Id] = antWithSameId;
			Assert.AreEqual(antWithSameId.Id, lhm.LastKey);
			Assert.AreEqual(antWithSameId, lhm.LastValue);
		}

		[Test]
		public void FirstKeyFirstValue()
		{
			LinkedHashMap<string, Player> lhm = new LinkedHashMap<string, Player>();
			Fill(lhm);
			Assert.AreEqual(players[0].Id, lhm.FirstKey);
			Assert.AreEqual(players[0], lhm.FirstValue);
			// override First
			Player antWithSameId = new Player("12341", "Another");
			lhm[antWithSameId.Id] = antWithSameId;
			Assert.AreEqual(players[1].Id, lhm.FirstKey);
			Assert.AreEqual(players[1], lhm.FirstValue);
		}

		[Test]
		public void Clear()
		{
			IDictionary<string, Player> lhm = new LinkedHashMap<string, Player>();
			Player p = new Player("78945", "Someone");
			lhm[p.Id] = p;
			lhm.Clear();
			Assert.AreEqual(0, lhm.Count);
			foreach (KeyValuePair<string, Player> pair in lhm)
				Assert.Fail("Should not be any entries but found Key = " + pair.Key + " and Value = " + pair.Value);
		}

		[Test]
		public void Contains()
		{
			LinkedHashMap<string, Player> lhm = new LinkedHashMap<string, Player>();
			Fill(lhm);
			Assert.IsTrue(lhm.Contains("12341"));
			Assert.IsFalse(lhm.Contains("55555"));
		}

		[Test]
		public void GetEnumerator()
		{
			IDictionary<string, Player> lhm = new LinkedHashMap<string, Player>();
			Fill(lhm);
			int index = 0;
			foreach (KeyValuePair<string, Player> pair in lhm)
			{
				Assert.AreEqual(players[index].Id, pair.Key);
				Assert.AreEqual(players[index], pair.Value);
				index++;
			}

			Assert.AreEqual(6, index);
		}

		[Test]
		public void GetEnumeratorEmpty()
		{
			IDictionary<string, Player> lhm = new LinkedHashMap<string, Player>();
			Assert.AreEqual(0, lhm.Count);
			int entries = 0;
			foreach (KeyValuePair<string, Player> pair in lhm)
				entries++;
			foreach (string s in lhm.Keys)
				entries++;
			foreach (Player value in lhm.Values)
				entries++;
			Assert.AreEqual(0, entries, "should not have any entries in the enumerators");
		}

		[Test]
		public Task GetEnumeratorModifyExceptionFromAddAsync()
		{
			try
			{
				IDictionary<string, Player> lhm = new LinkedHashMap<string, Player>();
				lhm["123"] = new Player("123", "yyyyyyy");
				Assert.Throws<InvalidOperationException>(() =>
				{
					foreach (KeyValuePair<string, Player> pair in lhm)
					{
						lhm["78945"] = new Player("78945", "Someone");
					}
				}

				);
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public Task GetEnumeratorModifyExceptionFromRemoveAsync()
		{
			try
			{
				IDictionary<string, Player> lhm = new LinkedHashMap<string, Player>();
				lhm["123"] = new Player("123", "yyyyyyy");
				Assert.Throws<InvalidOperationException>(() =>
				{
					foreach (KeyValuePair<string, Player> pair in lhm)
					{
						lhm.Remove(pair.Key);
					}
				}

				);
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public Task GetEnumeratorModifyExceptionFromUpdateAsync()
		{
			try
			{
				IDictionary<string, Player> lhm = new LinkedHashMap<string, Player>();
				lhm["123"] = new Player("123", "yyyyyyy");
				Assert.Throws<InvalidOperationException>(() =>
				{
					foreach (KeyValuePair<string, Player> pair in lhm)
					{
						lhm["123"] = new Player("123", "aaaaaaa");
					}
				}

				);
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public void Remove()
		{
			IDictionary<string, Player> lhm = new LinkedHashMap<string, Player>();
			Fill(lhm);
			// remove an item that exists
			bool removed = lhm.Remove("23411");
			Assert.IsTrue(removed);
			Assert.AreEqual(5, lhm.Count);
			// try to remove an item that does not exist
			removed = lhm.Remove("65432");
			Assert.IsFalse(removed);
			Assert.AreEqual(5, lhm.Count);
		}

		[Test]
		public void ContainsValue()
		{
			LinkedHashMap<string, Player> lhm = new LinkedHashMap<string, Player>();
			Fill(lhm);
			Assert.IsTrue(lhm.ContainsValue(new Player("55221", "Jonty Rhodes")));
			Assert.IsFalse(lhm.ContainsValue(new Player("55221", "SameKeyDiffName")));
		}

		[Test]
		public void CopyTo()
		{
			IDictionary<string, Player> lhm = new LinkedHashMap<string, Player>();
			Fill(lhm);
			KeyValuePair<string, Player>[] destArray = new KeyValuePair<string, Player>[lhm.Count + 1];
			destArray[0] = new KeyValuePair<string, Player>("999", new Player("999", "The number nine"));
			lhm.CopyTo(destArray, 1);
			for (int i = 1; i < destArray.Length; i++)
			{
				Assert.AreEqual(players[i - 1].Id, destArray[i].Key);
				Assert.AreEqual(players[i - 1], destArray[i].Value);
			}
		}

		[Test]
		public void Keys()
		{
			IDictionary<string, Player> lhm = new LinkedHashMap<string, Player>();
			Fill(lhm);
			int index = 0;
			foreach (string s in lhm.Keys)
			{
				Assert.AreEqual(players[index].Id, s);
				index++;
			}
		}

		[Test]
		public void Values()
		{
			IDictionary<string, Player> lhm = new LinkedHashMap<string, Player>();
			Fill(lhm);
			int index = 0;
			foreach (Player p in lhm.Values)
			{
				Assert.AreEqual(players[index], p);
				index++;
			}
		}

		[Test]
		public void Serialization()
		{
			IDictionary<string, Player> lhm = new LinkedHashMap<string, Player>();
			Fill(lhm);
			MemoryStream stream = new MemoryStream();
			BinaryFormatter f = new BinaryFormatter();
			f.Serialize(stream, lhm);
			stream.Position = 0;
			LinkedHashMap<string, Player> dlhm = (LinkedHashMap<string, Player>)f.Deserialize(stream);
			stream.Close();
			Assert.AreEqual(6, dlhm.Count);
			int index = 0;
			foreach (KeyValuePair<string, Player> pair in dlhm)
			{
				Assert.AreEqual(players[index].Id, pair.Key);
				Assert.AreEqual(players[index], pair.Value);
				index++;
			}

			Assert.AreEqual(6, index);
		}

		[Test, Explicit]
		public async Task ShowDiffAsync()
		{
			IDictionary<string, Player> dict = new Dictionary<string, Player>();
			IDictionary<string, Player> lhm = new LinkedHashMap<string, Player>();
			Fill(dict);
			Fill(lhm);
			// Override the first element
			Player o = new Player("12341", "Ovirride");
			dict[o.Id] = o;
			lhm[o.Id] = o;
			await (Console.Out.WriteLineAsync("Dictionary order:"));
			foreach (KeyValuePair<string, Player> pair in dict)
			{
				Console.Out.WriteLine("Key->{0}", pair.Key);
			}

			await (Console.Out.WriteLineAsync("LinkedHashMap order:"));
			foreach (KeyValuePair<string, Player> pair in lhm)
			{
				Console.Out.WriteLine("Key->{0}", pair.Key);
			}
		}

		[Test, Explicit]
		public async Task PerformanceAsync()
		{
			// Take care with this test because the result is not the same every times
			int numOfRuns = 4;
			int numOfEntries = Int16.MaxValue;
			long[] dictPopulateTicks = new long[numOfRuns];
			long[] dictItemTicks = new long[numOfRuns];
			long[] linkPopulateTicks = new long[numOfRuns];
			long[] linkItemTicks = new long[numOfRuns];
			for (int runIndex = 0; runIndex < numOfRuns; runIndex++)
			{
				string key;
				object value;
				IDictionary<string, object> dictionary = new Dictionary<string, object>();
				IDictionary<string, object> linked = new LinkedHashMap<string, object>();
				long dictStart = DateTime.Now.Ticks;
				for (int i = 0; i < numOfEntries; i++)
				{
					dictionary.Add("test" + i, new object ());
				}

				dictPopulateTicks[runIndex] = DateTime.Now.Ticks - dictStart;
				dictStart = DateTime.Now.Ticks;
				for (int i = 0; i < numOfEntries; i++)
				{
					key = "test" + i;
					value = dictionary[key];
				}

				dictItemTicks[runIndex] = DateTime.Now.Ticks - dictStart;
				dictionary.Clear();
				long linkStart = DateTime.Now.Ticks;
				for (int i = 0; i < numOfEntries; i++)
				{
					linked.Add("test" + i, new object ());
				}

				linkPopulateTicks[runIndex] = DateTime.Now.Ticks - linkStart;
				linkStart = DateTime.Now.Ticks;
				for (int i = 0; i < numOfEntries; i++)
				{
					key = "test" + i;
					value = linked[key];
				}

				linkItemTicks[runIndex] = DateTime.Now.Ticks - linkStart;
				linked.Clear();
			}

			for (int runIndex = 0; runIndex < numOfRuns; runIndex++)
			{
				decimal linkPopulateOverhead = (linkPopulateTicks[runIndex] / (decimal)dictPopulateTicks[runIndex]);
				decimal linkItemOverhead = (linkItemTicks[runIndex] / (decimal)dictItemTicks[runIndex]);
				string message = string.Format("LinkedHashMap vs Dictionary (Run-{0}) :", runIndex + 1);
				message += "\n POPULATE:";
				message += "\n\t linked took " + linkPopulateTicks[runIndex] + " ticks.";
				message += "\n\t dictionary took " + dictPopulateTicks[runIndex] + " ticks.";
				message += "\n\t for an overhead of " + linkPopulateOverhead;
				message += "\n RETRIVE:";
				message += "\n\t linked took " + linkItemTicks[runIndex] + " ticks.";
				message += "\n\t dictionary took " + dictItemTicks[runIndex] + " ticks.";
				message += "\n\t for an overhead of " + linkItemOverhead;
				await (Console.Out.WriteLineAsync(message));
				await (Console.Out.WriteLineAsync());
			}
		}
	}
}
#endif
