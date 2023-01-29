using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [Table("ScoreType")]
    public class TypeScore
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int IDScoreType { get; set; }
        [Required]
        public string NameScore { get; set; }
        [Required]
        public float PercentScore { get; set; }
        public string IDOrganization { get; set; }

        [ForeignKey("IDOrganization")]
        public virtual Organization Organization { get; set; }
    }
}
