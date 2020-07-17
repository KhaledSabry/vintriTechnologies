using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using vintriTechnologies.DAL.DatabaseModel;
using vintriTechnologies.Model;

namespace vintriTechnologies.Extension_Methods
{
    public static class JsonExtensions
    {
        public static string toJson(this Vote_DatabaseModel model)
        {
            return JsonSerializer.Serialize(model);
        }
        public static string toJson(this List<Vote_DatabaseModel> models)
        {
            return JsonSerializer.Serialize(models);
        }

        public static string toJson(this VoteSummaryModel model)
        {
            return JsonSerializer.Serialize(model);
        }
        public static string toJson(this List<VoteSummaryModel> models)
        {
            return JsonSerializer.Serialize(models);
        }

    }
}
