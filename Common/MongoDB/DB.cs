using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace MongoDB
{
    public class DB
    {
    private static DB _instance;
        private MongoDatabase _database;
        
        Task task = null;

        private DB()
        {
            
            string connectionString = Configuration.Settings("DBConnectionString", "mongodb://localhost");
            string dbName = Configuration.Settings("DBName", "Automation");
            MongoServer server = MongoServer.Create(connectionString);
            server.Connect();
            _database = server.GetDatabase(dbName);

            //CancellationTokenSource source = new CancellationTokenSource();
            //CancellationToken token = source.Token;

            task = Task.Factory.StartNew( () =>
            {
                while (true)
                {
                    Thread.Sleep(10 * 60 * 1000);
                    float cpuUsage = getCPUCOunter();
                    if (cpuUsage > 0.10)
                        continue;
                    Thread.Sleep(1 * 60 * 1000);
                    cpuUsage = getCPUCOunter();
                    if (cpuUsage > 0.10)
                        continue;
                    else
                        CleanDataBase();
                }
            });

            task.Start();

        }

        private void CleanDataBase()
        {
            //string strategy = Configuration.Settings("CleanStategy", "Cascade");
            //if (strategy.Equals("Cascade"))
            //{
            //    DeleteCascade();
            //}
            ////LeafOrphan maybe just useless, not sure yet.
            //if (strategy.Equals("LeafOrphan"))
            //{
            //    DeleteLeafOrphan();
            //}
        }

        public static DB GetInstance()
        {
            return _instance ?? (_instance = new DB());
        }

        public void DeleteAll()
        {
            _database.Drop();

            _instance = null;
        }

        //public void DeleteLeafOrphan<T>()
        //{
        //    //_contents.Remove(Query.EQ(Const.AttributeParentId, BsonNull));
        //    MongoCursor<T> orphans = _contents.Find(Query.EQ(Const.AttributeParentId, BsonNull.Value));
        //    foreach (var or in orphans)
        //    {
        //        if (hasChildren(or))
        //            continue;
        //        Remove(or.GetValue(Const.AttributeId).ToString());
        //    }

        //}


        //public void DeleteCascade()
        //{
        //    //parent id is null, then begin delete
        //    MongoCursor<T> kids = _contents.Find(Query.EQ(Const.AttributeParentId, BsonNull.Value));
        //    foreach (var bd in kids)
        //    {
        //        string id = bd.GetValue(Const.AttributeId, null);
        //        if (string.IsNullOrEmpty(id))
        //            continue;
        //        DeleteCascade(id);
        //    }
        //}

        //public void DeleteCascade<T>(string id)
        //{
        //    List<T> kids = FindKids<T>(id);
        //    foreach (var bd in kids)
        //    {
        //        string id = bd.GetValue(Const.AttributeId, null);
        //        if (string.IsNullOrEmpty(id))
        //            continue;
        //        DeleteCascade(id);
        //    }
        //    Remove(id);
        //}

        //private bool hasChildren(BsonDocument bd)
        //{
        //    string id = bd.GetValue(Const.AttributeId, null);
        //    MongoCursor<BsonDocument> kids = _contents.Find(Query.EQ(Const.AttributeParentId, id));
        //    return (kids.Count() > 0);
        //}

        //public bool Remove(string id)
        //{
        //    //TODO sometimes, we cannot remove something (like:it is already gone, has kids etc.)
        //    _contents.Remove(Query.EQ(Const.AttributeId, id));
        //}

        //public XElement Get(string id)
        //{         
        //    QueryComplete query = Query.EQ("_id", id);
        //    MongoCursor<Content> cursor = _contents.Find(query);

        //    if (cursor.Count() == 0)
        //        return "";
        //    Content data = (cursor.First());

        //    return data.ToString();
        //}

        private float getCPUCOunter()
        {

            PerformanceCounter cpuCounter = new PerformanceCounter();
            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";

            // will always start at 0
            float firstValue = cpuCounter.NextValue();
            System.Threading.Thread.Sleep(1000);
            // now matches task manager reading
            float secondValue = cpuCounter.NextValue();

            return secondValue;

        }

        public void Dispose()
        {
            
            if (_database.Server != null)
                _database.Server.Disconnect();
            
            if (task != null)
                task.Dispose();

        }

        //public T MapReduce<T>(string map, string reduce)
        //{
        //    T result = default(T);
            
        //    MapReduce mr = _provider.Database.CreateMapReduce();

        //    MapReduceResponse response =
        //        mr.Execute(new MapReduceOptions(typeof(T).Name)
        //        {
        //            Map = map,
        //            Reduce = reduce
        //        });

        //    IMongoCollection<MapReduceResult<T>> coll = response.GetCollection<MapReduceResult<T>>();
        //    MapReduceResult<T> r = coll.Find().FirstOrDefault();
        //    result = r.Value;

        //    return result;
        //}

        public void Find<T>(string id, ref T result)
        {
            
            result = _database.GetCollection<T>(typeof(T).Name).FindOneById( new BsonString(id));            
        }

        public List<T> FindKids<T>(string parentId)
        {
            MongoCursor<T> cursor = _database.GetCollection<T>(typeof(T).Name).Find(Query.EQ(Const.AttributeParentId, parentId));
            List<T> result = new List<T>();
            foreach (T t in cursor)
            {
                result.Add(t);
            }
            return result;
        }

        public void Add<T>(T item) 
        {

            _database.GetCollection<T>(typeof(T).Name).Insert(item);
        }

        public void Add<T>(IEnumerable<T> items)
        {
            _database.GetCollection<T>(typeof(T).Name).Insert(items);
        }

        public void Save<T>(T item) 
        {
            _database.GetCollection<T>(typeof(T).Name).Save(item);
        }

        public void Update<T>(T item) 
        {
            Save(item);
        }

        public void Delete<T>(string id)
        {
            _database.GetCollection<T>(typeof(T).Name).Remove(Query.EQ(Const.AttributeId, id));           

        }

        public void Drop<T>()
        {

            var col = _database.GetCollection<T>(typeof(T).Name);
            _database.DropCollection(typeof(T).Name);
        }

        public void CreateCappedCollection<T>(string name)
        {
            _database.GetCollection<T>(typeof(T).Name);
            _database.GetCollection<T>(typeof(T).Name).CreateIndex(new string[]{Const.AttributeParentId});
            
        }
    }
}

