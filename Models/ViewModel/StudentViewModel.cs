using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class StudentViewModel
    {
        public string FullName { get; set; }//

        public string Address { get; set; }//
    
        public string Email { get; set; }//

        public string PhoneNumber { get; set; }//

        public string Username { get; set; }//

        public string Password { get; set; }//

        public string CreateBy { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }

        public string AvatarPath { get; set; }//

        [DataType(DataType.Date)]
        public DateTime DayOfBirth { get; set; }//

        public string Gender { get; set; }//
    }
}
