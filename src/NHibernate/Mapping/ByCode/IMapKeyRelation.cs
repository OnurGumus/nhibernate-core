using System;

namespace NHibernate.Mapping.ByCode
{
	public partial interface IMapKeyRelation
	{
		void Element(Action<IMapKeyMapper> mapping);
		void ManyToMany(Action<IMapKeyManyToManyMapper> mapping);
		void Component(Action<IComponentMapKeyMapper> mapping);
	}

	public partial interface IMapKeyRelation<TKey>
	{
		void Element();
		void Element(Action<IMapKeyMapper> mapping);
		void ManyToMany();
		void ManyToMany(Action<IMapKeyManyToManyMapper> mapping);
		void Component(Action<IComponentMapKeyMapper<TKey>> mapping);
	}
}