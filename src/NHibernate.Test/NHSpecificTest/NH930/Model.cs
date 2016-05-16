using System;
using System.Collections;

namespace NHibernate.Test.NHSpecificTest.NH930
{
	public abstract partial class NVariable
	{
		protected int m_id;
		protected IList m_precedentVariables;

		public virtual int Id
		{
			get { return m_id; }
			set { m_id = value; }
		}

		public virtual IList PrecedentVariables
		{
			get { return m_precedentVariables; }
			set { m_precedentVariables = value; }
		}
	}

	public partial class NConditionalUDV : NVariable
	{
	}

	public partial class NFilterUDV : NVariable
	{
	}

}