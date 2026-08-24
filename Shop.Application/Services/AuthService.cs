using AutoMapper;
using Shop.Application.DTOs.UserDTOs;
using Shop.Application.Interfaces.Helpers;
using Shop.Application.Interfaces.Repository;
using Shop.Application.Interfaces.Services;
using ShopDomain.Models;
using System.Collections.Generic;
using System.Data;
using System.Formats.Asn1;
using System.Text;

namespace Shop.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IAuthRepository _repository;
        private readonly IHashHelper _hashHelper;
        private readonly IJWTService _jwtService;

        public AuthService(IMapper mapper, IAuthRepository repository, IHashHelper hashHelper, IJWTService jwtService)
        {
            _mapper = mapper;
            _repository = repository;
            _hashHelper = hashHelper;
            _jwtService = jwtService;
        }

        public async Task<(UserReadDTO? User, string? Token)> RegisterAsync(UserCreateDTO dto)
        {
            var isExist = await _repository.IsExistEmailAsync(dto.Email);
            if (!isExist)
            {
                var hash = _hashHelper.Hash(dto.Password);
                var user = _mapper.Map<User>(dto);

                var token = _jwtService.GenerateAccessToken(_mapper.Map<UserLoginDTO>(user), user.Role.ToString());
                var registerUser = await _repository.RegisterUserAsync(user, hash);
                if (registerUser != null)
                    return (_mapper.Map<UserReadDTO>(registerUser), token);
            }
            return (null, null);
        }

        public async Task<UserReadDTO?> ChangeUserRoleAsync(string email, UserChangeRoleDTO dto)
        {
            var user = await _repository.ChangeUserRoleAsync(email, dto.Role);
            if (user == null)
                return null;

            return _mapper.Map<UserReadDTO>(user);
        }
    }
}