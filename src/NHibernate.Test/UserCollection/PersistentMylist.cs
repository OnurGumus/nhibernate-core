using System;
using System.Collections.Generic;
using NHibernate.Collection.Generic;
using NHibernate.Engine;

namespace NHibernate.Test.UserCollection
{
	public partial class PersistentMylist : PersistentGenericList<Email>
	{
		public PersistentMylist(ISessionImplementor session, IList<Email> list) : base(session, list)
		{
		}

		public PersistentMylist(ISessionImplementor session) : base(session)
		{
		}
	}
}