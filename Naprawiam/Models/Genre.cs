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

     
        [JsonIgnore]
        public List<elastic> Adresses { get; set; }
    }
}