using System.Security.Cryptography;
using System.Text;

namespace MyContactsAPI.Helper
{
    public static class Cryptography
    {
        public static string MakeHash(this string value)
        {
            //var hash = MD5.Create();
            //var hash = SHA1.Create();
            var hash = SHA256.Create();
            //var hash = SHA384.Create();
            //var hash = SHA512.Create();
            var encode = new ASCIIEncoding();
            var array = encode.GetBytes(value);

            array = hash.ComputeHash(array);

            var stringHexa = new StringBuilder();

            foreach (var item in array)
            {
                stringHexa.Append(item.ToString("x2"));
            }
            
            return stringHexa.ToString();
        }

        public static string GenerateJwtSecretKey()
        {
            var rng = new RNGCryptoServiceProvider();
            var keyBytes = new byte[64]; // 512 bits é uma boa prática para chaves simétricas
            rng.GetBytes(keyBytes);
            return Convert.ToBase64String(keyBytes);
        }
    }
}
