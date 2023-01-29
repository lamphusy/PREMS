using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [Table("Statistics")]
    public class Statistics
    {
        [Key]
        public string ID { get; set; }
        [Required]
        public int Month { get; set; }
        [Required]
        public int Year { get; set; }
        [DataType(DataType.Currency)]
        public double Profit { get; set; }
        [DefaultValue(0)]
        public int NumOfRegister { get; set; }
        [DefaultValue(0)]
        public int NumOfOrganization { get; set; }
        [DefaultValue(0)]
        public int NumOfStudent { get; set; }
        [DefaultValue(0)]
        public int NumOfTeacher { get; set; }


    }
}
