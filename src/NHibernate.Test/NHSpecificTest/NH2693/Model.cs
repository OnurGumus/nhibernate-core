
namespace NHibernate.Test.NHSpecificTest.NH2693 {
   using System;
   using System.Collections.Generic;

   public partial class FirstLevel {
      public FirstLevel() {
         SecondLevels = new HashSet<SecondLevelComponent>();
      }

      public virtual Guid Id { get; set; }
      public virtual ICollection<SecondLevelComponent> SecondLevels { get; set; }
   }

   public partial class SecondLevelComponent {
      public virtual FirstLevel FirstLevel { get; set; }
      public virtual ThirdLevel ThirdLevel { get; set; }
      public virtual SpecificThirdLevel SpecificThirdLevel { get; set; }
      public virtual bool SomeBool { get; set; }
   }

   public abstract partial class ThirdLevel {
      public virtual Guid Id { get; set; }
   }

   public partial class SpecificThirdLevel : ThirdLevel {
      public SpecificThirdLevel() {
				FourthLevels = new HashSet<FourthLevel>();
      }

      public virtual ICollection<FourthLevel> FourthLevels { get; set; }
   }

   public partial class FourthLevel {
      public virtual Guid Id { get; set; }
      public virtual SpecificThirdLevel SpecificThirdLevel { get; set; }
      public virtual string SomeString { get; set; }
   }
}
