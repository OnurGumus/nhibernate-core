using System;
using System.Linq.Expressions;
using System.Reflection;

namespace NHibernate.Mapping.ByCode
{
	public partial interface IComponentMapKeyMapper
	{
		void Property(MemberInfo property, Action<IPropertyMapper> mapping);

		void ManyToOne(MemberInfo property, Action<IManyToOneMapper> mapping);
	}

	public partial interface IComponentMapKeyMapper<TComponent>
	{
		void Property<TProperty>(Expression<Func<TComponent, TProperty>> property, Action<IPropertyMapper> mapping);
		void Property<TProperty>(Expression<Func<TComponent, TProperty>> property);

		void ManyToOne<TProperty>(Expression<Func<TComponent, TProperty>> property, Action<IManyToOneMapper> mapping) where TProperty : class;
	}
}