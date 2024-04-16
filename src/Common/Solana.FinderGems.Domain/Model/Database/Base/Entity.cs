using MongoDB.Bson;

namespace Solana.FinderGems.Domain.Model.Database.Base
{
    public class Entity
    {
        public virtual ObjectId ID { get; set; } = ObjectId.GenerateNewId();
    }
}
