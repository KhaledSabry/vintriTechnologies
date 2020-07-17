using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace vintriTechnologies.DAL.DatabaseModel
{
    public class Vote_DatabaseModel
    {
        [JsonProperty("beerId")]
        public int beerId { get; set; } 

        [Required]
        //[DataType(DataType.EmailAddress, ErrorMessage = "username should Email address")]
        //[EmailAddress]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "username should Email address")]
        [JsonProperty("userName")]
        public string username { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rate value should be between 1 and 5")]
        [JsonProperty("rating")]
        public int rating { get; set; }

        [JsonProperty("comments")]
        public string comments { get; set; }


        [JsonProperty("voteDate")]
        public DateTime voteDate { get; set; }

        
    }
}
