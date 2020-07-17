using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using vintriTechnologies.BAL.Mapper;
using vintriTechnologies.DAL.Repository;
using vintriTechnologies.Helper;
using vintriTechnologies.Model;

namespace vintriTechnologies.BAL
{
    public class BeerSystem
    { 
        private readonly IHttpClientFactory _clientFactory;
        private readonly Settings _configs; 
        public BeerSystem(  Settings configs, IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _configs = configs; 
        }

        public async Task<bool> Rate(int beerId, VoteModel voteData)
        {
            var punkManagment = new PunkRepository(_configs, _clientFactory);
            var databaseManagment = new databaseRepository(_configs);

            bool isExists = await punkManagment.isExists(beerId);
            if (isExists)
            {
                var vote = VoteMapper.toDatabaseModel(DateTime.Now, beerId, voteData);
                databaseManagment.append(vote);
                return true;
            }
            else
            {
               // throw new System.ArgumentException("beerId not found.", "beerId");
                return false;
            }
        }

        public async Task<List<VoteSummaryModel>> Search(string Keyword)
        {
            var punkManagment = new PunkRepository(_configs, _clientFactory);
            var databaseManagment = new databaseRepository(_configs);

            List<DAL.PunkModel.Beer_PunkModel> searchResult = await punkManagment.Search(Keyword); 
            List<DAL.DatabaseModel.Vote_DatabaseModel> votes = databaseManagment.Load();

            var result = VoteMapper.toSummary(searchResult, votes); 
            return result ;

        }
        public void Clear()
        { 
            var databaseManagment = new databaseRepository(_configs); 
            databaseManagment.clear();
             

        }
    }
}
