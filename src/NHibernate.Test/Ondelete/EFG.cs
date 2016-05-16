namespace NHibernate.Test.Ondelete
{
	public partial class E
	{
		private int id;
		private string description;
		public E() { }

		public E(string description)
		{
			this.description = description;
		}

		public virtual int Id
		{
			get { return id; }
			set { id = value; }
		}

		public virtual string Description
		{
			get { return description; }
			set { description = value; }
		}
	}

	public partial class F : E
	{
		private string color;
		public F()
		{
		}

		public F(string description, string color)
			: base(description)
		{
			this.color = color;
		}

		public virtual string Color
		{
			get { return color; }
			set { color = value; }
		}
	}

	public partial class G : F
	{
		private string size;
		public G()
		{
		}

		public G(string description, string color, string size)
			: base(description, color)
		{
			this.size = size;
		}

		public virtual string Size
		{
			get { return size; }
			set { size = value; }
		}
	}
}
