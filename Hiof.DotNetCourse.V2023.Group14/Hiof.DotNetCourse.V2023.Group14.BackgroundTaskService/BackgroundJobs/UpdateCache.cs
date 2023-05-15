using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService.BackgroundJobs
{
    // Need to run BackgroundTaskService and UserAccountService on the same time
    // in order to start this job.
    public class UpdateCacheJob
    {
        private readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
        private readonly HttpClient _httpClient;

        public UpdateCacheJob(HttpClient client)
        {
            _httpClient = client;
        }

        // Method that updates the cache with the users in the database.
        public async Task Update()
        {
            var response = await _httpClient.GetAsync("https://localhost:7021/api/1.0/users/getUsers");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to get users from the API");
            }

            var content = await response.Content.ReadAsStringAsync();
            _cache.Set("UserData", content);
    
        }

        // Method to get the cached data.
        public List<V1User>? GetCachedUsers()
        {
            var cachedData = _cache.Get("UserData");
            if (cachedData == null)
            {
                throw new Exception("No users stored in the cache!");
            }

            var serializedUsers = cachedData.ToString();
            var users = JsonConvert.DeserializeObject<List<V1User>>(serializedUsers);
            return users;
        }
    }
}

