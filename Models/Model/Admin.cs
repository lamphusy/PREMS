using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [Table("Admin")]
    public class Admin
    {
        [Key]
        public string IDUser { get; set; }

        [ForeignKey("IDUser")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
