using System;
using System.Reflection;
using NHibernate.Properties;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Engine
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public static partial class UnsavedValueFactory
	{
		public static async Task<VersionValue> GetUnsavedVersionValueAsync(String versionUnsavedValue, IGetter versionGetter, IVersionType versionType, ConstructorInfo constructor)
		{
			if (versionUnsavedValue == null)
			{
				if (constructor != null)
				{
					object defaultValue = versionGetter.Get(Instantiate(constructor));
					if (defaultValue != null && defaultValue.GetType().IsValueType)
						return new VersionValue(defaultValue);
					else
					{
						// if the version of a newly instantiated object is not the same
						// as the version seed value, use that as the unsaved-value
						return versionType.IsEqual(await (versionType.SeedAsync(null)), defaultValue) ? VersionValue.VersionUndefined : new VersionValue(defaultValue);
					}
				}
				else
				{
					return VersionValue.VersionUndefined;
				}
			}
			else if ("undefined" == versionUnsavedValue)
			{
				return VersionValue.VersionUndefined;
			}
			else if ("null" == versionUnsavedValue)
			{
				return VersionValue.VersionSaveNull;
			}
			else if ("negative" == versionUnsavedValue)
			{
				return VersionValue.VersionNegative;
			}
			else
			{
				// NHibernate-specific
				try
				{
					return new VersionValue(versionType.FromStringValue(versionUnsavedValue));
				}
				catch (InvalidCastException ice)
				{
					throw new MappingException("Bad version type: " + versionType.Name, ice);
				}
				catch (Exception e)
				{
					throw new MappingException("Could not parse version unsaved-value: " + versionUnsavedValue, e);
				}
			}
		}
	}
}