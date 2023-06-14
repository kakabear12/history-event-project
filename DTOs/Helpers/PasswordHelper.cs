using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;

namespace DTOs.Helpers
{
    public class PasswordHelper
    {
        public static byte[] GetSecureSalt()
        {
            byte[] salt = new byte[32];
            RandomNumberGenerator.Fill(salt);
            return salt;
        }
        public static string HashUsingPbkdf2(string password, byte[] salt)
        {
            byte[] derivedKey = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, iterationCount: 100000, 32);

            return Convert.ToBase64String(derivedKey);
        }
    }
}
