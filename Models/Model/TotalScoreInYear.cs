using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class TotalScoreInYear
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string IDStudent { get; set; }
        public int IDYear { get; set; }
        [Required]
        [DefaultValue(0)]
        public float Score { get; set; }

        [ForeignKey("IDStudent")]
        public virtual Student Student { get; set; }
        [ForeignKey("IDYear")]
        public virtual SchoolYear SchoolYear { get; set; }
    }
}
