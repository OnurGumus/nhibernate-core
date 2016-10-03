#if NET_4_5
using System.Collections;
using System.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Id
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ForeignGenerator : IIdentifierGenerator, IConfigurable
	{
		/// <summary>
		/// Generates an identifier from the value of a Property. 
		/// </summary>
		/// <param name = "sessionImplementor">The <see cref = "ISessionImplementor"/> this id is being generated in.</param>
		/// <param name = "obj">The entity for which the id is being generated.</param>
		/// <returns>
		/// The identifier value from the associated object or  
		/// <see cref = "IdentifierGeneratorFactory.ShortCircuitIndicator"/> if the <c>session</c>
		/// already contains <c>obj</c>.
		/// </returns>
		public async Task<object> GenerateAsync(ISessionImplementor sessionImplementor, object obj)
		{
			ISession session = (ISession)sessionImplementor;
			var persister = sessionImplementor.Factory.GetEntityPersister(entityName);
			object associatedObject = persister.GetPropertyValue(obj, propertyName, sessionImplementor.EntityMode);
			if (associatedObject == null)
			{
				throw new IdentifierGenerationException("attempted to assign id from null one-to-one property: " + propertyName);
			}

			EntityType foreignValueSourceType;
			IType propertyType = persister.GetPropertyType(propertyName);
			if (propertyType.IsEntityType)
			{
				foreignValueSourceType = (EntityType)propertyType;
			}
			else
			{
				// try identifier mapper
				foreignValueSourceType = (EntityType)persister.GetPropertyType("_identifierMapper." + propertyName);
			}

			object id;
			try
			{
				id = await (ForeignKeys.GetEntityIdentifierIfNotUnsavedAsync(foreignValueSourceType.GetAssociatedEntityName(), associatedObject, sessionImplementor));
			}
			catch (TransientObjectException)
			{
				id = await (session.SaveAsync(foreignValueSourceType.GetAssociatedEntityName(), associatedObject));
			}

			if (await (session.ContainsAsync(obj)))
			{
				//abort the save (the object is already saved by a circular cascade)
				return IdentifierGeneratorFactory.ShortCircuitIndicator;
			}

			return id;
		}
	}
}
#endif
