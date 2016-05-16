using System;
namespace NHibernate.Test.NHSpecificTest.NH1033
{
	public partial class Animal
	{
		public virtual long Id
		{
			get;
			set;
		}

		public virtual string SerialNumber
		{
			get; 
			set;
		}

	}
}