

namespace Solana.SignatureReader.Domain.Model.CrossCutting.Solana.Request
{
    public class SignatureRPCRequest
    {
        public string? WalletHash { get; set; }
        public string? LastSignature { get; set; }
        public string? TransactionId { get; set; }


    }
}
