﻿using MongoDB.Driver;
using Silmoon.Data.MongoDB.MongoDB.Models;
using Silmoon.Data.MongoDB.MongoDB;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Silmoon.Data.MongoDB
{
    public interface IMongoService
    {
        MongoExecuter Executer { get; set; }

        void Add<T>(T obj) where T : IIdObject;
        void Adds<T>(T[] objs) where T : IIdObject;

        T[] Gets<T>(Expression<Func<T, bool>> whereFunc, int? offset = null, int? count = null) where T : IIdObject;
        T[] Gets<T, TOrderKey>(Expression<Func<T, bool>> whereFunc, Expression<Func<T, TOrderKey>> orderFunc, bool? ascending, int? offset = null, int? count = null) where T : IIdObject;
        IQueryable<T> GetsQuery<T>(Expression<Func<T, bool>> whereFunc, int? offset = null, int? count = null) where T : IIdObject;
        IQueryable<T> GetsQuery<T, TOrderKey>(Expression<Func<T, bool>> whereFunc, Expression<Func<T, TOrderKey>> orderFunc, bool? ascending, int? offset = null, int? count = null) where T : IIdObject;

        T Get<T>(Expression<Func<T, bool>> whereFunc) where T : IIdObject;
        T Get<T, TOrderKey>(Expression<Func<T, bool>> whereFunc, Expression<Func<T, TOrderKey>> orderFunc, bool? ascending) where T : IIdObject;

        UpdateResult Set<T>(T obj, Expression<Func<T, bool>> whereFunc = null, params Expression<Func<T, object>>[] updateFields) where T : IIdObject;
        UpdateResult Sets<T>(T obj, Expression<Func<T, bool>> whereFunc = null, params Expression<Func<T, object>>[] updateFields) where T : IIdObject;

        DeleteResult Delete<T>(Expression<Func<T, bool>> whereFunc) where T : IIdObject;
        DeleteResult Deletes<T>(Expression<Func<T, bool>> whereFunc) where T : IIdObject;
    }
}
