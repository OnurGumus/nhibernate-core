 using System.Collections.Generic;
 
namespace NHibernate.Test.NHSpecificTest.NH2093
 {
 	public partial class Person
 	{ 
 		public virtual int Id { get; set; }
 		public virtual string Name { get; set; }

	  public virtual string LazyField { get; set; }
 	} 
 
  public partial class Employee
  {
    public virtual int Id { get; set; }
    public virtual string Name { get; set; }

    public virtual Person Person { get; set; }
  }
 }
