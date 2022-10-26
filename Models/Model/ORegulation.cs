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
    [Table("ORegulation")]
    public class ORegulation
    {
        [Key]
        public string IDOrganization { get; set; }
        [DefaultValue(2)]
        public int NumberOfShift { get; set; }
        [DefaultValue(10)]
        public int NumberOfPeriod { get; set; }

        [ForeignKey("IDOrganization")]
        public virtual Organization Organization { get; set; }
    }
}
