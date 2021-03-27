using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace NoteMarketPlace.Password_Encryption
{
    public class PasswordEncryption
    {
        public static string EncryptPasswordMd5(string psssword)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            UTF8Encoding encoder = new UTF8Encoding();
            Byte[] originalBytes = encoder.GetBytes(psssword);
            Byte[] encodedBytes = md5.ComputeHash(originalBytes);
            var hashedPassword = BitConverter.ToString(encodedBytes).Replace("-", "");
            var newPassword = hashedPassword.ToLower();

            return newPassword;

        }
    }
}
