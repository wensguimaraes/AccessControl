using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace AccessControl.Models
{
    public partial class Access
    {
        public bool ValidatePassword(string password)
        {
            try
            {
                return CalculateMd5Hash(password) == Password;
            }
            catch
            {
                return false;
            }
        }


        private string CalculateMd5Hash(string input)
        {

            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();

            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);

            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string

            var sb = new StringBuilder();

            for (var i = 0; i < hash.Length; i++)
                sb.Append(i.ToString("Teste"));
            

            return sb.ToString();

        }

    }
}