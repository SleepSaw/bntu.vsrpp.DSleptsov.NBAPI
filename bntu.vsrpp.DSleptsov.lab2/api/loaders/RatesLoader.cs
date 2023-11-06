using bntu.vsrpp.DSleptsov.lab2.api.exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static bntu.vsrpp.DSleptsov.lab2.api.Rates;

namespace bntu.vsrpp.DSleptsov.lab2.api.loaders
{
    public static class RatesLoader
    {
        public static List<Rate> RATES;
        public static List<RateShort> RATES_SHORT;

        public static async Task loadRate(DateTime? date = null)
        {
            await GetExchangeRate(date);
        }

        public static async Task loadRateShort(int currencyId, DateTime startDate, DateTime endDate)
        {
            await GetExchangeRateDynamics(currencyId, startDate, endDate);
        }

        private static async Task GetExchangeRate(DateTime? date = null)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "https://api.nbrb.by/exrates/rates?periodicity=0";
                if (date.HasValue)
                {
                    url += $"&ondate={date.Value.ToString("yyyy-MM-dd")}";
                }

                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Rate[] rates = JsonConvert.DeserializeObject<Rate[]>(content);
                    RATES = rates.ToList();
                    RATES.Sort((r1, r2) => r1.Cur_Abbreviation.CompareTo(r2.Cur_Abbreviation));
                }
                else
                {
                    HttpStatusCode statusCode = response.StatusCode;
                    string reasonPhrase = response.ReasonPhrase;
                    throw new RatesLoadException($"Cannot load rates. Status code: {statusCode}, Reason: {reasonPhrase}");
                }
            }
        }

        private static async Task GetExchangeRateDynamics(int currencyId, DateTime startDate, DateTime endDate)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"https://api.nbrb.by/exrates/rates/dynamics/{currencyId}?startdate={startDate.ToString("yyyy-MM-dd")}&enddate={endDate.ToString("yyyy-MM-dd")}";
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    RateShort[] rateDynamics = Newtonsoft.Json.JsonConvert.DeserializeObject<RateShort[]>(content);
                    RATES_SHORT = rateDynamics.ToList();
                }
                else
                {
                    throw new RatesLoadException("Connot load rates short: " + response.StatusCode);
                }
            }
        }
    }
}
