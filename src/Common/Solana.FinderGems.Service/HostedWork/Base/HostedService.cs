using System.Timers;
using System;
using Solana.FinderGems.Domain.Service.HostedWork.Base;
using Solana.FinderGems.Domain.Repository;
using Solana.FinderGems.Domain.Model.Database;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Solana.FinderGems.Service
{
    public class HostedService<T> : BackgroundService where T : IHostWorkService

    {

        #region Readonly Variables

        private readonly IServiceProvider _serviceProvider;

        #endregion

        #region Properties
        protected System.Timers.Timer? Timer { get; set; }
        protected TimeSpan Interval { get; set; }
        protected CancellationToken CancellationToken { get; set; }
        protected T? Work { get; set; }
        
        protected IRunTimeControllerRepository RunTimeControllerRepository { get; private set; }
        protected RunTimeController? RunTimeController { get; set; }
        
        #endregion

        #region Constructors

        public HostedService(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
            this.RunTimeControllerRepository = null!;
            this.Timer = null!;
            this.Work = default;
        }

        #endregion

        #region ExecuteAsync Override

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this.CancellationToken = stoppingToken;
            //Init Service
            await TrySetPeriodicTimer();
            Timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            Timer.Enabled = true;
            Timer.AutoReset = true;
            LogMessage($"***** Init Of Service --> ({typeof(T)}) --> Start in: {Interval} *****");
            Timer?.Start();
        }

        #endregion

        #region Events of Timer Elapse

        private async void OnTimedEvent(object? sender, ElapsedEventArgs e)
        {
            TryStop();
            try
            {
                using (var scope = this._serviceProvider.CreateScope())
                {
                    this.InitScopedServices(scope);
                    await TrySetPeriodicTimer();
                    if (this.RunTimeController != null && (!this.RunTimeController!.IsRunning))
                    {
                        try
                        {
                            await SetRuntimeControllerAsync(true);
                            LogMessage($"Running --> {this.RunTimeController?.JobName}: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
                            await this.Work!.DoExecute(this.CancellationToken);
                            await SetRuntimeControllerAsync(false);
                            LogMessage($"End --> {this.RunTimeController?.JobName}: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
                            LogMessage($"Waiting for service {this.RunTimeController?.JobName} next tick in {Interval}");
                            await this.RunTimeControllerRepository.SaveChangesAsync();
                            TryStart();
                            DisposeServices();
                            if (!CancellationToken.IsCancellationRequested) return;
                        }
                        catch (Exception ex)
                        {
                            await SetRuntimeControllerAsync(false);
                            LogMessage($"SERVICE -------------> : {this.RunTimeController?.JobName}");
                            LogMessage($"Exceção: {ex.Message}");
                            LogMessage($"StackTrace: {ex.StackTrace}");
                            LogMessage($"InnerException: {ex.InnerException}");
                            LogMessage($"InnerException---> Message: {ex.InnerException?.Message}");
                            LogMessage($"InnerException--> StackTrace: {ex.InnerException?.StackTrace}");
                            LogMessage($"Waiting for service {this.RunTimeController?.JobName} tick in {Interval}");
                            TryStart();
                            DisposeServices();
                            if (!CancellationToken.IsCancellationRequested) return;
                        }
                    }
                    else
                    {
                        if (this.RunTimeController != null)
                        {
                            LogMessage($"Is Running --> {this.RunTimeController?.JobName}: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
                            await SetRuntimeControllerAsync(false);
                            LogMessage($"Recovery Service {this.RunTimeController?.JobName} ---> Waiting for next execute {Interval}");
                            TryStart();
                            DisposeServices();
                            if (!CancellationToken.IsCancellationRequested) return;
                        }
                        else
                        {
                            LogMessage($"Timer está nulo ou não configurado: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage(ex.Message);
            }
            this.LogMessage($"Ended --> {this.RunTimeController?.JobName}: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
        }

        #endregion

        #region RunTimeController Methods

        private void InitScopedServices(IServiceScope scope)
        {
            this.RunTimeControllerRepository = scope.ServiceProvider.GetRequiredService<IRunTimeControllerRepository>();
            this.Work = scope.ServiceProvider.GetRequiredService<T>();
        }

        private void DisposeServices()
        {
            try
            {
                this.RunTimeControllerRepository.Dispose();
                this.Work!.Dispose();
                this.RunTimeController = null;
            }
            finally { }
        }

        private async Task<RunTimeController?> GetRunTimeControllerAsync()
        {
            return await this.RunTimeControllerRepository!.FindFirstOrDefaultAsync(x => x.ID == this.Work!.ID);
        }

        private async Task SetRuntimeControllerAsync(bool isRunning)
        {
            RunTimeController!.IsRunning = isRunning;
            this.RunTimeControllerRepository.Update(RunTimeController!);
            await this.RunTimeControllerRepository.SaveChangesAsync();
        }

        private async Task TrySetPeriodicTimer()
        {
            if (this.RunTimeControllerRepository == null)
            {
                Interval = TimeSpan.FromMinutes((double)0.05);
                Timer = new System.Timers.Timer(Interval!);
                return;
            }
            if (RunTimeController == null)
            {
                RunTimeController = await GetRunTimeControllerAsync();
                var minutesForTimeSpan = RunTimeController?.ConfigurationTimer ?? (decimal)1.00;
                Interval = TimeSpan.FromMinutes((double)minutesForTimeSpan);
                Timer.Interval = Interval.TotalMilliseconds;
            }
        }

        #endregion

        #region Console Log

        private void LogMessage(string message)
        {
            Console.WriteLine(message);
        }

        #endregion

        #region Disposable

        public override void Dispose()
        {
            try
            {
                TryStop();
                Timer?.Dispose();
                Work?.Dispose();
            }
            finally
            {

            }
            base.Dispose();
        }

        #endregion

        #region Private Methods

        private void TryStop()
        {
            try
            {
                Timer?.Stop();
            }
            finally { }
        }

        private void TryStart()
        {
            try { if (!this.CancellationToken.IsCancellationRequested) Timer?.Start(); }
            finally { }
        }

        #endregion
    }
}
