using System.Collections;
using System.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Id
{
	/// <summary>
	/// An <see cref = "IIdentifierGenerator"/> that uses the value of 
	/// the id property of an associated object
	/// </summary>
	/// <remarks>
	/// <para>
	///	This id generation strategy is specified in the mapping file as 
	///	<code>
	///	&lt;generator class="foreign"&gt;
	///		&lt;param name="property"&gt;AssociatedObject&lt;/param&gt;
	///	&lt;/generator&gt;
	///	</code>
	/// </para>
	/// The mapping parameter <c>property</c> is required.
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ForeignGenerator : IIdentifierGenerator, IConfigurable
	{
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