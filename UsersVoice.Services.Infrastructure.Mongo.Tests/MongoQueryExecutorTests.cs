using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using Moq;
using UsersVoice.Infrastructure.Mongo;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Infrastructure.Mongo.Queries;
using UsersVoice.Infrastructure.Mongo.Services;
using UsersVoice.Services.Infrastructure.Common.Services;
using Xunit;

namespace UsersVoice.Services.Infrastructure.Mongo.Tests
{
    [TestClass]
    public class MongoQueryExecutorTests
    {
        [Fact]
        public async Task ReadOneAsync_should_throw_ArgumentNullException_if_queryDefinition_is_null()
        {
            var sut = new MongoQueryExecutor();

            await Xunit.Assert.ThrowsAsync<ArgumentNullException>(() => sut.ReadOneAsync<MongoQueryDefinition<DummyEntity>>(null));
        }

        [Fact]
        public async Task ReadOneAsync_should_throw_ArgumentException_if_queryDefinition_Repository_is_null()
        {
            var mockQueryDef = new Mock<IMongoQueryDefinition<DummyEntity>>();
            mockQueryDef.SetupGet(qd => qd.Repository).Returns((IRepository<DummyEntity>) null);
            mockQueryDef.SetupGet(qd => qd.Filter).Returns("{}");

            var sut = new MongoQueryExecutor();

            await Xunit.Assert.ThrowsAsync<ArgumentException>(() => sut.ReadOneAsync(mockQueryDef.Object));
        }

        [Fact]
        public async Task ReadOneAsync_should_throw_ArgumentException_if_queryDefinition_Filter_is_null()
        {
            var mockQueryDef = new Mock<IMongoQueryDefinition<DummyEntity>>();
            mockQueryDef.SetupGet(qd => qd.Repository).Returns(new Mock<IRepository<DummyEntity>>().Object);
            mockQueryDef.SetupGet(qd => qd.Filter).Returns((FilterDefinition<DummyEntity>) null);

            var sut = new MongoQueryExecutor();

            await Xunit.Assert.ThrowsAsync<ArgumentException>(() => sut.ReadOneAsync(mockQueryDef.Object));
        }

        [Fact]
        public async Task ReadAsync_should_throw_ArgumentNullException_if_queryDefinition_is_null()
        {
            var sut = new MongoQueryExecutor();

            await Xunit.Assert.ThrowsAsync<ArgumentNullException>(() => sut.ReadAsync<MongoQueryDefinition<DummyEntity>>(null));
        }

        [Fact]
        public async Task ReadAsync_should_throw_ArgumentException_if_queryDefinition_Repository_is_null()
        {
            var mockQueryDef = new Mock<IMongoQueryDefinition<DummyEntity>>();
            mockQueryDef.SetupGet(qd => qd.Repository).Returns((IRepository<DummyEntity>)null);
            mockQueryDef.SetupGet(qd => qd.Filter).Returns("{}");

            var sut = new MongoQueryExecutor();

            await Xunit.Assert.ThrowsAsync<ArgumentException>(() => sut.ReadAsync(mockQueryDef.Object));
        }

        [Fact]
        public async Task ReadAsync_should_throw_ArgumentException_if_queryDefinition_Filter_is_null()
        {
            var mockQueryDef = new Mock<IMongoQueryDefinition<DummyEntity>>();
            mockQueryDef.SetupGet(qd => qd.Repository).Returns(new Mock<IRepository<DummyEntity>>().Object);
            mockQueryDef.SetupGet(qd => qd.Filter).Returns((FilterDefinition<DummyEntity>)null);

            var sut = new MongoQueryExecutor();

            await Xunit.Assert.ThrowsAsync<ArgumentException>(() => sut.ReadAsync(mockQueryDef.Object));
        }
    }

    public class DummyEntity { }
}
