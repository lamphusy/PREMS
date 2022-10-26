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
        [Required]
        public string IDOrganization { get; set; }

        [Required]
        public string IDStudent { get; set; }
        public string Title { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Required]
        public string IDOldClass { get; set; }
        [Required]
        public string IDNewClass { get; set; }
        [Required]
        public int IDSemester { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }
        public bool Status { get; set; }

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
