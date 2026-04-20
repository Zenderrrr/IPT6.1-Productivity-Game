using FocusUp.Infrastructure.Repositories;
using FocusUp.Application.Services;
using System;
using System.IO;
using FocusUp.Domain.Models;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FocusUp.Application.Services.Auth
{
    public class AuthService
    {
        private readonly UserRepository _userRepository;
        private readonly UserStatsRepository _userStatsRepository;
        private readonly JwtTokenService _jwtTokenService;
        private readonly PasswordHasher _passwordHasher;
        private readonly UserRefreshTokenRepository _userRefreshTokenRepository;

        private readonly DatabaseConnection _databaseConnection;

        public AuthService(UserRepository userRepository, UserStatsRepository userStatsRepository, JwtTokenService jwtTokenService, PasswordHasher passwordHasher, DatabaseConnection databaseConnection, UserRefreshTokenRepository userRefreshTokenRepository)
        {
            _userRepository = userRepository;
            _userStatsRepository = userStatsRepository;
            _jwtTokenService = jwtTokenService;
            _passwordHasher = passwordHasher;
            _userRefreshTokenRepository = userRefreshTokenRepository;

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

        public UserRefreshToken? GetRefreshToken(string refreshToken)
        {
            var hashRefreshToken = HashToken(refreshToken);

            var userRefreshToken = _userRefreshTokenRepository.GetByToken(hashRefreshToken);

            if(userRefreshToken == null)
                return null;
            if (userRefreshToken.RevokedAt != null)
                return null;
            if(userRefreshToken.ExpiresAt < DateTime.UtcNow)
                return null;
            return userRefreshToken;
        }

        public (UserRefreshToken token, string plainToken) CreateRefreshToken(int userId)
        {
            var plainToken = GenerateSecureToken();
            var tokenHash = HashToken(plainToken);

            var newToken = new UserRefreshToken(
                    userId,
                    tokenHash,
                    DateTime.UtcNow.AddDays(30)
                );

            _userRefreshTokenRepository.Insert(newToken);
            return (newToken, plainToken);
        }

        public (UserRefreshToken token, string plainToken) RotateRefreshToken(string refreshToken)
        {
            var oldToken = GetRefreshToken(refreshToken) ?? throw new Exception("Token not found.");

            oldToken.SetRevokedAt(DateTime.UtcNow);
            _userRefreshTokenRepository.UpdateRevokedAt(oldToken);

            var plainToken = GenerateSecureToken();
            var tokenHash = HashToken(plainToken);

            var newToken = new UserRefreshToken(
                    oldToken.UserId,
                    tokenHash,
                    DateTime.UtcNow.AddDays(30)
                );

            _userRefreshTokenRepository.Insert(newToken);
            return (newToken, plainToken);
        }

        public void SetRevokedAtRefreshToken(UserRefreshToken userRefreshToken) => _userRefreshTokenRepository.UpdateRevokedAt(userRefreshToken);

        private string GenerateSecureToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Base64UrlEncoder.Encode(randomBytes);
        }

        private string HashToken(string token)
        {
            var bytes = Encoding.UTF8.GetBytes(token);
            var hash = SHA256.HashData(bytes);
            return Convert.ToHexString(hash);
        }
    }
}