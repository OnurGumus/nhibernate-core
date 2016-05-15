#if NET_4_5
using System;
using System.Collections.Generic;
using NHibernate.Classic;
using System.Threading.Tasks;

namespace NHibernate.DomainModel
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Qux : ILifecycle
	{
		public virtual async Task<LifecycleVeto> OnSaveAsync(ISession session)
		{
			_created = true;
			try
			{
				Foo = new Foo();
				await (session.SaveAsync(Foo));
			}
			catch (Exception e)
			{
				throw new CallbackException(e);
			}

			Foo.String = "child of a qux";
			return LifecycleVeto.NoVeto;
		}

		public virtual async Task<LifecycleVeto> OnDeleteAsync(ISession session)
		{
			_deleted = true;
			try
			{
				await (session.DeleteAsync(Foo));
			}
			catch (Exception e)
			{
				throw new CallbackException(e);
			}

			//if (child!=null) session.delete(child);
			return LifecycleVeto.NoVeto;
		}
	}
}
#endif
