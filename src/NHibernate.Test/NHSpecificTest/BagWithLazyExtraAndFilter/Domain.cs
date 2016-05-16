using System.Collections.Generic;

namespace NHibernate.Test.NHSpecificTest.BagWithLazyExtraAndFilter
{
	public partial class Env
	{
		public virtual long Id { get; set; }
		public virtual IList<MachineRequest> RequestsFailed { get; set; }
	}

	public partial class MachineRequest
	{
		public virtual long Id { get; set; }
		public virtual int RequestCompletionStatus { get; set; }
		public virtual long EnvId { get; set; }
	}
}
