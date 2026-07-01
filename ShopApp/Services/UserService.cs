using Shop.Api.Interfaces;
using ShopDomain.Models;

namespace Shop.Api.Services
{
    public class UserService : IUserService
    {
        private readonly List<User> _users = new();

        public void RegisterUser(User user)
        {
            if (user.Id == 0)
            {
                user.Id = _users.Count == 0 ? 1 : _users.Max(u => u.Id) + 1;
            }

            _users.Add(user);
        }
    }
}
