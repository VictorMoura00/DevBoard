using DevBoard.Modules.Projects.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevBoard.Modules.Projects.Data;

public sealed class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("projects", "projects");

        builder.HasKey(project => project.Id);

        builder.Property(project => project.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(project => project.Description)
            .HasMaxLength(2000);

        builder.Property(project => project.RepositoryUrl)
            .HasMaxLength(500);

        builder.Property(project => project.Status)
            .HasConversion<string>()
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(project => project.Tags)
            .HasColumnType("text[]")
            .IsRequired();

        builder.Property(project => project.CreatedAt)
            .IsRequired();

        builder.Property(project => project.UpdatedAt);

        builder.Property(project => project.ArchivedAt);

        builder.HasIndex(project => project.Name);
        builder.HasIndex(project => project.Status);
    }
}
