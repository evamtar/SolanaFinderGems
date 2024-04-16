

using Solana.FinderGems.Domain.Utils;

namespace Solana.SignatureReader.Domain.Model.CrossCutting.Solana.Response
{
    public class SignatureRPCResponse
    {
        public string? WalletHash { get; set; }
        public string? Signature { get; set; }
        public long? BlockTime { get; set; }
        public object? Err { get; set; }
        public string? ConfirmationStatus { get; set; }

        public DateTime? DateOfTransaction
        {
            get
            {
                if (this.BlockTime.HasValue)
                    return DateTimeTicks.Instance.ConvertTicksToDateTime(this.BlockTime.Value);
                return null;
            }
        }
    }
}
