using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace Tarea2_AdrianArayaG_UNED.Services
{
    public static class CryptoService
    {
        public static string NewSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider()) rng.GetBytes(salt);
            return Convert.ToBase64String(salt);
        }
        public static string Hash(string password, string base64Salt)
        {
            var salt = Convert.FromBase64String(base64Salt);
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                return Convert.ToBase64String(pbkdf2.GetBytes(32));
            }
        }
        public static bool Verify(string password, string base64Salt, string hash)
        {
            return Hash(password, base64Salt) == hash;
        }
    }
}