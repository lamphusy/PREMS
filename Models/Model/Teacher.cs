using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [Table("Teacher")]
    public class Teacher
    {
        [Key]
        public string IDUser { get; set; }
        public string IDOrganization { get; set; }

        [MaxLength(100)]
        public string IDCard { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }
        public DateTime StartJobDate { get; set; }
        public string CreateBy { get; set; }
    
        public string Degree { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        public decimal CoefficientsSalary { get; set; }
        public string Specialization { get; set; }
        public string AvatarPath { get; set; }
        public string Gender { get; set; }

        [ForeignKey("IDUser")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        [ForeignKey("IDOrganization")]
        public virtual Organization Organization { get; set; }
        [JsonIgnore]
        public virtual ICollection<Teach> Teaches { get; set; }
    }
}
