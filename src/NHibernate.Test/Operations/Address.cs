namespace NHibernate.Test.Operations
{
	public partial class Address
	{
		private Person resident;
		public virtual long Id { get; set; }
		public virtual string StreetAddress { get; set; }
		public virtual string City { get; set; }
		public virtual string Country { get; set; }

		public virtual Person Resident
		{
			get { return resident; }
			set
			{
				resident = value;
				resident.Address = this;
			}
		}
	}
}