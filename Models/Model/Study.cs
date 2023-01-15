using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Study
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int IDStudy { get; set; }
        //public int IndexInClass { get; set; }
        public string IDClass { get; set; }
        public string IDStudent { get; set; }

        [ForeignKey("IDClass")]
        public virtual Class Class { get; set; }
        [ForeignKey("IDStudent")]
        public virtual Student Student { get; set; }
    }
}
