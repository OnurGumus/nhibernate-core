namespace NHibernate.Test.NHSpecificTest.NH1092
{
	public partial class SubscriberAbstract
	{
		public virtual string Username { get; set; }
	}

	public partial class Subscriber1 : SubscriberAbstract { }

	public partial class Subscriber2 : SubscriberAbstract { }
}