using FocusUp.Infrastructure.Repositories;
using FocusUp.Application.Services;
using System;
using System.IO;

namespace FocusUp.Application.Services.Auth
{
    public class AuthService
    {
        private readonly UserRepository _userRepository;
        private readonly UserStatsRepository _userStatsRepository;
        private readonly JwtTokenService _jwtTokenService;
        private readonly PasswordHasher _passwordHasher;

        private readonly DatabaseConnection _databaseConnection;

        public AuthService(UserRepository userRepository, UserStatsRepository userStatsRepository, JwtTokenService jwtTokenService, PasswordHasher passwordHasher, DatabaseConnection databaseConnection)
        {
            _userRepository = userRepository;
            _userStatsRepository = userStatsRepository;
            _jwtTokenService = jwtTokenService;
            _passwordHasher = passwordHasher;

            _databaseConnection = databaseConnection;
        }

        public string Login(string usernameOrEmail, string password)
        {
            User? user = _userRepository.GetByEmail(usernameOrEmail) ?? _userRepository.GetByUsername(usernameOrEmail);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid credentials.");
            
            if(!_passwordHasher.PasswordVerify(password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid credentials.");

            return _jwtTokenService.CreateToken(user);
        }

        public int Register(string username, string email, string password)
        {
            if (_userRepository.ExistsByUsername(username))
                throw new InvalidDataException("Username already exists");
            if (_userRepository.ExistsByEmail(email))
                throw new InvalidDataException("Email already exists");

            var passwordHash = _passwordHasher.PasswordHashing(password);

            User user = new(username, email, passwordHash);

            using var connection = _databaseConnection.GetConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            int userId;
            try
            {
                userId = _userRepository.Insert(user, connection, transaction);

                UserStats userStats = new(userId);
                _userStatsRepository.Insert(userStats, connection, transaction);

                transaction.Commit();
                return userId;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}