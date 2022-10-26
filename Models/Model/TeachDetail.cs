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
        [Required]
        public int IDTeach { get; set; }
        [Required]
        public int IDShift { get; set; }
        [Required]
        public int Period { get; set; }
        [Required]
        public int WeekDay { get; set; }

        [ForeignKey("IDTeach")]
        public virtual Teach Teaches { get; set; }
        [ForeignKey("IDShift")]
        public virtual OShift OShift { get; set; }
    }
}
