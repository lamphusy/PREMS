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
        public string IDTeacher { get; set; }

        public string IDClass { get; set; }

        public string IDSubject { get; set; }


        [ForeignKey("IDTeacher")]
        public virtual Teacher Teacher { get; set; }
        [ForeignKey("IDSubject")]
        public virtual Subject Subject { get; set; }
        [ForeignKey("IDClass")]
        public virtual Class Class { get; set; }
    }
}
