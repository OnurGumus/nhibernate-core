namespace NHibernate.Test.NHSpecificTest.NH2043
{
    public partial class A
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }

        public virtual B B { get; set; }
    }

    public partial class AImpl : A
    {
    }

    public partial class B
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }

        public virtual A A { get; set; }
    }

    public partial class BImpl : B
    {
    }
}
