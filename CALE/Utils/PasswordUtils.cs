using Konscious.Security.Cryptography;
using System.Text;

namespace CALE.Utils
{
    public static class PasswordUtils
    {
        public static string HashPassword(string password)
        {
            using (var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password)))
            {
                argon2.DegreeOfParallelism = 8;
                argon2.MemorySize = 1024 * 1024;
                argon2.Iterations = 4;

                byte[] hash = argon2.GetBytes(32);

                string hashedPassword = Convert.ToBase64String(hash);
                return hashedPassword;
            }
        }

        public static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            string enteredHash = HashPassword(enteredPassword);
            return enteredHash == storedHash;
        }
    }
}
