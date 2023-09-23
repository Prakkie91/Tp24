using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tp24.Infrastructure.DataAccess.Entities;

namespace Tp24.Infrastructure.DataAccess.Configurations;

public class DebtorConfiguration : IEntityTypeConfiguration<DebtorDataModel>
{
    public void Configure(EntityTypeBuilder<DebtorDataModel> builder)
    {
        // Configure primary key
        builder.HasKey(d => d.Id);

        // Configure properties
        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(d => d.Reference)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(d => d.Address1)
            .HasMaxLength(255);

        builder.Property(d => d.Address2)
            .HasMaxLength(255);

        builder.Property(d => d.Town)
            .HasMaxLength(255);

        builder.Property(d => d.State)
            .HasMaxLength(255);

        builder.Property(d => d.Zip)
            .HasMaxLength(10);

        builder.Property(d => d.CountryCode)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(d => d.RegistrationNumber)
            .HasMaxLength(255);
    }
}