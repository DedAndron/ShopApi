using Shop.Application.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Interfaces.Services;

public interface IJWTService
{
    public string GenerateAccessToken(UserLoginDTO userLoginDto, string role);
    public string GenerateAccessToken(string email, string role);
    public (string Token, int ExpiresIn) GenerateRefreshToken();
}