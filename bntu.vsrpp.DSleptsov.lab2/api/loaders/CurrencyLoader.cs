using bntu.vsrpp.DSleptsov.lab2.api.exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace bntu.vsrpp.DSleptsov.lab2.api.loaders
{
    public static class CurrencyLoader
    {
        public static List<Currency> CURRENCIES;

        public static async Task loadCurrency()
        {
            await GetCurrencies();
        }

        private static async Task GetCurrencies()
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "https://api.nbrb.by/exrates/currencies";
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Currency[] currencies = Newtonsoft.Json.JsonConvert.DeserializeObject<Currency[]>(content);
                    CURRENCIES = currencies.ToList();
                    CURRENCIES.Sort((c1, c2) => c1.Cur_Abbreviation.CompareTo(c2.Cur_Abbreviation));
                }
                else
                {
                    HttpStatusCode statusCode = response.StatusCode;
                    string reasonPhrase = response.ReasonPhrase;
                    throw new CurrencyLoadException($"Cannot load rates. Status code: {statusCode}, Reason: {reasonPhrase}");
                }
            }
        }
    }
}
