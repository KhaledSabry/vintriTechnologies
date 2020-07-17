using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; 

namespace vintriTechnologies.Helper
{
    public class ValidationResultModel
    {
        public string Message { get; }
        public string ErrorsSummary { get; }

        public List<ValidationError> Errors { get; }

        public ValidationResultModel( ModelStateDictionary modelState)
        {
            this.Message = "Validation Failed";
            this.Errors = modelState.Keys
                    .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
                    .ToList();
            this.ErrorsSummary = string.Join(",",
                modelState.Keys
                    .SelectMany(key => modelState[key].Errors.Select(x =>  x.ErrorMessage ))
                    .ToList()).Trim(',');
        }
        public ValidationResultModel(Exception e)
        {
            this.Message = e.Message;
            this.Errors = null;
            this.ErrorsSummary = e.Message;
        }
        public ValidationResultModel(string Message,string ErrorsSummary)
        {
            this.Message = Message;
            this.Errors = null;
            this.ErrorsSummary = ErrorsSummary;
        }
    }
}
