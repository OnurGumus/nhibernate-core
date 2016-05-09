using System;
using NHibernate.Engine;
using NHibernate.Tuple.Component;
using System.Threading.Tasks;

namespace NHibernate.Type
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class EmbeddedComponentType : ComponentType
	{
		public override async Task<object> InstantiateAsync(object parent, ISessionImplementor session)
		{
			bool useParent = false;
			// NH Different implementation : since we are not sure about why H3.2 use the "parent"
			//useParent = parent != null && base.ReturnedClass.IsInstanceOfType(parent);
			return useParent ? parent : await (base.InstantiateAsync(parent, session));
		}
	}
}