using Service.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Service.Services
{
    public class HashPasswordService : IHashPasswordService
    {
        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                StringBuilder builder = new StringBuilder();
                foreach (var b in hashBytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public bool VerifyPassword(string password, string hash) 
        { 
            string computedHash = HashPassword(password);
            return string.Equals(computedHash, hash, StringComparison.OrdinalIgnoreCase);
        }
    }
}
