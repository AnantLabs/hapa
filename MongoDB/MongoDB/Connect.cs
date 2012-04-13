using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MongoDB
{
    public class Connect
    {
        private static Connect _instance;

        public readonly MongoCollection<Content> _contents;
        private readonly MongoDatabase _database;

        private Connect()
        {
            string connectionString = Configuration.Settings("DBConnectionString", "mongodb://localhost");

            MongoServer server = MongoServer.Create(connectionString);
            server.Connect();
            _database = server.GetDatabase(Configuration.Settings("DBName", "Automation"));
            _contents =
                _database.GetCollection<Content>("Data");            
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

        public void Revome(string id)
        {
            //TODO delete all its children first
            _contents.Remove(Query.EQ("_id", id));
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
    }
}
