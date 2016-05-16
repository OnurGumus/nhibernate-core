using System;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using NHibernate;
using NHibernate.Classic;

namespace NHibernate.Test.NHSpecificTest.NH1920
{

	public partial class Customer 
	{ 
		public virtual int Id { get; set; } 
		public virtual bool IsDeleted { get; set; } 
		public virtual IList<Order> Orders { get; set; } 
	} 

	public partial class Order 
	{ 
		public virtual int Id { get; set; } 
		public virtual bool IsDeleted { get; set; } 
		public virtual string Memo { get; set; } 
		public virtual Customer Customer { get; set; } 
	} 

}
