using Microsoft.EntityFrameworkCore;
using Solana.SignatureReader.Infra.Data.Mapper;

namespace Solana.SignatureReader.Infra.Data.Context
{
    public class MongoContext : DbContext
    {
        public MongoContext(DbContextOptions<MongoContext> options): base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.ApplyConfiguration(new RunTimeControllerMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}
