﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System.Collections;
using System.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Type;

namespace NHibernate.Id
{
	using System.Threading.Tasks;
	/// <content>
	/// Contains generated async methods
	/// </content>
	public partial class ForeignGenerator : IIdentifierGenerator, IConfigurable
	{

		#region IIdentifierGenerator Members

		/// <summary>
		/// Generates an identifier from the value of a Property. 
		/// </summary>
		/// <param name="sessionImplementor">The <see cref="ISessionImplementor"/> this id is being generated in.</param>
		/// <param name="obj">The entity for which the id is being generated.</param>
		/// <returns>
		/// The identifier value from the associated object or  
		/// <see cref="IdentifierGeneratorFactory.ShortCircuitIndicator"/> if the <c>session</c>
		/// already contains <c>obj</c>.
		/// </returns>
		public async Task<object> GenerateAsync(ISessionImplementor sessionImplementor, object obj)
		{
			ISession session = (ISession) sessionImplementor;

			var persister = sessionImplementor.Factory.GetEntityPersister(entityName);
			object associatedObject = persister.GetPropertyValue(obj, propertyName);

			if (associatedObject == null)
			{
				throw new IdentifierGenerationException("attempted to assign id from null one-to-one property: " + propertyName);
			}

			EntityType foreignValueSourceType;
			IType propertyType = persister.GetPropertyType(propertyName);
			if (propertyType.IsEntityType)
			{
				foreignValueSourceType = (EntityType) propertyType;
			}
			else
			{
				// try identifier mapper
				foreignValueSourceType = (EntityType) persister.GetPropertyType("_identifierMapper." + propertyName);
			}

			object id;
			try
			{
				id = ForeignKeys.GetEntityIdentifierIfNotUnsaved(
					foreignValueSourceType.GetAssociatedEntityName(),
					associatedObject,
					sessionImplementor);
			}
			catch (TransientObjectException)
			{
				id = await (session.SaveAsync(foreignValueSourceType.GetAssociatedEntityName(), associatedObject)).ConfigureAwait(false);
			}

			if (session.Contains(obj))
			{
				//abort the save (the object is already saved by a circular cascade)
				return IdentifierGeneratorFactory.ShortCircuitIndicator;
			}

			return id;
		}

		#endregion
		#region IConfigurable Members

		#endregion
	}
}