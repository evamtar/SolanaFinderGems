

using MongoDB.Bson;

namespace Solana.FinderGems.Domain.Service.HostedWork.Base
{
    public interface IHostWorkService : IDisposable
    {
        ObjectId ID { get; }
        Task DoExecute(CancellationToken cancellationToken);
    }
}
