// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Solana.SignatureReader.Service.HostedService;

var builder = Host.CreateApplicationBuilder(args);
ConfigureServices(builder.Services, builder.Configuration);

using IHost host = builder.Build();

host.Run();

static void ConfigureServices(IServiceCollection services, IConfiguration configuration) 
{
    #region Hosted Service
    services.AddHostedService<SignatureReaderService>();
    #endregion
}