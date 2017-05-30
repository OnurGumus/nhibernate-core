﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Text;
using log4net.Appender;
using log4net.Core;
using log4net.Repository.Hierarchy;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.ListsWithHoles
{
    using System.Collections;
    using System.Threading.Tasks;
    using System.Threading;

    [TestFixture]
    public class FixtureAsync : TestCase
    {

        protected override IList Mappings
        {
            get { return new string[] { "NHSpecificTest.ListsWithHoles.Mappings.hbm.xml" }; }
        }

        protected override string MappingsAssembly
        {
            get { return "NHibernate.Test"; }
        }

        [Test]
        public async Task CanHandleHolesInListAsync()
        {
            int parentId, firstChildId;
            using (ISession sess = OpenSession())
            using (ITransaction tx = sess.BeginTransaction())
            {
                Employee e = new Employee();
                e.Children.Add(new Employee());
                e.Children.Add(new Employee());
                await (sess.SaveAsync(e, CancellationToken.None));
                await (tx.CommitAsync(CancellationToken.None));
                parentId = e.Id;
                firstChildId = e.Children[0].Id;
            }

            using (ISession sess = OpenSession())
            using (ITransaction tx = sess.BeginTransaction())
            {
                await (sess.DeleteAsync(await (sess.GetAsync<Employee>(firstChildId, CancellationToken.None)), CancellationToken.None));
                await (tx.CommitAsync(CancellationToken.None));
            }


            using (ISession sess = OpenSession())
            using (ITransaction tx = sess.BeginTransaction())
            {
                Employee employee = await (sess.GetAsync<Employee>(parentId, CancellationToken.None));
                employee.Children.Add(new Employee());
                await (tx.CommitAsync(CancellationToken.None));
            }



            using (ISession sess = OpenSession())
            using (ITransaction tx = sess.BeginTransaction())
            {
                await (sess.DeleteAsync("from Employee", CancellationToken.None));
                await (tx.CommitAsync(CancellationToken.None));
            }
        }
    }
}
