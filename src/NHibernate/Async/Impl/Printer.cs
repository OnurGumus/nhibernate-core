using System.Collections;
using System.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Intercept;
using NHibernate.Metadata;
using NHibernate.Properties;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Impl
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public sealed partial class Printer
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name = "entity">an actual entity object, not a proxy!</param>
		/// <param name = "entityMode"></param>
		/// <returns></returns>
		public async Task<string> ToStringAsync(object entity, EntityMode entityMode)
		{
			IClassMetadata cm = _factory.GetClassMetadata(entity.GetType());
			if (cm == null)
			{
				return entity.GetType().FullName;
			}

			IDictionary<string, string> result = new Dictionary<string, string>();
			if (cm.HasIdentifierProperty)
			{
				result[cm.IdentifierPropertyName] = await (cm.IdentifierType.ToLoggableStringAsync(await (cm.GetIdentifierAsync(entity, entityMode)), _factory));
			}

			IType[] types = cm.PropertyTypes;
			string[] names = cm.PropertyNames;
			object[] values = cm.GetPropertyValues(entity, entityMode);
			for (int i = 0; i < types.Length; i++)
			{
				var value = values[i];
				if (Equals(LazyPropertyInitializer.UnfetchedProperty, value) || Equals(BackrefPropertyAccessor.Unknown, value))
				{
					result[names[i]] = value.ToString();
				}
				else
				{
					result[names[i]] = await (types[i].ToLoggableStringAsync(value, _factory));
				}
			}

			return cm.EntityName + CollectionPrinter.ToString(result);
		}

		public async Task<string> ToStringAsync(IType[] types, object[] values)
		{
			List<string> list = new List<string>(types.Length);
			for (int i = 0; i < types.Length; i++)
			{
				if (types[i] != null)
				{
					list.Add(await (types[i].ToLoggableStringAsync(values[i], _factory)));
				}
			}

			return CollectionPrinter.ToString(list);
		}

		public async Task<string> ToStringAsync(IDictionary<string, TypedValue> namedTypedValues)
		{
			IDictionary<string, string> result = new Dictionary<string, string>(namedTypedValues.Count);
			foreach (KeyValuePair<string, TypedValue> me in namedTypedValues)
			{
				TypedValue tv = me.Value;
				result[me.Key] = await (tv.Type.ToLoggableStringAsync(tv.Value, _factory));
			}

			return CollectionPrinter.ToString(result);
		}

		public async Task ToStringAsync(IEnumerator enumerator, EntityMode entityMode)
		{
			if (!log.IsDebugEnabled || !enumerator.MoveNext())
			{
				return;
			}

			log.Debug("listing entities:");
			int i = 0;
			do
			{
				if (i++ > 20)
				{
					log.Debug("more......");
					break;
				}

				log.Debug(await (ToStringAsync(enumerator.Current, entityMode)));
			}
			while (enumerator.MoveNext());
		}
	}
}