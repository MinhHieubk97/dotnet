using System;
using System.Threading;
using System.Threading.Tasks;
using brechtbaekelandt.cronJob.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace brechtbaekelandt.cronJob.Jobs
{
    public class JobRunner : IHostedService, IDisposable
    {

        private readonly IServiceScope _scope;

        private readonly IMyAwesomeService _myAwesomeService;

        private Timer _timer;

        private int _executedCount = 0;


        public JobRunner(IServiceScopeFactory scopeFactory)
        {
            this._scope = scopeFactory.CreateScope();

            this._myAwesomeService = this._scope.ServiceProvider.GetService<IMyAwesomeService>();
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            this._timer = new Timer(async (state) => await this.RunJobAsync(state), null, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(10));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this._timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            this._timer?.Dispose();
            this._scope?.Dispose();
        }

        private async Task RunJobAsync(object state)
        {
            await this._myAwesomeService.ExecuteAsync(this._executedCount < int.MaxValue ? this._executedCount++ : this._executedCount = 1);
        }
    }
}
