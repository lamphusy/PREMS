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
    [Table("ORegister")]
    public class ORegister
    {
        [Key]
        public string IdApplicationUser { get; set; }

        [Required]
        [DisplayName("Card id")]
        public string IdCard { get; set; }
        [DataType(DataType.Date)]
        public DateTime RegisterDate { get; set; }
        

        [ForeignKey("IdApplicationUser")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<Organization> Organizations { get; set; }

       


    }
}
