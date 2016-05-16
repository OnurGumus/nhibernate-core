namespace NHibernate.Test.NHSpecificTest.NH1617
{
    public partial class Order
    {
        public virtual int Id { get; set; }
        public virtual bool Status { get; set; }
        public virtual User User { get; set; }
    }
}
