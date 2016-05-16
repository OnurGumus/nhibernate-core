using System;
using System.Collections.Generic;

namespace NHibernate.Test.NHSpecificTest.NH2756
{
	public partial class Document
	{
		public Document()
		{
			Files = new List<DocumentFile>();
		}

		public virtual Guid Id { get; set; }
		public virtual ICollection<DocumentFile> Files { get; set; }
	}

	public partial class DocumentFile
	{
		public string Description { get; set; }
		public string Filename { get; private set; }
		public File File { get; set; }
	}

	public partial class File
	{
		public virtual Guid Id { get; set; }
		public virtual string Filename { get; set; }
	}
}
