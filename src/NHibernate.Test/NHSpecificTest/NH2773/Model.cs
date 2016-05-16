using System;

namespace NHibernate.Test.NHSpecificTest.NH2773 {
   [Serializable]
   public partial class MyEntity {
      public virtual Guid Id { get; set; }
      public virtual MyEntity OtherEntity { get; set; }
   }
}
