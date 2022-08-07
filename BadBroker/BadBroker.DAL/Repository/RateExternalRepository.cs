using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BadBroker.DAL.Context;
using BadBroker.DAL.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BadBroker.DAL.Repository
{
    public interface IRateExternalRepository
    {
        ExternalRate GetRates(string baseCurrencyKey, DateTime date);
    }

    public class RateExternalRepository : IRateExternalRepository
    {
        const string appId = "3f58d914a6704b2aa9d8516ba94d91a9";
        readonly string _urlTemplate = @"https://openexchangerates.org/api/historical/{0}.json?app_id={1}&base={2}";
        static readonly HttpClient client = new HttpClient();

        public RateExternalRepository()
        {
            
        }

        public  ExternalRate GetRates(string baseCurrencyKey, DateTime date)
        {
            var url = string.Format(_urlTemplate, date.ToString("yyyy-MM-dd"), appId, baseCurrencyKey);

            string responseBody = client.GetStringAsync(url).Result;

            var instance = JsonConvert.DeserializeObject<ExternalRate>(responseBody);

            return instance;
        }
    }
}
