using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Moq;
using UsersVoice.Infrastructure.Mongo;
using UsersVoice.Infrastructure.Mongo.Commands.Entities;
using UsersVoice.Infrastructure.Mongo.Queries.Entities;

namespace UsersVoice.Services.Infrastructure.Mongo.Tests
{
    public static class RepositoryHelpers
    {
        public static Mock<IRepository<TEntity>> MockQueriesRepo<TEntity>(IList<TEntity> entities, Func<TEntity, bool> finder) 
            where TEntity : IQueryEntity
        {
            entities = entities ?? new List<TEntity>();

            var mockRepo = new Mock<IRepository<TEntity>>();

            mockRepo.Setup(r => r.Find(It.IsAny<Expression<Func<TEntity, bool>>>()))
                .Returns((Expression<Func<TEntity, bool>> exp) =>
                {
                    var predicate = exp.Compile();
                    var filteredItems = entities.Where(predicate).ToArray();
                    var cursor = new FakeFindFluent<TEntity>(filteredItems);
                    return cursor;
                });

            mockRepo.Setup(r => r.InsertOneAsync(It.IsAny<TEntity>()))
            .Returns((TEntity i) =>
            {
                entities.Add(i);

                return Task.FromResult(i);
            });

            mockRepo.Setup(r => r.UpsertOneAsync(It.IsAny<Expression<Func<TEntity, bool>>>(),
             It.IsAny<TEntity>()))
             .Returns((Expression<Func<TEntity, bool>> f, TEntity i) =>
             {
                 var indexes = new List<int>();

                 if (null != finder)
                 {
                     int index = -1;
                     foreach (var entity in entities)
                     {
                         index++;
                         if (finder(entity))
                             indexes.Add(index);
                     }
                 }
                 else
                 {
                     int index = -1;
                     foreach (var entity in entities)
                     {
                         index++;
                         if (entity.Equals(i))
                             indexes.Add(index);
                     }
                 }

                 if (indexes.Any())
                 {
                     foreach (var index in indexes)
                         entities[index] = i;
                 }
                 else
                 {
                     entities.Add(i);
                 }

                 return Task.FromResult(i);
             });


            return mockRepo;
        }

        public static Mock<IRepository<TEntity>> MockCommandsRepo<TEntity>(IDictionary<Guid, TEntity> entities) where TEntity : ICommandEntity
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