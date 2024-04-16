using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.EntityFrameworkCore.Extensions;
using Solana.FinderGems.Domain.Model.Database;

namespace Solana.SignatureReader.Infra.Data.Mapper
{
    public class RunTimeControllerMap : IEntityTypeConfiguration<RunTimeController>
    {
        public void Configure(EntityTypeBuilder<RunTimeController> builder)
        {
            builder.ToCollection("RunTimeController");
            builder.Property(rt => rt.ID);
            builder.Property(rt => rt.JobName);
            builder.Property(rt => rt.ConfigurationTimer);
            builder.Property(rt => rt.IsRunning);
            builder.Property(rt => rt.LastExecuteDate);
            builder.HasKey(rt => rt.ID);
        }
    }
}
