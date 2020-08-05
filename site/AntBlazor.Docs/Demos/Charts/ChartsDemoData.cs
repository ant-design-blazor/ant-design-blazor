using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Charts
{
    public static class ChartsDemoData
    {
        public static async Task<FireworksSalesItem[]> FireworksSalesAsync(NavigationManager navigationManager,
            HttpClient httpClient)
        {
            var baseUrl = navigationManager.ToAbsoluteUri(navigationManager.BaseUri);
            return await httpClient.GetFromJsonAsync<FireworksSalesItem[]>(
                new Uri(baseUrl, "_content/AntDesign.Docs/chartsdata/fireworks-sales.json").ToString());
        }

        public static async Task<SalesItem[]> SalesAsync(NavigationManager navigationManager, HttpClient httpClient)
        {
            var baseUrl = navigationManager.ToAbsoluteUri(navigationManager.BaseUri);
            return await httpClient.GetFromJsonAsync<SalesItem[]>(
                new Uri(baseUrl, "_content/AntDesign.Docs/chartsdata/sales.json").ToString());
        }

        public static async Task<GDPItem[]> GDPAsync(NavigationManager navigationManager, HttpClient httpClient)
        {
            var baseUrl = navigationManager.ToAbsoluteUri(navigationManager.BaseUri);
            return await httpClient.GetFromJsonAsync<GDPItem[]>(
                new Uri(baseUrl, "_content/AntDesign.Docs/chartsdata/GDP.json").ToString());
        }

        public static async Task<EmissionsItem[]> EmissionsAsync(NavigationManager navigationManager,
            HttpClient httpClient)
        {
            var baseUrl = navigationManager.ToAbsoluteUri(navigationManager.BaseUri);
            return await httpClient.GetFromJsonAsync<EmissionsItem[]>(
                new Uri(baseUrl, "_content/AntDesign.Docs/chartsdata/emissions.json").ToString());
        }

        public static async Task<OilItem[]> OilAsync(NavigationManager navigationManager, HttpClient httpClient)
        {
            var baseUrl = navigationManager.ToAbsoluteUri(navigationManager.BaseUri);
            return await httpClient.GetFromJsonAsync<OilItem[]>(
                new Uri(baseUrl, "_content/AntDesign.Docs/chartsdata/oil.json").ToString());
        }

        public static async Task<IMDBItem[]> IMDBAsync(NavigationManager navigationManager, HttpClient httpClient)
        {
            var baseUrl = navigationManager.ToAbsoluteUri(navigationManager.BaseUri);
            return await httpClient.GetFromJsonAsync<IMDBItem[]>(
                new Uri(baseUrl, "_content/AntDesign.Docs/chartsdata/IMDB.json").ToString());
        }

        public static async Task<SmokingRateItem[]> SmokingRateAsync(NavigationManager navigationManager,
            HttpClient httpClient)
        {
            var baseUrl = navigationManager.ToAbsoluteUri(navigationManager.BaseUri);
            return await httpClient.GetFromJsonAsync<SmokingRateItem[]>(
                new Uri(baseUrl, "_content/AntDesign.Docs/chartsdata/smoking-rate.json").ToString());
        }

        public static async Task<BasementProdItem[]> BasementProdAsync(NavigationManager navigationManager,
            HttpClient httpClient)
        {
            var baseUrl = navigationManager.ToAbsoluteUri(navigationManager.BaseUri);
            return await httpClient.GetFromJsonAsync<BasementProdItem[]>(
                "https://gw.alipayobjects.com/os/basement_prod/a719cd4e-bd40-4878-a4b4-df8a6b531dfe.json");
        }

        public static async Task<JobpayingItem[]> JobpayingItemAsync(NavigationManager navigationManager,
            HttpClient httpClient)
        {
            var baseUrl = navigationManager.ToAbsoluteUri(navigationManager.BaseUri);
            return await httpClient.GetFromJsonAsync<JobpayingItem[]>(
                new Uri(baseUrl, "_content/AntDesign.Docs/chartsdata/jobpaying.json").ToString());
        }

        public static async Task<ContributionsItem[]> ContributionsItemAsync(NavigationManager navigationManager,
            HttpClient httpClient)
        {
            var baseUrl = navigationManager.ToAbsoluteUri(navigationManager.BaseUri);
            return await httpClient.GetFromJsonAsync<ContributionsItem[]>(
                new Uri(baseUrl, "_content/AntDesign.Docs/chartsdata/contributions.json").ToString());
        }

        public static async Task<FertilityItem[]> FertilityItemAsync(NavigationManager navigationManager,
            HttpClient httpClient)
        {
            return await httpClient.GetFromJsonAsync<FertilityItem[]>(
                "https://gw.alipayobjects.com/os/antvdemo/assets/data/fertility.json");
        }
    }

#pragma warning disable IDE1006 // 命名样式

    public class FireworksSalesItem
    {
        [JsonPropertyName("Date")] public string Date { get; set; }

        public int scales { get; set; }
    }

    public class SalesItem
    {
        public string 城市 { get; set; }
        public decimal 销售额 { get; set; }
    }

    public class GDPItem
    {
        public string name { get; set; }
        public string year { get; set; }
        public decimal gdp { get; set; }
    }

    public class EmissionsItem
    {
        public string year { get; set; }
        public int value { get; set; }
        public string category { get; set; }
    }

    public class OilItem
    {
        public string country { get; set; }
        public int date { get; set; }
        public decimal value { get; set; }
    }

    public class IMDBItem
    {
        public object Title { get; set; }
        public object Genre { get; set; }
        public object Revenue { get; set; }
        public object Rating { get; set; }
    }

    public class SmokingRateItem
    {
        public string iso3 { get; set; }

        [JsonPropertyName("change in female rate")]
        public object change_in_female_rate { get; set; }

        [JsonPropertyName("change in male rate")]
        public object change_in_male_rate { get; set; }

        public object female_2000 { get; set; }
        public object male_2000 { get; set; }
        public object female_2015 { get; set; }
        public object male_2015 { get; set; }
        public string income { get; set; }
        public int pop { get; set; }
        public string EN { get; set; }
        public string DE { get; set; }
        public string FR { get; set; }
        public string IT { get; set; }
        public string ES { get; set; }
        public string PT { get; set; }
        public string RU { get; set; }
        public string ZH { get; set; }
        public string JA { get; set; }
        public string AR { get; set; }
        public string continent { get; set; }
    }

    public class BasementProdItem
    {
        [JsonPropertyName("Month of Year")] public object MonthofYear { get; set; }

        public object Month_of_Year => MonthofYear;
        public object District { get; set; }
        public object AQHI { get; set; }
    }

    public class JobpayingItem
    {
        public object rank { get; set; }
        public object code { get; set; }
        public object prob { get; set; }
        public object average_annual_wage { get; set; }
        public object education { get; set; }
        public object occupation { get; set; }
        public object short_occupation { get; set; }
        public object len { get; set; }
        public object probability { get; set; }
        public object numbEmployed { get; set; }
        public object median_ann_wage { get; set; }
        public object employed_may2016 { get; set; }
        public object average_ann_wage { get; set; }
    }

    public class ContributionsItem
    {
        public object date { get; set; }
        public object commits { get; set; }
        public object month { get; set; }
        public object day { get; set; }
        public object week { get; set; }
    }

    public class FertilityItem
    {
        public object country { get; set; }
        public object year { get; set; }
        public object value { get; set; }
    }

#pragma warning restore IDE1006 // 命名样式
}
