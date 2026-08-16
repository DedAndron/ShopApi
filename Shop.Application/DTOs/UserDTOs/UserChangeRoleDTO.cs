using ShopDomain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.DTOs.UserDTOs;

public class UserChangeRoleDTO
{
    public UserRole Role { get; set; } = UserRole.User;
}
