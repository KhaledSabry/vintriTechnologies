using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace vintriTechnologies.Model
{
    public class VoteModel
    {

        [Required] 
        //[DataType(DataType.EmailAddress, ErrorMessage = "username should Email address")]
        //[EmailAddress]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "username should Email address")]
        public string username { get; set; }
        [Required]
        [Range(1, 5, ErrorMessage = "Rate value should be between 1 and 5")]
        public int rating { get; set; }
        public string comments { get; set; }
    }
}
