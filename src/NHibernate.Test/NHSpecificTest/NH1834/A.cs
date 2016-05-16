namespace NHibernate.Test.NHSpecificTest.NH1834
{
    public partial class A
    {
        public int Id { get; set; }
    }

    public partial class B
    {
        public int Id { get; set; }
        public A A { get; set; }
        public A A2 { get; set; }
    }
}