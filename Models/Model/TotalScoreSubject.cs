using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [Table("TotalScoreSubject")]
    public class TotalScoreSubject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string IDStudent { get; set; }
        public string IDSubject { get; set; }
        public int IDYear { get; set; }
        [Required]
        public float Score { get; set; }

        [ForeignKey("IDStudent")]
        public virtual Student Student { get; set; }
        [ForeignKey("IDSubject")]
        public virtual Subject Subject { get; set; }
        [ForeignKey("IDYear")]
        public virtual SchoolYear SchoolYear { get; set; }

    }
}
