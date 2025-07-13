using LoanProcessing.Domain.Entities;
using LoanProcessing.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanProcessing.Domain.Configurations
{
    public class LoanApplicationConfiguration : IEntityTypeConfiguration<LoanApplication>
    {

        public void Configure(EntityTypeBuilder<LoanApplication> builder)
        {
            builder.ToTable("LoanApplications");

            builder.HasKey(la => la.Id);

            builder.Property(la => la.LoanType)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(la => la.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(la => la.Currency)
                .IsRequired()
                .HasMaxLength(3);

            builder.Property(la => la.Period)
                .IsRequired();

            builder.Property(la => la.Status)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(la => la.UserId)
                .IsRequired();

            builder.HasOne(la => la.User)
                .WithMany(u => u.LoanApplications)
                .HasForeignKey(la => la.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
