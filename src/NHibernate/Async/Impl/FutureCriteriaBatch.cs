using System.Collections;
using System.Threading.Tasks;

namespace NHibernate.Impl
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FutureCriteriaBatch : FutureBatch<ICriteria, IMultiCriteria>
	{
		protected override async Task<IList> GetResultsFromAsync(IMultiCriteria multiApproach)
		{
			return await (multiApproach.ListAsync());
		}
	}
}