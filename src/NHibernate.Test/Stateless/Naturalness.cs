using System;
using System.Collections.Generic;

namespace NHibernate.Test.Stateless
{
	public abstract partial class Animal
	{
		public virtual int Id { get; protected set; }
		public virtual string Description { get; set; }
	}

	public partial class Reptile: Animal
	{
		public virtual float BodyTemperature { get; set; }
	}

	public partial class Human : Animal
	{
		public virtual string Name { get; set; }
		public virtual string NickName { get; set; }
		public virtual DateTime Birthdate { get; set; }
	}

	public partial class Family<T> where T: Animal
	{
		private ISet<T> childs;

		public virtual int Id { get; protected set; }
		public virtual T Father { get; set; }
		public virtual T Mother { get; set; }
		public virtual ISet<T> Childs
		{
			get { return childs; }
			set { childs = value; }
		}
	}
}