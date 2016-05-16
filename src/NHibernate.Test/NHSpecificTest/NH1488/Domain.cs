namespace NHibernate.Test.NHSpecificTest.NH1488
{
	public abstract partial class Category
	{
		private int id;
		private string name;
		protected Category() {}
		protected Category(string name)
		{
			this.name = name;
		}

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
	}

	public partial class CustomerCategory : Category
	{
		public CustomerCategory() {}
		public CustomerCategory(string name) : base(name) {}
	}
	public partial class OtherCategory : Category
	{
		public OtherCategory() {}
		public OtherCategory(string name) : base(name) {}
	}

	public partial class CustomerNoSmart
	{
		private Category category;
		private int id;
		private string name;
		public CustomerNoSmart() {}
		public CustomerNoSmart(string name)
		{
			this.name = name;
		}

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

		public virtual Category Category
		{
			get { return category; }
			set { category = value; }
		}
	}

	public partial class Customer
	{
		private CustomerCategory category;
		private int id;
		private string name;
		public Customer() { }
		public Customer(string name)
		{
			this.name = name;
		}

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

		public virtual CustomerCategory Category
		{
			get { return category; }
			set { category = value; }
		}
	}
}
