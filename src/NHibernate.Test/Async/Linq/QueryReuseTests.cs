﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System.Collections.Generic;
using System.Linq;
using NHibernate.DomainModel.Northwind.Entities;
using NUnit.Framework;
using NHibernate.Linq;

namespace NHibernate.Test.Linq
{
    using System.Threading.Tasks;
    using System.Threading;
    [TestFixture]
    public class QueryReuseTestsAsync : LinqTestCase
    {
        private IQueryable<User> _query;

        protected override void OnSetUp()
        {
            base.OnSetUp();

            _query = db.Users;
        }

        private async Task AssertQueryReuseableAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            IList<User> users = await (_query.ToListAsync(cancellationToken));
            Assert.AreEqual(3, users.Count);
        }

        [Test]
        public async Task CanReuseAfterFirstAsync()
        {
            var user = await (_query.FirstAsync(u => u.Name == "rahien", CancellationToken.None));

            Assert.IsNotNull(user);
            await (AssertQueryReuseableAsync(CancellationToken.None));
        }

        [Test]
        public async Task CanReuseAfterFirstOrDefaultAsync()
        {
            var user = await (_query.FirstOrDefaultAsync(u => u.Name == "rahien", CancellationToken.None));

            Assert.IsNotNull(user);
            await (AssertQueryReuseableAsync(CancellationToken.None));
        }

        [Test]
        public async Task CanReuseAfterSingleAsync()
        {
            var user = await (_query.SingleAsync(u => u.Name == "rahien", CancellationToken.None));

            Assert.IsNotNull(user);
            await (AssertQueryReuseableAsync(CancellationToken.None));
        }

        [Test]
        public async Task CanReuseAfterSingleOrDefaultAsync()
        {
            User user = await (_query.SingleOrDefaultAsync(u => u.Name == "rahien", CancellationToken.None));

            Assert.IsNotNull(user);
            await (AssertQueryReuseableAsync(CancellationToken.None));
        }

        [Test]
        public Task CanReuseAfterAggregateAsync()
        {
            try
            {
                User user = _query.Aggregate((u1, u2) => u1);

                Assert.IsNotNull(user);
                return AssertQueryReuseableAsync(CancellationToken.None);
            }
            catch (System.Exception ex)
            {
                return Task.FromException<object>(ex);
            }
        }

        [Test]
        public async Task CanReuseAfterAverageAsync()
        {
            double average = await (_query.AverageAsync(u => u.InvalidLoginAttempts, CancellationToken.None));

            Assert.AreEqual(5.0, average);
            await (AssertQueryReuseableAsync(CancellationToken.None));
        }

        [Test]
        public async Task CanReuseAfterCountAsync()
        {
            int totalCount = await (_query.CountAsync(CancellationToken.None));

            Assert.AreEqual(3, totalCount);
            await (AssertQueryReuseableAsync(CancellationToken.None));
        }

        [Test]
        public async Task CanReuseAfterCountWithPredicateAsync()
        {
            int count = await (_query.CountAsync(u => u.LastLoginDate != null, CancellationToken.None));

            Assert.AreEqual(1, count);
            await (AssertQueryReuseableAsync(CancellationToken.None));
        }

        [Test]
        public async Task CanReuseAfterLongCountAsync()
        {
            long totalCount = await (_query.LongCountAsync(CancellationToken.None));

            Assert.AreEqual(3, totalCount);
            await (AssertQueryReuseableAsync(CancellationToken.None));
        }

        [Test]
        public async Task CanReuseAfterLongCountWithPredicateAsync()
        {
            long totalCount = await (_query.LongCountAsync(u => u.LastLoginDate != null, CancellationToken.None));

            Assert.AreEqual(1, totalCount);
            await (AssertQueryReuseableAsync(CancellationToken.None));
        }

        [Test]
        public async Task CanReuseAfterMaxAsync()
        {
            int max = await (_query.MaxAsync(u => u.InvalidLoginAttempts, CancellationToken.None));

            Assert.AreEqual(6, max);
            await (AssertQueryReuseableAsync(CancellationToken.None));
        }

        [Test]
        public async Task CanReuseAfterMinAsync()
        {
            int min = await (_query.MinAsync(u => u.InvalidLoginAttempts, CancellationToken.None));

            Assert.AreEqual(4, min);
            await (AssertQueryReuseableAsync(CancellationToken.None));
        }

        [Test]
        public async Task CanReuseAfterSumAsync()
        {
            int sum = await (_query.SumAsync(u => u.InvalidLoginAttempts, CancellationToken.None));

            Assert.AreEqual(4 + 5 + 6, sum);
            await (AssertQueryReuseableAsync(CancellationToken.None));
        }
    }
}