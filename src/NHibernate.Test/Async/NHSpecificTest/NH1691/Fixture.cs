﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System.Collections.Generic;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH1691
{
	using System.Threading.Tasks;
	using System.Threading;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		private static Component GetInitializedComponent()
		{
			var component = new Component();
			var sub1 = new SubComponent();
			var sub2 = new SubComponent();
			component.Name = "Comp1";
			sub1.SubName = "Sub1";
			sub1.SubName1 = "Sub1x";

			sub2.SubName = "Sub2";
			sub2.SubName1 = "Sub2x";

			sub1.Nested = sub2;

			component.SubComponent = sub1;
			return component;
		}

		[Test]
		public async Task ComplexNestAsync()
		{
			Component comp1 = GetInitializedComponent();
			var nest = new Nest {Name = "NAME", Components = new List<Component> {comp1}};
			var deep1 = new DeepComponent {Prop1 = "PROP1", Prop2 = "PROP2", Prop3 = "PROP3", Prop4 = "PROP4"};
			Component innerComp = GetInitializedComponent();
			deep1.Component = innerComp;
			Component innerComp2 = GetInitializedComponent();
			var deep2 = new DeepComponent
			            	{Prop1 = "PROP1", Prop2 = "PROP2", Prop3 = "PROP3", Prop4 = "PROP4", Component = innerComp2};

			nest.ComplexComponents = new List<DeepComponent> {deep1, deep2};

			object nestId;
			using (ISession session = OpenSession())
			{
				using (ITransaction transaction = session.BeginTransaction())
				{
					await (session.SaveAsync(nest, CancellationToken.None));
					await (transaction.CommitAsync(CancellationToken.None));
					nestId = nest.Id;
				}
			}

			using (ISession session = OpenSession())
			{
				using (ITransaction transaction = session.BeginTransaction())
				{
					var loadedNest = await (session.LoadAsync<Nest>(nestId, CancellationToken.None));
					await (transaction.CommitAsync(CancellationToken.None));

					Assert.AreEqual(2, loadedNest.ComplexComponents.Count);
					Assert.IsNotNull(((DeepComponent) loadedNest.ComplexComponents[0]).Component.SubComponent.Nested.SubName1);
					Assert.IsNull(((DeepComponent) loadedNest.ComplexComponents[0]).Component.SubComponent.SubName1);
				}
			}

			using (ISession s = OpenSession())
			{
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.DeleteAsync("from Nest", CancellationToken.None));
					await (t.CommitAsync(CancellationToken.None));
				}
			}
		}

		[Test]
		public async Task NestedComponentCollectionAsync()
		{
			var nest = new Nest {Name = "NAME"};
			Component comp1 = GetInitializedComponent();

			nest.Components = new List<Component> {comp1};

			object nestId;
			using (ISession session = OpenSession())
			{
				using (ITransaction transaction = session.BeginTransaction())
				{
					await (session.SaveAsync(nest, CancellationToken.None));
					await (transaction.CommitAsync(CancellationToken.None));
					nestId = nest.Id;
				}
			}

			using (ISession session = OpenSession())
			{
				using (ITransaction transaction = session.BeginTransaction())
				{
					var nest2 = await (session.LoadAsync<Nest>(nestId, CancellationToken.None));
					await (transaction.CommitAsync(CancellationToken.None));

					Assert.IsNotNull(((Component) nest2.Components[0]).SubComponent.Nested.SubName);
				}
			}

			using (ISession s = OpenSession())
			{
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.DeleteAsync("from Nest", CancellationToken.None));
					await (t.CommitAsync(CancellationToken.None));
				}
			}
		}
	}
}