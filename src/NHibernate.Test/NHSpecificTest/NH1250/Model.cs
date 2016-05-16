namespace NHibernate.Test.NHSpecificTest.NH1250
{
	public abstract partial class Party
	{
		private int id;
		private string name;

		public virtual int Id
		{
			get { return id; }
			set { id = value; }
		}

		public virtual string Name
		{
			get { return name; }
			set { name = value; }
		}

		public virtual string DisplayForename
		{
			get { return displayName; }
			set { displayName = value; }
		}

#pragma warning disable 169
		private int classTypeId; // something for NHibernate to write to
#pragma warning restore 169
		private string displayName;

		public abstract int ClassTypeId
		{
			get;
		}
	}

	public partial class Person : Party
	{
		public override int ClassTypeId
		{
			get { return 1; }
		}
	}

	public partial class Company : Party
	{
		public override int ClassTypeId
		{
			get { return 2; }
		}
	}

	public partial class Client
	{
		private int id;
		private Party party;
		private string paymentTerms;

		public virtual int Id
		{
			get { return id; }
			set { id = value; }
		}

		public virtual Party Party
		{
			get { return party; }
			set { party = value; }
		}

		public virtual string PaymentTerms
		{
			get { return paymentTerms; }
			set { paymentTerms = value; }
		}
	}

	public partial class ClassWithFormula
	{
		private int id;
		private string name;
		private int age;
		private bool isAdult;

		public virtual int Id
		{
			get { return id; }
			set { id = value; }
		}

		public virtual string Name
		{
			get { return name; }
			set { name = value; }
		}

		public virtual int Age
		{
			get { return age; }
			set { age = value; }
		}

		public virtual bool IsAdult
		{
			get { return isAdult; }
			set { isAdult = value; }
		}
	}
}
