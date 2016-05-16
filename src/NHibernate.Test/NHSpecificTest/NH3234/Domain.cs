using System;
using System.Collections.Generic;

namespace NHibernate.Test.NHSpecificTest.NH3234
{
	public partial class GridLevel
	{
		public Guid Id { get; set; }
	}

	public partial class GridWidget
	{
		public GridWidget()
		{
			Levels = new List<GridLevel>();
		}

		public Guid Id { get; set; }

		public ICollection<GridLevel> Levels { get; private set; }
	}
}
