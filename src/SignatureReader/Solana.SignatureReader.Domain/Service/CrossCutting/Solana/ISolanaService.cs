using Solana.SignatureReader.Domain.Model.CrossCutting.Solana.Request;
using Solana.SignatureReader.Domain.Model.CrossCutting.Solana.Response;

namespace Solana.SignatureReader.Domain.Service.CrossCutting.Solana
{
    public interface ISolanaService
    {
        Task<List<SignatureRPCResponse>?> ExecuteRecoverySignaturesAsync(SignatureRPCRequest request);
    }
}
