using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly MongoDatabase _database;

        private readonly Task _task;

        private DB()
        {
            string connectionString = Configuration.Settings("DBConnectionString", "mongodb://localhost");
            string dbName = Configuration.Settings("DBName", "Automation");
            MongoServer server = MongoServer.Create(connectionString);
            server.Connect();
            _database = server.GetDatabase(dbName);

            //CancellationTokenSource source = new CancellationTokenSource();
            //CancellationToken token = source.Token;

            _task = Task.Factory.StartNew(() =>
                                             {
                                                 while (true)
                                                 {
                                                     Thread.Sleep(10*60*1000);
                                                     float cpuUsage = getCPUCOunter();
                                                     if (cpuUsage > 0.10)
                                                         continue;
                                                     Thread.Sleep(1*60*1000);
                                                     cpuUsage = getCPUCOunter();
                                                     if (cpuUsage > 0.10)
                                                         continue;
                                                     CleanDataBase();
                                                 }
                                             });

            //task.Start();
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
            var cpuCounter = new PerformanceCounter
                                 {CategoryName = "Processor", CounterName = "% Processor Time", InstanceName = "_Total"};

            // will always start at 0
            cpuCounter.NextValue();
            Thread.Sleep(1000);
            // now matches task manager reading
            float secondValue = cpuCounter.NextValue();

            return secondValue;
        }

        public void Dispose()
        {
            if (_database.Server != null)
                _database.Server.Disconnect();

            if (_task != null)
                _task.Dispose();
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

        public T Find<T>(string id)
        {
            return Find<T>("_id", id);
        }

        public T Find<T>(params string[] param)
        {
            IMongoQuery query = QueryCondition(param);
            return _database.GetCollection<T>(typeof (T).Name).FindOne(query);
        }

        public List<T> Finds<T>(IMongoQuery qc)
        {
            MongoCursor<T> cursor = _database.GetCollection<T>(typeof (T).Name).Find(qc);
            var result = new List<T>();
            foreach (T t in cursor)
            {
                result.Add(t);
            }
            return result;
        }

        public IMongoQuery QueryCondition(params string[] para)
        {
            int aLength = para.Length/2;
            var qc = new QueryComplete[aLength];
            for (int i = 0; i < para.Length; i++)
            {
                string keyName = para[i];
                if (i + 1 > para.Length - 1)
                    break;
                int number = i/2;
                i++;

                BsonValue value = new BsonString(para[i]);
                qc[number] = Query.EQ(keyName, value);
            }
            return Query.And(qc);
        }

        public List<T> FindKids<T>(string parentId)
        {
            return Finds<T>(Query.EQ(Const.AttributeParentId, parentId));
            //MongoCursor<T> cursor = _database.GetCollection<T>(typeof(T).Name).Find(Query.EQ(Const.AttributeParentId, parentId));
            //List<T> result = new List<T>();
            //foreach (T t in cursor)
            //{
            //    result.Add(t);
            //}
            //return result;
        }

        public void Add<T>(T item)
        {
            _database.GetCollection<T>(typeof (T).Name).Insert(item);
        }

        public void Add<T>(IEnumerable<T> items)
        {
            _database.GetCollection<T>(typeof (T).Name).Insert(items);
        }

        public void Save<T>(T item)
        {
            //_database.GetCollection(item.GetType().Name).Save(item);
            _database.GetCollection<T>(typeof (T).Name).Save(item);
        }

        public void Update<T>(T item)
        {
            Save(item);
        }

        public void Delete<T>(string id)
        {
            _database.GetCollection<T>(typeof (T).Name).Remove(Query.EQ("_id", id));
        }

        public void Drop<T>()
        {
            _database.GetCollection<T>(typeof (T).Name);
            _database.DropCollection(typeof (T).Name);
        }

        public void CreateCappedCollection<T>(string name)
        {
            _database.GetCollection<T>(typeof (T).Name);
            _database.GetCollection<T>(typeof (T).Name).CreateIndex(new[] {Const.AttributeParentId});
        }
    }
}