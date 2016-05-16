using System;

namespace NHibernate.Test.Criteria
{
	public partial class Reptile : Animal
	{
		private float bodyTemperature;

		public virtual float BodyTemperature
		{
			get { return bodyTemperature; }
			set { bodyTemperature = value; }
		}
	}

	public partial class Lizard : Reptile
	{
	}
}