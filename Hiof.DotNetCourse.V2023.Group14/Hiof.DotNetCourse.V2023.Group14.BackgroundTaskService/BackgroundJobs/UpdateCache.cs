using System;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.DTO.V1;
using Hiof.DotNetCourse.V2023.Group14.UserAccountService.Data;
using Microsoft.Extensions.Caching.Memory;

namespace Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService.BackgroundJobs
{
    public static class UpdateCacheJob
    {
        private static MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        // Method that updates the cache with the users in the database.
        public static void Update(UserAccountContext dbContext)
        {
            
            // Get a list of all the users from the database.
            List<V1UserDTO> users = dbContext.UserDTO.ToList();

            // Add the data to the cache.
            _cache.Set("UserDtoData", users);
        }

        // Method to get the cached data.
        public static List<V1UserDTO>? GetCachedUsers()
        {
            List<V1UserDTO>? users = _cache.Get<List<V1UserDTO>>("UserData");

            return users;
            
        }
    }

}

