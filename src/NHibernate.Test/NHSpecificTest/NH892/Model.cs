using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernate.Test.NHSpecificTest.NH892
{
	public partial class BlogPost
	{
		private int _ID;
		public virtual int ID
		{
			get { return _ID; }
			set { _ID = value; }
		}

		private string _Title;
		public virtual string Title
		{
			get { return _Title; }
			set { _Title = value; }
		}

		private User _Poster;
		public virtual User Poster
		{
			get { return _Poster; }
			set { _Poster = value; }
		}
	}

	public partial class User
	{
		private int _ID;
		public virtual int ID
		{
			get { return _ID; }
			set { _ID = value; }
		}

		private string _UserName;
		public virtual string UserName
		{
			get { return _UserName; }
			set { _UserName = value; }
		}
	}
}
