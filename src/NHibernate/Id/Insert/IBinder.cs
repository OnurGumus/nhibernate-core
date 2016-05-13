using System.Data;

namespace NHibernate.Id.Insert
{
	public partial interface IBinder
	{
		object Entity { get;}
		void BindValues(IDbCommand cm);
	}
}