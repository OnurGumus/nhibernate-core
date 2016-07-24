#if NET_4_5
using System;
using System.Collections.Generic;
using NHibernate.Mapping;
using NUnit.Framework;
using NHibernate.Cfg;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.FilterTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FilterSecondPassArgsFixtureAsync
	{
		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class FakeFilterable : IFilterable
		{
			public void AddFilter(string name, string condition)
			{
				throw new NotImplementedException();
			}

			public IDictionary<string, string> FilterMap
			{
				get
				{
					throw new NotImplementedException();
				}
			}
		}

		[Test]
		public Task CtorProtectionAsync()
		{
			try
			{
				Assert.Throws<ArgumentNullException>(() => new FilterSecondPassArgs(null, ""));
				Assert.Throws<ArgumentNullException>(() => new FilterSecondPassArgs(null, "a>1"));
				Assert.Throws<ArgumentNullException>(() => new FilterSecondPassArgs(new FakeFilterable(), null));
				Assert.Throws<ArgumentNullException>(() => new FilterSecondPassArgs(new FakeFilterable(), ""));
				Assert.DoesNotThrow(() => new FilterSecondPassArgs(new FakeFilterable(), "a>1"));
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
