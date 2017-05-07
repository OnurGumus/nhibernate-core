﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Type;
using NHibernate.Util;

namespace NHibernate.Id
{
	using System.Threading.Tasks;
	/// <content>
	/// Contains generated async methods
	/// </content>
	public partial class UUIDHexGenerator : IIdentifierGenerator, IConfigurable
	{

		#region IIdentifierGenerator Members

		/// <summary>
		/// Generate a new <see cref="string"/> for the identifier using the "uuid.hex" algorithm.
		/// </summary>
		/// <param name="session">The <see cref="ISessionImplementor"/> this id is being generated in.</param>
		/// <param name="obj">The entity for which the id is being generated.</param>
		/// <returns>The new identifier as a <see cref="string"/>.</returns>
		public virtual Task<object> GenerateAsync(ISessionImplementor session, object obj)
		{
			try
			{
				string guidString = GenerateNewGuid();
				if (format != FormatWithDigitsOnly && sep != null)
				{
					return Task.FromResult<object>(StringHelper.Replace(guidString, "-", sep));
				}

				return Task.FromResult<object>(guidString);
			}
			catch (Exception ex)
			{
				return Task.FromException<object>(ex);
			}
		}

		#endregion
		#region IConfigurable Members

		#endregion
	}
}