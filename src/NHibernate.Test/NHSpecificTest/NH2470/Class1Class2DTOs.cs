namespace NHibernate.Test.NHSpecificTest.NH2470
{
	public partial class Class2DTO : DTO { }

    public partial class Class1DTO : DTO
    {
        public Class2DTO[] Class2Ary { get; set; }
    }
}