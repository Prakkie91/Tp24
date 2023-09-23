using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tp24.Infrastructure.DataAccess.Entities;

namespace Tp24.Infrastructure.DataAccess.Configurations;

public class ReceivableConfiguration : IEntityTypeConfiguration<ReceivableDataModel>
{
    public void Configure(EntityTypeBuilder<ReceivableDataModel> builder)
    {
        // Set Primary Key (assuming BaseEntity has an Id property)
        builder.HasKey(r => r.Id);

        // Property Configurations
        builder.Property(r => r.Reference)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(r => r.CurrencyCode)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(r => r.IssueDate)
            .IsRequired();

        builder.Property(r => r.OpeningValue)
            .IsRequired();

        builder.Property(r => r.PaidValue)
            .IsRequired();

        builder.Property(r => r.DueDate)
            .IsRequired();

        builder.Property(r => r.ClosedDate);

        builder.Property(r => r.Cancelled);

        builder.HasOne(r => r.Debtor)
            .WithMany()
            .HasForeignKey(r => r.DebtorId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}