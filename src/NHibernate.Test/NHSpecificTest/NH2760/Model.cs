using System;
using System.Collections.Generic;

namespace NHibernate.Test.NHSpecificTest.NH2760
{
	public partial class User
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
		public virtual string Other { get; set; }

		public virtual ICollection<UserGroup> UserGroups { get; set; }

		public User()
		{
			UserGroups = new HashSet<UserGroup>();
		}
	}

	public partial class UserGroup
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
		public virtual string Other { get; set; }

		public virtual IEnumerable<User> Users { get; set; }

		public UserGroup()
		{
			Users = new List<User>();
		}
	}
}