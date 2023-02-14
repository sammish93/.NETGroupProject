using System;
using System.Security.Cryptography;
using System.Text;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Security
{
   public class PasswordEncryption
    {
        private const int keySize = 10;
        private const int iteration = 100;
        private static readonly HashAlgorithmName hashAlgo = HashAlgorithmName.SHA512;


        // Method that is used to encrypt passwords before they
        // are stored in the database. Returns a tuple that contains
        // both the hash string and the salt array.
        public static (string, byte[]) Encrypt(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(keySize);
            var encoding = Encoding.UTF8.GetBytes(password);

            var hash = Rfc2898DeriveBytes.Pbkdf2(encoding, salt, iteration, hashAlgo, keySize);
            var hashHex = Convert.ToHexString(hash);

            return (hashHex, salt);
        }

        // Method that is used to verify if an encrypted password
        // matches any of the encrypted passwords in the database.
        public static bool Verify(string password, string hash, byte[] salt)
        {
            var compareHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, iteration, hashAlgo, keySize);
            return compareHash.SequenceEqual(Convert.FromHexString(hash));
        }
    }
}

