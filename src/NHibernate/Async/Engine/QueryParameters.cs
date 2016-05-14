#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Impl;
using NHibernate.Param;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Engine
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public sealed partial class QueryParameters
	{
		public async Task LogParametersAsync(ISessionFactoryImplementor factory)
		{
			var print = new Printer(factory);
			if (PositionalParameterValues.Length != 0)
			{
				log.Debug("parameters: " + await (print.ToStringAsync(PositionalParameterTypes, PositionalParameterValues)));
			}

			if (NamedParameters != null)
			{
				log.Debug("named parameters: " + await (print.ToStringAsync(NamedParameters)));
			}
		}
	}
}
#endif
