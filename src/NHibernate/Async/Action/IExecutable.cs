#if NET_4_5
using NHibernate.Engine;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Action
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IExecutable
	{
		/// <summary> Execute this action</summary>
		Task ExecuteAsync();
	}
}
#endif
