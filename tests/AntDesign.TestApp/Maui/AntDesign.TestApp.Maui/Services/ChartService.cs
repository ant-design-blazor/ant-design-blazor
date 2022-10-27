using AntDesign.TestApp.Maui.Models;

namespace AntDesign.TestApp.Maui.Services
{
    public interface IChartService
    {
        Task<ChartDataItem[]> GetVisitDataAsync();
        Task<ChartDataItem[]> GetVisitData2Async();
        Task<ChartDataItem[]> GetSalesDataAsync();
        Task<RadarDataItem[]> GetRadarDataAsync();
    }

    public class ChartService : IChartService
    {
        private readonly JsonFileReader _jsonReader;

        public ChartService(JsonFileReader jsonReader)
        {
            _jsonReader = jsonReader;
        }

        public async Task<ChartDataItem[]> GetVisitDataAsync()
        {
            return (await GetChartDataAsync()).VisitData;
        }

        public async Task<ChartDataItem[]> GetVisitData2Async()
        {
            return (await GetChartDataAsync()).VisitData2;
        }

        public async Task<ChartDataItem[]> GetSalesDataAsync()
        {
            return (await GetChartDataAsync()).SalesData;
        }

        public async Task<RadarDataItem[]> GetRadarDataAsync()
        {
            return (await GetChartDataAsync()).RadarData;
        }

        private async Task<ChartData> GetChartDataAsync()
        {
            return await _jsonReader.ReadJsonAsync<ChartData>("data/fake_chart_data.json");
        }
    }
}
