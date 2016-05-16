namespace NHibernate.Test.NHSpecificTest.NH1609
{
    public partial class EntityA
    {
        public virtual long Id { get; set; }
    }

    public partial class EntityB
    {
        public virtual long Id { get; set; }
        public virtual EntityA A { get; set; }
        public virtual EntityC C { get; set; }
    }

    public partial class EntityC
    {
        public virtual long Id { get; set; }
    }
}
