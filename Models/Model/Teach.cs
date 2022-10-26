using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [Table("Teach")]
    public class Teach
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IDTeach { get; set; }
        [Required]
        public string IDTeacher { get; set; }
        [Required]

        public string IDClass { get; set; }
        [Required]

        public string IDSubject { get; set; }
        [Required]

        public int IDSemester { get; set; }

        [ForeignKey("IDTeacher")]
        public virtual Teacher Teacher { get; set; }
        [ForeignKey("IDSubject")]
        public virtual Subject Subject { get; set; }
        [ForeignKey("IDClass")]

        public virtual Class Class { get; set; }
        [ForeignKey("IDSemester")]

        public virtual Semester Semester { get; set; }
    }
}
