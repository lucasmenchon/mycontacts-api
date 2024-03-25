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
                .HasColumnType("varchar(120)")
                .HasMaxLength(120)
                .IsRequired(true);

            builder.OwnsOne(x => x.Email, email =>
            {
                email.Property(x => x.Address)
                    .HasColumnName("Email")
                    .HasColumnType("varchar(255)")
                    .IsRequired(true);

                email.OwnsOne(x => x.Verification, verification =>
                {
                    verification.Property(x => x.Code)
                        .HasColumnName("EmailVerificationCode")
                        .HasColumnType("varchar(255)")
                        .IsRequired(true);

                    verification.Property(x => x.ExpiresAt)
                        .HasColumnName("EmailVerificationExpiresAt")
                        .HasColumnType("timestamp")
                        .IsRequired(false);

                    verification.Property(x => x.VerifiedAt)
                        .HasColumnName("EmailVerificationVerifiedAt")
                        .HasColumnType("timestamp")
                        .IsRequired(false);

                    verification.Ignore(x => x.IsActive);
                });
            });

            builder.OwnsOne(x => x.Password, password =>
            {
                password.Property(x => x.Hash)
                    .HasColumnName("PasswordHash")
                    .HasColumnType("varchar(255)") 
                    .IsRequired();

                password.Property(x => x.ResetCode)
                    .HasColumnName("PasswordResetCode")
                    .HasColumnType("varchar(255)")
                    .IsRequired();
            });
        }

    }
}
