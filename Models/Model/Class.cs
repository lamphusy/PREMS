using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [Table("Class")]
    public class Class
    {
        [Key]
        public string IDClass { get; set; }

        [Required]
        public string Name { get; set; }

        public string IDHomeroomTeacher { get; set; }

        public int? Total { get; set; }

        public int IDSemester { get; set; }

        [ForeignKey("IDHomeroomTeacher")]
        public virtual Teacher HomeroomTeacher { get; set; }
        [ForeignKey("IDSemester")]
        public virtual Semester Semester { get; set; }

        public virtual ICollection<Study> Studies { get; set; }

    }
}
