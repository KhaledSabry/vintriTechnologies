using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.Swagger.Annotations;
using vintriTechnologies.BAL;
using vintriTechnologies.Filters;
using vintriTechnologies.Helper;
using vintriTechnologies.Model;
using vintriTechnologies.Extension_Methods;

namespace vintriTechnologies.Controllers
{ 
    [Route("[controller]")]
    public class BeerController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly Settings _configs; 
        public BeerController( Settings configs,IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _configs = configs; 
        }

        /// <summary>
        ///to clear the Json file - just for testing purpuse 
        /// </summary> 
        [HttpGet("clear/database")] // to clear the Json file - just for testing purpuse  
        public ActionResult clearJsonData()
        {
            var beerSystem = new BeerSystem(_configs, _clientFactory);
            beerSystem.Clear();
            return Ok();
        }


        /// <summary>
        /// Task 1: REST API endpoint to allow a user to add a rating to a beer.
        /// </summary> 
        /// <remarks>
        /// sample
        /// beerId:3
        /// voteData: {"username":"username","rating":4,"comments":"some comments"}
        /// </remarks> 
        /// <param name="beerId">beer Id  </param>
        /// <param name="voteData">voteData: json with properties: username, rating, comments.  </param>
        /// <response code="200">Successful operation</response>
        /// <response code="400">Invalid voteData supplied</response>
        /// <response code="404">beerId not found</response> 
        [Description("Task 1: REST API endpoint to allow a user to add a rating to a beer.")]
        [SwaggerResponse(HttpStatusCode.OK, Description = "Successfull operation")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Invalid voteData supplied")]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "beerId not found")]
        //[ModelStateValidationFilter] I have registered this filter to be applied on each action - no need to add it as decoration 
        [HttpPost("rate/{beerId}")]
        public async Task<ActionResult> Rate( int beerId,[FromBody] VoteModel voteData)
        { 
            try
            {
                var beerSystem = new BeerSystem( _configs, _clientFactory);

                var result = await beerSystem.Rate(beerId, voteData);
                if (result)
                    return Ok();
                else
                    return StatusCode((int)HttpStatusCode.NotFound, new ValidationResultModel("beerId", "beerId not found.")); 
            }
            catch (Exception e1)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ValidationResultModel(e1));
            }

        }

        [HttpGet("search/{keyword}")]
        /// <summary>
        /// Task 2: Add a REST API endpoint to retrieve a list of beers
        /// </summary> 
        [Description("Task 2: Add a REST API endpoint to retrieve a list of beers")]
        /// <remarks>
        /// sample
        /// keyword:Trashy 
        /// </remarks> 
        /// <param name="keyword">keyword </param> 
        /// <response code="200">Successful operation</response> 
        /// <response code="404">Anomaly not found</response>  
        public async Task<ActionResult> search(string keyword)
        {
            try
            {
                keyword = System.Web.HttpUtility.UrlDecode(keyword);
                var beerSystem = new BeerSystem(_configs, _clientFactory);
                var bearVotes = await beerSystem.Search(keyword);
                if (bearVotes != null && bearVotes.Count > 0)
                { 
                    // return Ok(bearVotes.toJson()); // use this if you want to return the Json response as text to be exactly like the provided example  
                    return Ok(new JsonResult(bearVotes));
                }
                else
                    return StatusCode((int)HttpStatusCode.NotFound, new ValidationResultModel("No data found", "No data found."));
            }
            catch (Exception e1)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ValidationResultModel(e1));
            }

        }
    }
}