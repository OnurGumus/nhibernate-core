using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Persister.Collection;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Type
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ArrayType : CollectionType
	{
		public override async Task<object> ReplaceElementsAsync(object original, object target, object owner, IDictionary copyCache, ISessionImplementor session)
		{
			Array org = (Array)original;
			Array result = (Array)target;
			int length = org.Length;
			if (length != result.Length)
			{
				//note: this affects the return value!
				result = (Array)InstantiateResult(original);
			}

			IType elemType = GetElementType(session.Factory);
			for (int i = 0; i < length; i++)
			{
				result.SetValue(await (elemType.ReplaceAsync(org.GetValue(i), null, session, owner, copyCache)), i);
			}

			return result;
		}

		public override async Task<string> ToLoggableStringAsync(object value, ISessionFactoryImplementor factory)
		{
			if (value == null)
			{
				return "null";
			}

			Array array = (Array)value;
			int length = array.Length;
			IList list = new List<object>(length);
			IType elemType = GetElementType(factory);
			for (int i = 0; i < length; i++)
			{
				list.Add(await (elemType.ToLoggableStringAsync(array.GetValue(i), factory)));
			}

			return CollectionPrinter.ToString(list);
		}

		public override async Task NullSafeSetAsync(IDbCommand st, object value, int index, ISessionImplementor session)
		{
			await (base.NullSafeSetAsync(st, session.PersistenceContext.GetCollectionHolder(value), index, session));
		}
	}
}