#if NET_4_5
using NHibernate.Persister.Entity;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Engine
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Versioning
	{
		/// <summary>
		/// Increment the given version number
		/// </summary>
		/// <param name = "version">The value of the current version.</param>
		/// <param name = "versionType">The <see cref = "IVersionType"/> of the versioned property.</param>
		/// <param name = "session">The current <see cref = "ISession"/>.</param>
		/// <returns>Returns the next value for the version.</returns>
		public static async Task<object> IncrementAsync(object version, IVersionType versionType, ISessionImplementor session)
		{
			object next = await (versionType.NextAsync(version, session));
			if (log.IsDebugEnabled)
			{
				log.Debug(string.Format("Incrementing: {0} to {1}", versionType.ToLoggableString(version, session.Factory), versionType.ToLoggableString(next, session.Factory)));
			}

			return next;
		}

		/// <summary>
		/// Create an initial version number
		/// </summary>
		/// <param name = "versionType">The <see cref = "IVersionType"/> of the versioned property.</param>
		/// <param name = "session">The current <see cref = "ISession"/>.</param>
		/// <returns>A seed value to initialize the versioned property with.</returns>
		public static async Task<object> SeedAsync(IVersionType versionType, ISessionImplementor session)
		{
			object seed = await (versionType.SeedAsync(session));
			if (log.IsDebugEnabled)
			{
				log.Debug("Seeding: " + seed);
			}

			return seed;
		}

		/// <summary>
		/// Seed the given instance state snapshot with an initial version number
		/// </summary>
		/// <param name = "fields">An array of objects that contains a snapshot of a persistent object.</param>
		/// <param name = "versionProperty">The index of the version property in the <c>fields</c> parameter.</param>
		/// <param name = "versionType">The <see cref = "IVersionType"/> of the versioned property.</param>
		/// <param name = "force">Force the version to initialize</param>
		/// <param name = "session">The current session, if any.</param>
		/// <returns><see langword = "true"/> if the version property needs to be seeded with an initial value.</returns>
		public static async Task<bool> SeedVersionAsync(object[] fields, int versionProperty, IVersionType versionType, bool ? force, ISessionImplementor session)
		{
			object initialVersion = fields[versionProperty];
			if (initialVersion == null || !force.HasValue || force.Value)
			{
				fields[versionProperty] = await (SeedAsync(versionType, session));
				return true;
			}
			else
			{
				if (log.IsDebugEnabled)
				{
					log.Debug("using initial version: " + initialVersion);
				}

				return false;
			}
		}
	}
}
#endif
