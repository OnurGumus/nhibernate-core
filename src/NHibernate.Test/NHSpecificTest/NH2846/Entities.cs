using System.Collections.Generic;


namespace NHibernate.Test.NHSpecificTest.NH2846
{
	public partial class Post
	{
		public Post()
		{
			Comments = new HashSet<Comment>();
		}

		public virtual int Id { get; set; }

		public virtual string Title { get; set; }

		public virtual Category Category { get; set; }

		public virtual ISet<Comment> Comments { get; set; }
	}

	public partial class Category
	{
		public virtual int Id { get; set; }

		public virtual string Title { get; set; }
	}

	public partial class Comment
	{
		public virtual int Id { get; set; }

		public virtual string Title { get; set; }

		public virtual Post Post { get; set; }
	}
}