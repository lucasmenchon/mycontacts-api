using Microsoft.EntityFrameworkCore;
using MyContactsAPI.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyContactsAPI.Maps
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasColumnName("Name")
                .HasColumnType("varchar")
                .HasMaxLength(120)
                .IsRequired(true);

            builder.Property(x => x.Username)
                .HasColumnName("Username")
                .HasColumnType("varchar")
                .HasMaxLength(120)
                .IsRequired(true);

            builder.OwnsOne(x => x.Email)
            .Property(x => x.Address)
            .HasColumnName("Email")
            .IsRequired(true);

            builder.OwnsOne(x => x.Email)
             .OwnsOne(x => x.Verification)
             .Property(x => x.Code)
             .HasColumnName("EmailVerificationCode")
             .IsRequired(true);

            builder.OwnsOne(x => x.Email)
           .OwnsOne(x => x.Verification)
           .Property(x => x.ExpiresAt)
           .HasColumnName("EmailVerificationExpiresAt")
           .HasColumnType("timestamp(0)")
           .IsRequired(false);

            builder.OwnsOne(x => x.Email)
                .OwnsOne(x => x.Verification)
                .Property(x => x.VerifiedAt)
                .HasColumnName("EmailVerificationVerifiedAt")
                .HasColumnType("timestamp(0)")
                .IsRequired(false);

            //builder.Property(x => x.RegisterDate)
            //    .HasColumnName("RegisterDate")
            //    .HasColumnType("timestamp(0)")
            //    .IsRequired();

            //builder.Property(x => x.UpdateDate)
            //    .HasColumnName("UpdateDate")
            //    .HasColumnType("timestamp(0)")
            //    .IsRequired();

            builder.OwnsOne(x => x.Email)
            .OwnsOne(x => x.Verification)
            .Ignore(x => x.IsActive);

            builder.OwnsOne(x => x.Password)
           .Property(x => x.Hash)
           .HasColumnName("PasswordHash")
           .IsRequired();

            builder.OwnsOne(x => x.Password)
                .Property(x => x.ResetCode)
                .HasColumnName("PasswordResetCode")
                .IsRequired();
        }

    }
}
