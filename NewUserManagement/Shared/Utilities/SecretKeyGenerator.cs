using System.Security.Cryptography;

namespace NewUserManagement.Shared.Utilities;
public  class SecretKeyGenerator
{
    public  string GenerateSecretKey()
    {
        // Generate a random key with 128 bits (16 bytes) length
        byte[] keyBytes = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(keyBytes);
        }

        // Convert the byte array to a base64-encoded string
        string secretKey = Convert.ToBase64String(keyBytes);
        return secretKey;
    }
}
