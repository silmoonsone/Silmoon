using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Silmoon.Data.MongoDB.Extensions;
using Silmoon.Data.MongoDB.Models;
using Silmoon.Data.MongoDB.Query;
using Silmoon.Data.QueryModel;
using Silmoon.Extension;
using Silmoon.Runtime;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Silmoon.Data.MongoDB
{
    public class MongoExecuter
    {
        public MongoConnect Connect { get; set; }
        public IMongoDatabase Database { get; set; }
        public MongoExecuter(MongoConnect connect, string database = null)
        {
            Connect = connect;
            Database = Connect.Client.GetDatabase(database ?? Connect.MongoUrl.DatabaseName);
        }
        public MongoExecuter(string Url, string database = null)
        {
            Connect = new MongoConnect(Url);
            Database = Connect.Client.GetDatabase(database ?? Connect.MongoUrl.DatabaseName);
        }

        public void AddObject<T>(string collectionName, T obj, IClientSessionHandle sessionHandle = null)
        {
            if (obj != null)
            {
                if (sessionHandle is null)
                    GetCollection<T>(collectionName).InsertOne(obj);
                else
                    GetCollection<T>(collectionName).InsertOne(sessionHandle, obj);
            }
        }
        public void AddObjects<T>(string collectionName, T[] obj, IClientSessionHandle sessionHandle = null)
        {
            if (!obj.IsNullOrEmpty())
            {
                if (sessionHandle is null)
                    GetCollection<T>(collectionName).InsertMany(obj);
                else
                    GetCollection<T>(collectionName).InsertMany(sessionHandle, obj);
            }
        }


        public T GetObject<T>(string collectionName, object findByObject, QueryOptions options = null, IClientSessionHandle sessionHandle = null) => GetFindResult(collectionName, MakeAeqFilter<T>(findByObject), options, sessionHandle).FirstOrDefault();
        [Obsolete]
        public T GetObject<T>(string collectionName, ExpandoObject findByObject, QueryOptions options = null, IClientSessionHandle sessionHandle = null) => GetFindResult(collectionName, MakeAeqFilter<T>(findByObject), options, sessionHandle).FirstOrDefault();
        public T GetObject<T>(string collectionName, Expression<Func<T, bool>> filterFunc, QueryOptions options = null, IClientSessionHandle sessionHandle = null) => GetFindResult(collectionName, (FilterDefinition<T>)filterFunc, options, sessionHandle).FirstOrDefault();
        public T GetObject<T>(string collectionName, FilterDefinition<T> filterDefinition, QueryOptions options = null, IClientSessionHandle sessionHandle = null) => GetFindResult(collectionName, filterDefinition, options, sessionHandle).FirstOrDefault();

        public T[] GetObjects<T>(string collectionName, object findByObject, QueryOptions options = null, IClientSessionHandle sessionHandle = null) => GetFindResult(collectionName, MakeAeqFilter<T>(findByObject), options, sessionHandle).ToList().ToArray();
        [Obsolete]
        public T[] GetObjects<T>(string collectionName, ExpandoObject findByObject, QueryOptions options = null, IClientSessionHandle sessionHandle = null) => GetFindResult(collectionName, MakeAeqFilter<T>(findByObject), options, sessionHandle).ToList().ToArray();
        public T[] GetObjects<T>(string collectionName, Expression<Func<T, bool>> filterFunc, QueryOptions options = null, IClientSessionHandle sessionHandle = null) => GetFindResult(collectionName, (FilterDefinition<T>)filterFunc, options, sessionHandle).ToList().ToArray();
        public T[] GetObjects<T>(string collectionName, FilterDefinition<T> filterDefinition, QueryOptions options = null, IClientSessionHandle sessionHandle = null) => GetFindResult(collectionName, filterDefinition, options, sessionHandle).ToList().ToArray();

        public LookupResult<T> GetObject<T>(string collectionName, object findByObject, MongoLookup[] lookups, QueryOptions options = null, IClientSessionHandle sessionHandle = null) where T : new() => GetObject(collectionName, MakeAeqFilter<T>(findByObject), lookups, options, sessionHandle);
        [Obsolete]
        public LookupResult<T> GetObject<T>(string collectionName, ExpandoObject findByObject, MongoLookup[] lookups, QueryOptions options = null, IClientSessionHandle sessionHandle = null) where T : new() => GetObject(collectionName, MakeAeqFilter<T>(findByObject), lookups, options, sessionHandle);
        public LookupResult<T> GetObject<T>(string collectionName, Expression<Func<T, bool>> filterFunc, MongoLookup[] lookups, QueryOptions options = null, IClientSessionHandle sessionHandle = null) where T : new() => GetObject(collectionName, (FilterDefinition<T>)filterFunc, lookups, options, sessionHandle);
        public LookupResult<T> GetObject<T>(string collectionName, FilterDefinition<T> filterDefinition, MongoLookup[] lookups, QueryOptions options = null, IClientSessionHandle sessionHandle = null) where T : new()
        {
            BsonDocument result = default;
            try
            {
                result = GetLookupResult(collectionName, filterDefinition, lookups, options, sessionHandle).FirstOrDefault();
                LookupResult<T> lr = new LookupResult<T>();
                foreach (var item2 in lookups)
                {
                    if (result[item2.As].AsBsonArray.Count == 1)
                        lr.AttachDocuments.Add(item2.As, new BsonDocument(result[item2.As][0].AsBsonDocument));
                    result.Remove(item2.As);
                }
                lr.Result = result.ToObject<T>();
                return lr;
            }
            catch (Exception e)
            {
                throw new MongoResultException(result, e.Message, e);
            }
        }
        public LookupResult<T>[] GetObjects<T>(string collectionName, object findByObject, MongoLookup[] lookups, QueryOptions options = null, IClientSessionHandle sessionHandle = null) where T : new() => GetObjects(collectionName, MakeAeqFilter<T>(findByObject), lookups, options, sessionHandle);
        [Obsolete]
        public LookupResult<T>[] GetObjects<T>(string collectionName, ExpandoObject findByObject, MongoLookup[] lookups, QueryOptions options = null, IClientSessionHandle sessionHandle = null) where T : new() => GetObjects(collectionName, MakeAeqFilter<T>(findByObject), lookups, options, sessionHandle);
        public LookupResult<T>[] GetObjects<T>(string collectionName, Expression<Func<T, bool>> filterFunc, MongoLookup[] lookups, QueryOptions options = null, IClientSessionHandle sessionHandle = null) where T : new() => GetObjects(collectionName, (FilterDefinition<T>)filterFunc, lookups, options, sessionHandle);
        public LookupResult<T>[] GetObjects<T>(string collectionName, FilterDefinition<T> filterDefinition, MongoLookup[] lookups, QueryOptions options = null, IClientSessionHandle sessionHandle = null) where T : new()
        {
            BsonDocument[] result = default;
            try
            {
                result = GetLookupResult(collectionName, filterDefinition, lookups, options, sessionHandle).ToList().ToArray();
                List<LookupResult<T>> results = new List<LookupResult<T>>();

                foreach (var item in result)
                {
                    LookupResult<T> lr = new LookupResult<T>();
                    foreach (var item2 in lookups)
                    {
                        if (item[item2.As].AsBsonArray.Count == 1)
                            lr.AttachDocuments.Add(item2.As, new BsonDocument(item[item2.As][0].AsBsonDocument));
                        item.Remove(item2.As);
                    }
                    lr.Result = item.ToObject<T>();
                    results.Add(lr);
                }
                return results.ToArray();
            }
            catch (Exception e)
            {
                throw new MongoResultsException(result, e.Message, e);
            }
        }

        public dynamic GetGroupingResult<T, TKey, TNewResult>(string collectionName, FilterDefinition<T> filterDefinition, Expression<Func<T, TKey>> id, Expression<Func<IGrouping<TKey, T>, TNewResult>> grouping, IClientSessionHandle sessionHandle)
        {
            if (sessionHandle is null)
            {
                var result = GetCollection<T>(collectionName).Aggregate()
                    .Match(filterDefinition)
                    .Group(id, grouping)
                    .ToList().ToArray();
                return result;
            }
            else
            {
                var result = GetCollection<T>(collectionName).Aggregate(sessionHandle)
                .Match(filterDefinition)
                .Group(id, grouping)
                .ToList().ToArray();
                return result;
            }
        }

        #region internal_functions
        IFindFluent<T, T> GetFindResult<T>(string collectionName, FilterDefinition<T> filterDefinition, QueryOptions options, IClientSessionHandle sessionHandle)
        {
            if (options is null) options = new QueryOptions();
            IFindFluent<T, T> result;
            if (sessionHandle is null) result = GetCollection<T>(collectionName).Find(filterDefinition);
            else result = GetCollection<T>(collectionName).Find(sessionHandle, filterDefinition);

            if (options.Sorts != null)
            {
                var sortBuilders = Builders<T>.Sort;
                SortDefinition<T> sort = null;

                foreach (var item in options.Sorts)
                {
                    if (sort is null)
                    {
                        if (item.Method == SortMethod.Asc)
                            sort = sortBuilders.Ascending(item.Name);
                        else
                            sort = sortBuilders.Descending(item.Name);
                    }
                    else
                    {
                        if (item.Method == SortMethod.Asc)
                            sort = sort.Ascending(item.Name);
                        else
                            sort = sort.Descending(item.Name);
                    }
                }
                result = result.Sort(sort);
            }

            if (options.Offset.HasValue) result = result.Skip(options.Offset);
            if (options.Count.HasValue) result = result.Limit(options.Count);


            if (options.ExcludedField != null && options.ExcludedField.Length != 0)
            {
                ProjectionDefinition<T> projection = null;
                foreach (var item in options.ExcludedField)
                {
                    if (projection is null) projection = Builders<T>.Projection.Exclude(item);
                    else projection = projection.Exclude(item);
                }
                result = result.Project<T>(projection);
            }
            return result;

        }
        IAggregateFluent<BsonDocument> GetLookupResult<T>(string collectionName, FilterDefinition<T> filterDefinition, MongoLookup[] lookups, QueryOptions options, IClientSessionHandle sessionHandle)
        {
            if (options is null) options = new QueryOptions();
            var collection = GetCollection<T>(collectionName);
            if (lookups != null)
            {
                IAggregateFluent<T> result0;
                if (sessionHandle is null) result0 = collection.Aggregate().Match(filterDefinition);
                else result0 = collection.Aggregate(sessionHandle).Match(filterDefinition);

                if (options.Sorts != null)
                {
                    var sortBuilders = Builders<T>.Sort;
                    SortDefinition<T> sort = null;

                    foreach (var item in options.Sorts)
                    {
                        if (sort is null)
                        {
                            if (item.Method == SortMethod.Asc)
                                sort = sortBuilders.Ascending(item.Name);
                            else
                                sort = sortBuilders.Descending(item.Name);
                        }
                        else
                        {
                            if (item.Method == SortMethod.Asc)
                                sort = sort.Ascending(item.Name);
                            else
                                sort = sort.Descending(item.Name);
                        }
                    }
                    result0 = result0.Sort(sort);
                }

                if (options.Offset.HasValue) result0 = result0.Skip(options.Offset.Value);
                if (options.Count.HasValue) result0 = result0.Limit(options.Count.Value);

                var result = result0.Lookup(lookups[0].ForeignCollection, lookups[0].LocalField, lookups[0].ForeignField, lookups[0].As);



                bool skipLookup = true;
                foreach (var item in lookups)
                {
                    if (!skipLookup) result = result.Lookup(item.ForeignCollection, item.LocalField, item.ForeignField, item.As);
                    skipLookup = false;
                }


                if (options.ExcludedField != null && options.ExcludedField.Length != 0)
                {
                    ProjectionDefinition<BsonDocument> projection = null;
                    foreach (var item in options.ExcludedField)
                    {
                        if (projection is null) projection = Builders<BsonDocument>.Projection.Exclude(item);
                        else projection = projection.Exclude(item);
                    }
                    result = result.Project<BsonDocument>(projection);
                }
                return result;
            }
            else
            {
                throw new ArgumentNullException("options.Lookups");
            }
        }
        #endregion

        public UpdateResult SetObject<T>(string collectionName, T obj, object findByObject, bool isUpsert = false, IClientSessionHandle sessionHandle = null, params string[] updateObjectFieldNames)
        {
            if (sessionHandle is null)
                return GetCollection<T>(collectionName).UpdateOne(MakeAeqFilter<T>(findByObject), MakeUpdate(obj, updateObjectFieldNames), new UpdateOptions() { IsUpsert = isUpsert });
            else
                return GetCollection<T>(collectionName).UpdateOne(sessionHandle, MakeAeqFilter<T>(findByObject), MakeUpdate(obj, updateObjectFieldNames), new UpdateOptions() { IsUpsert = isUpsert });
        }
        public UpdateResult SetObject<T>(string collectionName, T obj, object findByObject, bool isUpsert = false, IClientSessionHandle sessionHandle = null, params Expression<Func<T, object>>[] updateExpressions)
        {
            if (sessionHandle is null)
                return GetCollection<T>(collectionName).UpdateOne(MakeAeqFilter<T>(findByObject), MakeUpdate(obj, updateExpressions), new UpdateOptions() { IsUpsert = isUpsert });
            else
                return GetCollection<T>(collectionName).UpdateOne(sessionHandle, MakeAeqFilter<T>(findByObject), MakeUpdate(obj, updateExpressions), new UpdateOptions() { IsUpsert = isUpsert });
        }
        public UpdateResult SetObject<T>(string collectionName, T obj, FilterDefinition<T> filterDefinition, bool isUpsert = false, IClientSessionHandle sessionHandle = null, params string[] updateObjectFieldNames)
        {
            if (sessionHandle is null)
                return GetCollection<T>(collectionName).UpdateOne(filterDefinition, MakeUpdate(obj, updateObjectFieldNames), new UpdateOptions() { IsUpsert = isUpsert });
            else
                return GetCollection<T>(collectionName).UpdateOne(sessionHandle, filterDefinition, MakeUpdate(obj, updateObjectFieldNames), new UpdateOptions() { IsUpsert = isUpsert });
        }
        public UpdateResult SetObject<T>(string collectionName, T obj, FilterDefinition<T> filterDefinition, bool isUpsert = false, IClientSessionHandle sessionHandle = null, params Expression<Func<T, object>>[] updateExpressions)
        {
            if (sessionHandle is null)
                return GetCollection<T>(collectionName).UpdateOne(filterDefinition, MakeUpdate(obj, updateExpressions), new UpdateOptions() { IsUpsert = isUpsert });
            else
                return GetCollection<T>(collectionName).UpdateOne(sessionHandle, filterDefinition, MakeUpdate(obj, updateExpressions), new UpdateOptions() { IsUpsert = isUpsert });
        }
        public UpdateResult SetObject<T>(string collectionName, T obj, Expression<Func<T, bool>> filterFunc, bool isUpsert = false, IClientSessionHandle sessionHandle = null, params Expression<Func<T, object>>[] updateExpressions)
        {
            if (sessionHandle is null)
                return GetCollection<T>(collectionName).UpdateOne((FilterDefinition<T>)filterFunc, MakeUpdate(obj, updateExpressions), new UpdateOptions() { IsUpsert = isUpsert });
            else
                return GetCollection<T>(collectionName).UpdateOne(sessionHandle, (FilterDefinition<T>)filterFunc, MakeUpdate(obj, updateExpressions), new UpdateOptions() { IsUpsert = isUpsert });
        }
        public UpdateResult SetObjects<T>(string collectionName, T obj, object findByObject, bool isUpsert = false, IClientSessionHandle sessionHandle = null, params string[] updateObjectFieldNames)
        {
            if (sessionHandle is null)
                return GetCollection<T>(collectionName).UpdateMany(MakeAeqFilter<T>(findByObject), MakeUpdate(obj, updateObjectFieldNames), new UpdateOptions() { IsUpsert = isUpsert });
            else
                return GetCollection<T>(collectionName).UpdateMany(sessionHandle, MakeAeqFilter<T>(findByObject), MakeUpdate(obj, updateObjectFieldNames), new UpdateOptions() { IsUpsert = isUpsert });
        }
        public UpdateResult SetObjects<T>(string collectionName, T obj, object findByObject, bool isUpsert = false, IClientSessionHandle sessionHandle = null, params Expression<Func<T, object>>[] updateExpressions)
        {
            if (sessionHandle is null)
                return GetCollection<T>(collectionName).UpdateMany(MakeAeqFilter<T>(findByObject), MakeUpdate(obj, updateExpressions), new UpdateOptions() { IsUpsert = isUpsert });
            else
                return GetCollection<T>(collectionName).UpdateMany(sessionHandle, MakeAeqFilter<T>(findByObject), MakeUpdate(obj, updateExpressions), new UpdateOptions() { IsUpsert = isUpsert });
        }
        public UpdateResult SetObjects<T>(string collectionName, T obj, FilterDefinition<T> filterDefinition, bool isUpsert = false, IClientSessionHandle sessionHandle = null, params string[] updateObjectFieldNames)
        {
            if (sessionHandle is null)
                return GetCollection<T>(collectionName).UpdateMany(filterDefinition, MakeUpdate(obj, updateObjectFieldNames), new UpdateOptions() { IsUpsert = isUpsert });
            else
                return GetCollection<T>(collectionName).UpdateMany(sessionHandle, filterDefinition, MakeUpdate(obj, updateObjectFieldNames), new UpdateOptions() { IsUpsert = isUpsert });
        }
        public UpdateResult SetObjects<T>(string collectionName, T obj, FilterDefinition<T> filterDefinition, bool isUpsert = false, IClientSessionHandle sessionHandle = null, params Expression<Func<T, object>>[] updateExpressions)
        {
            if (sessionHandle is null)
                return GetCollection<T>(collectionName).UpdateMany(filterDefinition, MakeUpdate(obj, updateExpressions), new UpdateOptions() { IsUpsert = isUpsert });
            else
                return GetCollection<T>(collectionName).UpdateMany(sessionHandle, filterDefinition, MakeUpdate(obj, updateExpressions), new UpdateOptions() { IsUpsert = isUpsert });
        }
        public UpdateResult SetObjects<T>(string collectionName, T obj, Expression<Func<T, bool>> filterFunc, bool isUpsert = false, IClientSessionHandle sessionHandle = null, params Expression<Func<T, object>>[] updateExpressions)
        {
            if (sessionHandle is null)
                return GetCollection<T>(collectionName).UpdateMany((FilterDefinition<T>)filterFunc, MakeUpdate(obj, updateExpressions), new UpdateOptions() { IsUpsert = isUpsert });
            else
                return GetCollection<T>(collectionName).UpdateMany(sessionHandle, (FilterDefinition<T>)filterFunc, MakeUpdate(obj, updateExpressions), new UpdateOptions() { IsUpsert = isUpsert });
        }
        public ReplaceOneResult ReplaceObject<T>(string collectionName, T obj, object findByObject, bool isUpsert = false, IClientSessionHandle sessionHandle = null)
        {
            if (sessionHandle is null)
                return GetCollection<T>(collectionName).ReplaceOne(MakeAeqFilter<T>(findByObject), obj, new ReplaceOptions() { IsUpsert = isUpsert });
            else
                return GetCollection<T>(collectionName).ReplaceOne(sessionHandle, MakeAeqFilter<T>(findByObject), obj, new ReplaceOptions() { IsUpsert = isUpsert });
        }
        public ReplaceOneResult ReplaceObject<T>(string collectionName, T obj, FilterDefinition<T> filterDefinition, bool isUpsert = false, IClientSessionHandle sessionHandle = null)
        {
            if (sessionHandle is null)
                return GetCollection<T>(collectionName).ReplaceOne(filterDefinition, obj, new ReplaceOptions() { IsUpsert = isUpsert });
            else
                return GetCollection<T>(collectionName).ReplaceOne(sessionHandle, filterDefinition, obj, new ReplaceOptions() { IsUpsert = isUpsert });
        }
        public DeleteResult DeleteObject<T>(string collectionName, object findByObject, IClientSessionHandle sessionHandle = null)
        {
            if (sessionHandle is null)
                return GetCollection<T>(collectionName).DeleteOne(MakeAeqFilter<T>(findByObject));
            else
                return GetCollection<T>(collectionName).DeleteOne(sessionHandle, MakeAeqFilter<T>(findByObject));
        }
        public DeleteResult DeleteObject<T>(string collectionName, FilterDefinition<T> filterDefinition, IClientSessionHandle sessionHandle = null)
        {
            if (sessionHandle is null)
                return GetCollection<T>(collectionName).DeleteOne(filterDefinition);
            else
                return GetCollection<T>(collectionName).DeleteOne(sessionHandle, filterDefinition);
        }
        public DeleteResult DeleteObject<T>(string collectionName, Expression<Func<T, bool>> filterFunc, IClientSessionHandle sessionHandle = null)
        {
            if (sessionHandle is null)
                return GetCollection<T>(collectionName).DeleteOne(filterFunc);
            else
                return GetCollection<T>(collectionName).DeleteOne(sessionHandle, filterFunc);
        }
        public DeleteResult DeleteObjects<T>(string collectionName, object findByObject, IClientSessionHandle sessionHandle = null)
        {
            if (sessionHandle is null)
                return GetCollection<T>(collectionName).DeleteMany(MakeAeqFilter<T>(findByObject));
            else
                return GetCollection<T>(collectionName).DeleteMany(sessionHandle, MakeAeqFilter<T>(findByObject));
        }
        public DeleteResult DeleteObjects<T>(string collectionName, FilterDefinition<T> filterDefinition, IClientSessionHandle sessionHandle = null)
        {
            if (sessionHandle is null)
                return GetCollection<T>(collectionName).DeleteMany(filterDefinition);
            else
                return GetCollection<T>(collectionName).DeleteMany(sessionHandle, filterDefinition);
        }
        public DeleteResult DeleteObjects<T>(string collectionName, Expression<Func<T, bool>> filterFunc, IClientSessionHandle sessionHandle = null)
        {
            if (sessionHandle is null)
                return GetCollection<T>(collectionName).DeleteMany(filterFunc);
            else
                return GetCollection<T>(collectionName).DeleteMany(sessionHandle, filterFunc);
        }

        public int Count<T>(string collectionName, object findByObject, IClientSessionHandle sessionHandle = null)
        {
            if (sessionHandle is null)
                return (int)GetCollection<T>(collectionName).CountDocuments(MakeAeqFilter<T>(findByObject));
            else
                return (int)GetCollection<T>(collectionName).CountDocuments(sessionHandle, MakeAeqFilter<T>(findByObject));
        }
        [Obsolete]
        public long Count<T>(string collectionName, ExpandoObject findByObject, IClientSessionHandle sessionHandle = null)
        {
            if (sessionHandle is null)
                return GetCollection<T>(collectionName).CountDocuments(MakeAeqFilter<T>(findByObject));
            else
                return GetCollection<T>(collectionName).CountDocuments(sessionHandle, MakeAeqFilter<T>(findByObject));
        }
        public long Count<T>(string collectionName, Expression<Func<T, bool>> filterFunc, IClientSessionHandle sessionHandle = null)
        {
            if (sessionHandle is null)
                return GetCollection<T>(collectionName).CountDocuments(filterFunc);
            else
                return GetCollection<T>(collectionName).CountDocuments(sessionHandle, filterFunc);
        }
        public long Count<T>(string collectionName, FilterDefinition<T> filterDefinition, IClientSessionHandle sessionHandle = null)
        {
            if (sessionHandle is null)
                return GetCollection<T>(collectionName).CountDocuments(filterDefinition);
            else
                return GetCollection<T>(collectionName).CountDocuments(sessionHandle, filterDefinition);
        }
        public long EstimatedCount<T>(string collectionName)
        {
            return GetCollection<T>(collectionName).EstimatedDocumentCount();
        }
        public long EstimatedCount(string collectionName)
        {
            return GetCollection<BsonDocument>(collectionName).EstimatedDocumentCount();
        }

        public bool Exists<T>(string collectionName, Expression<Func<T, bool>> filterFunc, IClientSessionHandle sessionHandle = null)
        {
            if (sessionHandle is null)
                return GetCollection<T>(collectionName).Find(filterFunc).Any();
            else
                return GetCollection<T>(collectionName).Find(sessionHandle, filterFunc).Any();
        }

        public string CreateIndex<T>(string collectionName, Expression<Func<T, object>> keyFunc, bool isUnique = false, IClientSessionHandle sessionHandle = null)
        {
            if (sessionHandle is null)
            {
                var indexModel = new CreateIndexModel<T>(Builders<T>.IndexKeys.Ascending(keyFunc), new CreateIndexOptions() { Unique = isUnique });
                return GetCollection<T>(collectionName).Indexes.CreateOne(indexModel);
            }
            else
            {
                var indexModel = new CreateIndexModel<T>(Builders<T>.IndexKeys.Ascending(keyFunc), new CreateIndexOptions() { Unique = isUnique });
                return GetCollection<T>(collectionName).Indexes.CreateOne(sessionHandle, indexModel);

            }
        }

        UpdateDefinition<T> MakeUpdate<T>(T obj, params string[] updateObjectFieldNames)
        {
            var paras = obj.GetProperties();
            var updateBuilder = Builders<T>.Update;
            UpdateDefinition<T> update = null;

            foreach (var item in paras)
            {
                if (updateObjectFieldNames.Contains(item.Key))
                {
                    if (update is null)
                        update = updateBuilder.Set(item.Key, item.Value.GetValue(obj));
                    else update = update.Set(item.Key, item.Value.GetValue(obj));
                }
            }


            return update;
        }
        UpdateDefinition<T> MakeUpdate<T>(T obj, params Expression<Func<T, object>>[] updateExpressions)
        {
            var updateDefinitions = updateExpressions.Select(expr =>
            {
                // 解析表达式，获取字段的名称和值
                var memberExpression = expr.Body as MemberExpression;

                if (memberExpression == null && expr.Body is UnaryExpression unaryExpression) memberExpression = unaryExpression.Operand as MemberExpression;
                if (memberExpression == null) throw new ArgumentException("无效的表达式。只支持属性表达式。");

                var propertyName = memberExpression.Member.Name;
                var propertyInfo = memberExpression.Member as PropertyInfo;
                if (propertyInfo == null) throw new ArgumentException($"表达式指向的成员'{propertyName}'不是一个属性。");

                var propertyValue = propertyInfo.GetValue(obj);
                return Builders<T>.Update.Set(propertyName, propertyValue);
            });

            return Builders<T>.Update.Combine(updateDefinitions);
        }
        FilterDefinition<T> MakeAeqFilter<T>(object findByObject)
        {
            var paras = findByObject.GetProperties();
            var fb = Builders<T>.Filter;
            var filter = fb.Empty;
            foreach (var item in paras)
            {
                filter &= fb.Eq(item.Key, item.Value.GetValue(findByObject));
            }
            return filter;
        }
        [Obsolete]
        FilterDefinition<T> MakeAeqFilter<T>(ExpandoObject findByObject)
        {
            var paras = findByObject.GetProperties();
            var fb = Builders<T>.Filter;
            var filter = fb.Empty;
            foreach (var item in paras)
            {
                filter &= fb.Eq(item.Key, item.Value.Value);
            }
            return filter;
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName) => Database.GetCollection<T>(collectionName);
        public IQueryable<T> GetQueryable<T>(string collectionName, IClientSessionHandle sessionHandle = null)
        {
            if (sessionHandle is null)
                return GetCollection<T>(collectionName).AsQueryable();
            else
                return GetCollection<T>(collectionName).AsQueryable(sessionHandle);
        }

        public IClientSessionHandle StartSession(bool startTranscation)
        {
            var sessionHandle = Connect.Client.StartSession();
            if (startTranscation) sessionHandle.StartTransaction();
            return sessionHandle;
        }
        public void EndSession(IClientSessionHandle session) => session.Dispose();
    }
}