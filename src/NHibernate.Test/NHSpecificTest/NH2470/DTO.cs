using System;

namespace NHibernate.Test.NHSpecificTest.NH2470
{
	public abstract partial class DTO
    {
        public Guid ID;
        public int EntityVersion;
    }
}