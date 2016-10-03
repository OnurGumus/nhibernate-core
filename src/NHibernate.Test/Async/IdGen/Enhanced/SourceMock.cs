#if NET_4_5
using NHibernate.Id.Enhanced;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.IdGen.Enhanced
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SourceMock : IAccessCallback
	{
		public Task<long> GetNextValueAsync()
		{
			try
			{
				return Task.FromResult<long>(GetNextValue());
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<long>(ex);
			}
		}
	}
}
#endif
