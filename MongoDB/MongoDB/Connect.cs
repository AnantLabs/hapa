using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace MongoDB
{
    public class Connect : IDisposable
    {
        private static Connect _instance;

        public readonly MongoCollection<BsonDocument> _contents;
        MongoServer server = null;
        Task task = null;

        private Connect()
        {
            MongoDatabase _database;
            string connectionString = Configuration.Settings("DBConnectionString", "mongodb://localhost");

            server = MongoServer.Create(connectionString);
            server.Connect();
            _database = server.GetDatabase(Configuration.Settings("DBName", "Automation"));
            string collctionName = Configuration.Settings("CollectionName", "Data");
            _contents =
                _database.GetCollection<BsonDocument>(collctionName);   
  
            task = .Factory.StartNew(Action()=>{
                while(true){
                    Thread.Sleep(10*60*1000);
                    float cpuUsage = getCPUCOunter();
                    if(cpuUsage>0.10)
                        continue;
                    Thread.Sleep(1*60*1000);
                    float cpuUsage = getCPUCOunter();
                    if(cpuUsage>0.10)
                        continue;
                    else
                        CleanDataBase();
                }
            },null,TaskCreationOptions.None,ProcessPriorityClass.BelowNormal);
            
            task.Start();
            
        }

        private void CleanDataBase()
        {
            string strategy = Configuration.Settings("CleanStategy", "Cascade");
            if (strategy.Equals("Cascade"))
            {
                DeleteCascade();
            }
            if (strategy.Equals("LeafOrphan"))
            {
                DeleteLeafOrphan();
            }
        }

        public static Connect GetInstance()
        {
            return _instance ?? (_instance = new Connect());
        }

        public void DeleteAll()
        {
            _contents.Drop();
            _instance = null;
        }

        public void DeleteLeafOrphan()
        {
            //_contents.Remove(Query.EQ(Const.AttributeParentId, BsonNull));
            MongoCursor<BsonDocument> orphans = _contents.Find(Query.EQ(Const.AttributeParentId, BsonNull.Value));
            foreach (var or in orphans)
            {
                if (hasChildren(or))
                    continue;
               Remove(or.GetValue(Const.AttributeId).ToString());
            }

        }

        
        public void DeleteCascade()
        {
            MongoCursor<BsonDocument> kids = _contents.Find(Query.EQ(Const.AttributeParentId, BsonNull.Value));
            foreach (var bd in kids)
            {
                string id = bd.GetValue(Const.AttributeId, null);
                if (string.IsNullOrEmpty(id))
                    continue;
                Remove(id);
            }
        }

        public void DeleteCascade(string id)
        {
            MongoCursor<BsonDocument> kids = _contents.Find(Query.EQ(Const.AttributeParentId, id));
            foreach (var bd in kids)
            {
                string id = bd.GetValue(Const.AttributeId, null);
                if (string.IsNullOrEmpty(id))
                    continue;
                DeleteCascade(id);
            }
            Remove(id);
        }

        private bool hasChildren(BsonDocument bd)
        {
            string id = bd.GetValue(Const.AttributeId, null);
            MongoCursor<BsonDocument> kids = _contents.Find(Query.EQ(Const.AttributeParentId, id));
            return (kids.Count() > 0);
        }

        public bool Remove(string id)
        {
            //TODO sometimes, we cannot remove something (like:it is already gone, has kids etc.)
            _contents.Remove(Query.EQ(Const.AttributeId, id));
        }

        //public XElement Get(string id)
        //{         
        //    QueryComplete query = Query.EQ("_id", id);
        //    MongoCursor<Content> cursor = _contents.Find(query);

        //    if (cursor.Count() == 0)
        //        return "";
        //    Content data = (cursor.First());
            
        //    return data.ToString();
        //}

        public float getCPUCOunter()
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
            if (server != null)
                server.Disconnect();
            if (task != null)
                task.Dispose();
            
        }

        
    }
}
