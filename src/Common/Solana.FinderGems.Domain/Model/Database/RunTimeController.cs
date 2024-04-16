using Solana.FinderGems.Domain.Model.Database.Base;

namespace Solana.FinderGems.Domain.Model.Database
{
    public class RunTimeController : Entity
    {
        public string JobName { get; set; } = string.Empty;
        public decimal ConfigurationTimer { get; set; }
        public bool IsRunning { get; set; }
        public DateTime LastExecuteDate { get; set; }
    }
}
