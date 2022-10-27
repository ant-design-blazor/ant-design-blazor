using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AntDesign.TestApp.Maui.Models;

namespace AntDesign.TestApp.Maui.Services
{
    public interface IProfileService
    {
        Task<BasicProfileDataType> GetBasicAsync();
        Task<AdvancedProfileData> GetAdvancedAsync();
    }

    public class ProfileService : IProfileService
    {
        private readonly JsonFileReader _jsonReader;

        public ProfileService(JsonFileReader jsonReader)
        {
            _jsonReader = jsonReader;
        }

        public async Task<BasicProfileDataType> GetBasicAsync()
        {
            return await _jsonReader.ReadJsonAsync<BasicProfileDataType>("data/basic.json");
        }

        public async Task<AdvancedProfileData> GetAdvancedAsync()
        {
            return await _jsonReader.ReadJsonAsync<AdvancedProfileData>("data/advanced.json");
        }
    }
}
