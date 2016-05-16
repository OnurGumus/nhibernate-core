namespace NHibernate.Test.NHSpecificTest.NH719
{
	public partial class A
	{
		private int id;
		private string foo;

		protected A()
		{
		}

		public A(int id, string foo)
		{
			this.id = id;
			this.foo = foo;
		}

		public virtual int Id
		{
			get { return id; }
			set { id = value; }
		}

		public virtual string Foo
		{
			get { return foo; }
			set { foo = value; }
		}
	}

	public partial class B
	{
		private int id;
		private string foo;

		protected B()
		{
		}

		public B(int id, string foo)
		{
			this.id = id;
			this.foo = foo;
		}

		public virtual int Id
		{
			get { return id; }
			set { id = value; }
		}


		public virtual string Foo
		{
			get { return foo; }
			set { foo = value; }
		}
	}

	public partial class NotCached
	{
		private int id;
		private object owner;

		protected NotCached()
		{
		}

		public NotCached(int id, object owner)
		{
			this.id = id;
			this.owner = owner;
		}

		public int Id
		{
			get { return id; }
			set { id = value; }
		}

		public object Owner
		{
			get { return owner; }
			set { owner = value; }
		}
	}

	public partial class Cached
	{
		private int id;
		private object owner;

		protected Cached()
		{
		}

		public Cached(int id, object owner)
		{
			this.id = id;
			this.owner = owner;
		}

		public int Id
		{
			get { return id; }
			set { id = value; }
		}

		public object Owner
		{
			get { return owner; }
			set { owner = value; }
		}
	}
}