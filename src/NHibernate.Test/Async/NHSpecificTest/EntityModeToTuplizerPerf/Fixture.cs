#if NET_4_5
using System;
using System.Diagnostics;
using NHibernate.Tuple;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.EntityModeToTuplizerPerf
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture
	{
		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private partial class TuplizerStub : ITuplizer
		{
			public Task<object> InstantiateAsync()
			{
				try
				{
					return Task.FromResult<object>(Instantiate());
				}
				catch (Exception ex)
				{
					return TaskHelper.FromException<object>(ex);
				}
			}
		}
	}
}
#endif
