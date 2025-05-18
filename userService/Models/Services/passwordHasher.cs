using System.Security.Cryptography;
using System.Text;

public static class PasswordHasher
{
    private const int SaltSize = 16; // 128
    private const int KeySize = 32; // 256 
    private const int Iterations = 100_000;

    public static string HashPassword(string password)
    {
        if (string.IsNullOrEmpty(password))
            throw new ArgumentNullException(nameof(password), "Password cannot be null or empty");

        using var rng = RandomNumberGenerator.Create();
        var salt = new byte[SaltSize];
        rng.GetBytes(salt);

        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
        var hash = pbkdf2.GetBytes(KeySize);

        // iterations.salt.Hash (Base64)
        return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }

    public static bool VerifyPassword(string password, string hashedPassword)
    {
        try
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hashedPassword))
                return false;

            var parts = hashedPassword.Split('.');
            if (parts.Length != 3) 
                return false;

            if (!int.TryParse(parts[0], out int iterations))
                return false;

            if (iterations != Iterations) // Ensure we're using the same iteration count
                return false;

            byte[] salt;
            byte[] hash;

            try
            {
                salt = Convert.FromBase64String(parts[1]);
                hash = Convert.FromBase64String(parts[2]);
            }
            catch (FormatException)
            {
                return false; // Invalid Base64 string
            }

            if (salt.Length != SaltSize || hash.Length != KeySize)
                return false;

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
            var computedHash = pbkdf2.GetBytes(KeySize);

            return CryptographicOperations.FixedTimeEquals(hash, computedHash);
        }
        catch (Exception)
        {
            return false; // Any unexpected error results in verification failure
        }
    }

    public static bool IsValidHashFormat(string hashedPassword)
    {
        if (string.IsNullOrEmpty(hashedPassword))
            return false;

        var parts = hashedPassword.Split('.');
        if (parts.Length != 3)
            return false;

        return int.TryParse(parts[0], out _) &&
               !string.IsNullOrEmpty(parts[1]) &&
               !string.IsNullOrEmpty(parts[2]);
    }
}
