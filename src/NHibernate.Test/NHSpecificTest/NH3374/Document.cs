using System;

namespace NHibernate.Test.NHSpecificTest.NH3374
{
	public partial class Document
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
        public virtual Blob Blob { get; set; }
	}

    public partial class Blob
    {
        public virtual int Id { get; set; }
        public virtual byte[] Bytes { get; set; }
    }
}