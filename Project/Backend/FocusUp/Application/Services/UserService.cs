using FocusUp.Application.Services.Auth;
using FocusUp.Common.Exceptions;
using FocusUp.Infrastructure.Repositories;
using System;
using System.Data.Common;

namespace FocusUp.Application.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository) => _userRepository = userRepository;

        public User? GetUserById(int id) => _userRepository.GetById(id);

        public User? GetUserByUsername(string username) => _userRepository.GetByUsername(username);

        public User? GetUserByEmail(string email) => _userRepository.GetByEmail(email);

        public void UpdateProfile(int userId, string username, string email)
        {
            User? user = _userRepository.GetById(userId) ?? throw new UserNotFoundException(userId);
            user.UpdateProfile(username, email);
            _userRepository.Update(user);
        }

        public void ChangePassword(int userId, string newPassword, DbTransaction transaction)
        {
            string newPasswordHash = new PasswordHasher().PasswordHashing(newPassword);

            _userRepository.UpdatePassword(userId, newPasswordHash, transaction);
        }

        public void DeleteUser(int userId) => _userRepository.Delete(userId);

        public bool UsernameExists(string username) => _userRepository.ExistsByUsername(username);

        public bool EmailExists(string email) => _userRepository.ExistsByEmail(email);
    }
}