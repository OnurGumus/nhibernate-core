namespace NHibernate.Test.GhostProperty
{
	public partial class Order
	{
		public virtual int Id { get; set; }
		private Payment payment;

		public virtual Payment Payment
		{
			get { return payment; }
			set { payment = value; }
		}

		public virtual string ALazyProperty { get; set; }
		public virtual string NoLazyProperty { get; set; }
	}

	public abstract partial class Payment
	{
		public virtual int Id { get; set; }
	}

	public partial class WireTransfer : Payment{}
	public partial class CreditCard : Payment { }
}