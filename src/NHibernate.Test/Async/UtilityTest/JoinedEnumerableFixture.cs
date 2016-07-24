#if NET_4_5
using System;
using System.Collections;
using NHibernate.Util;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.UtilityTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class JoinedEnumerableFixtureAsync
	{
		/// <summary>
		/// Test that the JoinedEnumerable works with a single wrapped
		/// IEnumerable as expected when fully enumerating the collection.
		/// </summary>
		[Test]
		public void WrapsSingle()
		{
			int[] expected = new int[]{1, 2, 3};
			EnumerableTester first;
			JoinedEnumerable joined = InitSingle(out first);
			int index = 0;
			foreach (int actual in joined)
			{
				Assert.AreEqual(expected[index], actual, "Failure at " + index.ToString());
				index++;
			}

			Assert.AreEqual(expected.Length, index, "Every expected value was found");
			Assert.IsTrue(first.WasDisposed, "should have been disposed of.");
		}

		/// <summary>
		/// Test that the wrapped IEnumerator has Dispose called even when
		/// the JoinedEnumerator doesn't enumerate all the way through the
		/// collection.
		/// </summary>
		[Test]
		public void WrapsSingleBreak()
		{
			int[] expected = new int[]{1, 2, 3};
			EnumerableTester first;
			JoinedEnumerable joined = InitSingle(out first);
			foreach (int actual in joined)
			{
				Assert.AreEqual(expected[0], actual, "first one was not what was expected.");
				break;
			}

			// ensure that the first was disposed of even though we didn't enumerate
			// all the way through
			Assert.IsTrue(first.WasDisposed, "should have been disposed of.");
		}

		/// <summary>
		/// Test that the JoinedEnumerable works with multiple wrapped
		/// IEnumerable as expected when fully enumerating the collections.
		/// </summary>
		[Test]
		public void WrapsMultiple()
		{
			int[] expected = new int[]{1, 2, 3, 4, 5, 6};
			EnumerableTester first;
			EnumerableTester second;
			JoinedEnumerable joined = InitMultiple(out first, out second);
			int index = 0;
			foreach (int actual in joined)
			{
				Assert.AreEqual(expected[index], actual, "Failure at " + index.ToString());
				index++;
			}

			Assert.AreEqual(expected.Length, index, "Every expected value was found");
			Assert.IsTrue(first.WasDisposed, "first should have been disposed of.");
			Assert.IsTrue(second.WasDisposed, "second should have been disposed of. ");
		}

		/// <summary>
		/// Test that the JoinedEnumerable works with multiple wrapped
		/// IEnumerable as expected when breaking out.
		/// </summary>
		[Test]
		public void WrapsMultipleBreak()
		{
			// break in the first IEnumerator
			WrapsMultipleBreak(2);
			// ensure behavior is consistent if break in 2nd IEnumerator
			WrapsMultipleBreak(4);
		}

		private void WrapsMultipleBreak(int breakIndex)
		{
			int[] expected = new int[]{1, 2, 3, 4, 5, 6};
			EnumerableTester first;
			EnumerableTester second;
			JoinedEnumerable joined = InitMultiple(out first, out second);
			int index = 0;
			foreach (int actual in joined)
			{
				Assert.AreEqual(expected[index], actual, "Failure at " + index.ToString());
				index++;
				if (index == breakIndex)
				{
					break;
				}
			}

			Assert.IsTrue(first.WasDisposed, "first should have been disposed of.");
			Assert.IsTrue(second.WasDisposed, "second should have been disposed of. ");
		}

		private JoinedEnumerable InitSingle(out EnumerableTester first)
		{
			first = new EnumerableTester(new ArrayList(new int[]{1, 2, 3}));
			return new JoinedEnumerable(new IEnumerable[]{first});
		}

		private JoinedEnumerable InitMultiple(out EnumerableTester first, out EnumerableTester second)
		{
			first = new EnumerableTester(new ArrayList(new int[]{1, 2, 3}));
			second = new EnumerableTester(new ArrayList(new int[]{4, 5, 6}));
			return new JoinedEnumerable(new IEnumerable[]{first, second});
		}
	}
}
#endif
