using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Silmoon.Data.MongoDB.Extensions;
using Silmoon.Data.MongoDB.MongoDB.Models;
using Silmoon.Data.MongoDB.MongoDB.Query;
using Silmoon.Data.QueryModel;
using Silmoon.Extension;
using Silmoon.Runtime;
using Silmoon.Runtime.Collections;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Silmoon.Data.MongoDB.MongoDB
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

        public void AddObject<T>(string collectionName, T obj)
        {
            GetCollection<T>(collectionName).InsertOne(obj);
        }
        public void AddObjects<T>(string collectionName, T[] obj)
        {
            GetCollection<T>(collectionName).InsertMany(obj);
        }


        public T GetObject<T>(string collectionName, object findByObject, QueryOptions options = null)
        {
            var result = GetFindResult(collectionName, MakeAeqFilter<T>(findByObject), options);
            return result.FirstOrDefault();
        }
        [Obsolete]
        public T GetObject<T>(string collectionName, ExpandoObject findByObject, QueryOptions options = null)
        {
            var result = GetFindResult(collectionName, MakeAeqFilter<T>(findByObject), options);
            return result.FirstOrDefault();
        }
        public T GetObject<T>(string collectionName, Expression<Func<T, bool>> filterFunc, QueryOptions options = null)
        {
            var result = GetFindResult(collectionName, (FilterDefinition<T>)filterFunc, options);
            return result.FirstOrDefault();
        }
        public T GetObject<T>(string collectionName, FilterDefinition<T> filterDefinition, QueryOptions options = null)
        {
            var result = GetFindResult(collectionName, filterDefinition, options);
            return result.FirstOrDefault();
        }

        public T[] GetObjects<T>(string collectionName, object findByObject, QueryOptions options = null)
        {
            var result = GetFindResult(collectionName, MakeAeqFilter<T>(findByObject), options);
            return result.ToList().ToArray();
        }
        [Obsolete]
        public T[] GetObjects<T>(string collectionName, ExpandoObject findByObject, QueryOptions options = null)
        {
            var result = GetFindResult(collectionName, MakeAeqFilter<T>(findByObject), options);
            return result.ToList().ToArray();
        }
        public T[] GetObjects<T>(string collectionName, Expression<Func<T, bool>> filterFunc, QueryOptions options = null)
        {
            var result = GetFindResult(collectionName, (FilterDefinition<T>)filterFunc, options);
            return result.ToList().ToArray();
        }
        public T[] GetObjects<T>(string collectionName, FilterDefinition<T> filterDefinition, QueryOptions options = null)
        {
            var result = GetFindResult(collectionName, filterDefinition, options);
            return result.ToList().ToArray();
        }

        public LookupResult<T> GetObject<T>(string collectionName, object findByObject, MongoLookup[] lookups, QueryOptions options = null) where T : new()
        {
            return GetObject(collectionName, MakeAeqFilter<T>(findByObject), lookups, options);
        }
        [Obsolete]
        public LookupResult<T> GetObject<T>(string collectionName, ExpandoObject findByObject, MongoLookup[] lookups, QueryOptions options = null) where T : new()
        {
            return GetObject(collectionName, MakeAeqFilter<T>(findByObject), lookups, options);
        }
        public LookupResult<T> GetObject<T>(string collectionName, Expression<Func<T, bool>> filterFunc, MongoLookup[] lookups, QueryOptions options = null) where T : new()
        {
            return GetObject(collectionName, (FilterDefinition<T>)filterFunc, lookups, options);
        }
        public LookupResult<T> GetObject<T>(string collectionName, FilterDefinition<T> filterDefinition, MongoLookup[] lookups, QueryOptions options = null) where T : new()
        {
            BsonDocument result = default;
            try
            {
                result = GetLookupResult(collectionName, filterDefinition, lookups, options).FirstOrDefault();
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
        public LookupResult<T>[] GetObjects<T>(string collectionName, object findByObject, MongoLookup[] lookups, QueryOptions options = null) where T : new()
        {
            return GetObjects(collectionName, MakeAeqFilter<T>(findByObject), lookups, options);
        }
        [Obsolete]
        public LookupResult<T>[] GetObjects<T>(string collectionName, ExpandoObject findByObject, MongoLookup[] lookups, QueryOptions options = null) where T : new()
        {
            return GetObjects(collectionName, MakeAeqFilter<T>(findByObject), lookups, options);
        }
        public LookupResult<T>[] GetObjects<T>(string collectionName, Expression<Func<T, bool>> filterFunc, MongoLookup[] lookups, QueryOptions options = null) where T : new()
        {
            return GetObjects(collectionName, (FilterDefinition<T>)filterFunc, lookups, options);
        }
        public LookupResult<T>[] GetObjects<T>(string collectionName, FilterDefinition<T> filterDefinition, MongoLookup[] lookups, QueryOptions options = null) where T : new()
        {
            BsonDocument[] result = default;
            try
            {
                result = GetLookupResult(collectionName, filterDefinition, lookups, options).ToList().ToArray();
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

        public dynamic GetGroupingResult<T, TKey, TNewResult>(string collectionName, FilterDefinition<T> filterDefinition, Expression<Func<T, TKey>> id, Expression<Func<IGrouping<TKey, T>, TNewResult>> grouping)
        {
            var result = GetCollection<T>(collectionName).Aggregate()
                .Match(filterDefinition)
                .Group(id, grouping)
                .ToList().ToArray();
            return result;
        }

        #region internal_functions
        IFindFluent<T, T> GetFindResult<T>(string collectionName, FilterDefinition<T> filterDefinition, QueryOptions options = null)
        {
            if (options is null) options = new QueryOptions();
            var result = GetCollection<T>(collectionName).Find(filterDefinition);

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
        IAggregateFluent<BsonDocument> GetLookupResult<T>(string collectionName, FilterDefinition<T> filterDefinition, MongoLookup[] lookups, QueryOptions options = null)
        {
            if (options is null) options = new QueryOptions();

            var collection = GetCollection<T>(collectionName);
            if (lookups != null)
            {
                var result0 = collection.Aggregate().Match(filterDefinition);
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

        public UpdateResult SetObject<T>(string collectionName, T obj, object findByObject, bool isUpsert = false, params string[] updateObjectFieldNames)
        {
            return GetCollection<T>(collectionName).UpdateOne(MakeAeqFilter<T>(findByObject), MakeUpdate(obj, updateObjectFieldNames), new UpdateOptions() { IsUpsert = isUpsert });
        }
        public UpdateResult SetObject<T>(string collectionName, T obj, object findByObject, bool isUpsert = false, params Expression<Func<T, object>>[] updateExpressions)
        {
            return GetCollection<T>(collectionName).UpdateOne(MakeAeqFilter<T>(findByObject), MakeUpdate(obj, updateExpressions), new UpdateOptions() { IsUpsert = isUpsert });
        }
        public UpdateResult SetObject<T>(string collectionName, T obj, FilterDefinition<T> filterDefinition, bool isUpsert = false, params string[] updateObjectFieldNames)
        {
            return GetCollection<T>(collectionName).UpdateOne(filterDefinition, MakeUpdate(obj, updateObjectFieldNames), new UpdateOptions() { IsUpsert = isUpsert });
        }
        public UpdateResult SetObject<T>(string collectionName, T obj, FilterDefinition<T> filterDefinition, bool isUpsert = false, params Expression<Func<T, object>>[] updateExpressions)
        {
            return GetCollection<T>(collectionName).UpdateOne(filterDefinition, MakeUpdate(obj, updateExpressions), new UpdateOptions() { IsUpsert = isUpsert });
        }
        public UpdateResult SetObject<T>(string collectionName, T obj, Expression<Func<T, bool>> filterFunc, bool isUpsert = false, params Expression<Func<T, object>>[] updateExpressions)
        {
            return GetCollection<T>(collectionName).UpdateOne((FilterDefinition<T>)filterFunc, MakeUpdate(obj, updateExpressions), new UpdateOptions() { IsUpsert = isUpsert });
        }

        public UpdateResult SetObjects<T>(string collectionName, T obj, object findByObject, bool isUpsert = false, params string[] updateObjectFieldNames)
        {
            return GetCollection<T>(collectionName).UpdateMany(MakeAeqFilter<T>(findByObject), MakeUpdate(obj, updateObjectFieldNames), new UpdateOptions() { IsUpsert = isUpsert });
        }
        public UpdateResult SetObjects<T>(string collectionName, T obj, object findByObject, bool isUpsert = false, params Expression<Func<T, object>>[] updateExpressions)
        {
            return GetCollection<T>(collectionName).UpdateMany(MakeAeqFilter<T>(findByObject), MakeUpdate(obj, updateExpressions), new UpdateOptions() { IsUpsert = isUpsert });
        }
        public UpdateResult SetObjects<T>(string collectionName, T obj, FilterDefinition<T> filterDefinition, bool isUpsert = false, params string[] updateObjectFieldNames)
        {
            return GetCollection<T>(collectionName).UpdateMany(filterDefinition, MakeUpdate(obj, updateObjectFieldNames), new UpdateOptions() { IsUpsert = isUpsert });
        }
        public UpdateResult SetObjects<T>(string collectionName, T obj, FilterDefinition<T> filterDefinition, bool isUpsert = false, params Expression<Func<T, object>>[] updateExpressions)
        {
            return GetCollection<T>(collectionName).UpdateMany(filterDefinition, MakeUpdate(obj, updateExpressions), new UpdateOptions() { IsUpsert = isUpsert });
        }
        public UpdateResult SetObjects<T>(string collectionName, T obj, Expression<Func<T, bool>> filterFunc, bool isUpsert = false, params Expression<Func<T, object>>[] updateExpressions)
        {
            return GetCollection<T>(collectionName).UpdateMany((FilterDefinition<T>)filterFunc, MakeUpdate(obj, updateExpressions), new UpdateOptions() { IsUpsert = isUpsert });
        }


        public ReplaceOneResult ReplaceObject<T>(string collectionName, T obj, object findByObject, bool isUpsert = false)
        {
            return GetCollection<T>(collectionName).ReplaceOne(MakeAeqFilter<T>(findByObject), obj, new ReplaceOptions() { IsUpsert = isUpsert });
        }
        public ReplaceOneResult ReplaceObject<T>(string collectionName, T obj, FilterDefinition<T> filterDefinition, bool isUpsert = false)
        {
            return GetCollection<T>(collectionName).ReplaceOne(filterDefinition, obj, new ReplaceOptions() { IsUpsert = isUpsert });
        }

        public DeleteResult DeleteObject<T>(string collectionName, object findByObject)
        {
            return GetCollection<T>(collectionName).DeleteOne(MakeAeqFilter<T>(findByObject));
        }
        public DeleteResult DeleteObject<T>(string collectionName, FilterDefinition<T> filterDefinition)
        {
            return GetCollection<T>(collectionName).DeleteOne(filterDefinition);
        }
        public DeleteResult DeleteObject<T>(string collectionName, Expression<Func<T, bool>> filterFunc)
        {
            return GetCollection<T>(collectionName).DeleteOne(filterFunc);
        }
        public DeleteResult DeleteObjects<T>(string collectionName, object findByObject)
        {
            return GetCollection<T>(collectionName).DeleteMany(MakeAeqFilter<T>(findByObject));
        }
        public DeleteResult DeleteObjects<T>(string collectionName, FilterDefinition<T> filterDefinition)
        {
            return GetCollection<T>(collectionName).DeleteMany(filterDefinition);
        }
        public DeleteResult DeleteObjects<T>(string collectionName, Expression<Func<T, bool>> filterFunc)
        {
            return GetCollection<T>(collectionName).DeleteMany(filterFunc);
        }

        public int Count<T>(string collectionName, object findByObject)
        {
            return (int)GetCollection<T>(collectionName).CountDocuments(MakeAeqFilter<T>(findByObject));
        }
        [Obsolete]
        public int Count<T>(string collectionName, ExpandoObject findByObject)
        {
            return (int)GetCollection<T>(collectionName).CountDocuments(MakeAeqFilter<T>(findByObject));
        }
        public int Count<T>(string collectionName, Expression<Func<T, bool>> filterFunc)
        {
            return (int)GetCollection<T>(collectionName).CountDocuments(filterFunc);
        }
        public int Count<T>(string collectionName, FilterDefinition<T> filterDefinition)
        {
            return (int)GetCollection<T>(collectionName).CountDocuments(filterDefinition);
        }
        public bool Exists<T>(string collectionName, Expression<Func<T, bool>> filterFunc)
        {
            return GetCollection<T>(collectionName).Find(filterFunc).Any();
        }

        public string CreateIndex<T>(string collectionName, Expression<Func<T, object>> keyFunc, bool isUnique = false)
        {
            var indexModel = new CreateIndexModel<T>(Builders<T>.IndexKeys.Ascending(keyFunc), new CreateIndexOptions() { Unique = isUnique });
            return GetCollection<T>(collectionName).Indexes.CreateOne(indexModel);
        }

        public UpdateDefinition<T> MakeUpdate<T>(T obj, params string[] updateObjectFieldNames)
        {
            var paras = ObjectRef.GetProperties(obj);

            var updateBuilder = Builders<T>.Update;

            UpdateDefinition<T> update = null;

            foreach (var item in paras)
            {
                if (updateObjectFieldNames.Contains(item.Key))
                {
                    if (update is null)
                        update = updateBuilder.Set(item.Key, item.Value.GetValue(obj));
                    else
                        update = update.Set(item.Key, item.Value.GetValue(obj));
                }
            }


            return update;
        }
        public UpdateDefinition<T> MakeUpdate<T>(T obj, params Expression<Func<T, object>>[] updateExpressions)
        {
            var updateDefinitions = updateExpressions.Select(expr =>
            {
                // 解析表达式，获取字段的名称和值
                var memberExpression = expr.Body as MemberExpression;
                if (memberExpression == null && expr.Body is UnaryExpression unaryExpression)
                {
                    memberExpression = unaryExpression.Operand as MemberExpression;
                }

                if (memberExpression == null)
                {
                    throw new ArgumentException("无效的表达式。只支持属性表达式。");
                }

                var propertyName = memberExpression.Member.Name;
                var propertyInfo = memberExpression.Member as PropertyInfo;
                if (propertyInfo == null)
                {
                    throw new ArgumentException($"表达式指向的成员'{propertyName}'不是一个属性。");
                }

                var propertyValue = propertyInfo.GetValue(obj);
                return Builders<T>.Update.Set(propertyName, propertyValue);
            });

            return Builders<T>.Update.Combine(updateDefinitions);
        }
        public FilterDefinition<T> MakeAeqFilter<T>(object findByObject)
        {
            var paras = ObjectRef.GetProperties(findByObject);
            var fb = Builders<T>.Filter;
            var filter = fb.Empty;
            foreach (var item in paras)
            {
                filter &= fb.Eq(item.Key, item.Value.GetValue(findByObject));
            }
            return filter;
        }
        [Obsolete]
        public FilterDefinition<T> MakeAeqFilter<T>(ExpandoObject findByObject)
        {
            var paras = ObjectRef.GetProperties(findByObject);
            var fb = Builders<T>.Filter;
            var filter = fb.Empty;
            foreach (var item in paras)
            {
                filter &= fb.Eq(item.Key, item.Value.Value);
            }
            return filter;
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return Database.GetCollection<T>(collectionName);
        }
        public IMongoQueryable<T> GetQueryable<T>(string collectionName)
        {
            return GetCollection<T>(collectionName).AsQueryable();
        }
    }
}
