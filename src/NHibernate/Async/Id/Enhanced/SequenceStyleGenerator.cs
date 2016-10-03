#if NET_4_5
using System.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Id.Enhanced
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SequenceStyleGenerator : IPersistentIdentifierGenerator, IConfigurable
	{
		public virtual async Task<object> GenerateAsync(ISessionImplementor session, object obj)
		{
			return await (Optimizer.GenerateAsync(DatabaseStructure.BuildCallback(session)));
		}
	}
}
#endif
