using MongoDB.Driver;
using System;
using System.Linq;
using System.Linq.Expressions;
using Silmoon.Data.MongoDB.Models;

namespace Silmoon.Data.MongoDB.Interfaces
{
    public interface IMongoService
    {
        MongoExecuter Executer { get; set; }

        void Add<T>(T obj, IClientSessionHandle sessionHandle = null) where T : IIdObject;
        void Adds<T>(T[] objs, IClientSessionHandle sessionHandle = null) where T : IIdObject;

        T[] Gets<T>(Expression<Func<T, bool>> whereFunc, int? offset = null, int? count = null, IClientSessionHandle sessionHandle = null) where T : IIdObject;
        T[] Gets<T, TOrderKey>(Expression<Func<T, bool>> whereFunc, Expression<Func<T, TOrderKey>> orderFunc, bool? ascending, int? offset = null, int? count = null, IClientSessionHandle sessionHandle = null) where T : IIdObject;
        IQueryable<T> GetsQuery<T>(Expression<Func<T, bool>> whereFunc, int? offset = null, int? count = null, IClientSessionHandle sessionHandle = null) where T : IIdObject;
        IQueryable<T> GetsQuery<T, TOrderKey>(Expression<Func<T, bool>> whereFunc, Expression<Func<T, TOrderKey>> orderFunc, bool? ascending, int? offset = null, int? count = null, IClientSessionHandle sessionHandle = null) where T : IIdObject;

        T Get<T>(Expression<Func<T, bool>> whereFunc, IClientSessionHandle sessionHandle = null) where T : IIdObject;
        T Get<T, TOrderKey>(Expression<Func<T, bool>> whereFunc, Expression<Func<T, TOrderKey>> orderFunc, bool? ascending, IClientSessionHandle sessionHandle = null) where T : IIdObject;

        UpdateResult Set<T>(T obj, Expression<Func<T, bool>> whereFunc = null, IClientSessionHandle sessionHandle = null, params Expression<Func<T, object>>[] updateFields) where T : IIdObject;
        UpdateResult Sets<T>(T obj, Expression<Func<T, bool>> whereFunc = null, IClientSessionHandle sessionHandle = null, params Expression<Func<T, object>>[] updateFields) where T : IIdObject;
        UpdateResult Set<T>(T obj, bool isUpsert = false, Expression<Func<T, bool>> whereFunc = null, IClientSessionHandle sessionHandle = null, params Expression<Func<T, object>>[] updateFields) where T : IIdObject;
        UpdateResult Sets<T>(T obj, bool isUpsert = false, Expression<Func<T, bool>> whereFunc = null, IClientSessionHandle sessionHandle = null, params Expression<Func<T, object>>[] updateFields) where T : IIdObject;

        DeleteResult Delete<T>(Expression<Func<T, bool>> whereFunc, IClientSessionHandle sessionHandle = null) where T : IIdObject;
        DeleteResult Deletes<T>(Expression<Func<T, bool>> whereFunc, IClientSessionHandle sessionHandle = null) where T : IIdObject;
    }
}
