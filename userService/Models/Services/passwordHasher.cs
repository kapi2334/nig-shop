using System.Security.Cryptography;
using System.Text;

public static class PasswordHasher
{
    private const int SaltSize = 16; // 128
    private const int KeySize = 32; // 256 
    private const int Iterations = 100_000;

    public static string HashPassword(string password)
    {
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
        var parts = hashedPassword.Split('.');
        if (parts.Length != 3) return false;

        int iterations = int.Parse(parts[0]);
        byte[] salt = Convert.FromBase64String(parts[1]);
        byte[] hash = Convert.FromBase64String(parts[2]);

        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
        var computedHash = pbkdf2.GetBytes(hash.Length);

        return CryptographicOperations.FixedTimeEquals(hash, computedHash);
    }
}
