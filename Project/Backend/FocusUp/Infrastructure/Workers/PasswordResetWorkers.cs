using FocusUp.Application.Services;
using Resend;

namespace FocusUp.Infrastructure.Workers
{
    public class PasswordResetWorkers : BackgroundService
    {
        private readonly PasswordResetQueue _passwordResetQueue;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<PasswordResetWorkers> _logger;

        public PasswordResetWorkers(PasswordResetQueue passwordResetQueue, IServiceScopeFactory serviceScopeFactory, ILogger<PasswordResetWorkers> logger)
        {
            _passwordResetQueue = passwordResetQueue;
            _scopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async System.Threading.Tasks.Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                await foreach (var email in _passwordResetQueue.ReadAllAsync(cancellationToken))
                {
                    try
                    {
                        using var scope = _scopeFactory.CreateScope();
                        var service = scope.ServiceProvider.GetRequiredService<PasswordResetService>();
                        await service.ProcessRequestAsync(email);
                    }
                    catch (ResendException exception)
                    {
                        _logger.LogError(exception,
                            "Resend hat den Versand abgelehnt oder konnte nicht erreicht werden. " +
                            "HTTP-Status: {StatusCode}, Fehlertyp: {ErrorType}.",
                            (int?)exception.StatusCode, exception.ErrorType);
                    }
                    catch (Exception exception)
                    {
                        _logger.LogError(exception,
                            "Passwort-Reset-Anfrage konnte nicht verarbeitet werden.");
                    }
                }
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {

            }
        }
    }
}