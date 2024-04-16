// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Solana.FinderGems.Domain.Repository;
using Solana.FinderGems.Domain.Service.HostedWork;
using Solana.SignatureReader.Infra.Data.Context;
using Solana.SignatureReader.Infra.Data.Repository;
using Solana.SignatureReader.Service.HostedService;
using Solana.SignatureReader.Service.HostedWork;

var builder = Host.CreateApplicationBuilder(args);
ConfigureServices(builder.Services, builder.Configuration);

using IHost host = builder.Build();

host.Run();

static void ConfigureServices(IServiceCollection services, IConfiguration configuration) 
{
    #region Context

    services.AddDbContext<MongoContext>(options => options.UseMongoDB(configuration.GetConnectionString("MongoDB") ?? string.Empty, configuration.GetSection("Mongo:Database").Value ?? string.Empty), ServiceLifetime.Scoped);

    #endregion

    #region Hosted Service

    services.AddHostedService<SignatureReaderService>();

    #endregion

    #region Repositories

    services.AddScoped<IRunTimeControllerRepository, RunTimeControllerRepository>();

    #endregion

    /////MUDA

    #region Hosted Work

    services.AddScoped<ISignatureReaderWork, SignatureReaderWork>();

    #endregion
}