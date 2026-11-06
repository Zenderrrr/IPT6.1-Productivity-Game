using FocusUp.Application.Interfaces;
using FocusUp.Domain.Models;
using FocusUp.Infrastructure.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace FocusUp.Application.Services
{
    public class PasswordResetService
    {
        private readonly UserService _userService;
        private readonly PasswordResetTokenRepository _passwordResetTokenRepository;
        private readonly IEmailService _emailService;
        private readonly PasswordResetQueue _passwordResetQueue;

        private readonly ILogger<PasswordResetService> _logger;

        public PasswordResetService(UserService userService, PasswordResetTokenRepository passwordResetTokenRepository, IEmailService emailService, PasswordResetQueue passwordResetQueue, ILogger<PasswordResetService> logger)
        {
            _userService = userService;
            _passwordResetTokenRepository = passwordResetTokenRepository;
            _emailService = emailService;
            _passwordResetQueue = passwordResetQueue;
            _logger = logger;
        }

        public bool RequestReset(string email)
        {
            return _passwordResetQueue.TryEnqueue(email.Trim());
        }

        public async System.Threading.Tasks.Task ProcessRequestAsync(string email)
        {
            var user = _userService.GetUserByEmail(email);
            if (user == null)
            {
                _logger.LogInformation(
                    "Passwort-Reset übersprungen: Kein passendes Konto gefunden.");
                return;
            }

            string rawToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
            var now = DateTime.UtcNow;

            var reset = new PasswordResetToken
            {
                UserId = user.Id,
                ExpiresAt = now.AddMinutes(30),
                TokenHash = HashToken(rawToken),
            };

            if (_passwordResetTokenRepository.Insert(reset) == 0)
            {
                _logger.LogInformation(
                    "Passwort-Reset übersprungen: In den letzten 60 Sekunden wurde bereits ein Token erstellt.");
                return;
            }

            _logger.LogInformation(
                "Passwort-Reset-Token erstellt; E-Mail-Versand wird gestartet.");

            string link = $"https://www.focusup.ch/reset-password?token={rawToken}";

            await _emailService.SendEmailAsync(
                user.Email,
                "FocusUp Passwort zurücksetzen",
                $"Setze dein Passwort über diesen Link zurück:\n{link}\n\n" +
                "Der Link ist 30 Minuten gültig.\n" +
                "Falls du das nicht angefordert hast, kannst du diese E-Mail ignorieren.");
        }

        public bool ResetPassword(string rawToken, string newPassword)
        {
            return _passwordResetTokenRepository.TryCompleteReset(
                    HashToken(rawToken),
                    (userId, transaction) => _userService.ChangePassword(userId, newPassword, transaction)
                );
        }

        private static string HashToken(string rawToken) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(rawToken)));
    }
}