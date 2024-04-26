using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace onlineshopping.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Email Required")]
        [DataType(DataType.EmailAddress,ErrorMessage ="Email is invalid")]

        public string Email { get; set; }
        [Required(ErrorMessage = "Password Required")]
        [DataType(DataType.Password)]
       

        public string Password { get; set; }

        public string? UserType { get; set; }




    }
}
