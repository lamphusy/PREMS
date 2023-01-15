using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class TeacherViewModel
    {
        public string IDUser { get; set; }
        [Required]
        public string FullName { get; set; } 

        [DataType(DataType.Date)]
        public DateTime DayOfBirth { get; set; }  

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }  

        [Required]
        public string Password { get; set; }  

        [Required]
        public string Username { get; set; } 

        [MaxLength(100)]
        public string IDCard { get; set; } 

        public string Address { get; set; }

        public string AvatarPath { get; set; } 

        public string PhoneNumber { get; set; } 

        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }

        public DateTime StartJobDate { get; set; }

        public string CreateBy { get; set; }

        public string Degree { get; set; }

        public string Gender { get; set; }

        public string Specialization { get; set; }

    }
}
