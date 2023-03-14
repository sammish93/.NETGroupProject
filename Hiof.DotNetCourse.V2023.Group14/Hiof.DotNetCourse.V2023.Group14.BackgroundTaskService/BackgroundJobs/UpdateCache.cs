using System;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.DTO.V1;
using Hiof.DotNetCourse.V2023.Group14.UserAccountService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService.BackgroundJobs
{
    public static class UpdateCacheJob
    {
        private static MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        // Method that updates the cache with the users in the database.
        public static void Update(UserAccountContext dbContext)
        {
            // Get a list of all the users from the database.
            var users = dbContext.Users.AsNoTracking().Select(u => new V1UserDTO
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email
            })
                .ToList();

            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var serializedObject = JsonConvert.SerializeObject(users, settings);

            _cache.Set("UserData", serializedObject);
        }

        // Method to get the cached data.
        public static List<V1UserDTO>? GetCachedUsers()
        {
            List<V1UserDTO>? users = _cache.Get<List<V1UserDTO>>("UserData");

            return users;
            
        }
    }
}

