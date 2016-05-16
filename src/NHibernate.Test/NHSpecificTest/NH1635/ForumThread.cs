using System.Collections.Generic;

namespace NHibernate.Test.NHSpecificTest.NH1635
{
	public partial class ForumThread
	{
		public ForumThread()
		{
			Messages = new List<ForumMessage>();
		}

		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
		public virtual IList<ForumMessage> Messages { get; set; }
		public virtual ForumMessage LatestMessage { get; set; }
	}
}