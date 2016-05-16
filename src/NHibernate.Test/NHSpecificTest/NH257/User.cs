using System;
using System.Collections.Generic;

namespace NHibernate.Test.NHSpecificTest.NH257
{
	[Serializable]
	public partial class User : Party
	{
		private ISet<UserGroup> _groups = new HashSet<UserGroup>();

		public ISet<UserGroup> Groups
		{
			get { return _groups; }
			set { _groups = value; }
		}
	}
}