using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AntDesign.TestApp.Maui.Models;

namespace AntDesign.TestApp.Maui.Services
{
    public interface IProjectService
    {
        Task<NoticeType[]> GetProjectNoticeAsync();
        Task<ActivitiesType[]> GetActivitiesAsync();
        Task<ListItemDataType[]> GetFakeListAsync(int count = 0);
        Task<NoticeItem[]> GetNoticesAsync();
    }

    public class ProjectService : IProjectService
    {
        private readonly JsonFileReader _jsonReader;

        public ProjectService(JsonFileReader jsonReader)
        {
            _jsonReader = jsonReader;
        }

        public async Task<NoticeType[]> GetProjectNoticeAsync()
        {
            return await _jsonReader.ReadJsonAsync<NoticeType[]>("data/notice.json");
        }

        public async Task<NoticeItem[]> GetNoticesAsync()
        {
            return await _jsonReader.ReadJsonAsync<NoticeItem[]>("data/notices.json");
        }

        public async Task<ActivitiesType[]> GetActivitiesAsync()
        {
            return await _jsonReader.ReadJsonAsync<ActivitiesType[]>("data/activities.json");
        }

        public async Task<ListItemDataType[]> GetFakeListAsync(int count = 0)
        {
            var data = await _jsonReader.ReadJsonAsync<ListItemDataType[]>("data/fake_list.json");
            return count > 0 ? data.Take(count).ToArray() : data;
        }
    }
}
