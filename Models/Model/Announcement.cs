using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [Table("Announcement")]
    public class Announcement
    {
        public string ID { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreateBy { get; set; }
        public int IDTarget { get; set; }
        public string IDOrganization { get; set; }

        [ForeignKey("IDTarget")]
        public virtual AnnouncementTarget AnnouncementTarget { get; set; }
        [ForeignKey("IDOrganization")]
        public virtual Organization Organization { get; set; }
    }
}
