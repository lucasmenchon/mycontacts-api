using MyContactsAPI.SharedContext;

namespace MyContactsAPI.Models
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
                throw new Exception("Este item já foi ativado");

            if (ExpiresAt < DateTime.UtcNow)
                throw new Exception("Este código já expirou");

            if (!string.Equals(code.Trim(), Code.Trim(), StringComparison.CurrentCultureIgnoreCase))
                throw new Exception("Código de verificação inválido");

            ExpiresAt = null;
            VerifiedAt = DateTimeOffset.UtcNow;
        }
    }
}
