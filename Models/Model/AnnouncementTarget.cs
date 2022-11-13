using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class AnnouncementTarget
    {
        [Key]
        public int IDTarget { get; set; }
        [Required]
        public string Meaning { get; set; }
        
        public string IDOrganization { get; set; }

        [ForeignKey("IDOrganization")]
        public virtual Organization Organization { get; set; }
    }
}
