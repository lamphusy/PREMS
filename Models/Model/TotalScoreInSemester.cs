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
    [Table("TotalScore")]
    public class TotalScoreInSemester
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string IDStudent { get; set; }
        public int IDSemester { get; set; }
        [Required]
        [DefaultValue(0)]
        public float Score { get; set; }

        [ForeignKey("IDStudent")]
        public virtual Student Student { get; set; }
        [ForeignKey("IDSemester")]
        public virtual Semester Semester { get; set; }
    }
}
