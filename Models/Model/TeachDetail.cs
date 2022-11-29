using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class TeachDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int IDTeach { get; set; }
        [Required]
        public int IDPeriod { get; set; }
        [Required]
        public int WeekDay { get; set; }

        [ForeignKey("IDTeach")]
        public virtual Teach Teaches { get; set; }
        [ForeignKey("IDPeriod")]
        public virtual OPeriodLesson OPeriodLesson { get; set; }
    }
}
