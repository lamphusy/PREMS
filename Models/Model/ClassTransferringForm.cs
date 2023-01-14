using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [Table("ClassTransferringForm")]
    public class ClassTransferringForm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string IDOrganization { get; set; }
        public string IDStudent { get; set; }
        public string Title { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public string IDOldClass { get; set; }
        public string IDNewClass { get; set; }
        public int IDSemester { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }

        //0: is haven't read
        //-1: is denied
        //1: is accepted
        public int Status { get; set; }

        [ForeignKey("IDStudent")]
        public virtual Student Student { get; set; }
        [ForeignKey("IDOldClass")]
        public virtual Class OldClass { get; set; }
        [ForeignKey("IDNewClass")]
        public virtual Class NewClass { get; set; }
        [ForeignKey("IDSemester")]
        public virtual Semester Semester { get; set; }
        [ForeignKey("IDOrganization")]
        public virtual Organization Organization { get; set; }

    }
}
