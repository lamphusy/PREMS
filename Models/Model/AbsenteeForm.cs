using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [Table("AbsenteeForm")]
    public class AbsenteeForm
    {
        [Key]
        public int ID { get; set; }
        public string Title { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public string IDStudent { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public int IDShift { get; set; }
        public int Status { get; set; }
        public DateTime CreateDate { get; set; }
        public int IDSemester { get; set; }
        public string IDOrganization { get; set; }

        [ForeignKey("IDShift")]
        public virtual OShift Shift { get; set; }
        [ForeignKey("IDStudent")]
        public virtual Student Student { get; set; }
        [ForeignKey("IDSemester")]
        public virtual Semester Semester { get; set; }
        [ForeignKey("IDOrganization")]
        public virtual Organization Organization { get; set; }
    }
}
