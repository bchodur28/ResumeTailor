using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ResumeTailor.Application.Common.Interfaces;

namespace ResumeTailor.Infrastructure.BackgroundServices
{
    public class CreditReservationRecoveryWorker(IServiceScopeFactory scopeFactory, ILogger<CreditReservationRecoveryWorker> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    await using var scope =
                        scopeFactory.CreateAsyncScope();

                    var recoveryService = scope.ServiceProvider
                        .GetRequiredService<ICreditReservationRecoveryService>();

                    await recoveryService.RecoverAsync(stoppingToken);
                } catch (OperationCanceledException)
                      when (stoppingToken.IsCancellationRequested)
                {
                    break;
                } catch (Exception ex)
                {
                    logger.LogError(
                        ex,
                        "Credit reservation recovery failed.");
                }
            }
        }
    }
}
