using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Moq;
using UsersVoice.Infrastructure.Mongo;
using UsersVoice.Infrastructure.Mongo.Commands.Entities;

namespace UsersVoice.Services.Infrastructure.Mongo.Tests
{
    public static class RepositoryHelpers
    {
        public static Mock<IRepository<TEntity>> MockRepo<TEntity>(IDictionary<Guid, TEntity> entities) where TEntity : IEntity
        {
            entities = entities ?? new Dictionary<Guid, TEntity>();

            var mockRepo = new Mock<IRepository<TEntity>>();
            mockRepo.Setup(r => r.Find(It.IsAny<Expression<Func<TEntity, bool>>>()))
                .Returns((Expression<Func<TEntity, bool>> exp) =>
                {
                    var predicate = exp.Compile();
                    var filteredItems = entities.Values.Where(predicate).ToArray();
                    var cursor = new FakeFindFluent<TEntity>(filteredItems);
                    return cursor;
                });

            mockRepo.Setup(r => r.UpsertOneAsync(It.IsAny<Expression<Func<TEntity, bool>>>(),
               It.IsAny<TEntity>()))
               .Returns((Expression<Func<TEntity, bool>> f, TEntity i) =>
               {
                   if (!entities.ContainsKey(i.Id))
                       entities.Add(i.Id, i);
                   else 
                       entities[i.Id] = i;
                   return Task.FromResult(i);
               });

            mockRepo.Setup(r => r.InsertOneAsync(It.IsAny<TEntity>()))
              .Returns((TEntity i) =>
              {
                  entities[i.Id] = i;
                      
                  return Task.FromResult(i);
              });

            return mockRepo;
        }
    }
}