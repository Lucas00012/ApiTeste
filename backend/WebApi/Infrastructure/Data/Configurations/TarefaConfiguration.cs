using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApi.Core.Entities;
using WebApi.Core.Enums.Tarefa;

namespace WebApi.Infrastructure.Data.Configurations
{
    public class TarefaConfiguration : IEntityTypeConfiguration<Tarefa>
    {
        public void Configure(EntityTypeBuilder<Tarefa> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Status)
                .HasConversion(new EnumToStringConverter<StatusTarefa>());
        }
    }
}
