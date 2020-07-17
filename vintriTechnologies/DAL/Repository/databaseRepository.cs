using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using vintriTechnologies.DAL.DatabaseModel; 
using vintriTechnologies.Helper;
using vintriTechnologies.Extension_Methods;
using Microsoft.AspNetCore.Hosting;
using System.Text.Json;

namespace vintriTechnologies.DAL.Repository
{
    public class databaseRepository
    { 
        private readonly Settings _configs; 
        public databaseRepository( Settings configs )
        { 
            this._configs = configs; 
        }
        public List<Vote_DatabaseModel> Load()
        {
            string jsonString = "";
            List<Vote_DatabaseModel> result = new List<Vote_DatabaseModel>();
            using (StreamReader sr = new StreamReader(this._configs.dbJsonPath))
            {
                jsonString = sr.ReadToEnd();
            }
            if (!string.IsNullOrEmpty(jsonString))
                result = JsonSerializer.Deserialize<List<Vote_DatabaseModel>>(jsonString);
            return result;
        }
        public void append(Vote_DatabaseModel voteData)
        {
            List<Vote_DatabaseModel> contents=this.Load();
            contents.Add(voteData); 
            string jsonString =  contents.toJson(); 
            System.IO.File.WriteAllText(this._configs.dbJsonPath, jsonString);
        }
        public void clear()
        {
            string path = this._configs.dbJsonPath;
            System.IO.File.WriteAllText(path, "");
        }
    }
}
