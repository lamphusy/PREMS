using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class UserViewModel
    {
        [Required]
        public string IdApplicationUser { get; set; }
        [StringLength(256)]
        public string FullName { get; set; }
        public string Nation { get; set; }
        [StringLength(256)]
        public string Address { get; set; }

        public DateTime? DayOfBirth { get; set; }

        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        public string Username { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DisplayName("Card id")]
        public string IdCard { get; set; }

        [DataType(DataType.Date)]
        public DateTime RegisterDate { get; set; }

    }
}
