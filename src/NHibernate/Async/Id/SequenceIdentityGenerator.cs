#if NET_4_5
using NHibernate.Engine;
using NHibernate.Id.Insert;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Id
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SequenceIdentityGenerator : SequenceGenerator, IPostInsertIdentifierGenerator
	{
		public override Task<object> GenerateAsync(ISessionImplementor session, object obj)
		{
			try
			{
				return Task.FromResult<object>(Generate(session, obj));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}
#endif
