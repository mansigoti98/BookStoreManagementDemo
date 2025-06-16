using System.Security.Cryptography;
using System.Text;

public static class PasswordHelper
{
    public static void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = Convert.ToBase64String(hmac.Key);
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            passwordHash = Convert.ToBase64String(hash);
        }
    }

    public static bool VerifyPassword(string password, string storedHash, string storedSalt)
    {
        var key = Convert.FromBase64String(storedSalt);
        using (var hmac = new HMACSHA512(key))
        {
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            var computedHashString = Convert.ToBase64String(computedHash);
            return computedHashString == storedHash;
        }
    }
}
