using System.Security.Cryptography;
using System.Text;

namespace EnterprisePOS.Services
{
    public static class SecurityHelper
    {
        public static string HashPassword(string plainPassword)
        {
            if (string.IsNullOrEmpty(plainPassword)) return string.Empty;
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(plainPassword));
                var builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static bool VerifyPassword(string plainPassword, string storedHash)
        {
            string hashedInput = HashPassword(plainPassword);
            return string.Equals(hashedInput, storedHash, System.StringComparison.OrdinalIgnoreCase);
        }
    }
}
