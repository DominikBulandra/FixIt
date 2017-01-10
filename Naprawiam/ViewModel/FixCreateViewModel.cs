using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Bson.Serialization.Attributes;

namespace Naprawiam.ViewModel
{
    public class FixCreateViewModel
    {

        public ObjectId Id { get; set; }
        public string idu { get; set; }
        public string StringObjectId { get; set; }
        [Required]
        [MinLength(7, ErrorMessage = "zły adres")]
        [Display(Name = "Adres strony z instrukcją")]
        public string Adres { get; set; }
        [Display(Name = "Opis")]
        public string Description { get; set; }

        
    }
}