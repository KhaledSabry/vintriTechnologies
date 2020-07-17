using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vintriTechnologies.Model
{
    public class VoteSummaryModel
    {
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("description")]
        public string description { get; set; }

        [JsonProperty("userRatings")]
        public List<VoteModel> userRatings { get; set; }
    }
}
