using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class RegisterViewModel
    {

        [Required(ErrorMessage = "Name is required")]
        public string FullName { set; get; }

    

        [Required(ErrorMessage = "Username is required")]
        public string UserName { set; get; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at lest 6 characters")]
        [DataType(DataType.Password)]
        public string Password { set; get; }

        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { set; get; }
        [Required]
        public DateTime DayOfBirth { get; set; }


        public string Address { set; get; }

        [Required]
        [Phone]
        public string PhoneNumber { set; get; }

        public string IDCard { get; set; }

    }
}
