using System;

namespace NHibernate.Test.NHSpecificTest.NH2266
{
	public abstract partial class Token { public virtual int Id { get; set; } }

	public partial class SecurityToken : Token { public virtual string Owner { get; set; } }

	public abstract partial class TemporaryToken : Token { public virtual DateTime ExpiryDate { get; set; } } 
}