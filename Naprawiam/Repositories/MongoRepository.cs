using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Naprawiam.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core;
using System.Threading.Tasks;


namespace Naprawiam.Repositories
{
    public interface IAdressRespository
    {
        IEnumerable<Adress> AllAdresses();

        Adress GetById(ObjectId id);

        void Add(Adress adress);

        void Update(Adress adress);

        bool Remove(ObjectId id);
    }
    public class AdressRepository : IAdressRespository
    {
        //private readonly Settings _settings;
        private readonly IMongoDatabase _database;

        public AdressRepository()
        {
            //_settings = settings.Options;
            _database = Connect();
        }

        public void Add(Adress adress)
        {
            _database.GetCollection<Adress>("Adresses").InsertOne(adress);
        }

        public IEnumerable<Adress> AllAdresses()
        {
            IMongoCollection<Adress> UserDetails = _database.GetCollection<Adress>("Adresses");
            var filter = Builders<Adress>.Filter.Empty;
            var adresses = UserDetails.Find(filter).ToList();
            return adresses;
        }

        public Adress GetById(ObjectId id)
        {
            //ar query = Query<Adress>.EQ(e => e.Id, id);
            var filter = Builders<Adress>.Filter.Eq("_id", id);
            var adress = _database.GetCollection<Adress>("Adresses").Find(filter).First() as Adress;

            return adress;
        }
        public IEnumerable<Adress> GetByUserId(string id)
        {
            //ar query = Query<Adress>.EQ(e => e.Id, id);
            var filter = Builders<Adress>.Filter.Eq("idu", id);
            var adress = _database.GetCollection<Adress>("Adresses").Find(filter).ToList();

            return adress;
        }

        public bool Remove(ObjectId id)
        {
            //var query = Query<Adress>.EQ(e => e.Id, id);
            var filter = Builders<Adress>.Filter.Eq("Id", id);
            var adress = _database.GetCollection<Adress>("Adresses").DeleteOne(filter);

            return true;
        }

        public void Update(Adress adress)
        {
            //var query = Query<Adress>.EQ(e => e.Id, adress.Id);
            //var update = Update<Adress>.Replace(adress); // update modifiers
            //_database.GetCollection<Adress>("Adresses").Update(query, update);

            var collection = _database.GetCollection<Adress>("Adresses");
            var filter = Builders<Adress>.Filter.Eq(s=> s.Description, adress.Description);
            //var update = Builders<Adress>.Update.Set("Adres", adress.Adres);
            //var update = Builders<Adress>.Update.Set(s=> s.GrabbedWeb, adress.GrabbedWeb);
            //var result = collection.UpdateOneAsync(filter, update);
            var result = collection.ReplaceOneAsync(filter, adress);
        }

        private IMongoDatabase Connect()
        {
            var connectionstring = "mongodb://localhost:27017";
            var client = new MongoClient(connectionstring);



            var database = client.GetDatabase("AdressDB");

            return database;
        }
    }

}