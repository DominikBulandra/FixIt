using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Naprawiam.Models
{
    public class Adress
    {
        public ObjectId Id { get; set; }
        public string idu { get; set; }

      
        public string Adres { get; set; }
        
        public string Description { get; set; }
        public string GrabbedWeb { get; set; }
    }
}