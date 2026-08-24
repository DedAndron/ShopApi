using ShopDomain.Models;
using ShopDomain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Interfaces.Repository;

public interface IAuthRepository
{
    Task<User>? RegisterUserAsync(User user, string hash);
    Task<bool> IsExistEmailAsync(string email);
    Task<User?> ChangeUserRoleAsync(string email, UserRole role);
}