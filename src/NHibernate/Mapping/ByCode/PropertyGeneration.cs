using NHibernate.Cfg.MappingSchema;

namespace NHibernate.Mapping.ByCode
{
	public abstract partial class PropertyGeneration
	{
		public static PropertyGeneration Never = new NeverPropertyGeneration();
		public static PropertyGeneration Insert = new InsertPropertyGeneration();
		public static PropertyGeneration Always = new AlwaysPropertyGeneration();

		internal abstract HbmPropertyGeneration ToHbm();

		public partial class AlwaysPropertyGeneration : PropertyGeneration
		{
			internal override HbmPropertyGeneration ToHbm()
			{
				return HbmPropertyGeneration.Always;
			}
		}

		public partial class InsertPropertyGeneration : PropertyGeneration
		{
			internal override HbmPropertyGeneration ToHbm()
			{
				return HbmPropertyGeneration.Insert;
			}
		}

		public partial class NeverPropertyGeneration : PropertyGeneration
		{
			internal override HbmPropertyGeneration ToHbm()
			{
				return HbmPropertyGeneration.Never;
			}
		}
	}
}