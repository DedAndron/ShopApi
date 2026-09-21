using Shop.Application.DTOs.UserDTOs;
using Shop.Application.DTOs.DeliveryAddressDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Interfaces.Services;

public interface IAuthService
{
    Task<(UserReadDTO? User, string? Token)> RegisterAsync(UserCreateDTO dto);
    Task<UserReadDTO?> ChangeUserRoleAsync(string email, UserChangeRoleDTO dto);
    Task<DeliveryAddressReadDTO?> AddDeliveryAddressAsync(string email, DeliveryAddressCreateDTO dto, CancellationToken cancellationToken);
}