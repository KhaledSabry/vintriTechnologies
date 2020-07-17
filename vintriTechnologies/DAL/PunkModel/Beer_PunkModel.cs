using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vintriTechnologies.DAL.PunkModel
{
    public class Beer_PunkModel
    {
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("name")]
        public string name { get; set; } 
        [JsonProperty("description")]
        public string description { get; set; } 
    }
}
