using FriendsApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FriendsApi.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext context)
        {
            if (await context.Users.AnyAsync()) return;

            var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
            var users =JsonSerializer.Deserialize<List<AppUser>> (userData);
            foreach (var user in users)
            {
                using var hmac = new HMACSHA512();
                
                user.userName = user.userName.ToLower();
                user.passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
                user.passwordSalt = hmac.Key;


                context.Users.Add(user);
            }
            await context.SaveChangesAsync();
        }
    }
}
