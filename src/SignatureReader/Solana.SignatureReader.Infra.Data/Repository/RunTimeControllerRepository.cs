using Solana.FinderGems.Domain.Model.Database;
using Solana.FinderGems.Domain.Repository;
using Solana.FinderGems.Infra.Data.Repository.Base;
using Solana.SignatureReader.Infra.Data.Context;

namespace Solana.SignatureReader.Infra.Data.Repository
{
    public class RunTimeControllerRepository : Repository<RunTimeController>, IRunTimeControllerRepository
    {
        public RunTimeControllerRepository(MongoContext context) : base(context)
        {
        }
    }
}
