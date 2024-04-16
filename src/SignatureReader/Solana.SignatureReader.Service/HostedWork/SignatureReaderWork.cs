using MongoDB.Bson;
using Solana.FinderGems.Domain.Service.HostedWork;

namespace Solana.SignatureReader.Service.HostedWork
{
    public class SignatureReaderWork : ISignatureReaderWork
    {
        public ObjectId ID => new ObjectId("661dc9efeb573b03881635d5");

        public SignatureReaderWork()
        {
            
        }

        public async Task DoExecute(CancellationToken cancellationToken)
        {
            //
            Console.WriteLine("HELLO WORLD");
        }
        public void Dispose()
        {
            //
            try 
            {
                
            }
            catch{ }
            GC.SuppressFinalize(this);
        }
    }
}
