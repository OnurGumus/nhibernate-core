﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;

namespace NHibernate.Param
{
	using System.Threading.Tasks;
	/// <content>
	/// Contains generated async methods
	/// </content>
	public partial class PositionalParameterSpecification : AbstractExplicitParameterSpecification
	{

		public override Task BindAsync(DbCommand command, IList<Parameter> sqlQueryParametersList, QueryParameters queryParameters, ISessionImplementor session)
		{
			try
			{
				Bind(command, sqlQueryParametersList, 0, sqlQueryParametersList, queryParameters, session);
				return Task.CompletedTask;
			}
			catch (Exception ex)
			{
				return Task.FromException<object>(ex);
			}
		}
	}
}