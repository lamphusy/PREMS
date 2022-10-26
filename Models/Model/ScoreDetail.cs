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
    [Table("ScoreDetail")]
    public class ScoreDetail
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string IDStudent { get; set; }
        [Required]

        public string IDSubject { get; set; }
        [Required]

        public int IDSemester { get; set; }
        [Required]

        public string IDScoreType { get; set; }
        [DefaultValue(0)]
        public float Score { get; set; }

        [ForeignKey("IDStudent")]
        virtual public Student Student { get; set; }
        [ForeignKey("IDSubject")]
        virtual public Subject Subject { get; set; }
        [ForeignKey("IDSemester")]
        virtual public Semester Semester { get; set; }

    }
}
