using Solana.FinderGems.Domain.Service.HostedWork;
using Solana.FinderGems.Service;

namespace Solana.SignatureReader.Service.HostedService
{
    public class SignatureReaderService : HostedService<ISignatureReaderWork>
    {
        public SignatureReaderService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
