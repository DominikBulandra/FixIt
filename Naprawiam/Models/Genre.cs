using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Naprawiam.ViewModel;
namespace Naprawiam.Models
{
    public partial class Genre
    {
        public int GenreId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        //The below attribute excludes the Albums
        //property from JSON serialization, preventing
        //circular references when serializing albums.
        [JsonIgnore]
        public List<elastic> Adresses { get; set; }
    }
}