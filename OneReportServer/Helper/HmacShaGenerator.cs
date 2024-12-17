using System.Security.Cryptography;
using System.Text;

namespace OneReportServer.Helper;

public static class HmacShaGenerator
{
    /// <summary>
    /// Generates an HMAC-SHA256 token using the provided message and secret.
    /// </summary>
    /// <param name="message">The message to be hashed.</param>
    /// <param name="secret">The secret key for hashing.</param>
    /// <returns>A Base64-encoded HMAC token.</returns>
    public static string CreateToken(string message, string secret)
    {
        // Validate inputs
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message cannot be null or empty.", nameof(message));

        if (string.IsNullOrEmpty(secret))
            throw new ArgumentException("Secret cannot be null or empty.", nameof(secret));

        // Use UTF-8 encoding for broader character set support
        byte[] keyBytes = Encoding.UTF8.GetBytes(secret);
        byte[] messageBytes = Encoding.UTF8.GetBytes(message);

        // Use static method for better performance (available in .NET 6+)
        byte[] hashBytes = HMACSHA256.HashData(keyBytes, messageBytes);

        // Convert the hash to Base64 for easier transmission
        return Convert.ToBase64String(hashBytes);
    }
}