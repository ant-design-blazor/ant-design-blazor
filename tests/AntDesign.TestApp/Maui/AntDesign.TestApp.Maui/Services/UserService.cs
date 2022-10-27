using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AntDesign.TestApp.Maui.Models;

namespace AntDesign.TestApp.Maui.Services
{
    public interface IUserService
    {
        Task<CurrentUser> GetCurrentUserAsync();
    }

    public class UserService : IUserService
    {
        private readonly JsonFileReader _jsonReader;

        public UserService(JsonFileReader jsonReader)
        {
            _jsonReader = jsonReader;
        }

        public async Task<CurrentUser> GetCurrentUserAsync()
        {
            return await _jsonReader.ReadJsonAsync<CurrentUser>("data/current_user.json");
        }
    }
}
