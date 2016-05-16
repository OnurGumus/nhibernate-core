#if NET_4_5
using System;
using System.Data.Common;
using NHibernate.SqlTypes;
using NHibernate.Type;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.DomainModel.NHSpecific
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NullableInt32Type : NullableTypesType
	{
		public override Task<bool[]> ToColumnNullnessAsync(object value, Engine.IMapping mapping)
		{
			try
			{
				return Task.FromResult<bool[]>(ToColumnNullness(value, mapping));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<bool[]>(ex);
			}
		}
	}
}
#endif
