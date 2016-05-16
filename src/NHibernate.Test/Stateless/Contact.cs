namespace NHibernate.Test.Stateless
{
    public partial class Contact
    {
        private int id;
        private Org org;

        public virtual int Id
        {
            get
            {
                return id;
            }
            
            set
            {
                id = value;
            }
        }

        public virtual Org Org
        {
            get
            {
                return org;
            }

            set
            {
                org = value;
            }
        }
    }
}
