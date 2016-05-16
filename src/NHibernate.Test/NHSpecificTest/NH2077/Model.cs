using System.Collections.Generic;

namespace NHibernate.Test.NHSpecificTest.NH2077
{
	public partial class Person
	{ 
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
        public virtual ICollection<Person> Children { get; set; }
	} 

}
