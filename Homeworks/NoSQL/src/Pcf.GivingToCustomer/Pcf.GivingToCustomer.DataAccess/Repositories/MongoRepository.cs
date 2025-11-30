using MongoDB.Driver;
using Pcf.GivingToCustomer.Core.Abstractions.Repositories;
using Pcf.GivingToCustomer.Core.Domain.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Pcf.GivingToCustomer.DataAccess.Repositories
{
    public class MongoRepository<T> : IRepository<T> where T : MongoBaseEntity
    {
        protected readonly IMongoCollection<T> _collection;

        public MongoRepository(IMongoCollection<T> collection)
        {
            _collection = collection;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetRangeByIdsAsync(List<Guid> ids)
        {
            return await _collection.Find(x => ids.Contains(x.Id)).ToListAsync(); 
        }

        public async Task<T> GetFirstWhere(Expression<Func<T, bool>> predicate)
        {
            return await _collection.Find(predicate).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate)
        {
            return await _collection.Find(predicate).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            if (entity.EntityId == Guid.Empty)
                entity.EntityId = Guid.NewGuid();

            await _collection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            await _collection.ReplaceOneAsync(x => x.EntityId == entity.EntityId, entity);
        }

        public async Task DeleteAsync(T entity)
        {
            await _collection.DeleteOneAsync(x => x.EntityId == entity.EntityId);
        }
    }
}
