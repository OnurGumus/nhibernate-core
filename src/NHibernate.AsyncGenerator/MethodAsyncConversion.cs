using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.AsyncGenerator
{
	public enum MethodAsyncConversion
	{
		None = 0,
		ToAsync = 1,
		/// <summary>
		/// Will convert to async only if there is invoked at least one method that has an async counterpart
		/// </summary>
		Smart = 3
	}
}
