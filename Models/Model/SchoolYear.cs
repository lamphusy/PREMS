using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class SchoolYear
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        
        public int ID { get; set; }

        [Required]
        public int LastYear { get; set; }

        [Required]
        public int NextYear { get; set; }

        public string IDOrganization { get; set; }
        [ForeignKey("IDOrganization")]
        public virtual Organization Organization { get; set; }
    }
}
