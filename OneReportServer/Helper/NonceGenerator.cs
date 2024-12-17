using System.Security.Cryptography;

namespace OneReportServer.Helper
{
    public static class NonceGenerator
    {
        private const int MaxBase64NonceLength = 40; // Max Base64-encoded nonce length
        private const int MaxRawNonceSize = 30; // Corresponding raw byte size for 40 Base64 characters

        /// <summary>
        /// Generates a secure nonce as a Base64-encoded string with a maximum size of 40 characters.
        /// </summary>
        /// <param name="nonceSize">The size of the nonce in bytes. Default is 30 bytes.</param>
        /// <returns>A unique nonce string.</returns>
        public static string GenerateNonce(int nonceSize = MaxRawNonceSize)
        {
            if (nonceSize > MaxRawNonceSize)
            {
                throw new ArgumentOutOfRangeException(nameof(nonceSize),
                    $"Nonce size cannot exceed {MaxRawNonceSize} bytes.");
            }

            byte[] nonceBytes = new byte[nonceSize];
            RandomNumberGenerator.Fill(nonceBytes);
            return Convert.ToBase64String(nonceBytes);
        }

        /// <summary>
        /// Generates a secure nonce with an associated timestamp.
        /// </summary>
        /// <returns>A combined nonce and timestamp in the format "nonce|timestamp".</returns>
        public static string GenerateNonceWithTimestamp()
        {
            string nonce = GenerateNonce(); // Fixed size of 30 bytes, resulting in 40 Base64 characters
            string timestamp = DateTime.UtcNow.ToString("o"); // ISO 8601 format
            return $"{nonce}|{timestamp}";
        }

        /// <summary>
        /// Validates a nonce with an embedded timestamp.
        /// </summary>
        /// <param name="combinedNonce">The combined nonce and timestamp string.</param>
        /// <param name="validityPeriodMinutes">The validity period in minutes for the nonce.</param>
        /// <returns>True if the nonce is valid and within the specified validity period; otherwise, false.</returns>
        public static bool ValidateNonceWithTimestamp(string combinedNonce, int validityPeriodMinutes = 5)
        {
            if (string.IsNullOrEmpty(combinedNonce) || !combinedNonce.Contains('|'))
            {
                return false;
            }

            string[] parts = combinedNonce.Split('|');
            if (parts.Length != 2)
            {
                return false;
            }

            string timestampPart = parts[1];
            if (!DateTime.TryParse(timestampPart, out DateTime timestamp))
            {
                return false;
            }

            TimeSpan difference = DateTime.UtcNow - timestamp;
            return Math.Abs(difference.TotalMinutes) <= validityPeriodMinutes;
        }
    }
}