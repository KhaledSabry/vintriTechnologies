using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks; 
using vintriTechnologies.DAL.PunkModel;
using vintriTechnologies.Helper; 
namespace vintriTechnologies.DAL.Repository
{
    public class PunkRepository
    {   
        private readonly IHttpClientFactory _clientFactory;
        private readonly Settings _configs;
        public PunkRepository(Settings configs, IHttpClientFactory clientFactory)
        {
            this._clientFactory = clientFactory;
            this._configs = configs;
        }

        public async Task<bool> isExists(int beerId)
        {
            var beer = await this.GetBeer(beerId);
            return beer != null; 
        }
        public  async Task<Beer_PunkModel> GetBeer(int beerId)
        {
            string url = string.Format("{0}/{1}/{2}", this._configs.punkapiBaseUrl.Trim('/'), "beers", beerId); 
            Beer_PunkModel result = new Beer_PunkModel(); 

            var request = new RequestGenerator(  _clientFactory );

            var response =  await request.Get(url);


            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    string jsonString = await response.Content.ReadAsStringAsync();
                    result = JsonSerializer.Deserialize<List<Beer_PunkModel>>(jsonString).FirstOrDefault(); 
                    break;
                case System.Net.HttpStatusCode.NotFound:
                    result = null;
                    break;
                default:
                    result = null;
                    throw new System. Exception("Unknow Error in punk api." ); 
            }

             
            return result;
        }

        public async Task<List<Beer_PunkModel>> Search(string keyword)
        {
            string url = string.Format("{0}/{1}{2}", this._configs.punkapiBaseUrl.Trim('/'), "beers?beer_name=", keyword);
            List<Beer_PunkModel> result = new List<Beer_PunkModel>();

            var request = new RequestGenerator(_clientFactory);

            var response = await request.Get(url);


            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    string jsonString = await response.Content.ReadAsStringAsync();
                    result = JsonSerializer.Deserialize<List<Beer_PunkModel>>(jsonString);
                    break;
                case System.Net.HttpStatusCode.NotFound:
                    result = null;
                    break;

                default:
                    result = null;
                    throw new System.Exception("Unknow Error in punk api.");
            }
             
            return result;
        }
    }
}
