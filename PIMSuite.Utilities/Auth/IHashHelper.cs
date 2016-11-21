using System;

namespace PIMSuite.Utilities.Auth
{
    public interface IHashHelper
    {
        String Hash(String text);
        bool Verify(String text, String saltedHash);
    }

    public class HashHelper : IHashHelper
    {
        private const int WorkFactor = 5;

        public String Hash(String text)
        {
            var saltedHash = BCrypt.Net.BCrypt.HashPassword(text, WorkFactor);
            return saltedHash;
        }

        public bool Verify(String text, String saltedHash)
        {
            return BCrypt.Net.BCrypt.Verify(text, saltedHash);
        }
    }
}