using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        

        private static TripleDESCryptoServiceProvider _tripleDes;
        private static MD5CryptoServiceProvider _md5;
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
        public static string Encrypt(string stringToEncrypt)
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


        public Access Clone()
        {
            return new Access
            {
                Id = Id,
                Name = Name,
                LastName = LastName,
                Profile = Profile,
                Active = Active,
                Email = Email,
                Password = string.Empty
            };
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
        [Index(IsUnique = true)]
        public string Email { get; set; }

        
        [Required()]
        [StringLength(1000, MinimumLength = 6)]
        //[RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$")]
        [DataType(DataType.Password)]
        //[MembershipPassword()]
        [Display(Name = "Password")]
        public string Password { get; set; }



        [Required()]
        [Display(Name = "Active")]
        public bool Active { get; set; }


        [Required()]
        [Display(Name = "Profile")]
        public string Profile { get; set; }


        [Required()]
        [StringLength(30, MinimumLength = 2)]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string Name { get; set; }




        [Required()]
        [StringLength(30, MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }



    }

    public enum Profiles
    {
        User,
        Administrator
    }

}