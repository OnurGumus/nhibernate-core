using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.AsyncGenerator
{
	public class MethodAnalyzeResult
	{
		public bool IsValid { get; set; } = true;

		public HashSet<MethodReferenceResult> ReferenceResults { get; } = new HashSet<MethodReferenceResult>();

		//public bool CanBeCompletelyAsync => CanBeAsnyc && ReferenceResults.All(o => o.CanBeAsync);

		public bool CanSkipAsync { get; internal set; }

		public bool MustRunSynchronized { get; internal set; }

		public bool HasBody { get; internal set; }

		public bool Yields { get; internal set; }

		public bool IsEmpty { get; internal set; }
	}
}
