using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;

namespace AccessControl.Models
{

    [MetadataType(typeof(PartialAccess))]
    public partial class Access
    {
        private TripleDESCryptoServiceProvider _tripleDes;
        private MD5CryptoServiceProvider _md5;
        private const string Key = "T3st3";



        public bool ValidatePassword(string password)
        {
            try
            {
                return Password == Encrypt(password);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Encripta uma string com base em uma chave
        /// </summary>
        /// <param name="stringToEncrypt"></param>
        /// <returns></returns>
        private string Encrypt(string stringToEncrypt)
        {
            try
            {
                // Converts the Value to an array of the bytes
                byte[] byteArray = Encoding.UTF8.GetBytes(Key);
                _md5 = new MD5CryptoServiceProvider();
                


                // Definição da chave e da cifra (que neste caso é Electronic 
                // Codebook, ou seja, encriptação individual para cada bloco) 
                _tripleDes = new TripleDESCryptoServiceProvider
                {
                    Key = _md5.ComputeHash(byteArray),
                    Mode = CipherMode.ECB
                };

                var buffer = Encoding.ASCII.GetBytes(stringToEncrypt);
                return Convert.ToBase64String(_tripleDes.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length));
            }
            catch (Exception)
            {
                return string.Empty;
            }

        }

    }

    public class PartialAccess
    {
        [Key]
        [ScaffoldColumn(false)]
        public int Id { get; set; }





        [Required()]
        [DataType(DataType.EmailAddress)]
        [StringLength(128)]
        public string Email { get; set; }




        [Required()]
        [StringLength(10, MinimumLength = 6)]
        //[RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$")]
        [DataType(DataType.Password)]
        //[MembershipPassword()]
        [Display(Name = "Password")]
        public string Password { get; set; }





        [Display(Name = "Active")]
        public bool Active { get; set; }



        [Display(Name = "Profile")]
        public string Profile { get; set; }


        
        [StringLength(30, MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string Name { get; set; }




        
        [StringLength(30, MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }



    }

}