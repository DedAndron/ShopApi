using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ShopDomain.Enum;
using ShopDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(ShopDbContext db)
    {
        var adminExists = await db.Users
            .AnyAsync(u => u.Role == UserRole.Admin);

        if (!adminExists)
        {
            var admin = new User
            {
                Email = "admin@example.com",
                Role = UserRole.Admin
            };

            db.Users.Add(admin);
            await db.SaveChangesAsync();
        }
    }
}
