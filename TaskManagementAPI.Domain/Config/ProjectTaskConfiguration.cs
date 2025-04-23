using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagementAPI.Domain.Models;

namespace TaskManagementAPI.Domain.Config
{
    internal class ProjectTaskConfiguration : IEntityTypeConfiguration<ProjectTask>
    {
        public void Configure(EntityTypeBuilder<ProjectTask> builder)
        {
            builder.Property(v => v.Id)
                .ValueGeneratedOnAdd();

            builder.Property(v => v.Title)
                .IsRequired();

            builder.Property(v => v.Version)
                .IsRowVersion();
        }
    }
}
