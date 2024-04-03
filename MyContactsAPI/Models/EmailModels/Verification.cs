using MyContactsAPI.SharedContext;

namespace MyContactsAPI.Models.EmailModels
{
    public class Verification : ValueObject
    {
        public Verification()
        {
        }

        public string Code { get; } = Guid.NewGuid().ToString("N")[..6].ToUpper();
        public DateTimeOffset? ExpiresAt { get; private set; } = DateTimeOffset.UtcNow.AddHours(2);
        public DateTimeOffset? VerifiedAt { get; private set; } = null;
        public bool IsActive => VerifiedAt != null && ExpiresAt == null;

        public void Verify(string code)
        {
            if (IsActive)
                throw new Exception("This item has already been activated");

            if (ExpiresAt < DateTime.UtcNow)
                throw new Exception("This code has already expired");

            if (!string.Equals(code.Trim(), Code.Trim(), StringComparison.CurrentCultureIgnoreCase))
                throw new Exception("Invalid verification code");

            ExpiresAt = null;
            VerifiedAt = DateTimeOffset.UtcNow;
        }
    }
}
