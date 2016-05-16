namespace NHibernate.Test.NHSpecificTest.NH2242
{
    public partial class EscapedFormulaDomainClass
    {
        private int id;

        private int formula;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int Formula
        {
            get { return formula; }
            set { formula = value; }
        }
    }
}