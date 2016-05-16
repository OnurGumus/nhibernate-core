using System;
using System.Collections.Generic;

namespace NHibernate.Test.NHSpecificTest.NH2507
{
    public enum Sex
    {
        Undefined = 0,
        Male = 1,
        Female = 2
    }

    public partial class Animal
    {
        public virtual int Id { get; set; }
        public virtual string Description { get; set; }
        public virtual double BodyWeight { get; set; }
        public virtual Sex Sex { get; set; }
        public virtual Animal Mother { get; set; }
        public virtual Animal Father { get; set; }
        public virtual IList<Animal> Children { get; set; }
        public virtual string SerialNumber { get; set; }
    }

    public abstract partial class Reptile : Animal
    {
        public virtual double BodyTemperature { get; set; }
    }

    public partial class Lizard : Reptile { }

    public abstract partial class Mammal : Animal
    {
        public virtual bool Pregnant { get; set; }
        public virtual DateTime? BirthDate { get; set; }
    }

    public partial class Dog : Mammal { }

    public partial class Cat : Mammal { }
}