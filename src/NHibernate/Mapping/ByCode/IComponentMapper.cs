using System;
using System.Linq.Expressions;
using System.Reflection;

namespace NHibernate.Mapping.ByCode
{
	public partial interface IComponentAttributesMapper : IEntityPropertyMapper
	{
		void Parent(MemberInfo parent);
		void Parent(MemberInfo parent, Action<IComponentParentMapper> parentMapping);
		void Update(bool consideredInUpdateQuery);
		void Insert(bool consideredInInsertQuery);
		void Lazy(bool isLazy);
		void Unique(bool unique);
		void Class(System.Type componentType);
	}

	public partial interface IComponentMapper : IComponentAttributesMapper, IPropertyContainerMapper {}

	public partial interface IComponentAttributesMapper<TComponent> : IEntityPropertyMapper
	{
		void Parent<TProperty>(Expression<Func<TComponent, TProperty>> parent) where TProperty : class;
		void Parent<TProperty>(Expression<Func<TComponent, TProperty>> parent, Action<IComponentParentMapper> parentMapping) where TProperty : class;
		void Update(bool consideredInUpdateQuery);
		void Insert(bool consideredInInsertQuery);
		void Lazy(bool isLazy);
		void Unique(bool unique);
		void Class<TConcrete>() where TConcrete : TComponent;
	}

	public partial interface IComponentMapper<TComponent> : IComponentAttributesMapper<TComponent>, IPropertyContainerMapper<TComponent>
	{}
}