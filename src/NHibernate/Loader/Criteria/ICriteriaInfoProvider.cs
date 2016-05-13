using System;
using NHibernate.Persister.Entity;
using NHibernate.Type;

namespace NHibernate.Loader.Criteria
{
    public partial interface ICriteriaInfoProvider 
    {
        string Name { get; }
        string[] Spaces { get; }
        IPropertyMapping PropertyMapping { get; }
        IType GetType(String relativePath);
    }
}