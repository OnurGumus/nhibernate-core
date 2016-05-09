using NHibernate.Persister.Entity;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Engine
{
	/// <summary>
	/// Utility methods for managing versions and timestamps
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Versioning
	{
		public static async Task<object> IncrementAsync(object version, IVersionType versionType, ISessionImplementor session)
		{
			object next = await (versionType.NextAsync(version, session));
			if (log.IsDebugEnabled)
			{
				log.Debug(string.Format("Incrementing: {0} to {1}", await (versionType.ToLoggableStringAsync(version, session.Factory)), await (versionType.ToLoggableStringAsync(next, session.Factory))));
			}

			return next;
		}

		public static async Task<object> SeedAsync(IVersionType versionType, ISessionImplementor session)
		{
			object seed = await (versionType.SeedAsync(session));
			if (log.IsDebugEnabled)
			{
				log.Debug("Seeding: " + seed);
			}

			return seed;
		}

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