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
    public class Semester
    {
        [Key]
        public int IdSemester { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public int SemesterNum { get; set; }

        [DefaultValue(false)]
        public bool IsNow { get; set; }
        public int IDYear { get; set; }

        [JsonIgnore]
        [ForeignKey("IDYear")]
        public SchoolYear SchoolYear { get; set; }
    }
}
