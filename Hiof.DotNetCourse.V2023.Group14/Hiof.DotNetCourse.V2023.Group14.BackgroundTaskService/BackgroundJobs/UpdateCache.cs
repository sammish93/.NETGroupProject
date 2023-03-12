using System;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.UserAccountService.Data;
using Microsoft.Extensions.Caching.Memory;

namespace Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService.BackgroundJobs
{
    public static class UpdateCacheJob
    {
        // Method that updates the cache with the users in the database.
        public static void Update(UserAccountContext dbContext)
        {
            // Get a list of all the users from the database.
            List<V1User> users = dbContext.Users.ToList();

            var cache = new MemoryCache(new MemoryCacheOptions());

            // Add the data to the cache.
            cache.Set("UserData", users);
        }

        // Method to get the cached data.
        public static List<V1User>? GetCachedUsers()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            List<V1User>? users = cache.Get<List<V1User>>("UserData");

            return users;
        }
    }

}

