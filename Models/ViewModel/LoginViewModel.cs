using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "You have to put in username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "You have to put in password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        
    }
}
