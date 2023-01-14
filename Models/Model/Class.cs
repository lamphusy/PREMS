using Models;
using Newtonsoft.Json;
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
    [Table("Class")]
    public class Class
    {
        [Key]
        public string IDClass { get; set; }

        [Required]
        public string Name { get; set; }

        public string IDHomeroomTeacher { get; set; }

        [DefaultValue(0)]
        public int? Total { get; set; }

        public int IDYear { get; set; }

        public string IDOrganization { get; set; }
        [ForeignKey("IDHomeroomTeacher")]
        public virtual Teacher HomeroomTeacher { get; set; }
        [JsonIgnore]
        [ForeignKey("IDYear")]
        public virtual SchoolYear SchoolYear { get; set; }
        [JsonIgnore]
        [ForeignKey("IDOrganization")]
        public Organization Organization { get; set; }
        [JsonIgnore]
        public virtual ICollection<Study> Studies { get; set; }



    }
}
