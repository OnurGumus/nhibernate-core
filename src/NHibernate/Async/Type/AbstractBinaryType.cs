using System;
using System.Collections;
using System.Data;
using System.Text;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Type
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractBinaryType : MutableType, IVersionType, IComparer
	{
		public override async Task<int> CompareAsync(object x, object y, EntityMode? entityMode)
		{
			byte[] xbytes = ToInternalFormat(x);
			byte[] ybytes = ToInternalFormat(y);
			if (xbytes.Length < ybytes.Length)
				return -1;
			if (xbytes.Length > ybytes.Length)
				return 1;
			for (int i = 0; i < xbytes.Length; i++)
			{
				if (xbytes[i] < ybytes[i])
					return -1;
				if (xbytes[i] > ybytes[i])
					return 1;
			}

			return 0;
		}

		public override async Task<int> GetHashCodeAsync(object x, EntityMode entityMode)
		{
			byte[] bytes = ToInternalFormat(x);
			int hashCode = 1;
			unchecked
			{
				for (int j = 0; j < bytes.Length; j++)
				{
					hashCode = 31 * hashCode + bytes[j];
				}
			}

			return hashCode;
		}

		public async Task<object> SeedAsync(ISessionImplementor session)
		{
			return null;
		}

		//      Note : simply returns null for seed() and next() as the only known
		//      application of binary types for versioning is for use with the
		//      TIMESTAMP datatype supported by Sybase and SQL Server, which
		//      are completely db-generated values...
		public async Task<object> NextAsync(object current, ISessionImplementor session)
		{
			return current;
		}
	}
}