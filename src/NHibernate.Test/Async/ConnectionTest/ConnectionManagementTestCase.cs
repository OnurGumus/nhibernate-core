#if NET_4_5
using System;
using System.Collections;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.ConnectionTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class ConnectionManagementTestCaseAsync : TestCaseAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new string[]{"ConnectionTest.Silly.hbm.xml"};
			}
		}

		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		protected virtual void Prepare()
		{
		}

		protected virtual void Done()
		{
		}

		protected abstract ISession GetSessionUnderTest();
		protected virtual Task ReleaseAsync(ISession session)
		{
			try
			{
				if (session != null && session.IsOpen)
				{
					try
					{
						session.Close();
					}
					catch (Exception x)
					{
					// Ignore
					}
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
