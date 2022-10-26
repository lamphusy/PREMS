using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [Table("Subject")]
    public class Subject
    {
        [Key]
        public string IDSubject { get; set; }

        public string SubjectName { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public string IDOrganization { get; set; }

        [ForeignKey("IDOrganization")]
        public virtual Organization Organization { get; set; }
    }
}
