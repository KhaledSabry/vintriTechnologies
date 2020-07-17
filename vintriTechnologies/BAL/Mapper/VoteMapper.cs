using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vintriTechnologies.DAL.DatabaseModel; 
using vintriTechnologies.Model;

namespace vintriTechnologies.BAL.Mapper
{
    public class VoteMapper
    {
        public static VoteModel toModel(Vote_DatabaseModel entity)
        {
            VoteModel model = new VoteModel();
            if (entity != null)
            {
                model.username = entity.username;
                model.rating = entity.rating;
                model.comments = entity.comments;
            }
            return model;
        }
        public static Vote_DatabaseModel toDatabaseModel(DateTime voteDate, int beerId, VoteModel model)
        {
            Vote_DatabaseModel entity = new Vote_DatabaseModel();
            if (model != null)
            {
                entity.username = model.username;
                entity.rating = model.rating;
                entity.comments = model.comments;

                entity.voteDate = voteDate;
                entity.beerId = beerId;

            }
            return entity;
        }
        public static List<VoteSummaryModel> toSummary(List<DAL.PunkModel.Beer_PunkModel> beersList, List<DAL.DatabaseModel.Vote_DatabaseModel> votes)
        {
            var result = (
            from sr in beersList
            join v in votes on sr.id equals v.beerId into srv
            from sr_v in srv.DefaultIfEmpty()
            group sr_v by new
            {
                id = sr.id,
                name = sr.name,
                description = sr.description
            } into grouped
            orderby grouped.Key.id
            select new VoteSummaryModel()
            {
                id = grouped.Key.id,
                name = grouped.Key.name,
                description = grouped.Key.description,
                userRatings = grouped.Where(a=>a!=null).Select(a => new VoteModel()
                {
                    username = a.username,
                    rating = a.rating,
                    comments = a.comments
                }).ToList()
            }).ToList();

            return result;
        }
    }
}
